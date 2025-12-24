using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BancoPan.Cadastro.Infra.Data;

public class CadastroDbContextFactory : IDesignTimeDbContextFactory<CadastroDbContext>
{
    public CadastroDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CadastroDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=BancoPanCadastro;User Id=sa;Password=BancoPan@2025;TrustServerCertificate=true;");

        return new CadastroDbContext(optionsBuilder.Options);
    }
}
