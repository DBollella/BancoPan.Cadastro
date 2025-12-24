using BancoPan.Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BancoPan.Cadastro.Infra.Data;

public class CadastroDbContext : DbContext
{
    public CadastroDbContext(DbContextOptions<CadastroDbContext> options) : base(options)
    {
    }

    public DbSet<PessoaFisica> PessoasFisicas { get; set; }
    public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.ToTable("Enderecos");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Cep)
                .IsRequired()
                .HasMaxLength(8);

            entity.Property(e => e.Logradouro)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Numero)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.Complemento)
                .HasMaxLength(100);

            entity.Property(e => e.Bairro)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Localidade)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Uf)
                .IsRequired()
                .HasMaxLength(2);

            entity.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Regiao)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Ibge)
                .HasMaxLength(10);

            entity.Property(e => e.Ddd)
                .HasMaxLength(3);

            entity.HasIndex(e => e.Cep);
        });

        modelBuilder.Entity<PessoaFisica>(entity =>
        {
            entity.ToTable("PessoasFisicas");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Telefone)
                .HasMaxLength(20);

            entity.Property(e => e.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            entity.Property(e => e.Rg)
                .HasMaxLength(20);

            entity.Property(e => e.DataNascimento)
                .IsRequired();

            entity.HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Cpf).IsUnique();
            entity.HasIndex(e => e.Email);
        });

        modelBuilder.Entity<PessoaJuridica>(entity =>
        {
            entity.ToTable("PessoasJuridicas");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.RazaoSocial)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(200);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Telefone)
                .HasMaxLength(20);

            entity.Property(e => e.Cnpj)
                .IsRequired()
                .HasMaxLength(14);

            entity.Property(e => e.InscricaoEstadual)
                .HasMaxLength(20);

            entity.Property(e => e.DataAbertura)
                .IsRequired();

            entity.HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Cnpj).IsUnique();
            entity.HasIndex(e => e.Email);
        });
    }
}
