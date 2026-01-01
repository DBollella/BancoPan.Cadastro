using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "O CEP é obrigatório")]
    [RegularExpression(@"^\d{8}$|^\d{5}-\d{3}$", ErrorMessage = "CEP inválido")]
    public string Cep { get; set; } = string.Empty;

    [Required(ErrorMessage = "O logradouro é obrigatório")]
    public string Logradouro { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número é obrigatório")]
    public string Numero { get; set; } = string.Empty;

    public string? Complemento { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório")]
    public string Bairro { get; set; } = string.Empty;

    [Required(ErrorMessage = "A localidade é obrigatória")]
    public string Localidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A UF é obrigatória")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter 2 caracteres")]
    public string Uf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório")]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "A região é obrigatória")]
    public string Regiao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O código IBGE é obrigatório")]
    public string Ibge { get; set; } = string.Empty;

    [Required(ErrorMessage = "O DDD é obrigatório")]
    public string Ddd { get; set; } = string.Empty;
}

public class AtualizarEnderecoDto
{
    [Required(ErrorMessage = "O CEP é obrigatório")]
    [RegularExpression(@"^\d{8}$|^\d{5}-\d{3}$", ErrorMessage = "CEP inválido")]
    public string Cep { get; set; } = string.Empty;

    [Required(ErrorMessage = "O logradouro é obrigatório")]
    public string Logradouro { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número é obrigatório")]
    public string Numero { get; set; } = string.Empty;

    public string? Complemento { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório")]
    public string Bairro { get; set; } = string.Empty;

    [Required(ErrorMessage = "A localidade é obrigatória")]
    public string Localidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A UF é obrigatória")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter 2 caracteres")]
    public string Uf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório")]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "A região é obrigatória")]
    public string Regiao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O código IBGE é obrigatório")]
    public string Ibge { get; set; } = string.Empty;

    [Required(ErrorMessage = "O DDD é obrigatório")]
    public string Ddd { get; set; } = string.Empty;
}
