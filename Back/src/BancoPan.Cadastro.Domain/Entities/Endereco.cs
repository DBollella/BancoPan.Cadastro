namespace BancoPan.Cadastro.Domain.Entities;

public class Endereco : Entity
{
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string? Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Localidade { get; private set; }
    public string Uf { get; private set; }
    public string Estado { get; private set; }
    public string Regiao { get; private set; }
    public string Ibge { get; private set; }
    public string Ddd { get; private set; }

    protected Endereco() { }

    public Endereco(
        string cep,
        string logradouro,
        string numero,
        string bairro,
        string localidade,
        string uf,
        string estado,
        string regiao,
        string ibge,
        string ddd,
        string? complemento = null)
    {
        ValidarCep(cep);
        ValidarUf(uf);
        Cep = RemoverFormatacaoCep(cep);
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Localidade = localidade;
        Uf = uf.ToUpper();
        Estado = estado;
        Regiao = regiao;
        Ibge = ibge;
        Ddd = ddd;
    }

    public void Atualizar(
        string cep,
        string logradouro,
        string numero,
        string bairro,
        string localidade,
        string uf,
        string estado,
        string regiao,
        string ibge,
        string ddd,
        string? complemento = null)
    {
        ValidarCep(cep);
        ValidarUf(uf);
        Cep = RemoverFormatacaoCep(cep);
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Localidade = localidade;
        Uf = uf.ToUpper();
        Estado = estado;
        Regiao = regiao;
        Ibge = ibge;
        Ddd = ddd;
        AtualizarDataModificacao();
    }

    private static void ValidarCep(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            throw new ArgumentException("CEP é obrigatório", nameof(cep));

        var cepLimpo = RemoverFormatacaoCep(cep);
        if (cepLimpo.Length != 8 || !cepLimpo.All(char.IsDigit))
            throw new ArgumentException("CEP inválido", nameof(cep));
    }

    private static void ValidarUf(string uf)
    {
        if (string.IsNullOrWhiteSpace(uf))
            throw new ArgumentException("UF é obrigatória", nameof(uf));

        if (uf.Length != 2)
            throw new ArgumentException("UF deve ter 2 caracteres", nameof(uf));
    }

    private static string RemoverFormatacaoCep(string cep)
    {
        return cep.Replace("-", "").Replace(".", "").Trim();
    }

    public string ObterCepFormatado()
    {
        return $"{Cep.Substring(0, 5)}-{Cep.Substring(5)}";
    }
}
