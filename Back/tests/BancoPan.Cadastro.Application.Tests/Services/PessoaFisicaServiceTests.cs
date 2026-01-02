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
public class PessoaFisicaServiceTests
{
    private IUnitOfWork _unitOfWork;
    private ICacheService _cacheService;
    private IPessoaFisicaRepository _repository;
    private IEnderecoRepository _enderecoRepository;
    private PessoaFisicaService _service;

    [SetUp]
    public void SetUp()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _cacheService = Substitute.For<ICacheService>();
        _repository = Substitute.For<IPessoaFisicaRepository>();
        _enderecoRepository = Substitute.For<IEnderecoRepository>();
        _unitOfWork.PessoasFisicas.Returns(_repository);
        _unitOfWork.Enderecos.Returns(_enderecoRepository);
        _service = new PessoaFisicaService(_unitOfWork, _cacheService);
    }

    [Test]
    public async Task Quando_ObterPorIdExisteNoCache_Devemos_RetornarDoCache()
    {
        var id = Guid.NewGuid();
        var pessoaDto = new Application.DTOs.PessoaFisicaDto
        {
            Id = id,
            Nome = "João da Silva",
            Cpf = PessoaFisicaBuilder.CpfValido
        };
        var cacheKey = $"pessoasfisicas:id:{id}";

        _cacheService.GetAsync<Application.DTOs.PessoaFisicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaFisicaDto?>(pessoaDto));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().BeEquivalentTo(pessoaDto);
        await _repository.DidNotReceive().ObterPorIdAsync(Arg.Any<Guid>());
    }

    [Test]
    public async Task Quando_ObterPorIdNaoExisteNoCache_Devemos_BuscarNoBancoDeDados()
    {
        var id = Guid.NewGuid();
        var pessoa = PessoaFisicaBuilder.Novo().Build();
        var cacheKey = $"pessoasfisicas:id:{id}";

        _cacheService.GetAsync<Application.DTOs.PessoaFisicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaFisicaDto?>(null));

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaFisica?>(pessoa));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be(pessoa.Nome);
        resultado.Email.Should().Be(pessoa.Email);
        await _cacheService.Received(1).SetAsync(
            cacheKey,
            Arg.Any<Application.DTOs.PessoaFisicaDto>(),
            Arg.Any<TimeSpan>());
    }

    [Test]
    public async Task Quando_ObterPorIdNaoExiste_Devemos_RetornarNull()
    {
        var id = Guid.NewGuid();
        var cacheKey = $"pessoasfisicas:id:{id}";

        _cacheService.GetAsync<Application.DTOs.PessoaFisicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaFisicaDto?>(null));

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaFisica?>(null));

        var resultado = await _service.ObterPorIdAsync(id);

        resultado.Should().BeNull();
        await _cacheService.DidNotReceive().SetAsync(
            Arg.Any<string>(),
            Arg.Any<Application.DTOs.PessoaFisicaDto>(),
            Arg.Any<TimeSpan>());
    }

    [Test]
    public async Task Quando_ObterPorCpfExiste_Devemos_RetornarPessoa()
    {
        var cpf = PessoaFisicaBuilder.CpfValido;
        var pessoa = PessoaFisicaBuilder.Novo().ComCpf(cpf).Build();
        var cacheKey = $"pessoasfisicas:cpf:{cpf}";

        _cacheService.GetAsync<Application.DTOs.PessoaFisicaDto>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PessoaFisicaDto?>(null));

        _repository.ObterPorCpfAsync(cpf)
            .Returns(Task.FromResult<PessoaFisica?>(pessoa));

        var resultado = await _service.ObterPorCpfAsync(cpf);

        resultado.Should().NotBeNull();
        resultado!.Cpf.Should().Be(cpf);
        await _cacheService.Received(1).SetAsync(
            cacheKey,
            Arg.Any<Application.DTOs.PessoaFisicaDto>(),
            Arg.Any<TimeSpan>());
    }

    [Test]
    public async Task Quando_CriarComDadosValidos_Devemos_CriarPessoa()
    {
        var dto = CriarPessoaFisicaDtoBuilder.Novo().Build();

        _repository.ExistePorCpfAsync(dto.Cpf)
            .Returns(Task.FromResult(false));

        _enderecoRepository.ExisteAsync(dto.EnderecoId)
            .Returns(Task.FromResult(true));

        _repository.AdicionarAsync(Arg.Any<PessoaFisica>())
            .Returns(Task.CompletedTask);

        _unitOfWork.CommitAsync()
            .Returns(1);

        var resultado = await _service.CriarAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be(dto.Nome);
        resultado.Cpf.Should().Be(dto.Cpf);
        await _repository.Received(1).AdicionarAsync(Arg.Any<PessoaFisica>());
        await _unitOfWork.Received(1).CommitAsync();
        await _cacheService.Received(1).RemoveByPrefixAsync("pessoasfisicas");
    }

    [Test]
    public async Task Quando_CriarComCpfDuplicado_Devemos_LancarExcecao()
    {
        var dto = CriarPessoaFisicaDtoBuilder.Novo().Build();

        _repository.ExistePorCpfAsync(dto.Cpf)
            .Returns(Task.FromResult(true));

        var action = async () => await _service.CriarAsync(dto);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*CPF*");
    }

    [Test]
    public async Task Quando_AtualizarPessoaExistente_Devemos_Atualizar()
    {
        var id = Guid.NewGuid();
        var pessoaExistente = PessoaFisicaBuilder.Novo().Build();
        var dtoAtualizado = new Application.DTOs.AtualizarPessoaFisicaDto
        {
            Nome = "João Carlos da Silva",
            Email = "joao.carlos@example.com",
            DataNascimento = new DateTime(1991, 6, 20),
            Telefone = "(11) 99999-9999",
            Rg = "99.999.999-9"
        };

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaFisica?>(pessoaExistente));

        _unitOfWork.CommitAsync()
            .Returns(1);

        var resultado = await _service.AtualizarAsync(id, dtoAtualizado);

        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be(dtoAtualizado.Nome);
        await _unitOfWork.Received(1).CommitAsync();
        await _cacheService.Received(1).RemoveByPrefixAsync("pessoasfisicas");
    }

    [Test]
    public async Task Quando_AtualizarPessoaNaoExistente_Devemos_RetornarNull()
    {
        var id = Guid.NewGuid();
        var dto = new Application.DTOs.AtualizarPessoaFisicaDto
        {
            Nome = "João Carlos",
            Email = "joao@example.com",
            DataNascimento = DateTime.Today.AddYears(-30)
        };

        _repository.ObterPorIdAsync(id)
            .Returns(Task.FromResult<PessoaFisica?>(null));

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
        await _cacheService.Received(1).RemoveByPrefixAsync("pessoasfisicas");
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
        var pessoas = new List<PessoaFisica>
        {
            PessoaFisicaBuilder.Novo().ComNome("João da Silva").Build(),
            PessoaFisicaBuilder.Novo().ComNome("Maria Silva").Build()
        };

        var pagedResult = new PagedResult<PessoaFisica>
        {
            Items = pessoas,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        var cacheKey = "pessoasfisicas:paginado:page1:size10";

        _cacheService.GetAsync<Application.DTOs.PagedResultDto<Application.DTOs.PessoaFisicaDto>>(cacheKey)
            .Returns(Task.FromResult<Application.DTOs.PagedResultDto<Application.DTOs.PessoaFisicaDto>?>(null));

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
            Arg.Any<Application.DTOs.PagedResultDto<Application.DTOs.PessoaFisicaDto>>(),
            Arg.Any<TimeSpan>());
    }
}
