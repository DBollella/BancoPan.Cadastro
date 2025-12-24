namespace BancoPan.Cadastro.Domain.Entities;

public class PessoaFisica : Pessoa
{
    public string Cpf { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string? Rg { get; private set; }

    protected PessoaFisica() { }

    public PessoaFisica(
        string nome,
        string email,
        string cpf,
        DateTime dataNascimento,
        Guid enderecoId,
        string? telefone = null,
        string? rg = null)
        : base(nome, email, enderecoId, telefone)
    {
        ValidarCpf(cpf);
        ValidarDataNascimento(dataNascimento);

        Cpf = RemoverFormatacaoCpf(cpf);
        DataNascimento = dataNascimento;
        Rg = rg;
    }

    public void Atualizar(
        string nome,
        string email,
        DateTime dataNascimento,
        string? telefone = null,
        string? rg = null)
    {
        ValidarDataNascimento(dataNascimento);

        AtualizarDadosComuns(nome, email, telefone);
        DataNascimento = dataNascimento;
        Rg = rg;
        AtualizarDataModificacao();
    }

    private static void ValidarCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório", nameof(cpf));

        var cpfLimpo = RemoverFormatacaoCpf(cpf);

        if (cpfLimpo.Length != 11 || !cpfLimpo.All(char.IsDigit))
            throw new ArgumentException("CPF inválido", nameof(cpf));

        if (cpfLimpo.Distinct().Count() == 1)
            throw new ArgumentException("CPF inválido", nameof(cpf));

        var soma = 0;
        for (int i = 0; i < 9; i++)
            soma += (cpfLimpo[i] - '0') * (10 - i);

        var resto = soma % 11;
        var digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

        if (digitoVerificador1 != (cpfLimpo[9] - '0'))
            throw new ArgumentException("CPF inválido", nameof(cpf));

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += (cpfLimpo[i] - '0') * (11 - i);

        resto = soma % 11;
        var digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

        if (digitoVerificador2 != (cpfLimpo[10] - '0'))
            throw new ArgumentException("CPF inválido", nameof(cpf));
    }

    private static void ValidarDataNascimento(DateTime dataNascimento)
    {
        if (dataNascimento >= DateTime.Today)
            throw new ArgumentException("Data de nascimento deve ser anterior à data atual", nameof(dataNascimento));

        var idade = DateTime.Today.Year - dataNascimento.Year;
        if (dataNascimento.Date > DateTime.Today.AddYears(-idade))
            idade--;

        if (idade > 150)
            throw new ArgumentException("Data de nascimento inválida", nameof(dataNascimento));
    }

    private static string RemoverFormatacaoCpf(string cpf)
    {
        return cpf.Replace(".", "").Replace("-", "").Trim();
    }

    public string ObterCpfFormatado()
    {
        return $"{Cpf.Substring(0, 3)}.{Cpf.Substring(3, 3)}.{Cpf.Substring(6, 3)}-{Cpf.Substring(9)}";
    }

    public int CalcularIdade()
    {
        var idade = DateTime.Today.Year - DataNascimento.Year;
        if (DataNascimento.Date > DateTime.Today.AddYears(-idade))
            idade--;

        return idade;
    }
}
