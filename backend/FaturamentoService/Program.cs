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

// -----------------------------
// DATABASE
// -----------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConn")));

// -----------------------------
// IDENTITY
// -----------------------------
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
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

// -----------------------------
// CORS (libera tudo)
// -----------------------------
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// -----------------------------
// HTTPCLIENT (Estoqueservice)
// -----------------------------
var estoqueUrl =
    Environment.GetEnvironmentVariable("ESTOQUE_URL")
    ?? builder.Configuration["EstoqueService"]
    ?? "https://estoqueservice-production.up.railway.app/";

builder.Services.AddHttpClient("estoque", c =>
{
    c.BaseAddress = new Uri(estoqueUrl);
})
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200 * (retry + 1))))
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -----------------------------
// MIGRATIONS
// -----------------------------
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>()
        .Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// ---------------------------------------------------------
// 🔥 MIDDLEWARE DE CORS MANUAL - NECESSÁRIO NO RAILWAY
// ---------------------------------------------------------
app.Use(async (context, next) =>
{
    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    context.Response.Headers["Access-Control-Allow-Headers"] = "*";
    context.Response.Headers["Access-Control-Allow-Methods"] = "*";

    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }

    await next();
});

// ---------------------------------------------------------
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// -----------------------------
// AUTH
// -----------------------------
app.MapPost("/auth/register", async (
    [FromBody] RegisterRequest req,
    [FromServices] UserManager<IdentityUser> userManager) =>
{
    var user = new IdentityUser { UserName = req.Email, Email = req.Email };
    var result = await userManager.CreateAsync(user, req.Senha);

    return !result.Succeeded
        ? Results.BadRequest(result.Errors)
        : Results.Ok(new { message = "Usuário criado com sucesso!" });
});

app.MapPost("/auth/login", async (
    [FromBody] LoginRequest req,
    [FromServices] SignInManager<IdentityUser> signInManager) =>
{
    var result = await signInManager.PasswordSignInAsync(req.Email, req.Senha, false, false);

    return result.Succeeded
        ? Results.Ok(new { authenticated = true })
        : Results.BadRequest(new { error = "Credenciais inválidas" });
});

// -----------------------------
// NOTAS
// -----------------------------
app.MapPost("/notas", (
    [FromServices] NotasRepo repo,
    [FromBody] NotaFiscal nota) =>
{
    if (nota.Itens == null || nota.Itens.Count == 0)
        return Results.BadRequest(new { erro = "Nota deve conter itens" });

    var criada = repo.Criar(nota);
    return Results.Created($"/notas/{criada.Numero}", criada);
});

app.MapGet("/notas", ([FromServices] NotasRepo repo) =>
{
    return Results.Ok(repo.Listar());
});

app.MapPost("/notas/{numero:int}/imprimir", async (
    int numero,
    HttpContext http,
    [FromServices] NotasRepo repo,
    [FromServices] IHttpClientFactory httpFactory) =>
{
    var nota = repo.Obter(numero);
    if (nota is null)
        return Results.NotFound(new { erro = "Nota não encontrada" });

    if (nota.Status != StatusNota.Aberta)
        return Results.BadRequest(new { erro = "Somente notas em aberto podem ser impressas" });

    var client = httpFactory.CreateClient("estoque");

    foreach (var item in nota.Itens)
    {
        var resp = await client.PostAsync($"/produtos/{item.CodigoProduto}/reservar?quantidade={item.Quantidade}", null);

        if (resp.StatusCode == HttpStatusCode.Conflict)
            return Results.Conflict(new { erro = $"Sem saldo do produto {item.CodigoProduto}" });

        if (!resp.IsSuccessStatusCode)
            return Results.Json(new { erro = "Erro ao comunicar com EstoqueService" }, statusCode: 503);
    }

    repo.Fechar(nota);
    return Results.Ok(new { mensagem = "Nota fechada", numero, status = nota.Status.ToString() });
});

app.MapPost("/notas/{numero:int}/cancelar", (
    int numero,
    [FromServices] NotasRepo repo) =>
{
    var nota = repo.Obter(numero);
    if (nota is null)
        return Results.NotFound(new { erro = "Nota não encontrada" });

    repo.Cancelar(nota);
    return Results.Ok(new { mensagem = "Nota cancelada", numero, status = nota.Status.ToString() });
});

app.Run();
