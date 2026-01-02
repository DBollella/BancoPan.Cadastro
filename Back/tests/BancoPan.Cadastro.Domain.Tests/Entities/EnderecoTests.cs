using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Tests.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace BancoPan.Cadastro.Domain.Tests.Entities;

[TestFixture]
public class EnderecoTests
{
    [Test]
    public void Quando_CriarComDadosValidos_Devemos_ObterEnderecoValido()
    {
        var endereco = EnderecoBuilder.Novo()
            .Build();

        endereco.Should().NotBeNull();
        endereco.Cep.Should().Be(EnderecoBuilder.CepValido.Replace("-", ""));
        endereco.Logradouro.Should().Be("Rua Teste");
        endereco.Numero.Should().Be("123");
        endereco.Bairro.Should().Be("Centro");
        endereco.Localidade.Should().Be("São Paulo");
        endereco.Uf.Should().Be("SP");
        endereco.Estado.Should().Be("São Paulo");
        endereco.Regiao.Should().Be("Sudeste");
    }

    [Test]
    public void Quando_CriarComComplemento_Devemos_ObterEnderecoComComplemento()
    {
        var complemento = "Apto 101";

        var endereco = EnderecoBuilder.Novo()
            .ComComplemento(complemento)
            .Build();

        endereco.Should().NotBeNull();
        endereco.Complemento.Should().Be(complemento);
    }

    [Test]
    public void Quando_CriarComCepVazio_Devemos_LancarArgumentException()
    {
        var action = () => EnderecoBuilder.Novo()
            .ComCep("")
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CEP é obrigatório*");
    }

    [Test]
    public void Quando_CriarComCepInvalido_Devemos_LancarArgumentException()
    {
        var action = () => EnderecoBuilder.Novo()
            .ComCep(EnderecoBuilder.CepInvalido)
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*CEP inválido*");
    }

    [Test]
    public void Quando_CriarComUfInvalida_Devemos_LancarArgumentException()
    {
        var action = () => EnderecoBuilder.Novo()
            .ComUf("S")
            .Build();

        action.Should().Throw<ArgumentException>()
            .WithMessage("*UF deve ter 2 caracteres*");
    }

    [Test]
    public void Quando_AtualizarComDadosValidos_Devemos_ObterEnderecoAtualizado()
    {
        var endereco = EnderecoBuilder.Novo().Build();

        var novoCep = "98765-432";
        var novoLogradouro = "Avenida Nova";
        var novoNumero = "456";
        var novoBairro = "Jardim";
        var novaLocalidade = "Rio de Janeiro";
        var novoUf = "RJ";
        var novoEstado = "Rio de Janeiro";
        var novaRegiao = "Sudeste";
        var novoIbge = "3304557";
        var novoDdd = "21";

        endereco.Atualizar(novoCep, novoLogradouro, novoNumero, novoBairro, novaLocalidade, novoUf, novoEstado, novaRegiao, novoIbge, novoDdd);

        endereco.Cep.Should().Be(novoCep.Replace("-", ""));
        endereco.Logradouro.Should().Be(novoLogradouro);
        endereco.AtualizadoEm.Should().NotBeNull();
    }

    [Test]
    public void Quando_ObterCepFormatado_Devemos_ObterCepComHifen()
    {
        var endereco = EnderecoBuilder.Novo()
            .ComCep(EnderecoBuilder.CepValido)
            .Build();

        var cepFormatado = endereco.ObterCepFormatado();

        cepFormatado.Should().Be(EnderecoBuilder.CepValido);
    }
}
