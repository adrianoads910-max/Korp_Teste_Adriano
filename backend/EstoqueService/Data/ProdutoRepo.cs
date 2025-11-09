using EstoqueService.Models;

namespace EstoqueService.Data;

public class ProdutoRepo
{
    private readonly Dictionary<string, Produto> _produtos = new();

    public Produto Criar(Produto p)
    {
        _produtos[p.Codigo] = p;
        return p;
    }

    public Produto? Obter(string codigo)
    {
        _produtos.TryGetValue(codigo, out var produto);
        return produto;
    }

    public bool Reservar(string codigo, int quantidade)
    {
        if (!_produtos.TryGetValue(codigo, out var produto))
            return false;

        if (produto.Saldo < quantidade)
            return false;

        produto.Saldo -= quantidade;
        return true;
    }
}
