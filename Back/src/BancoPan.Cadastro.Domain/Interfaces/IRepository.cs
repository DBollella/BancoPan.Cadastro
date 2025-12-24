using BancoPan.Cadastro.Domain.Entities;

namespace BancoPan.Cadastro.Domain.Interfaces;

public interface IRepository<T> where T : Entity
{
    Task<T?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<T>> ObterTodosAsync();
    Task AdicionarAsync(T entity);
    Task AtualizarAsync(T entity);
    Task RemoverAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
}
