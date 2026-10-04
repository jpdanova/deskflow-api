using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados")]
public class ChamadosController : ControllerBase
{
    private readonly IChamadoService _service;

    public ChamadosController(IChamadoService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<ChamadoResponse>> Abrir(ChamadoCreateRequest request)
    {
        var criado = await _service.AbrirAsync(request);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChamadoDetalheResponse>> ObterPorId(int id) =>
        Ok(await _service.ObterDetalhadoAsync(id));

    [HttpPatch("{id:int}/iniciar")]
    public async Task<ActionResult<ChamadoResponse>> Iniciar(int id) =>
        Ok(await _service.IniciarAsync(id));

    [HttpPatch("{id:int}/encerrar")]
    public async Task<ActionResult<ChamadoResponse>> Encerrar(int id, ChamadoEncerrarRequest request) =>
        Ok(await _service.EncerrarAsync(id, request));
}