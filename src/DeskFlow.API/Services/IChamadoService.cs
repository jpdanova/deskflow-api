using DeskFlow.API.Models.DTOs;

namespace DeskFlow.API.Services;

public interface IChamadoService
{
    Task<ChamadoResponse> AbrirAsync(ChamadoCreateRequest request);
    Task<ChamadoDetalheResponse> ObterDetalhadoAsync(int id);
    Task<ChamadoResponse> IniciarAsync(int id);
}