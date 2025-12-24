using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Application.Interfaces;
using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;

namespace BancoPan.Cadastro.Application.Services;

public class PessoaFisicaService : IPessoaFisicaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private const string CachePrefix = "pessoasfisicas";
    private const string CacheKeyAll = $"{CachePrefix}:all";

    public PessoaFisicaService(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<PessoaFisicaDto?> ObterPorIdAsync(Guid id)
    {
        var cacheKey = $"{CachePrefix}:id:{id}";
        var cached = await _cacheService.GetAsync<PessoaFisicaDto>(cacheKey);
        if (cached != null)
            return cached;

        var pessoa = await _unitOfWork.PessoasFisicas.ObterPorIdAsync(id);
        if (pessoa == null)
            return null;

        var dto = MapearParaDto(pessoa);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

    public async Task<PessoaFisicaDto?> ObterPorCpfAsync(string cpf)
    {
        var cacheKey = $"{CachePrefix}:cpf:{cpf}";
        var cached = await _cacheService.GetAsync<PessoaFisicaDto>(cacheKey);
        if (cached != null)
            return cached;

        var pessoa = await _unitOfWork.PessoasFisicas.ObterPorCpfAsync(cpf);
        if (pessoa == null)
            return null;

        var dto = MapearParaDto(pessoa);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

    public async Task<IEnumerable<PessoaFisicaDto>> ObterTodosAsync()
    {
        var cached = await _cacheService.GetAsync<List<PessoaFisicaDto>>(CacheKeyAll);
        if (cached != null)
            return cached;

        var pessoas = await _unitOfWork.PessoasFisicas.ObterTodosAsync();
        var dtos = pessoas.Select(MapearParaDto).ToList();

        await _cacheService.SetAsync(CacheKeyAll, dtos, TimeSpan.FromMinutes(5));

        return dtos;
    }

    public async Task<PessoaFisicaDto> CriarAsync(CriarPessoaFisicaDto dto)
    {
        if (await _unitOfWork.PessoasFisicas.ExistePorCpfAsync(dto.Cpf))
            throw new InvalidOperationException("Já existe uma pessoa física cadastrada com este CPF");

        if (!await _unitOfWork.Enderecos.ExisteAsync(dto.EnderecoId))
            throw new InvalidOperationException("Endereço não encontrado");

        var pessoa = new PessoaFisica(
            dto.Nome,
            dto.Email,
            dto.Cpf,
            dto.DataNascimento,
            dto.EnderecoId,
            dto.Telefone,
            dto.Rg
        );

        await _unitOfWork.PessoasFisicas.AdicionarAsync(pessoa);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return MapearParaDto(pessoa);
    }

    public async Task<PessoaFisicaDto?> AtualizarAsync(Guid id, AtualizarPessoaFisicaDto dto)
    {
        var pessoa = await _unitOfWork.PessoasFisicas.ObterPorIdAsync(id);

        if (pessoa == null)
            return null;

        pessoa.Atualizar(
            dto.Nome,
            dto.Email,
            dto.DataNascimento,
            dto.Telefone,
            dto.Rg
        );

        await _unitOfWork.PessoasFisicas.AtualizarAsync(pessoa);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return MapearParaDto(pessoa);
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        if (!await _unitOfWork.PessoasFisicas.ExisteAsync(id))
            return false;

        await _unitOfWork.PessoasFisicas.RemoverAsync(id);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return true;
    }

    private static PessoaFisicaDto MapearParaDto(PessoaFisica pessoa)
    {
        return new PessoaFisicaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            Telefone = pessoa.Telefone,
            Cpf = pessoa.ObterCpfFormatado(),
            DataNascimento = pessoa.DataNascimento,
            Rg = pessoa.Rg,
            Idade = pessoa.CalcularIdade(),
            Endereco = pessoa.Endereco != null ? new EnderecoDto
            {
                Id = pessoa.Endereco.Id,
                Cep = pessoa.Endereco.ObterCepFormatado(),
                Logradouro = pessoa.Endereco.Logradouro,
                Numero = pessoa.Endereco.Numero,
                Complemento = pessoa.Endereco.Complemento,
                Bairro = pessoa.Endereco.Bairro,
                Localidade = pessoa.Endereco.Localidade,
                Uf = pessoa.Endereco.Uf,
                Estado = pessoa.Endereco.Estado,
                Regiao = pessoa.Endereco.Regiao,
                Ibge = pessoa.Endereco.Ibge,
                Ddd = pessoa.Endereco.Ddd
            } : null,
            CriadoEm = pessoa.CriadoEm,
            AtualizadoEm = pessoa.AtualizadoEm
        };
    }
}
