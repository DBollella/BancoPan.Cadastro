using BancoPan.Cadastro.Application.DTOs;

namespace BancoPan.Cadastro.Application.Tests.Builders;

public class CriarPessoaFisicaDtoBuilder
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

    public static CriarPessoaFisicaDtoBuilder Novo() => new();

    public CriarPessoaFisicaDtoBuilder ComNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public CriarPessoaFisicaDtoBuilder ComEmail(string email)
    {
        _email = email;
        return this;
    }

    public CriarPessoaFisicaDtoBuilder ComCpf(string cpf)
    {
        _cpf = cpf;
        return this;
    }

    public CriarPessoaFisicaDtoBuilder ComDataNascimento(DateTime dataNascimento)
    {
        _dataNascimento = dataNascimento;
        return this;
    }

    public CriarPessoaFisicaDtoBuilder ComEnderecoId(Guid enderecoId)
    {
        _enderecoId = enderecoId;
        return this;
    }

    public CriarPessoaFisicaDtoBuilder ComTelefone(string telefone)
    {
        _telefone = telefone;
        return this;
    }

    public CriarPessoaFisicaDtoBuilder ComRg(string rg)
    {
        _rg = rg;
        return this;
    }

    public CriarPessoaFisicaDto Build()
    {
        return new CriarPessoaFisicaDto
        {
            Nome = _nome,
            Email = _email,
            Cpf = _cpf,
            DataNascimento = _dataNascimento,
            EnderecoId = _enderecoId,
            Telefone = _telefone,
            Rg = _rg
        };
    }
}
