using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.DTOs;

public class InteracaoCreateRequest
{
    [Required(ErrorMessage = "O autor é obrigatório.")]
    [MaxLength(120, ErrorMessage = "O autor deve ter no máximo 120 caracteres.")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [MaxLength(2000, ErrorMessage = "A mensagem deve ter no máximo 2000 caracteres.")]
    public string Mensagem { get; set; } = string.Empty;
}