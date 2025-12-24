namespace BancoPan.Cadastro.Domain.Entities;

public abstract class Pessoa : Entity
{
    public string Nome { get; protected set; }
    public string Email { get; protected set; }
    public string? Telefone { get; protected set; }
    public Guid EnderecoId { get; protected set; }
    public virtual Endereco Endereco { get; protected set; }

    protected Pessoa() { }

    protected Pessoa(string nome, string email, Guid enderecoId, string? telefone = null)
    {
        ValidarNome(nome);
        ValidarEmail(email);

        Nome = nome;
        Email = email;
        Telefone = telefone;
        EnderecoId = enderecoId;
    }

    public void AtualizarDadosComuns(string nome, string email, string? telefone = null)
    {
        ValidarNome(nome);
        ValidarEmail(email);

        Nome = nome;
        Email = email;
        Telefone = telefone;
        AtualizarDataModificacao();
    }

    public void AtualizarEndereco(Guid enderecoId)
    {
        if (enderecoId == Guid.Empty)
            throw new ArgumentException("EnderecoId inválido", nameof(enderecoId));

        EnderecoId = enderecoId;
        AtualizarDataModificacao();
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (nome.Length < 3)
            throw new ArgumentException("Nome deve ter no mínimo 3 caracteres", nameof(nome));
    }

    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        var atIndex = email.IndexOf('@');
        var dotIndex = email.LastIndexOf('.');

        if (atIndex <= 0 || dotIndex <= atIndex || dotIndex >= email.Length - 1)
            throw new ArgumentException("Email inválido", nameof(email));
    }
}
