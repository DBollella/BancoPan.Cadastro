using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoPan.Cadastro.Infra.Repositories;

public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(CadastroDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Endereco>> ObterPorCepAsync(string cep)
    {
        var cepLimpo = cep.Replace("-", "").Replace(".", "").Trim();
        return await _dbSet
            .Where(e => e.Cep == cepLimpo)
            .ToListAsync();
    }
}
