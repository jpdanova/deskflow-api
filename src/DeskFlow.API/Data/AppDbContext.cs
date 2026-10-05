using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Chamado> Chamados => Set<Chamado>();
    public DbSet<Interacao> Interacoes => Set<Interacao>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Chamado>()
            .HasOne(c => c.Categoria)
            .WithMany(c => c.Chamados)
            .HasForeignKey(c => c.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Interacao>()
            .HasOne(i => i.Chamado)
            .WithMany(c => c.Interacoes)
            .HasForeignKey(i => i.ChamadoId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Chamado>().Property(c => c.Prioridade).HasConversion<string>().HasMaxLength(20);
        mb.Entity<Chamado>().Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
    }
}