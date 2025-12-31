using BancoPan.Cadastro.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace BancoPan.Cadastro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly CadastroDbContext _context;
    private readonly IConnectionMultiplexer _redis;

    public SeedController(CadastroDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }

    /// <summary>
    /// Seeds the database with 30 items for each entity (Endereço, Pessoa Física, Pessoa Jurídica)
    /// WARNING: This will delete all existing data before seeding!
    /// </summary>
    [HttpPost("execute")]
    public async Task<IActionResult> SeedDatabase()
    {
        try
        {
            var seeder = new DataSeeder(_context);
            await seeder.SeedAsync();

            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync("pessoasfisicas:all");
            await db.KeyDeleteAsync("pessoasjuridicas:all");
            await db.KeyDeleteAsync("enderecos:all");

            return Ok(new
            {
                message = "Database seeded successfully and cache cleared!",
                data = new
                {
                    enderecos = 30,
                    pessoasFisicas = 30,
                    pessoasJuridicas = 30
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "Error seeding database",
                error = ex.Message
            });
        }
    }
}
