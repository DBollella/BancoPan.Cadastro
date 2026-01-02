using BancoPan.Cadastro.Domain.Entities;

namespace BancoPan.Cadastro.Domain.Tests.Builders;

public class PessoaJuridicaBuilder
{
    public const string CnpjValido = "11.222.333/0001-81";
    public const string CnpjInvalido = "11.111.111/1111-11";
    public const string EmailValido = "contato@empresa.com";
    public const string EmailInvalido = "emailinvalido";

    private string _razaoSocial = "Empresa Teste Ltda";
    private string _email = EmailValido;
    private string _cnpj = CnpjValido;
    private DateTime _dataAbertura = new(2020, 1, 15);
    private Guid _enderecoId = Guid.NewGuid();
    private string? _nomeFantasia = null;
    private string? _telefone = null;
    private string? _inscricaoEstadual = null;

    public static PessoaJuridicaBuilder Novo() => new();

    public PessoaJuridicaBuilder ComRazaoSocial(string razaoSocial)
    {
        _razaoSocial = razaoSocial;
        return this;
    }

    public PessoaJuridicaBuilder ComEmail(string email)
    {
        _email = email;
        return this;
    }

    public PessoaJuridicaBuilder ComCnpj(string cnpj)
    {
        _cnpj = cnpj;
        return this;
    }

    public PessoaJuridicaBuilder ComDataAbertura(DateTime dataAbertura)
    {
        _dataAbertura = dataAbertura;
        return this;
    }

    public PessoaJuridicaBuilder ComEnderecoId(Guid enderecoId)
    {
        _enderecoId = enderecoId;
        return this;
    }

    public PessoaJuridicaBuilder ComNomeFantasia(string nomeFantasia)
    {
        _nomeFantasia = nomeFantasia;
        return this;
    }

    public PessoaJuridicaBuilder ComTelefone(string telefone)
    {
        _telefone = telefone;
        return this;
    }

    public PessoaJuridicaBuilder ComInscricaoEstadual(string inscricaoEstadual)
    {
        _inscricaoEstadual = inscricaoEstadual;
        return this;
    }

    public PessoaJuridicaBuilder ComCnpjValido()
    {
        _cnpj = CnpjValido;
        return this;
    }

    public PessoaJuridicaBuilder ComCnpjInvalido()
    {
        _cnpj = CnpjInvalido;
        return this;
    }

    public PessoaJuridicaBuilder ComEmailValido()
    {
        _email = EmailValido;
        return this;
    }

    public PessoaJuridicaBuilder ComEmailInvalido()
    {
        _email = EmailInvalido;
        return this;
    }

    public PessoaJuridica Build()
    {
        return new PessoaJuridica(
            _razaoSocial,
            _email,
            _cnpj,
            _dataAbertura,
            _enderecoId,
            _nomeFantasia,
            _telefone,
            _inscricaoEstadual);
    }
}
