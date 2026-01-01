using BancoPan.Cadastro.Domain.Entities;
using BancoPan.Cadastro.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpPost("execute")]
    public async Task<IActionResult> SeedDatabase()
    {
        try
        {
            var seeder = new DataSeeder(_context);
            await seeder.SeedAsync();

            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            var pfKeys = server.Keys(pattern: "pessoasfisicas:*").ToArray();
            if (pfKeys.Length > 0)
                await db.KeyDeleteAsync(pfKeys);

            var pjKeys = server.Keys(pattern: "pessoasjuridicas:*").ToArray();
            if (pjKeys.Length > 0)
                await db.KeyDeleteAsync(pjKeys);

            var endKeys = server.Keys(pattern: "enderecos:*").ToArray();
            if (endKeys.Length > 0)
                await db.KeyDeleteAsync(endKeys);

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

      [HttpPost("execute-1000-pessoas-fisicas")]
    public async Task<IActionResult> Seed1000PessoasFisicas()
    {
        try
        {
            _context.PessoasFisicas.RemoveRange(_context.PessoasFisicas);
            await _context.SaveChangesAsync();

            var enderecos = await _context.Enderecos.ToListAsync();

            if (enderecos.Count == 0)
            {
                return BadRequest(new
                {
                    message = "No addresses found. Please run the main seed endpoint first.",
                    error = "Database must have addresses before seeding pessoas físicas"
                });
            }

            var pessoasFisicas = new List<PessoaFisica>();
            var nomesPrefixos = new[]
            {
                "João", "Maria", "Pedro", "Ana", "Carlos", "Juliana", "Rafael", "Fernanda", "Lucas", "Mariana",
                "Bruno", "Camila", "Diego", "Letícia", "Gustavo", "Patrícia", "Rodrigo", "Amanda", "Felipe", "Vanessa",
                "Thiago", "Larissa", "Marcelo", "Renata", "André", "Carolina", "Fabio", "Beatriz", "Leonardo", "Gabriela",
                "Ricardo", "Paula", "Vitor", "Cristina", "Daniel", "Adriana", "Henrique", "Tatiana", "Maurício", "Silvia"
            };

            var sobrenomes = new[]
            {
                "Silva", "Santos", "Oliveira", "Costa", "Souza", "Ferreira", "Almeida", "Lima", "Rodrigues", "Pereira",
                "Carvalho", "Martins", "Araújo", "Barbosa", "Ribeiro", "Gomes", "Dias", "Cardoso", "Nascimento", "Rocha",
                "Castro", "Correia", "Pinto", "Vieira", "Mendes", "Freitas", "Cunha", "Moreira", "Teixeira", "Monteiro",
                "Alves", "Ramos", "Cavalcanti", "Barros", "Nogueira", "Campos", "Rezende", "Melo", "Azevedo", "Lopes"
            };

            for (int i = 0; i < 1000; i++)
            {
                var prefixo = nomesPrefixos[i % nomesPrefixos.Length];
                var sobrenome = sobrenomes[i % sobrenomes.Length];
                var nome = $"{prefixo} {sobrenome}";

                if (i >= 40)
                {
                    var segundoSobrenome = sobrenomes[(i / 10) % sobrenomes.Length];
                    nome = $"{prefixo} {sobrenome} {segundoSobrenome}";
                }

                var pessoaFisica = new PessoaFisica(
                    nome,
                    $"{nome.ToLower().Replace(" ", ".")}{i}@email.com",
                    GerarCpfValido(i),
                    DateTime.Now.AddYears(-(18 + (i % 60))),
                    enderecos[i % enderecos.Count].Id, // Reutiliza os endereços ciclicamente
                    $"({11 + (i % 89):D2}) 9{8000 + (i % 2000):D4}-{1000 + (i % 9000):D4}",
                    $"MG-{10000000 + i}"
                );
                pessoasFisicas.Add(pessoaFisica);
            }

            _context.PessoasFisicas.AddRange(pessoasFisicas);
            await _context.SaveChangesAsync();

            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: "pessoasfisicas:*").ToArray();
            if (keys.Length > 0)
                await db.KeyDeleteAsync(keys);

            return Ok(new
            {
                message = "1000 Pessoas Físicas seeded successfully and cache cleared!",
                data = new
                {
                    pessoasFisicas = 1000,
                    enderecosReutilizados = enderecos.Count
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "Error seeding pessoas físicas",
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    private static string GerarCpfValido(int seed)
    {
        var random = new Random(seed + 12345);
        var cpf = new int[11];

        for (int i = 0; i < 9; i++)
            cpf[i] = random.Next(0, 10);

        var soma = 0;
        for (int i = 0; i < 9; i++)
            soma += cpf[i] * (10 - i);
        var resto = soma % 11;
        cpf[9] = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += cpf[i] * (11 - i);
        resto = soma % 11;
        cpf[10] = resto < 2 ? 0 : 11 - resto;

        return string.Join("", cpf);
    }
}
