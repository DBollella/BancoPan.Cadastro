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
public class PessoaJuridicaServiceTests
{
    private IUnitOfWork _unitOfWork;
    private ICacheService _cacheService;
    private IPessoaJuridicaRepository _repository;
    private IEnderecoRepository _enderecoRepository;
    private PessoaJuridicaService _service;

    [SetUp]
    public void SetUp()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _cacheService = Substitute.For<ICacheService>();
        _repository = Substitute.For<IPessoaJuridicaRepository>();
        _enderecoRepository = Substitute.For<IEnderecoRepository>();
        _unitOfWork.PessoasJuridicas.Returns(_repository);
        _unitOfWork.Enderecos.Returns(_enderecoRepository);
        _service = new PessoaJuridicaService(_unitOfWork, _cacheService);
    }

    [Test]
    public async Task Quando_ObterPorIdExisteNoCache_Devemos_RetornarDoCache()
    {
        var id = Guid.NewGuid();
        var pessoaDto = new Application.DTOs.PessoaJuridicaDto
        {
            Id = id,
            RazaoSocial = "Empresa Teste Ltda",
            Cnpj = CriarPessoaJuridicaDtoBuilder.CnpjValido
        };
        var cacheKey = $"pessoasjuridicas:id:{id}";

        _cacheService.GetAsync<Application.DTOs.PessoaJuridicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaJuridicaDto?>(pessoaDto));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().BeEquivalentTo(pessoaDto);
        await _repository.DidNotReceive().ObterPorIdAsync(Arg.Any<Guid>());
    }

    [Test]
    public async Task Quando_ObterPorIdNaoExisteNoCache_Devemos_BuscarNoBancoDeDados()
    {
        var id = Guid.NewGuid();
        var pessoa = PessoaJuridicaBuilder.Novo().Build();
        var cacheKey = $"pessoasjuridicas:id:{id}";

        _cacheService.GetAsync<Application.DTOs.PessoaJuridicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaJuridicaDto?>(null));

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaJuridica?>(pessoa));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().NotBeNull();
        resultado!.RazaoSocial.Should().Be(pessoa.RazaoSocial);
        resultado.Email.Should().Be(pessoa.Email);
        await _cacheService.Received(1).SetAsync(
            cacheKey,
            Arg.Any<Application.DTOs.PessoaJuridicaDto>(),
            Arg.Any<TimeSpan>());
    }

    [Test]
    public async Task Quando_ObterPorIdNaoExiste_Devemos_RetornarNull()
    {
        var id = Guid.NewGuid();
        var cacheKey = $"pessoasjuridicas:id:{id}";

        _cacheService.GetAsync<Application.DTOs.PessoaJuridicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaJuridicaDto?>(null));

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaJuridica?>(null));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().BeNull();
        await _cacheService.DidNotReceive().SetAsync(
            Arg.Any<string>(),
            Arg.Any<Application.DTOs.PessoaJuridicaDto>(),
            Arg.Any<TimeSpan>());
    }

    [Test]
    public async Task Quando_ObterPorCnpjExiste_Devemos_RetornarPessoa()
    {
        var cnpj = CriarPessoaJuridicaDtoBuilder.CnpjValido;
        var pessoa = PessoaJuridicaBuilder.Novo().ComCnpj(cnpj).Build();
        var cacheKey = $"pessoasjuridicas:cnpj:{cnpj}";

        _cacheService.GetAsync<Application.DTOs.PessoaJuridicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaJuridicaDto?>(null));

        _repository.ObterPorCnpjAsync(cnpj)
            .Returns(Task.FromResult<PessoaJuridica?>(pessoa));

        var resultado = await _service.ObterPorCnpjAsync(cnpj);

        resultado.Should().NotBeNull();
        resultado!.Cnpj.Should().Be(cnpj);
        await _cacheService.Received(1).SetAsync(
            cacheKey,
            Arg.Any<Application.DTOs.PessoaJuridicaDto>(),
            Arg.Any<TimeSpan>());
    }

    [Test]
    public async Task Quando_CriarComDadosValidos_Devemos_CriarPessoa()
    {
        var dto = CriarPessoaJuridicaDtoBuilder.Novo().Build();

        _repository.ExistePorCnpjAsync(dto.Cnpj)
            .Returns(Task.FromResult(false));

        _enderecoRepository.ExisteAsync(dto.EnderecoId)
            .Returns(Task.FromResult(true));

        _repository.AdicionarAsync(Arg.Any<PessoaJuridica>())
            .Returns(Task.CompletedTask);

        _unitOfWork.CommitAsync()
            .Returns(1);

        var resultado = await _service.CriarAsync(dto);

        resultado.Should().NotBeNull();
        resultado.RazaoSocial.Should().Be(dto.RazaoSocial);
        resultado.Cnpj.Should().Be(dto.Cnpj);
        await _repository.Received(1).AdicionarAsync(Arg.Any<PessoaJuridica>());
        await _unitOfWork.Received(1).CommitAsync();
        await _cacheService.Received(1).RemoveByPrefixAsync("pessoasjuridicas");
    }

    [Test]
    public async Task Quando_CriarComCnpjDuplicado_Devemos_LancarExcecao()
    {
        var dto = CriarPessoaJuridicaDtoBuilder.Novo().Build();

        _repository.ExistePorCnpjAsync(dto.Cnpj)
            .Returns(Task.FromResult(true));

        var action = async () => await _service.CriarAsync(dto);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*CNPJ*");
    }

    [Test]
    public async Task Quando_AtualizarPessoaExistente_Devemos_Atualizar()
    {
        var id = Guid.NewGuid();
        var pessoaExistente = PessoaJuridicaBuilder.Novo().Build();
        var dtoAtualizado = new Application.DTOs.AtualizarPessoaJuridicaDto
        {
            RazaoSocial = "Nova Empresa Teste Ltda",
            Email = "novo@empresa.com",
            DataAbertura = new DateTime(2019, 5, 10),
            NomeFantasia = "Novo Nome Fantasia",
            Telefone = "(11) 9999-8888",
            InscricaoEstadual = "999.999.999.999"
        };

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaJuridica?>(pessoaExistente));

        _unitOfWork.CommitAsync()
            .Returns(1);

        var resultado = await _service.AtualizarAsync(id, dtoAtualizado);

        resultado.Should().NotBeNull();
        resultado!.RazaoSocial.Should().Be(dtoAtualizado.RazaoSocial);
        await _unitOfWork.Received(1).CommitAsync();
        await _cacheService.Received(1).RemoveByPrefixAsync("pessoasjuridicas");
    }

    [Test]
    public async Task Quando_AtualizarPessoaNaoExistente_Devemos_RetornarNull()
    {
        var id = Guid.NewGuid();
        var dto = new Application.DTOs.AtualizarPessoaJuridicaDto
        {
            RazaoSocial = "Empresa Teste",
            Email = "empresa@example.com",
            DataAbertura = DateTime.Today.AddYears(-5)
        };

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaJuridica?>(null));

        var resultado = await _service.AtualizarAsync(id, dto);

        resultado.Should().BeNull();
        await _unitOfWork.DidNotReceive().CommitAsync();
        await _cacheService.DidNotReceive().RemoveByPrefixAsync(Arg.Any<string>());
    }

    [Test]
    public async Task Quando_RemoverPessoaExistente_Devemos_Remover()
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
        await _cacheService.Received(1).RemoveByPrefixAsync("pessoasjuridicas");
    }

    [Test]
    public async Task Quando_RemoverPessoaNaoExistente_Devemos_RetornarFalse()
    {
        var id = Guid.NewGuid();

        _repository.ExisteAsync(id)
            .Returns(Task.FromResult(false));

        var resultado = await _service.RemoverAsync(id);

        resultado.Should().BeFalse();
        await _repository.DidNotReceive().RemoverAsync(Arg.Any<Guid>());
        await _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Test]
    public async Task Quando_ObterPaginado_Devemos_RetornarResultadoPaginado()
    {
        var parameters = new PaginationParameters { PageNumber = 1, PageSize = 10 };
        var pessoas = new List<PessoaJuridica>
        {
            PessoaJuridicaBuilder.Novo().ComRazaoSocial("Empresa A Ltda").Build(),
            PessoaJuridicaBuilder.Novo().ComRazaoSocial("Empresa B S/A").Build()
        };

        var pagedResult = new PagedResult<PessoaJuridica>
        {
            Items = pessoas,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        var cacheKey = "pessoasjuridicas:paginado:page1:size10";

        _cacheService.GetAsync<Application.DTOs.PagedResultDto<Application.DTOs.PessoaJuridicaDto>>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PagedResultDto<Application.DTOs.PessoaJuridicaDto>?>(null));

        _repository.ObterPaginadoAsync(parameters)
            .Returns(Task.FromResult(pagedResult));

        var resultado = await _service.ObterPaginadoAsync(parameters);

        resultado.Should().NotBeNull();
        resultado.Items.Should().HaveCount(2);
        resultado.TotalCount.Should().Be(2);
        resultado.PageNumber.Should().Be(1);
        resultado.PageSize.Should().Be(10);
        await _cacheService.Received(1).SetAsync(
            cacheKey,
            Arg.Any<Application.DTOs.PagedResultDto<Application.DTOs.PessoaJuridicaDto>>(),
            Arg.Any<TimeSpan>());
    }
}
