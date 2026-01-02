using BancoPan.Cadastro.Application.DTOs;

namespace BancoPan.Cadastro.Application.Tests.Builders;

public class CriarPessoaJuridicaDtoBuilder
{
    public const string CnpjValido = "11.222.333/0001-81";
    public const string CnpjInvalido = "11.111.111/1111-11";
    public const string EmailValido = "contato@empresa.com";
    public const string EmailInvalido = "emailinvalido";

    private string _razaoSocial = "Empresa Teste Ltda";
    private string _email = EmailValido;
    private string _cnpj = CnpjValido;
    private DateTime _dataAbertura = new DateTime(2020, 1, 15);
    private Guid _enderecoId = Guid.NewGuid();
    private string? _nomeFantasia = "Empresa Teste";
    private string? _telefone = "(11) 3333-4444";
    private string? _inscricaoEstadual = "123.456.789.012";

    public static CriarPessoaJuridicaDtoBuilder Novo() => new();

    public CriarPessoaJuridicaDtoBuilder ComRazaoSocial(string razaoSocial)
    {
        _razaoSocial = razaoSocial;
        return this;
    }

    public CriarPessoaJuridicaDtoBuilder ComEmail(string email)
    {
        _email = email;
        return this;
    }

    public CriarPessoaJuridicaDtoBuilder ComCnpj(string cnpj)
    {
        _cnpj = cnpj;
        return this;
    }

    public CriarPessoaJuridicaDtoBuilder ComDataAbertura(DateTime dataAbertura)
    {
        _dataAbertura = dataAbertura;
        return this;
    }

    public CriarPessoaJuridicaDtoBuilder ComEnderecoId(Guid enderecoId)
    {
        _enderecoId = enderecoId;
        return this;
    }

    public CriarPessoaJuridicaDtoBuilder ComNomeFantasia(string? nomeFantasia)
    {
        _nomeFantasia = nomeFantasia;
        return this;
    }

    public CriarPessoaJuridicaDtoBuilder ComTelefone(string? telefone)
    {
        _telefone = telefone;
        return this;
    }

    public CriarPessoaJuridicaDtoBuilder ComInscricaoEstadual(string? inscricaoEstadual)
    {
        _inscricaoEstadual = inscricaoEstadual;
        return this;
    }

    public CriarPessoaJuridicaDto Build()
    {
        return new CriarPessoaJuridicaDto
        {
            RazaoSocial = _razaoSocial,
            Email = _email,
            Cnpj = _cnpj,
            DataAbertura = _dataAbertura,
            EnderecoId = _enderecoId,
            NomeFantasia = _nomeFantasia,
            Telefone = _telefone,
            InscricaoEstadual = _inscricaoEstadual
        };
    }
}
