using BancoPan.Cadastro.Domain.Tests.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace BancoPan.Cadastro.Domain.Tests.Entities;

[TestFixture]
public class PessoaFisicaTests
{
    [Test]
    public void Quando_CriarComDadosValidos_Devemos_ObterPessoaFisicaValida()
    {
        var enderecoId = Guid.NewGuid();
        var nome = "João da Silva";
        var dataNascimento = new DateTime(1990, 5, 15);
        var telefone = "(11) 98765-4321";
        var rg = "12.345.678-9";

        var pessoa = PessoaFisicaBuilder.Novo()
            .ComNome(nome)
            .ComEmail(PessoaFisicaBuilder.EmailValido)
            .ComCpf(PessoaFisicaBuilder.CpfValido)
            .ComDataNascimento(dataNascimento)
            .ComEnderecoId(enderecoId)
            .ComTelefone(telefone)
            .ComRg(rg)
            .Build();

        pessoa.Should().NotBeNull();
        pessoa.Nome.Should().Be(nome);
        pessoa.Email.Should().Be(PessoaFisicaBuilder.EmailValido);
        pessoa.Cpf.Should().Be(PessoaFisicaBuilder.CpfValido.Replace(".", "").Replace("-", ""));
        pessoa.DataNascimento.Should().Be(dataNascimento);
        pessoa.Telefone.Should().Be(telefone);
        pessoa.Rg.Should().Be(rg);
        pessoa.EnderecoId.Should().Be(enderecoId);
    }

    [Test]
    public void Quando_CriarSemTelefoneERg_Devemos_ObterPessoaFisicaSemDadosOpcionais()
    {
        var pessoa = PessoaFisicaBuilder.Novo().Build();

        pessoa.Telefone.Should().BeNull();
        pessoa.Rg.Should().BeNull();
    }

    [Test]
    public void Quando_CriarComCpfVazio_Devemos_LancarArgumentException()
    {
        var action = () => PessoaFisicaBuilder.Novo()
            .ComCpf("")
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CPF é obrigatório*");
    }

    [TestCase("123.456.789-00")]
    [TestCase("000.000.000-00")]
    public void Quando_CriarComCpfInvalido_Devemos_LancarArgumentException(string cpfInvalido)
    {
        var action = () => PessoaFisicaBuilder.Novo()
            .ComCpf(cpfInvalido)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CPF inválido*");
    }

    [Test]
    public void Quando_CriarComCpfInvalidoPadrao_Devemos_LancarArgumentException()
    {
        var action = () => PessoaFisicaBuilder.Novo()
            .ComCpf(PessoaFisicaBuilder.CpfInvalido)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CPF inválido*");
    }

    [Test]
    public void Quando_CriarComDataNascimentoFutura_Devemos_LancarArgumentException()
    {
        var dataFutura = DateTime.Today.AddDays(1);

        var action = () => PessoaFisicaBuilder.Novo()
            .ComDataNascimento(dataFutura)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Data de nascimento deve ser anterior à data atual*");
    }

    [Test]
    public void Quando_CriarComDataNascimentoMuitoAntiga_Devemos_LancarArgumentException()
    {
        var dataAntiga = new DateTime(1800, 1, 1);

        var action = () => PessoaFisicaBuilder.Novo()
            .ComDataNascimento(dataAntiga)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Data de nascimento inválida*");
    }

    [Test]
    public void Quando_AtualizarComDadosValidos_Devemos_ObterPessoaFisicaAtualizada()
    {
        var pessoa = PessoaFisicaBuilder.Novo().Build();

        var novoNome = "João Carlos da Silva";
        var novoEmail = "joao.carlos@example.com";
        var novaDataNascimento = new DateTime(1991, 6, 20);
        var novoTelefone = "(11) 99999-9999";
        var novoRg = "99.999.999-9";

        pessoa.Atualizar(novoNome, novoEmail, novaDataNascimento, novoTelefone, novoRg);

        pessoa.Nome.Should().Be(novoNome);
        pessoa.Email.Should().Be(novoEmail);
        pessoa.DataNascimento.Should().Be(novaDataNascimento);
        pessoa.Telefone.Should().Be(novoTelefone);
        pessoa.Rg.Should().Be(novoRg);
        pessoa.AtualizadoEm.Should().NotBeNull();
    }

    [Test]
    public void Quando_ObterCpfFormatado_Devemos_ObterCpfComPontosEHifen()
    {
        var pessoa = PessoaFisicaBuilder.Novo()
            .ComCpf(PessoaFisicaBuilder.CpfValido.Replace(".", "").Replace("-", ""))
            .Build();

        var cpfFormatado = pessoa.ObterCpfFormatado();

        cpfFormatado.Should().Be(PessoaFisicaBuilder.CpfValido);
    }

    [Test]
    public void Quando_CalcularIdade_Devemos_ObterIdadeCorreta()
    {
        var dataAtual = DateTime.Today;
        var dataNascimento = dataAtual.AddYears(-30).AddDays(-1);

        var pessoa = PessoaFisicaBuilder.Novo()
            .ComDataNascimento(dataNascimento)
            .Build();

        var idade = pessoa.CalcularIdade();

        idade.Should().Be(30);
    }

    [Test]
    public void Quando_CalcularIdadeAindaNaoFezAniversario_Devemos_ObterIdadeMenosUm()
    {
        var dataAtual = DateTime.Today;
        var dataNascimento = dataAtual.AddYears(-30).AddDays(1);

        var pessoa = PessoaFisicaBuilder.Novo()
            .ComDataNascimento(dataNascimento)
            .Build();

        var idade = pessoa.CalcularIdade();

        idade.Should().Be(29);
    }
}
