using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.Entities;

public class Interacao
{
    public int Id { get; set; }
    public int ChamadoId { get; set; }
    public Chamado? Chamado { get; set; }

    [Required, MaxLength(120)]
    public string Autor { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Mensagem { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
}