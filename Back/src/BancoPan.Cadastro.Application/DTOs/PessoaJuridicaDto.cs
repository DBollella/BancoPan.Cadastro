using System.ComponentModel.DataAnnotations;

namespace BancoPan.Cadastro.Application.DTOs;

public class PessoaJuridicaDto
{
    public Guid Id { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string Cnpj { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; }
    public string? InscricaoEstadual { get; set; }
    public int TempoAtuacao { get; set; }
    public Guid EnderecoId { get; set; }
    public EnderecoDto? Endereco { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}

public class CriarPessoaJuridicaDto
{
    [Required(ErrorMessage = "A razão social é obrigatória")]
    [MinLength(3, ErrorMessage = "A razão social deve ter no mínimo 3 caracteres")]
    public string RazaoSocial { get; set; } = string.Empty;

    public string? NomeFantasia { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "O CNPJ é obrigatório")]
    [RegularExpression(@"^\d{14}$|^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido")]
    public string Cnpj { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de abertura é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime DataAbertura { get; set; }

    public string? InscricaoEstadual { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório")]
    public Guid EnderecoId { get; set; }
}

public class AtualizarPessoaJuridicaDto
{
    [Required(ErrorMessage = "A razão social é obrigatória")]
    [MinLength(3, ErrorMessage = "A razão social deve ter no mínimo 3 caracteres")]
    public string RazaoSocial { get; set; } = string.Empty;

    public string? NomeFantasia { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "A data de abertura é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime DataAbertura { get; set; }

    public string? InscricaoEstadual { get; set; }
}
