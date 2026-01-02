using BancoPan.Cadastro.Application.DTOs;

namespace BancoPan.Cadastro.Application.Tests.Builders;

public class CriarEnderecoDtoBuilder
{
    public const string CepValido = "12345-678";
    public const string CepInvalido = "123";

    private string _cep = CepValido;
    private string _logradouro = "Rua Teste";
    private string _numero = "123";
    private string _bairro = "Centro";
    private string _localidade = "São Paulo";
    private string _uf = "SP";
    private string _estado = "São Paulo";
    private string _regiao = "Sudeste";
    private string _ibge = "3550308";
    private string _ddd = "11";
    private string? _complemento = null;

    public static CriarEnderecoDtoBuilder Novo() => new();

    public CriarEnderecoDtoBuilder ComCep(string cep)
    {
        _cep = cep;
        return this;
    }

    public CriarEnderecoDtoBuilder ComLogradouro(string logradouro)
    {
        _logradouro = logradouro;
        return this;
    }

    public CriarEnderecoDtoBuilder ComNumero(string numero)
    {
        _numero = numero;
        return this;
    }

    public CriarEnderecoDtoBuilder ComBairro(string bairro)
    {
        _bairro = bairro;
        return this;
    }

    public CriarEnderecoDtoBuilder ComLocalidade(string localidade)
    {
        _localidade = localidade;
        return this;
    }

    public CriarEnderecoDtoBuilder ComUf(string uf)
    {
        _uf = uf;
        return this;
    }

    public CriarEnderecoDtoBuilder ComEstado(string estado)
    {
        _estado = estado;
        return this;
    }

    public CriarEnderecoDtoBuilder ComRegiao(string regiao)
    {
        _regiao = regiao;
        return this;
    }

    public CriarEnderecoDtoBuilder ComIbge(string ibge)
    {
        _ibge = ibge;
        return this;
    }

    public CriarEnderecoDtoBuilder ComDdd(string ddd)
    {
        _ddd = ddd;
        return this;
    }

    public CriarEnderecoDtoBuilder ComComplemento(string complemento)
    {
        _complemento = complemento;
        return this;
    }

    public CriarEnderecoDto Build()
    {
        return new CriarEnderecoDto
        {
            Cep = _cep,
            Logradouro = _logradouro,
            Numero = _numero,
            Bairro = _bairro,
            Localidade = _localidade,
            Uf = _uf,
            Estado = _estado,
            Regiao = _regiao,
            Ibge = _ibge,
            Ddd = _ddd,
            Complemento = _complemento
        };
    }
}
