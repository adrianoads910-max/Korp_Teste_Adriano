using System.Net;
using FaturamentoService.Data;
using FaturamentoService.Models;
using FaturamentoService.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configurar banco SQLite + EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConn")));

// ✅ Configurar Identity (senha simples permitida)
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 3;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// ✅ Injeta o repositório usando SQLite
builder.Services.AddScoped<NotasRepo>();

// ✅ CORS liberado
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ✅ HttpClient para EstoqueService com POLLY (resiliência)
builder.Services.AddHttpClient("estoque", c =>
{
    c.BaseAddress = new Uri(
        Environment.GetEnvironmentVariable("ESTOQUE_URL") ?? "http://localhost:5229/"
    );
})
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, tentativa => TimeSpan.FromMilliseconds(200 * (tentativa + 1))))
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Rodar migrations automaticamente no startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// ✅ Endpoint: Registrar usuário
app.MapPost("/auth/register", async (
    [FromBody] RegisterRequest req,
    UserManager<IdentityUser> userManager) =>
{
    var user = new IdentityUser { UserName = req.Email, Email = req.Email };
    var result = await userManager.CreateAsync(user, req.Senha);

    if (!result.Succeeded)
        return Results.BadRequest(result.Errors);

    return Results.Ok(new { message = "Usuário criado com sucesso!" });
});

// ✅ Endpoint: Login
app.MapPost("/auth/login", async (
    [FromBody] LoginRequest req,
    SignInManager<IdentityUser> signInManager) =>
{
    var result = await signInManager.PasswordSignInAsync(req.Email, req.Senha, false, false);

    return result.Succeeded
        ? Results.Ok(new { authenticated = true })
        : Results.BadRequest(new { error = "Credenciais inválidas" });
});

// ✅ Criar nota fiscal
app.MapPost("/notas", (NotasRepo repo, NotaFiscal nota) =>
{
    if (nota.Itens == null || nota.Itens.Count == 0)
        return Results.BadRequest(new { erro = "Nota deve conter itens" });

    var criada = repo.Criar(nota);
    return Results.Created($"/notas/{criada.Numero}", criada);
});

// ✅ Listar notas
app.MapGet("/notas", (NotasRepo repo) =>
{
    return Results.Ok(repo.Listar());
});

// ✅ Imprimir nota (reserva estoque via EstoqueService)
app.MapPost("/notas/{numero:int}/imprimir", async (int numero, HttpContext http, NotasRepo repo, IHttpClientFactory httpClientFactory) =>
{
    var idempotencyKey = http.Request.Headers["Idempotency-Key"].FirstOrDefault();

    if (!string.IsNullOrEmpty(idempotencyKey) && repo.JaProcessado(idempotencyKey))
        return Results.Ok(new { mensagem = "Requisição já processada (idempotência)" });

    var nota = repo.Obter(numero);
    if (nota is null)
        return Results.NotFound(new { erro = "Nota não encontrada" });

    if (nota.Status != StatusNota.Aberta)
        return Results.BadRequest(new { erro = "Somente notas em aberto podem ser impressas" });

    var client = httpClientFactory.CreateClient("estoque");

    foreach (var item in nota.Itens)
    {
        var response = await client.PostAsync($"/produtos/{item.CodigoProduto}/reservar?quantidade={item.Quantidade}", null);

        if (response.StatusCode == HttpStatusCode.Conflict)
            return Results.Conflict(new { erro = $"Sem saldo no produto {item.CodigoProduto}" });

        if (!response.IsSuccessStatusCode)
            return Results.Json(new { erro = "Falha ao comunicar com o serviço de estoque" }, statusCode: 503);
    }

    repo.Fechar(nota);

    if (!string.IsNullOrEmpty(idempotencyKey))
        repo.MarcarProcessado(idempotencyKey);

    return Results.Ok(new { mensagem = "Nota impressa e fechada", numero, status = nota.Status.ToString() });
});

// ✅ Cancelar nota fiscal
app.MapPost("/notas/{numero:int}/cancelar", (int numero, NotasRepo repo) =>
{
    var nota = repo.Obter(numero);
    if (nota is null)
        return Results.NotFound(new { erro = "Nota não encontrada" });

    if (nota.Status == StatusNota.Fechada)
        return Results.BadRequest(new { erro = "Nota já está fechada" });

    repo.Cancelar(nota);
    return Results.Ok(new { mensagem = "Nota cancelada com sucesso", numero, status = nota.Status.ToString() });
});

app.Run();
