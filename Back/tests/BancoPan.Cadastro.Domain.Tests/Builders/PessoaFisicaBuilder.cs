using BancoPan.Cadastro.Domain.Entities;

namespace BancoPan.Cadastro.Domain.Tests.Builders;

public class PessoaFisicaBuilder
{
    public const string CpfValido = "123.456.789-09";
    public const string CpfInvalido = "111.111.111-11";
    public const string EmailValido = "joao@example.com";
    public const string EmailInvalido = "emailinvalido";

    private string _nome = "João da Silva";
    private string _email = EmailValido;
    private string _cpf = CpfValido;
    private DateTime _dataNascimento = new(1990, 5, 15);
    private Guid _enderecoId = Guid.NewGuid();
    private string? _telefone = null;
    private string? _rg = null;

    public static PessoaFisicaBuilder Novo() => new();

    public PessoaFisicaBuilder ComNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public PessoaFisicaBuilder ComEmail(string email)
    {
        _email = email;
        return this;
    }

    public PessoaFisicaBuilder ComCpf(string cpf)
    {
        _cpf = cpf;
        return this;
    }

    public PessoaFisicaBuilder ComDataNascimento(DateTime dataNascimento)
    {
        _dataNascimento = dataNascimento;
        return this;
    }

    public PessoaFisicaBuilder ComEnderecoId(Guid enderecoId)
    {
        _enderecoId = enderecoId;
        return this;
    }

    public PessoaFisicaBuilder ComTelefone(string telefone)
    {
        _telefone = telefone;
        return this;
    }

    public PessoaFisicaBuilder ComRg(string rg)
    {
        _rg = rg;
        return this;
    }

    public PessoaFisicaBuilder ComCpfValido()
    {
        _cpf = CpfValido;
        return this;
    }

    public PessoaFisicaBuilder ComCpfInvalido()
    {
        _cpf = CpfInvalido;
        return this;
    }

    public PessoaFisicaBuilder ComEmailValido()
    {
        _email = EmailValido;
        return this;
    }

    public PessoaFisicaBuilder ComEmailInvalido()
    {
        _email = EmailInvalido;
        return this;
    }

    public PessoaFisica Build()
    {
        return new PessoaFisica(
            _nome,
            _email,
            _cpf,
            _dataNascimento,
            _enderecoId,
            _telefone,
            _rg);
    }
}
