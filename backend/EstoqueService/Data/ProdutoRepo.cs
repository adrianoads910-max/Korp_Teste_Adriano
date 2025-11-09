using EstoqueService.Models;
using System.Collections.Generic;

namespace EstoqueService.Data
{
    public class ProdutoRepo
    {
        private readonly Dictionary<string, Produto> _produtos = new();

        public void Criar(Produto produto)
        {
            _produtos[produto.Codigo] = produto;
        }

        public Produto? Obter(string codigo)
        {
            return _produtos.ContainsKey(codigo) ? _produtos[codigo] : null;
        }

        public bool Reservar(string codigo, int quantidade)
        {
            if (!_produtos.ContainsKey(codigo))
                return false;

            var produto = _produtos[codigo];

            if (produto.Saldo < quantidade)
                return false;

            produto.Saldo -= quantidade;
            return true;
        }

        // ✅ MÉTODO QUE ESTAVA FALTANDO
        public List<Produto> Listar()
        {
            return _produtos.Values.ToList();
        }
    }
}
