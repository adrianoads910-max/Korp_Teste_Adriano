namespace EstoqueService.Models;

public class Produto
{
    public string Codigo { get; set; } = default!;
    public string Descricao { get; set; } = default!;
    public int Saldo { get; set; }
}
