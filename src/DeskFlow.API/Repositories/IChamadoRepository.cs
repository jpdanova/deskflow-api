using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public interface IChamadoRepository
{
    Task<Chamado?> ObterPorIdAsync(int id);
    Task<Chamado?> ObterDetalhadoAsync(int id);
    Task<List<Chamado>> ListarAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId);
    Task AdicionarAsync(Chamado chamado);
    Task AtualizarAsync(Chamado chamado);
    Task AdicionarInteracaoAsync(Interacao interacao);
}