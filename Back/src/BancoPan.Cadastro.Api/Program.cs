using BancoPan.Cadastro.Application.Interfaces;
using BancoPan.Cadastro.Application.Services;
using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Infra.Cache;
using BancoPan.Cadastro.Infra.Data;
using BancoPan.Cadastro.Infra.ExternalServices;
using BancoPan.Cadastro.Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisConnection = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    try
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Tentando conectar ao Redis em: {RedisConnection}", redisConnection);

        var connection = ConnectionMultiplexer.Connect(redisConnection);

        if (connection.IsConnected)
        {
            logger.LogInformation("Conexão com Redis estabelecida com sucesso");
            return connection;
        }

        logger.LogWarning("Redis não está conectado. A aplicação continuará sem cache");
        return null!;
    }
    catch (Exception ex)
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Não foi possível conectar ao Redis. A aplicação continuará sem cache");
        return null!;
    }
});
builder.Services.AddSingleton<ICacheService, RedisCacheService>();

builder.Services.AddDbContext<CadastroDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("BancoPan.Cadastro.Infra")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHttpClient<IViaCepService, ViaCepService>();

builder.Services.AddScoped<IEnderecoService, EnderecoService>();
builder.Services.AddScoped<IPessoaFisicaService, PessoaFisicaService>();
builder.Services.AddScoped<IPessoaJuridicaService, PessoaJuridicaService>();

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Iniciando criacao do banco de dados...");
        var context = services.GetRequiredService<CadastroDbContext>();

        logger.LogInformation("Criando banco de dados e tabelas...");
        context.Database.EnsureCreated();

        logger.LogInformation("Banco de dados criado com sucesso!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "ERRO CRITICO ao aplicar migrations no banco de dados");
        throw;
    }
}

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "QA")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
