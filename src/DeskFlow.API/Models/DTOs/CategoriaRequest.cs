using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.DTOs;

public class CategoriaRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;
}