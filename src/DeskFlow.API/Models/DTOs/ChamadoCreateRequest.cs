using System.ComponentModel.DataAnnotations;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Models.DTOs;

public class ChamadoCreateRequest
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome do solicitante é obrigatório.")]
    [MaxLength(120, ErrorMessage = "O nome do solicitante deve ter no máximo 120 caracteres.")]
    public string SolicitanteNome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A prioridade é obrigatória (Baixa, Media ou Alta).")]
    [EnumDataType(typeof(Prioridade), ErrorMessage = "Prioridade inválida. Use Baixa, Media ou Alta.")]
    public Prioridade? Prioridade { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe uma categoria válida.")]
    public int CategoriaId { get; set; }
}