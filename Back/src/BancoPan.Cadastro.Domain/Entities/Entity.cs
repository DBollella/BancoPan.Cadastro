namespace BancoPan.Cadastro.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CriadoEm { get; protected set; }
    public DateTime? AtualizadoEm { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
    }

    protected void AtualizarDataModificacao()
    {
        AtualizadoEm = DateTime.UtcNow;
    }
}
