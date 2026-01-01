using BancoPan.Cadastro.Domain.Common;
using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoPan.Cadastro.Infra.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly CadastroDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(CadastroDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> ObterTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<PagedResult<T>> ObterPaginadoAsync(PaginationParameters parameters)
    {
        var query = _dbSet.AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedResult<T>(items, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public virtual async Task AdicionarAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task AtualizarAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task RemoverAsync(Guid id)
    {
        var entity = await ObterPorIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual async Task<bool> ExisteAsync(Guid id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
}
