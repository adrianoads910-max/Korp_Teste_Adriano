using System.Net;
using FaturamentoService.Data;
using FaturamentoService.Models;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Banco em memória
builder.Services.AddSingleton<NotasRepo>();

// ✅ Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// HttpClient para EstoqueService
builder.Services.AddHttpClient("estoque", c =>
{
    c.BaseAddress = new Uri("http://localhost:5229/"); // porta do EstoqueService
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

app.UseSwagger();
app.UseSwaggerUI();

// ✅ aplicar CORS antes dos endpoints
app.UseCors("AllowAll");

app.MapPost("/notas", (NotasRepo repo, NotaFiscal nota) =>
{
    if (nota.Itens == null || nota.Itens.Count == 0)
        return Results.BadRequest(new { erro = "Nota deve conter itens" });

    var criada = repo.Criar(nota);
    return Results.Created($"/notas/{criada.Numero}", criada);
});

// ✅ Novo endpoint LISTAR notas
app.MapGet("/notas", (NotasRepo repo) =>
{
    return Results.Ok(repo.Listar());
});

app.MapPost("/notas/{numero:int}/imprimir", async (int numero, HttpContext http, NotasRepo repo, IHttpClientFactory httpClientFactory) =>
{
    var idempotencyKey = http.Request.Headers["Idempotency-Key"].FirstOrDefault();

    if (!string.IsNullOrEmpty(idempotencyKey) && repo.JaProcessado(idempotencyKey))
        return Results.Ok(new { mensagem = "Requisição já processada (idempotência)" });

    var nota = repo.Obter(numero);
    if (nota is null) return Results.NotFound(new { erro = "Nota não encontrada" });

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

app.Run();
