using System.ComponentModel.DataAnnotations;

namespace BancoPan.Cadastro.Application.DTOs;

public class PessoaFisicaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? Rg { get; set; }
    public int Idade { get; set; }
    public Guid EnderecoId { get; set; }
    public EnderecoDto? Endereco { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}

public class CriarPessoaFisicaDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório")]
    [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF inválido")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }

    public string? Rg { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório")]
    public Guid EnderecoId { get; set; }
}

public class AtualizarPessoaFisicaDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "A data de nascimento é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }

    public string? Rg { get; set; }
}
