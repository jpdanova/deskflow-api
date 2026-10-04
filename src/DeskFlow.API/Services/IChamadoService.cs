using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Services;

public interface IChamadoService
{
    Task<ChamadoResponse> AbrirAsync(ChamadoCreateRequest request);
    Task<ChamadoDetalheResponse> ObterDetalhadoAsync(int id);
    Task<List<ChamadoResponse>> ListarAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId);
    Task<ChamadoResponse> IniciarAsync(int id);
    Task<ChamadoResponse> EncerrarAsync(int id, ChamadoEncerrarRequest request);
    Task<InteracaoResponse> AdicionarInteracaoAsync(int chamadoId, InteracaoCreateRequest request);
}