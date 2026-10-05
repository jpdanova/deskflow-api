using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.Entities;

public class Categoria
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public List<Chamado> Chamados { get; set; } = new();
}