using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Infra.Data;
using BancoPan.Cadastro.Infra.Repositories;

namespace BancoPan.Cadastro.Infra.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CadastroDbContext _context;
    private IPessoaFisicaRepository? _pessoasFisicas;
    private IPessoaJuridicaRepository? _pessoasJuridicas;
    private IEnderecoRepository? _enderecos;

    public UnitOfWork(CadastroDbContext context)
    {
        _context = context;
    }

    public IPessoaFisicaRepository PessoasFisicas
    {
        get
        {
            _pessoasFisicas ??= new PessoaFisicaRepository(_context);
            return _pessoasFisicas;
        }
    }

    public IPessoaJuridicaRepository PessoasJuridicas
    {
        get
        {
            _pessoasJuridicas ??= new PessoaJuridicaRepository(_context);
            return _pessoasJuridicas;
        }
    }

    public IEnderecoRepository Enderecos
    {
        get
        {
            _enderecos ??= new EnderecoRepository(_context);
            return _enderecos;
        }
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        await Task.Run(() =>
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            }
        });
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
