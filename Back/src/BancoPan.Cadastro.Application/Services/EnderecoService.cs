using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Application.Interfaces;
using BancoPan.Cadastro.Domain.Common;
using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;

namespace BancoPan.Cadastro.Application.Services;

public class EnderecoService : IEnderecoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IViaCepService _viaCepService;
    private readonly ICacheService _cacheService;
    private const string CachePrefix = "enderecos";
    private const string CacheKeyAll = $"{CachePrefix}:all";

    public EnderecoService(
        IUnitOfWork unitOfWork,
        IViaCepService viaCepService,
        ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _viaCepService = viaCepService;
        _cacheService = cacheService;
    }

    public async Task<EnderecoDto?> ObterPorIdAsync(Guid id)
    {
        var cacheKey = $"{CachePrefix}:id:{id}";
        var cached = await _cacheService.GetAsync<EnderecoDto>(cacheKey);
        if (cached != null)
            return cached;

        var endereco = await _unitOfWork.Enderecos.ObterPorIdAsync(id);
        if (endereco == null)
            return null;

        var dto = MapearParaDto(endereco);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return dto;
    }

    public async Task<IEnumerable<EnderecoDto>> ObterTodosAsync()
    {
        var cached = await _cacheService.GetAsync<List<EnderecoDto>>(CacheKeyAll);
        if (cached != null)
            return cached;

        var enderecos = await _unitOfWork.Enderecos.ObterTodosAsync();
        var dtos = enderecos.Select(MapearParaDto).ToList();

        await _cacheService.SetAsync(CacheKeyAll, dtos, TimeSpan.FromMinutes(5));

        return dtos;
    }

    public async Task<PagedResultDto<EnderecoDto>> ObterPaginadoAsync(PaginationParameters parameters)
    {
        var cacheKey = $"{CachePrefix}:paginado:page{parameters.PageNumber}:size{parameters.PageSize}";
        var cached = await _cacheService.GetAsync<PagedResultDto<EnderecoDto>>(cacheKey);
        if (cached != null)
            return cached;

        var result = await _unitOfWork.Enderecos.ObterPaginadoAsync(parameters);

        var dto = new PagedResultDto<EnderecoDto>
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

    public async Task<EnderecoDto?> ConsultarViaCepAsync(string cep)
    {
        var viaCepResponse = await _viaCepService.ConsultarCepAsync(cep);

        if (viaCepResponse == null)
            return null;

        return new EnderecoDto
        {
            Cep = viaCepResponse.Cep,
            Logradouro = viaCepResponse.Logradouro,
            Bairro = viaCepResponse.Bairro,
            Localidade = viaCepResponse.Localidade,
            Uf = viaCepResponse.Uf,
            Estado = viaCepResponse.Estado,
            Regiao = viaCepResponse.Regiao,
            Ibge = viaCepResponse.Ibge,
            Ddd = viaCepResponse.Ddd
        };
    }

    public async Task<EnderecoDto> CriarAsync(CriarEnderecoDto dto)
    {
        var endereco = new Endereco(
            dto.Cep,
            dto.Logradouro,
            dto.Numero,
            dto.Bairro,
            dto.Localidade,
            dto.Uf,
            dto.Estado,
            dto.Regiao,
            dto.Ibge,
            dto.Ddd,
            dto.Complemento
        );

        await _unitOfWork.Enderecos.AdicionarAsync(endereco);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return MapearParaDto(endereco);
    }

    public async Task<EnderecoDto?> AtualizarAsync(Guid id, AtualizarEnderecoDto dto)
    {
        var endereco = await _unitOfWork.Enderecos.ObterPorIdAsync(id);

        if (endereco == null)
            return null;

        endereco.Atualizar(
            dto.Cep,
            dto.Logradouro,
            dto.Numero,
            dto.Bairro,
            dto.Localidade,
            dto.Uf,
            dto.Estado,
            dto.Regiao,
            dto.Ibge,
            dto.Ddd,
            dto.Complemento
        );

        await _unitOfWork.Enderecos.AtualizarAsync(endereco);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return MapearParaDto(endereco);
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        if (!await _unitOfWork.Enderecos.ExisteAsync(id))
            return false;

        await _unitOfWork.Enderecos.RemoverAsync(id);
        await _unitOfWork.CommitAsync();

        await _cacheService.RemoveByPrefixAsync(CachePrefix);

        return true;
    }

    private static EnderecoDto MapearParaDto(Endereco endereco)
    {
        return new EnderecoDto
        {
            Id = endereco.Id,
            Cep = endereco.ObterCepFormatado(),
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero,
            Complemento = endereco.Complemento,
            Bairro = endereco.Bairro,
            Localidade = endereco.Localidade,
            Uf = endereco.Uf,
            Estado = endereco.Estado,
            Regiao = endereco.Regiao,
            Ibge = endereco.Ibge,
            Ddd = endereco.Ddd
        };
    }
}