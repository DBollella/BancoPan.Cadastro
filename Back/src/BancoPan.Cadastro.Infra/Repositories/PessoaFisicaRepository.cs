using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoPan.Cadastro.Infra.Repositories;

public class PessoaFisicaRepository : Repository<PessoaFisica>, IPessoaFisicaRepository
{
    public PessoaFisicaRepository(CadastroDbContext context) : base(context)
    {
    }

    public override async Task<PessoaFisica?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<PessoaFisica>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(p => p.Endereco)
            .ToListAsync();
    }

    public async Task<PessoaFisica?> ObterPorCpfAsync(string cpf)
    {
        var cpfLimpo = cpf.Replace(".", "").Replace("-", "").Trim();
        return await _dbSet
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.Cpf == cpfLimpo);
    }

    public async Task<bool> ExistePorCpfAsync(string cpf)
    {
        var cpfLimpo = cpf.Replace(".", "").Replace("-", "").Trim();
        return await _dbSet.AnyAsync(p => p.Cpf == cpfLimpo);
    }
}
