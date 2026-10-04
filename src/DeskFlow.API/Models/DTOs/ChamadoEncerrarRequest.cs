using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.DTOs;

public class ChamadoEncerrarRequest
{
    [Required(ErrorMessage = "A solução é obrigatória para encerrar o chamado.")]
    [MaxLength(2000, ErrorMessage = "A solução deve ter no máximo 2000 caracteres.")]
    public string Solucao { get; set; } = string.Empty;
}