using BancoPan.Cadastro.Domain.Entities;

namespace BancoPan.Cadastro.Domain.Interfaces;

public interface IPessoaFisicaRepository : IRepository<PessoaFisica>
{
    Task<PessoaFisica?> ObterPorCpfAsync(string cpf);
    Task<bool> ExistePorCpfAsync(string cpf);
}
