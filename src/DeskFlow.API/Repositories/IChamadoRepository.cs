using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public interface IChamadoRepository
{
    Task<Chamado?> ObterPorIdAsync(int id);
    Task<Chamado?> ObterDetalhadoAsync(int id);
    Task AdicionarAsync(Chamado chamado);
    Task AtualizarAsync(Chamado chamado);
}