using System.ComponentModel.DataAnnotations;

namespace FaturamentoService.Models;

public enum StatusNota { Aberta = 0, Fechada = 1 }

public class NotaItem
{
    [Required]
    public string CodigoProduto { get; set; } = default!;

    [Required]
    public int Quantidade { get; set; }
}

public class NotaFiscal
{
    [Key]
    public int Numero { get; set; }

    public StatusNota Status { get; set; } = StatusNota.Aberta;

    public List<NotaItem> Itens { get; set; } = new();
}
