using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoPan.Cadastro.Infra.Repositories;

public class PessoaJuridicaRepository : Repository<PessoaJuridica>, IPessoaJuridicaRepository
{
    public PessoaJuridicaRepository(CadastroDbContext context) : base(context)
    {
    }

    public override async Task<PessoaJuridica?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<PessoaJuridica>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(p => p.Endereco)
            .ToListAsync();
    }

    public async Task<PessoaJuridica?> ObterPorCnpjAsync(string cnpj)
    {
        var cnpjLimpo = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
        return await _dbSet
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.Cnpj == cnpjLimpo);
    }

    public async Task<bool> ExistePorCnpjAsync(string cnpj)
    {
        var cnpjLimpo = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
        return await _dbSet.AnyAsync(p => p.Cnpj == cnpjLimpo);
    }
}
