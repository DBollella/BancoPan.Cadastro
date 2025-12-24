using BancoPan.Cadastro.Domain.Entities;

namespace BancoPan.Cadastro.Domain.Interfaces;

public interface IPessoaJuridicaRepository : IRepository<PessoaJuridica>
{
    Task<PessoaJuridica?> ObterPorCnpjAsync(string cnpj);
    Task<bool> ExistePorCnpjAsync(string cnpj);
}
