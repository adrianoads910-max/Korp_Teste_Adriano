using FaturamentoService.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturamentoService.Data;

public class NotasRepo
{
    private readonly AppDbContext _db;
    private readonly HashSet<string> _idempotencia = new();

    public NotasRepo(AppDbContext db)
    {
        _db = db;
    }

    public NotaFiscal Criar(NotaFiscal nota)
    {
        nota.Status = StatusNota.Aberta; // garante status inicial
        _db.Notas.Add(nota);
        _db.SaveChanges();
        return nota;
    }

    public NotaFiscal? Obter(int numero)
    {
        return _db.Notas
            .Include(n => n.Itens)   // <--- essencial para retornar os itens
            .FirstOrDefault(n => n.Numero == numero);
    }

    public List<NotaFiscal> Listar()
    {
        return _db.Notas
            .Include(n => n.Itens)   // <--- lista notas com itens
            .ToList();
    }

    public void Fechar(NotaFiscal nota)
    {
        nota.Status = StatusNota.Fechada;
        _db.SaveChanges();
    }

    public void Cancelar(NotaFiscal nota)
    {
        nota.Status = StatusNota.Cancelado;
        _db.SaveChanges();
    }

    public bool JaProcessado(string chave) =>
        _idempotencia.Contains(chave);

    public void MarcarProcessado(string chave) =>
        _idempotencia.Add(chave);
}
