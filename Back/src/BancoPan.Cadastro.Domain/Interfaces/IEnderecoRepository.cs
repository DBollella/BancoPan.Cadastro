using BancoPan.Cadastro.Domain.Entities;

namespace BancoPan.Cadastro.Domain.Interfaces;

public interface IEnderecoRepository : IRepository<Endereco>
{
    Task<IEnumerable<Endereco>> ObterPorCepAsync(string cep);
}
