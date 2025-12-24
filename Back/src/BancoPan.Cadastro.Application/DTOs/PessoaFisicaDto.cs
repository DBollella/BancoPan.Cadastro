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
    public EnderecoDto? Endereco { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}

public class CriarPessoaFisicaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? Rg { get; set; }
    public Guid EnderecoId { get; set; }
}

public class AtualizarPessoaFisicaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? Rg { get; set; }
}
