using DeskFlow.API.Exceptions;
using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class ChamadoService : IChamadoService
{
    private readonly IChamadoRepository _repository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ChamadoService(IChamadoRepository repository, ICategoriaRepository categoriaRepository)
    {
        _repository = repository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ChamadoResponse> AbrirAsync(ChamadoCreateRequest request)
    {
        var titulo = request.Titulo.Trim();
        var descricao = request.Descricao.Trim();
        var solicitante = request.SolicitanteNome.Trim();

        if (titulo.Length == 0 || descricao.Length == 0 || solicitante.Length == 0)
            throw new BusinessException("Título, descrição e nome do solicitante são obrigatórios.");

        if (await _categoriaRepository.ObterPorIdAsync(request.CategoriaId) is null)
            throw new BusinessException($"A categoria {request.CategoriaId} não existe.");

        var chamado = new Chamado
        {
            Titulo = titulo,
            Descricao = descricao,
            SolicitanteNome = solicitante,
            Prioridade = request.Prioridade!.Value,
            CategoriaId = request.CategoriaId,
            Status = StatusChamado.Aberto,
            DataAbertura = DateTime.UtcNow
        };

        await _repository.AdicionarAsync(chamado);
        return Mapear(chamado);
    }

    public async Task<ChamadoDetalheResponse> ObterDetalhadoAsync(int id)
    {
        var chamado = await _repository.ObterDetalhadoAsync(id)
            ?? throw new NotFoundException($"Chamado {id} não encontrado.");

        return new ChamadoDetalheResponse
        {
            Id = chamado.Id,
            Titulo = chamado.Titulo,
            Descricao = chamado.Descricao,
            Prioridade = chamado.Prioridade,
            Status = chamado.Status,
            SolicitanteNome = chamado.SolicitanteNome,
            DataAbertura = chamado.DataAbertura,
            DataFechamento = chamado.DataFechamento,
            Solucao = chamado.Solucao,
            CategoriaId = chamado.CategoriaId,
            Categoria = chamado.Categoria is null
                ? null
                : new CategoriaResponse { Id = chamado.Categoria.Id, Nome = chamado.Categoria.Nome },
            Interacoes = chamado.Interacoes.Select(i => new InteracaoResponse
            {
                Id = i.Id,
                Autor = i.Autor,
                Mensagem = i.Mensagem,
                DataRegistro = i.DataRegistro
            }).ToList()
        };
    }

    private static ChamadoResponse Mapear(Chamado c) => new()
    {
        Id = c.Id,
        Titulo = c.Titulo,
        Descricao = c.Descricao,
        Prioridade = c.Prioridade,
        Status = c.Status,
        SolicitanteNome = c.SolicitanteNome,
        DataAbertura = c.DataAbertura,
        DataFechamento = c.DataFechamento,
        Solucao = c.Solucao,
        CategoriaId = c.CategoriaId
    };
}