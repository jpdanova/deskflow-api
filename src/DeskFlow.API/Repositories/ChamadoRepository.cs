using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class ChamadoRepository : IChamadoRepository
{
    private readonly AppDbContext _context;

    public ChamadoRepository(AppDbContext context) => _context = context;

    public Task<Chamado?> ObterPorIdAsync(int id) =>
        _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);

    public Task<Chamado?> ObterDetalhadoAsync(int id) =>
        _context.Chamados
            .AsNoTracking()
            .Include(c => c.Categoria)
            .Include(c => c.Interacoes.OrderBy(i => i.DataRegistro))
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<Chamado>> ListarAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId)
    {
        var query = _context.Chamados.AsNoTracking().AsQueryable();

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        if (prioridade.HasValue)
            query = query.Where(c => c.Prioridade == prioridade.Value);

        if (categoriaId.HasValue)
            query = query.Where(c => c.CategoriaId == categoriaId.Value);

        return await query.OrderByDescending(c => c.DataAbertura).ToListAsync();
    }

    public async Task AdicionarAsync(Chamado chamado)
    {
        _context.Chamados.Add(chamado);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Chamado chamado)
    {
        _context.Chamados.Update(chamado);
        await _context.SaveChangesAsync();
    }

    public async Task AdicionarInteracaoAsync(Interacao interacao)
    {
        _context.Interacoes.Add(interacao);
        await _context.SaveChangesAsync();
    }
}