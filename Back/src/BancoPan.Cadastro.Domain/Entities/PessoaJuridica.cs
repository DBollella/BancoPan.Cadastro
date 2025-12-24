namespace BancoPan.Cadastro.Domain.Entities;

public class PessoaJuridica : Pessoa
{
    public string Cnpj { get; private set; }
    public string RazaoSocial { get; private set; }
    public string? NomeFantasia { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public string? InscricaoEstadual { get; private set; }

    protected PessoaJuridica() { }

    public PessoaJuridica(
        string razaoSocial,
        string email,
        string cnpj,
        DateTime dataAbertura,
        Guid enderecoId,
        string? nomeFantasia = null,
        string? telefone = null,
        string? inscricaoEstadual = null)
        : base(razaoSocial, email, enderecoId, telefone)
    {
        ValidarCnpj(cnpj);
        ValidarDataAbertura(dataAbertura);
        ValidarRazaoSocial(razaoSocial);

        Cnpj = RemoverFormatacaoCnpj(cnpj);
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        DataAbertura = dataAbertura;
        InscricaoEstadual = inscricaoEstadual;
    }

    public void Atualizar(
        string razaoSocial,
        string email,
        DateTime dataAbertura,
        string? nomeFantasia = null,
        string? telefone = null,
        string? inscricaoEstadual = null)
    {
        ValidarDataAbertura(dataAbertura);
        ValidarRazaoSocial(razaoSocial);

        AtualizarDadosComuns(razaoSocial, email, telefone);
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        DataAbertura = dataAbertura;
        InscricaoEstadual = inscricaoEstadual;
        AtualizarDataModificacao();
    }

    private static void ValidarCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ é obrigatório", nameof(cnpj));

        var cnpjLimpo = RemoverFormatacaoCnpj(cnpj);

        if (cnpjLimpo.Length != 14 || !cnpjLimpo.All(char.IsDigit))
            throw new ArgumentException("CNPJ inválido", nameof(cnpj));

        if (cnpjLimpo.Distinct().Count() == 1)
            throw new ArgumentException("CNPJ inválido", nameof(cnpj));

        var multiplicadores1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicadores2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var soma = 0;
        for (int i = 0; i < 12; i++)
            soma += (cnpjLimpo[i] - '0') * multiplicadores1[i];

        var resto = soma % 11;
        var digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

        if (digitoVerificador1 != (cnpjLimpo[12] - '0'))
            throw new ArgumentException("CNPJ inválido", nameof(cnpj));

        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += (cnpjLimpo[i] - '0') * multiplicadores2[i];

        resto = soma % 11;
        var digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

        if (digitoVerificador2 != (cnpjLimpo[13] - '0'))
            throw new ArgumentException("CNPJ inválido", nameof(cnpj));
    }

    private static void ValidarDataAbertura(DateTime dataAbertura)
    {
        if (dataAbertura >= DateTime.Today)
            throw new ArgumentException("Data de abertura deve ser anterior à data atual", nameof(dataAbertura));

        if (dataAbertura.Year < 1800)
            throw new ArgumentException("Data de abertura inválida", nameof(dataAbertura));
    }

    private static void ValidarRazaoSocial(string razaoSocial)
    {
        if (string.IsNullOrWhiteSpace(razaoSocial))
            throw new ArgumentException("Razão Social é obrigatória", nameof(razaoSocial));

        if (razaoSocial.Length < 3)
            throw new ArgumentException("Razão Social deve ter no mínimo 3 caracteres", nameof(razaoSocial));
    }

    private static string RemoverFormatacaoCnpj(string cnpj)
    {
        return cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
    }

    public string ObterCnpjFormatado()
    {
        return $"{Cnpj.Substring(0, 2)}.{Cnpj.Substring(2, 3)}.{Cnpj.Substring(5, 3)}/{Cnpj.Substring(8, 4)}-{Cnpj.Substring(12)}";
    }

    public int CalcularTempoAtuacao()
    {
        return DateTime.Today.Year - DataAbertura.Year;
    }
}
