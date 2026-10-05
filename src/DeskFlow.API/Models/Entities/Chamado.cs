using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.Entities;

public class Chamado
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Descricao { get; set; } = string.Empty;

    public Prioridade Prioridade { get; set; }
    public StatusChamado Status { get; set; } = StatusChamado.Aberto;

    [Required, MaxLength(120)]
    public string SolicitanteNome { get; set; } = string.Empty;

    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime? DataFechamento { get; set; }

    [MaxLength(2000)]
    public string? Solucao { get; set; }

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public List<Interacao> Interacoes { get; set; } = new();
}