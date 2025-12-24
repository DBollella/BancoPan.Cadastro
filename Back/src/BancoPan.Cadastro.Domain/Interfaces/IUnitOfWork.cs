namespace BancoPan.Cadastro.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPessoaFisicaRepository PessoasFisicas { get; }
    IPessoaJuridicaRepository PessoasJuridicas { get; }
    IEnderecoRepository Enderecos { get; }
    Task<int> CommitAsync();
    Task RollbackAsync();
}
