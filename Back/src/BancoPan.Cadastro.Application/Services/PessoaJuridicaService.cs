using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Application.Interfaces;
using BancoPan.Cadastro.Domain.Common;
using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;

namespace BancoPan.Cadastro.Application.Services;

public class PessoaJuridicaService : IPessoaJuridicaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private const string CachePrefix = "pessoasjuridicas";
    private const string CacheKeyAll = $"{CachePrefix}:all";

    public PessoaJuridicaService(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<PessoaJuridicaDto?> ObterPorIdAsync(Guid id)
    {
        var cacheKey = $"{CachePrefix}:id:{id}";
        var cached = await _cacheService.GetAsync<PessoaJuridicaDto>(cacheKey);
        if (cached != null)
            return cached;

        var pessoa = await _unitOfWork.PessoasJuridicas.ObterPorIdAsync(id);
        if (pessoa == null)
            return null;

        var dto = MapearParaDto(pessoa);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

    public async Task<PessoaJuridicaDto?> ObterPorCnpjAsync(string cnpj)
    {
        var cacheKey = $"{CachePrefix}:cnpj:{cnpj}";
        var cached = await _cacheService.GetAsync<PessoaJuridicaDto>(cacheKey);
        if (cached != null)
            return cached;

        var pessoa = await _unitOfWork.PessoasJuridicas.ObterPorCnpjAsync(cnpj);
        if (pessoa == null)
            return null;

        var dto = MapearParaDto(pessoa);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

    public async Task<IEnumerable<PessoaJuridicaDto>> ObterTodosAsync()
    {
        var cached = await _cacheService.GetAsync<List<PessoaJuridicaDto>>(CacheKeyAll);
        if (cached != null)
            return cached;

        var pessoas = await _unitOfWork.PessoasJuridicas.ObterTodosAsync();
        var dtos = pessoas.Select(MapearParaDto).ToList();

        await _cacheService.SetAsync(CacheKeyAll, dtos, TimeSpan.FromMinutes(5));

        return dtos;
    }

    public async Task<PagedResultDto<PessoaJuridicaDto>> ObterPaginadoAsync(PaginationParameters parameters)
    {
        var cacheKey = $"{CachePrefix}:paginado:page{parameters.PageNumber}:size{parameters.PageSize}";
        var cached = await _cacheService.GetAsync<PagedResultDto<PessoaJuridicaDto>>(cacheKey);
        if (cached != null)
            return cached;

        var result = await _unitOfWork.PessoasJuridicas.ObterPaginadoAsync(parameters);

        var dto = new PagedResultDto<PessoaJuridicaDto>
        {
            Items = result.Items.Select(MapearParaDto),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            HasPreviousPage = result.HasPreviousPage,
            HasNextPage = result.HasNextPage
        };

        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

    public async Task<PessoaJuridicaDto> CriarAsync(CriarPessoaJuridicaDto dto)
    {
        if (await _unitOfWork.PessoasJuridicas.ExistePorCnpjAsync(dto.Cnpj))
            throw new InvalidOperationException("Já existe uma pessoa jurídica cadastrada com este CNPJ");

        if (!await _unitOfWork.Enderecos.ExisteAsync(dto.EnderecoId))
            throw new InvalidOperationException("Endereço não encontrado");

        var pessoa = new PessoaJuridica(
            dto.RazaoSocial,
            dto.Email,
            dto.Cnpj,
            dto.DataAbertura,
            dto.EnderecoId,
            dto.NomeFantasia,
            dto.Telefone,
            dto.InscricaoEstadual
        );

        await _unitOfWork.PessoasJuridicas.AdicionarAsync(pessoa);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return MapearParaDto(pessoa);
    }

    public async Task<PessoaJuridicaDto?> AtualizarAsync(Guid id, AtualizarPessoaJuridicaDto dto)
    {
        var pessoa = await _unitOfWork.PessoasJuridicas.ObterPorIdAsync(id);

        if (pessoa == null)
            return null;

        pessoa.Atualizar(
            dto.RazaoSocial,
            dto.Email,
            dto.DataAbertura,
            dto.NomeFantasia,
            dto.Telefone,
            dto.InscricaoEstadual
        );

        await _unitOfWork.PessoasJuridicas.AtualizarAsync(pessoa);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return MapearParaDto(pessoa);
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        if (!await _unitOfWork.PessoasJuridicas.ExisteAsync(id))
            return false;

        await _unitOfWork.PessoasJuridicas.RemoverAsync(id);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return true;
    }

    private static PessoaJuridicaDto MapearParaDto(PessoaJuridica pessoa)
    {
        return new PessoaJuridicaDto
        {
            Id = pessoa.Id,
            RazaoSocial = pessoa.RazaoSocial,
            NomeFantasia = pessoa.NomeFantasia,
            Email = pessoa.Email,
            Telefone = pessoa.Telefone,
            Cnpj = pessoa.ObterCnpjFormatado(),
            DataAbertura = pessoa.DataAbertura,
            InscricaoEstadual = pessoa.InscricaoEstadual,
            TempoAtuacao = pessoa.CalcularTempoAtuacao(),
            EnderecoId = pessoa.EnderecoId,
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
