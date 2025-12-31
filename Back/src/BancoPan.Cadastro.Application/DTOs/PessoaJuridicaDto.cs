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
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string Cnpj { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; }
    public string? InscricaoEstadual { get; set; }
    public Guid EnderecoId { get; set; }
}

public class AtualizarPessoaJuridicaDto
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? NomeFantasia { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public DateTime DataAbertura { get; set; }
    public string? InscricaoEstadual { get; set; }
}
