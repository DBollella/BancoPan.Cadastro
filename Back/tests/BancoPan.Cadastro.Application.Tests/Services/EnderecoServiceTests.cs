using BancoPan.Cadastro.Application.Interfaces;
using BancoPan.Cadastro.Application.Services;
using BancoPan.Cadastro.Application.Tests.Builders;
using BancoPan.Cadastro.Domain.Common;
using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Domain.Tests.Builders;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BancoPan.Cadastro.Application.Tests.Services;

[TestFixture]
public class EnderecoServiceTests
{
    private IUnitOfWork _unitOfWork;
    private ICacheService _cacheService;
    private IViaCepService _viaCepService;
    private IEnderecoRepository _repository;
    private EnderecoService _service;

    [SetUp]
    public void SetUp()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _cacheService = Substitute.For<ICacheService>();
        _viaCepService = Substitute.For<IViaCepService>();
        _repository = Substitute.For<IEnderecoRepository>();
        _unitOfWork.Enderecos.Returns(_repository);
        _service = new EnderecoService(_unitOfWork, _viaCepService, _cacheService);
    }

    [Test]
    public async Task Quando_ObterPorIdExisteNoCache_Devemos_RetornarDoCache()
    {
        var id = Guid.NewGuid();
        var enderecoDto = new Application.DTOs.EnderecoDto
        {
            Id = id,
            Cep = EnderecoBuilder.CepValido,
            Logradouro = "Rua Teste"
        };
        var cacheKey = $"enderecos:id:{id}";

        _cacheService.GetAsync<Application.DTOs.EnderecoDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.EnderecoDto?>(enderecoDto));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().BeEquivalentTo(enderecoDto);
        await _repository.DidNotReceive().ObterPorIdAsync(Arg.Any<Guid>());
    }

    [Test]
    public async Task Quando_ObterPorIdNaoExisteNoCache_Devemos_BuscarNoBancoDeDados()
    {
        var id = Guid.NewGuid();
        var endereco = EnderecoBuilder.Novo().Build();
        var cacheKey = $"enderecos:id:{id}";

        _cacheService.GetAsync<Application.DTOs.EnderecoDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.EnderecoDto?>(null));

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<Endereco?>(endereco));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().NotBeNull();
        resultado!.Logradouro.Should().Be(endereco.Logradouro);
        resultado.Localidade.Should().Be(endereco.Localidade);
        await _cacheService.Received(1).SetAsync(
            cacheKey,
            Arg.Any<Application.DTOs.EnderecoDto>(),
            Arg.Any<TimeSpan>());
    }

    [Test]
    public async Task Quando_CriarComDadosValidos_Devemos_CriarEndereco()
    {
        var dto = CriarEnderecoDtoBuilder.Novo().Build();

        _repository.AdicionarAsync(Arg.Any<Endereco>())
            .Returns(Task.CompletedTask);

        _unitOfWork.CommitAsync()
            .Returns(1);

        var resultado = await _service.CriarAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Logradouro.Should().Be(dto.Logradouro);
        resultado.Cep.Should().Be(dto.Cep);
        await _repository.Received(1).AdicionarAsync(Arg.Any<Endereco>());
        await _unitOfWork.Received(1).CommitAsync();
        await _cacheService.Received(1).RemoveByPrefixAsync("enderecos");
    }

    [Test]
    public async Task Quando_AtualizarEnderecoExistente_Devemos_Atualizar()
    {
        var id = Guid.NewGuid();
        var enderecoExistente = EnderecoBuilder.Novo().Build();
        var dtoAtualizado = new Application.DTOs.AtualizarEnderecoDto
        {
            Cep = "98765-432",
            Logradouro = "Avenida Nova",
            Numero = "456",
            Bairro = "Jardim",
            Localidade = "Rio de Janeiro",
            Uf = "RJ",
            Estado = "Rio de Janeiro",
            Regiao = "Sudeste",
            Ibge = "3304557",
            Ddd = "21"
        };

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<Endereco?>(enderecoExistente));

        _unitOfWork.CommitAsync()
            .Returns(1);

        var resultado = await _service.AtualizarAsync(id, dtoAtualizado);

        resultado.Should().NotBeNull();
        resultado!.Logradouro.Should().Be(dtoAtualizado.Logradouro);
        await _unitOfWork.Received(1).CommitAsync();
        await _cacheService.Received(1).RemoveByPrefixAsync("enderecos");
    }

    [Test]
    public async Task Quando_RemoverEnderecoExistente_Devemos_Remover()
    {
        var id = Guid.NewGuid();

        _repository.ExisteAsync(id)
            .Returns(Task.FromResult(true));

        _unitOfWork.CommitAsync()
            .Returns(1);

        var resultado = await _service.RemoverAsync(id);

        resultado.Should().BeTrue();
        await _repository.Received(1).RemoverAsync(id);
        await _unitOfWork.Received(1).CommitAsync();
        await _cacheService.Received(1).RemoveByPrefixAsync("enderecos");
    }

    [Test]
    public async Task Quando_ObterPaginado_Devemos_RetornarResultadoPaginado()
    {
        var parameters = new PaginationParameters { PageNumber = 1, PageSize = 10 };
        var enderecos = new List<Endereco>
        {
            EnderecoBuilder.Novo().ComLogradouro("Rua A").Build(),
            EnderecoBuilder.Novo().ComLogradouro("Rua B").Build()
        };

        var pagedResult = new PagedResult<Endereco>
        {
            Items = enderecos,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        var cacheKey = "enderecos:paginado:page1:size10";

        _cacheService.GetAsync<Application.DTOs.PagedResultDto<Application.DTOs.EnderecoDto>>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PagedResultDto<Application.DTOs.EnderecoDto>?>(null));

        _repository.ObterPaginadoAsync(parameters)
            .Returns(Task.FromResult(pagedResult));

        var resultado = await _service.ObterPaginadoAsync(parameters);

        resultado.Should().NotBeNull();
        resultado.Items.Should().HaveCount(2);
        resultado.TotalCount.Should().Be(2);
        await _cacheService.Received(1).SetAsync(
            cacheKey,
            Arg.Any<Application.DTOs.PagedResultDto<Application.DTOs.EnderecoDto>>(),
            Arg.Any<TimeSpan>());
    }
}
