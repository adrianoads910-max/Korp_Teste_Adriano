using EstoqueService.Data;
using EstoqueService.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Banco em memória
builder.Services.AddSingleton<ProdutoRepo>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// -----------------------------
// ORDEM CORRETA
// -----------------------------
app.UseRouting();
app.UseCors("AllowAll");
// -----------------------------

// Criar produto
app.MapPost("/produtos", (ProdutoRepo repo, Produto produto) =>
{
    repo.Criar(produto);
    return Results.Created($"/produtos/{produto.Codigo}", produto);
});

// Listar produtos
app.MapGet("/produtos", (ProdutoRepo repo) =>
{
    return Results.Ok(repo.Listar());
});

// Buscar produto
app.MapGet("/produtos/{codigo}", (ProdutoRepo repo, string codigo) =>
{
    var produto = repo.Obter(codigo);
    return produto is null ? Results.NotFound() : Results.Ok(produto);
});

// Reservar estoque
app.MapPost("/produtos/{codigo}/reservar", (ProdutoRepo repo, string codigo, int quantidade) =>
{
    var reservado = repo.Reservar(codigo, quantidade);
    return reservado
        ? Results.Ok()
        : Results.Conflict(new { erro = "Saldo insuficiente" });
});

app.Run();
