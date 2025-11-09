using FaturamentoService.Models;

namespace FaturamentoService.Data;

public class NotasRepo
{
    private readonly object _lock = new();
    private readonly Dictionary<int, NotaFiscal> _notas = new();
    private int _sequencia = 0;
    private readonly HashSet<string> _idempotencia = new();

    public NotaFiscal Criar(NotaFiscal nota)
    {
        lock (_lock)
        {
            nota.Numero = ++_sequencia;
            nota.Status = StatusNota.Aberta;  // ✅ garante status inicial
            _notas[nota.Numero] = nota;
            return nota;
        }
    }

    public NotaFiscal? Obter(int numero) =>
        _notas.TryGetValue(numero, out var n) ? n : null;

    public List<NotaFiscal> Listar()
    {
        lock (_lock)
        {
            return _notas.Values.ToList();
        }
    }

    public void Fechar(NotaFiscal nota) =>
        nota.Status = StatusNota.Fechada;

    public void Cancelar(NotaFiscal nota) =>
    nota.Status = StatusNota.Cancelada;
    

    public bool JaProcessado(string chave) =>
        _idempotencia.Contains(chave);

    public void MarcarProcessado(string chave) =>
        _idempotencia.Add(chave);
}
