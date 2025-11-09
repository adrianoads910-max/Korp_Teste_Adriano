using EstoqueService.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Data;
public class EstoqueDb : DbContext
{
    public EstoqueDb(DbContextOptions<EstoqueDb> options) : base(options) {}
    public DbSet<Produto> Produtos => Set<Produto>();
}