namespace BancoPan.Cadastro.Application.DTOs;

public class EnderecoDto
{
    public Guid Id { get; set; }
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Regiao { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public string Ddd { get; set; } = string.Empty;
}

public class CriarEnderecoDto
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Regiao { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public string Ddd { get; set; } = string.Empty;
}

public class AtualizarEnderecoDto
{
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
}
