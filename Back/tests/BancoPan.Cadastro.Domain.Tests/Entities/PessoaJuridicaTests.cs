using BancoPan.Cadastro.Domain.Tests.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace BancoPan.Cadastro.Domain.Tests.Entities;

[TestFixture]
public class PessoaJuridicaTests
{
    [Test]
    public void Quando_CriarComDadosValidos_Devemos_ObterPessoaJuridicaValida()
    {
        var enderecoId = Guid.NewGuid();
        var razaoSocial = "Empresa Teste Ltda";
        var dataAbertura = new DateTime(2020, 1, 15);
        var nomeFantasia = "Empresa Teste";
        var telefone = "(11) 3333-4444";
        var inscricaoEstadual = "123.456.789.012";

        var pessoa = PessoaJuridicaBuilder.Novo()
            .ComRazaoSocial(razaoSocial)
            .ComEmail(PessoaJuridicaBuilder.EmailValido)
            .ComCnpj(PessoaJuridicaBuilder.CnpjValido)
            .ComDataAbertura(dataAbertura)
            .ComEnderecoId(enderecoId)
            .ComNomeFantasia(nomeFantasia)
            .ComTelefone(telefone)
            .ComInscricaoEstadual(inscricaoEstadual)
            .Build();

        pessoa.Should().NotBeNull();
        pessoa.RazaoSocial.Should().Be(razaoSocial);
        pessoa.Email.Should().Be(PessoaJuridicaBuilder.EmailValido);
        pessoa.Cnpj.Should().Be(PessoaJuridicaBuilder.CnpjValido.Replace(".", "").Replace("/", "").Replace("-", ""));
        pessoa.DataAbertura.Should().Be(dataAbertura);
        pessoa.NomeFantasia.Should().Be(nomeFantasia);
        pessoa.Telefone.Should().Be(telefone);
        pessoa.InscricaoEstadual.Should().Be(inscricaoEstadual);
        pessoa.EnderecoId.Should().Be(enderecoId);
    }

    [Test]
    public void Quando_CriarSemDadosOpcionais_Devemos_ObterPessoaJuridicaSemDadosOpcionais()
    {
        var pessoa = PessoaJuridicaBuilder.Novo().Build();

        pessoa.NomeFantasia.Should().BeNull();
        pessoa.Telefone.Should().BeNull();
        pessoa.InscricaoEstadual.Should().BeNull();
    }

    [Test]
    public void Quando_CriarComCnpjVazio_Devemos_LancarArgumentException()
    {
        var action = () => PessoaJuridicaBuilder.Novo()
            .ComCnpj("")
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CNPJ é obrigatório*");
    }

    [TestCase("11.222.333/0001-00")]
    [TestCase("00.000.000/0000-00")]
    public void Quando_CriarComCnpjInvalido_Devemos_LancarArgumentException(string cnpjInvalido)
    {
        var action = () => PessoaJuridicaBuilder.Novo()
            .ComCnpj(cnpjInvalido)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CNPJ inválido*");
    }

    [Test]
    public void Quando_CriarComCnpjInvalidoPadrao_Devemos_LancarArgumentException()
    {
        var action = () => PessoaJuridicaBuilder.Novo()
            .ComCnpj(PessoaJuridicaBuilder.CnpjInvalido)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CNPJ inválido*");
    }

    [Test]
    public void Quando_CriarComDataAberturaFutura_Devemos_LancarArgumentException()
    {
        var dataFutura = DateTime.Today.AddDays(1);

        var action = () => PessoaJuridicaBuilder.Novo()
            .ComDataAbertura(dataFutura)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Data de abertura deve ser anterior à data atual*");
    }

    [Test]
    public void Quando_CriarComDataAberturaMuitoAntiga_Devemos_LancarArgumentException()
    {
        var dataAntiga = new DateTime(1700, 1, 1);

        var action = () => PessoaJuridicaBuilder.Novo()
            .ComDataAbertura(dataAntiga)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Data de abertura inválida*");
    }

    [Test]
    public void Quando_CriarComRazaoSocialVazia_Devemos_LancarArgumentException()
    {
        var action = () => PessoaJuridicaBuilder.Novo()
            .ComRazaoSocial("")
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Nome é obrigatório*");
    }

    [Test]
    public void Quando_CriarComRazaoSocialMuitoCurta_Devemos_LancarArgumentException()
    {
        var action = () => PessoaJuridicaBuilder.Novo()
            .ComRazaoSocial("AB")
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Nome deve ter no mínimo 3 caracteres*");
    }

    [Test]
    public void Quando_AtualizarComDadosValidos_Devemos_ObterPessoaJuridicaAtualizada()
    {
        var pessoa = PessoaJuridicaBuilder.Novo().Build();

        var novaRazaoSocial = "Nova Empresa Teste Ltda";
        var novoEmail = "novo@empresa.com";
        var novaDataAbertura = new DateTime(2019, 5, 10);
        var novoNomeFantasia = "Novo Nome Fantasia";
        var novoTelefone = "(11) 9999-8888";
        var novaInscricaoEstadual = "999.999.999.999";

        pessoa.Atualizar(novaRazaoSocial, novoEmail, novaDataAbertura, novoNomeFantasia, novoTelefone, novaInscricaoEstadual);

        pessoa.RazaoSocial.Should().Be(novaRazaoSocial);
        pessoa.Email.Should().Be(novoEmail);
        pessoa.DataAbertura.Should().Be(novaDataAbertura);
        pessoa.NomeFantasia.Should().Be(novoNomeFantasia);
        pessoa.Telefone.Should().Be(novoTelefone);
        pessoa.InscricaoEstadual.Should().Be(novaInscricaoEstadual);
        pessoa.AtualizadoEm.Should().NotBeNull();
    }

    [Test]
    public void Quando_ObterCnpjFormatado_Devemos_ObterCnpjComPontosBarraEHifen()
    {
        var pessoa = PessoaJuridicaBuilder.Novo()
            .ComCnpj(PessoaJuridicaBuilder.CnpjValido.Replace(".", "").Replace("/", "").Replace("-", ""))
            .Build();

        var cnpjFormatado = pessoa.ObterCnpjFormatado();

        cnpjFormatado.Should().Be(PessoaJuridicaBuilder.CnpjValido);
    }

    [Test]
    public void Quando_CalcularTempoAtuacao_Devemos_ObterAnosDeAtuacao()
    {
        var anoAtual = DateTime.Today.Year;
        var dataAbertura = new DateTime(anoAtual - 5, 1, 1);

        var pessoa = PessoaJuridicaBuilder.Novo()
            .ComDataAbertura(dataAbertura)
            .Build();

        var tempoAtuacao = pessoa.CalcularTempoAtuacao();

        tempoAtuacao.Should().Be(5);
    }
}
