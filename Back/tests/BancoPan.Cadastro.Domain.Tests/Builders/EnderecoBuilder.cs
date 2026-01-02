using BancoPan.Cadastro.Domain.Entities;

namespace BancoPan.Cadastro.Domain.Tests.Builders;

public class EnderecoBuilder
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

    public static EnderecoBuilder Novo() => new();

    public EnderecoBuilder ComCep(string cep)
    {
        _cep = cep;
        return this;
    }

    public EnderecoBuilder ComLogradouro(string logradouro)
    {
        _logradouro = logradouro;
        return this;
    }

    public EnderecoBuilder ComNumero(string numero)
    {
        _numero = numero;
        return this;
    }

    public EnderecoBuilder ComBairro(string bairro)
    {
        _bairro = bairro;
        return this;
    }

    public EnderecoBuilder ComLocalidade(string localidade)
    {
        _localidade = localidade;
        return this;
    }

    public EnderecoBuilder ComUf(string uf)
    {
        _uf = uf;
        return this;
    }

    public EnderecoBuilder ComEstado(string estado)
    {
        _estado = estado;
        return this;
    }

    public EnderecoBuilder ComRegiao(string regiao)
    {
        _regiao = regiao;
        return this;
    }

    public EnderecoBuilder ComIbge(string ibge)
    {
        _ibge = ibge;
        return this;
    }

    public EnderecoBuilder ComDdd(string ddd)
    {
        _ddd = ddd;
        return this;
    }

    public EnderecoBuilder ComComplemento(string complemento)
    {
        _complemento = complemento;
        return this;
    }

    public EnderecoBuilder ComCepValido()
    {
        _cep = CepValido;
        return this;
    }

    public EnderecoBuilder ComCepInvalido()
    {
        _cep = CepInvalido;
        return this;
    }

    public Endereco Build()
    {
        return new Endereco(
            _cep,
            _logradouro,
            _numero,
            _bairro,
            _localidade,
            _uf,
            _estado,
            _regiao,
            _ibge,
            _ddd,
            _complemento);
    }
}
