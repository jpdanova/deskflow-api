using DeskFlow.API.Exceptions;
using DeskFlow.API.Models.DTOs;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository) => _repository = repository;

    public async Task<List<CategoriaResponse>> ListarAsync()
    {
        var categorias = await _repository.ListarAsync();
        return categorias.Select(Mapear).ToList();
    }

    public async Task<CategoriaResponse> ObterPorIdAsync(int id)
    {
        var categoria = await _repository.ObterPorIdAsync(id)
            ?? throw new NotFoundException($"Categoria {id} não encontrada.");
        return Mapear(categoria);
    }

    public async Task<CategoriaResponse> CriarAsync(CategoriaRequest request)
    {
        var nome = request.Nome.Trim();
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("O nome da categoria é obrigatório.");

        if (await _repository.ExisteNomeAsync(nome))
            throw new BusinessException($"Já existe uma categoria com o nome '{nome}'.");

        var categoria = new Categoria { Nome = nome };
        await _repository.AdicionarAsync(categoria);
        return Mapear(categoria);
    }

    public async Task<CategoriaResponse> AtualizarAsync(int id, CategoriaRequest request)
    {
        var categoria = await _repository.ObterPorIdAsync(id)
            ?? throw new NotFoundException($"Categoria {id} não encontrada.");

        var nome = request.Nome.Trim();
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("O nome da categoria é obrigatório.");

        if (await _repository.ExisteNomeAsync(nome, id))
            throw new BusinessException($"Já existe uma categoria com o nome '{nome}'.");

        categoria.Nome = nome;
        await _repository.AtualizarAsync(categoria);
        return Mapear(categoria);
    }

    public async Task RemoverAsync(int id)
    {
        var categoria = await _repository.ObterPorIdAsync(id)
            ?? throw new NotFoundException($"Categoria {id} não encontrada.");

        if (await _repository.PossuiChamadosAsync(id))
            throw new BusinessException("Não é possível excluir a categoria porque existem chamados associados a ela.");

        await _repository.RemoverAsync(categoria);
    }

    private static CategoriaResponse Mapear(Categoria c) => new() { Id = c.Id, Nome = c.Nome };
}