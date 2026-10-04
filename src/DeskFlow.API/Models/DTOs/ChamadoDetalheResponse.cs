namespace DeskFlow.API.Models.DTOs;

public class ChamadoDetalheResponse : ChamadoResponse
{
    public CategoriaResponse? Categoria { get; set; }
    public List<InteracaoResponse> Interacoes { get; set; } = new();
}