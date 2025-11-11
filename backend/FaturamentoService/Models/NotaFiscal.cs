using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturamentoService.Models;

public enum StatusNota { Aberta = 0, Fechada = 1, Cancelado = 2 }

public class NotaItem
{
    public int Id { get; set; }  // ✅ chave primária para EF

    [Required]
    public string CodigoProduto { get; set; } = default!;

    [Required]
    public int Quantidade { get; set; }

    // 🔗 relacionamento com NotaFiscal
    public int NotaFiscalNumero { get; set; }
}


public class NotaFiscal
{
    [Key]
    public int Numero { get; set; }

    public StatusNota Status { get; set; } = StatusNota.Aberta;

    public List<NotaItem> Itens { get; set; } = new();
}

