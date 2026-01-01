using BancoPan.Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BancoPan.Cadastro.Infra.Data;

public class DataSeeder
{
    private readonly CadastroDbContext _context;

    public DataSeeder(CadastroDbContext context)
    {
        _context = context;
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

    private static string GerarCnpjValido(int seed)
    {
        var random = new Random(seed + 54321);
        var cnpj = new int[14];

        for (int i = 0; i < 12; i++)
            cnpj[i] = random.Next(0, 10);

        var multiplicadores1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicadores2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var soma = 0;
        for (int i = 0; i < 12; i++)
            soma += cnpj[i] * multiplicadores1[i];
        var resto = soma % 11;
        cnpj[12] = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += cnpj[i] * multiplicadores2[i];
        resto = soma % 11;
        cnpj[13] = resto < 2 ? 0 : 11 - resto;

        return string.Join("", cnpj);
    }

    public async Task SeedAsync()
    {
        _context.PessoasFisicas.RemoveRange(_context.PessoasFisicas);
        _context.PessoasJuridicas.RemoveRange(_context.PessoasJuridicas);
        _context.Enderecos.RemoveRange(_context.Enderecos);
        await _context.SaveChangesAsync();

        var enderecos = new List<Endereco>();
        var enderecoData = new[]
        {
            new { Cep = "01310100", Logradouro = "Avenida Paulista", Numero = "1000", Bairro = "Bela Vista", Localidade = "São Paulo", Uf = "SP", Estado = "São Paulo", Regiao = "Sudeste", Ibge = "3550308", Ddd = "11" },
            new { Cep = "20040020", Logradouro = "Avenida Rio Branco", Numero = "156", Bairro = "Centro", Localidade = "Rio de Janeiro", Uf = "RJ", Estado = "Rio de Janeiro", Regiao = "Sudeste", Ibge = "3304557", Ddd = "21" },
            new { Cep = "30130100", Logradouro = "Avenida Afonso Pena", Numero = "1500", Bairro = "Centro", Localidade = "Belo Horizonte", Uf = "MG", Estado = "Minas Gerais", Regiao = "Sudeste", Ibge = "3106200", Ddd = "31" },
            new { Cep = "40020000", Logradouro = "Avenida Sete de Setembro", Numero = "500", Bairro = "Centro", Localidade = "Salvador", Uf = "BA", Estado = "Bahia", Regiao = "Nordeste", Ibge = "2927408", Ddd = "71" },
            new { Cep = "80010000", Logradouro = "Rua XV de Novembro", Numero = "300", Bairro = "Centro", Localidade = "Curitiba", Uf = "PR", Estado = "Paraná", Regiao = "Sul", Ibge = "4106902", Ddd = "41" },
            new { Cep = "60010000", Logradouro = "Rua Barão do Rio Branco", Numero = "1200", Bairro = "Centro", Localidade = "Fortaleza", Uf = "CE", Estado = "Ceará", Regiao = "Nordeste", Ibge = "2304400", Ddd = "85" },
            new { Cep = "50010000", Logradouro = "Avenida Dantas Barreto", Numero = "800", Bairro = "São José", Localidade = "Recife", Uf = "PE", Estado = "Pernambuco", Regiao = "Nordeste", Ibge = "2611606", Ddd = "81" },
            new { Cep = "90010000", Logradouro = "Rua dos Andradas", Numero = "1234", Bairro = "Centro", Localidade = "Porto Alegre", Uf = "RS", Estado = "Rio Grande do Sul", Regiao = "Sul", Ibge = "4314902", Ddd = "51" },
            new { Cep = "70040000", Logradouro = "Setor Comercial Sul", Numero = "100", Bairro = "Asa Sul", Localidade = "Brasília", Uf = "DF", Estado = "Distrito Federal", Regiao = "Centro-Oeste", Ibge = "5300108", Ddd = "61" },
            new { Cep = "69010000", Logradouro = "Avenida Eduardo Ribeiro", Numero = "520", Bairro = "Centro", Localidade = "Manaus", Uf = "AM", Estado = "Amazonas", Regiao = "Norte", Ibge = "1302603", Ddd = "92" },
            new { Cep = "66010000", Logradouro = "Avenida Presidente Vargas", Numero = "900", Bairro = "Campina", Localidade = "Belém", Uf = "PA", Estado = "Pará", Regiao = "Norte", Ibge = "1501402", Ddd = "91" },
            new { Cep = "88010000", Logradouro = "Rua Felipe Schmidt", Numero = "700", Bairro = "Centro", Localidade = "Florianópolis", Uf = "SC", Estado = "Santa Catarina", Regiao = "Sul", Ibge = "4205407", Ddd = "48" },
            new { Cep = "78020000", Logradouro = "Avenida Getúlio Vargas", Numero = "1500", Bairro = "Centro", Localidade = "Cuiabá", Uf = "MT", Estado = "Mato Grosso", Regiao = "Centro-Oeste", Ibge = "5103403", Ddd = "65" },
            new { Cep = "79002000", Logradouro = "Rua 14 de Julho", Numero = "3200", Bairro = "Centro", Localidade = "Campo Grande", Uf = "MS", Estado = "Mato Grosso do Sul", Regiao = "Centro-Oeste", Ibge = "5002704", Ddd = "67" },
            new { Cep = "74010000", Logradouro = "Avenida Goiás", Numero = "800", Bairro = "Centro", Localidade = "Goiânia", Uf = "GO", Estado = "Goiás", Regiao = "Centro-Oeste", Ibge = "5208707", Ddd = "62" },
            new { Cep = "49010000", Logradouro = "Rua João Pessoa", Numero = "150", Bairro = "Centro", Localidade = "Aracaju", Uf = "SE", Estado = "Sergipe", Regiao = "Nordeste", Ibge = "2800308", Ddd = "79" },
            new { Cep = "57020000", Logradouro = "Rua do Comércio", Numero = "350", Bairro = "Centro", Localidade = "Maceió", Uf = "AL", Estado = "Alagoas", Regiao = "Nordeste", Ibge = "2704302", Ddd = "82" },
            new { Cep = "58010000", Logradouro = "Avenida Dom Pedro II", Numero = "450", Bairro = "Centro", Localidade = "João Pessoa", Uf = "PB", Estado = "Paraíba", Regiao = "Nordeste", Ibge = "2507507", Ddd = "83" },
            new { Cep = "64000000", Logradouro = "Rua Álvaro Mendes", Numero = "600", Bairro = "Centro", Localidade = "Teresina", Uf = "PI", Estado = "Piauí", Regiao = "Nordeste", Ibge = "2211001", Ddd = "86" },
            new { Cep = "65010000", Logradouro = "Rua Grande", Numero = "250", Bairro = "Centro", Localidade = "São Luís", Uf = "MA", Estado = "Maranhão", Regiao = "Nordeste", Ibge = "2111300", Ddd = "98" },
            new { Cep = "59010000", Logradouro = "Avenida Rio Branco", Numero = "720", Bairro = "Cidade Alta", Localidade = "Natal", Uf = "RN", Estado = "Rio Grande do Norte", Regiao = "Nordeste", Ibge = "2408102", Ddd = "84" },
            new { Cep = "76801000", Logradouro = "Avenida Brasília", Numero = "1800", Bairro = "Centro", Localidade = "Porto Velho", Uf = "RO", Estado = "Rondônia", Regiao = "Norte", Ibge = "1100205", Ddd = "69" },
            new { Cep = "69900000", Logradouro = "Avenida Getúlio Vargas", Numero = "950", Bairro = "Centro", Localidade = "Rio Branco", Uf = "AC", Estado = "Acre", Regiao = "Norte", Ibge = "1200401", Ddd = "68" },
            new { Cep = "69301000", Logradouro = "Avenida Ville Roy", Numero = "500", Bairro = "Centro", Localidade = "Boa Vista", Uf = "RR", Estado = "Roraima", Regiao = "Norte", Ibge = "1400100", Ddd = "95" },
            new { Cep = "68900000", Logradouro = "Rua São José", Numero = "1100", Bairro = "Centro", Localidade = "Macapá", Uf = "AP", Estado = "Amapá", Regiao = "Norte", Ibge = "1600303", Ddd = "96" },
            new { Cep = "77001000", Logradouro = "Avenida Tocantins", Numero = "400", Bairro = "Centro", Localidade = "Palmas", Uf = "TO", Estado = "Tocantins", Regiao = "Norte", Ibge = "1721000", Ddd = "63" },
            new { Cep = "01310200", Logradouro = "Rua Augusta", Numero = "2000", Bairro = "Consolação", Localidade = "São Paulo", Uf = "SP", Estado = "São Paulo", Regiao = "Sudeste", Ibge = "3550308", Ddd = "11" },
            new { Cep = "22041001", Logradouro = "Avenida Atlântica", Numero = "1500", Bairro = "Copacabana", Localidade = "Rio de Janeiro", Uf = "RJ", Estado = "Rio de Janeiro", Regiao = "Sudeste", Ibge = "3304557", Ddd = "21" },
            new { Cep = "30190000", Logradouro = "Rua da Bahia", Numero = "1200", Bairro = "Centro", Localidade = "Belo Horizonte", Uf = "MG", Estado = "Minas Gerais", Regiao = "Sudeste", Ibge = "3106200", Ddd = "31" },
            new { Cep = "13010000", Logradouro = "Rua Barão de Jaguara", Numero = "1500", Bairro = "Centro", Localidade = "Campinas", Uf = "SP", Estado = "São Paulo", Regiao = "Sudeste", Ibge = "3509502", Ddd = "19" }
        };

        for (int i = 0; i < 30; i++)
        {
            var data = enderecoData[i];
            var endereco = new Endereco(
                data.Cep,
                data.Logradouro,
                data.Numero,
                data.Bairro,
                data.Localidade,
                data.Uf,
                data.Estado,
                data.Regiao,
                data.Ibge,
                data.Ddd,
                i % 3 == 0 ? $"Apto {(i + 1) * 10}" : null
            );
            enderecos.Add(endereco);
        }

        _context.Enderecos.AddRange(enderecos);
        await _context.SaveChangesAsync();

        var pessoasFisicas = new List<PessoaFisica>();
        var nomes = new[]
        {
            "João da Silva", "Maria Santos", "Pedro Oliveira", "Ana Costa", "Carlos Souza",
            "Juliana Ferreira", "Rafael Almeida", "Fernanda Lima", "Lucas Rodrigues", "Mariana Pereira",
            "Bruno Carvalho", "Camila Martins", "Diego Araújo", "Letícia Barbosa", "Gustavo Ribeiro",
            "Patrícia Gomes", "Rodrigo Dias", "Amanda Cardoso", "Felipe Nascimento", "Vanessa Rocha",
            "Thiago Castro", "Larissa Correia", "Marcelo Pinto", "Renata Vieira", "André Mendes",
            "Carolina Freitas", "Fabio Cunha", "Beatriz Moreira", "Leonardo Teixeira", "Gabriela Monteiro"
        };

        for (int i = 0; i < 30; i++)
        {
            var pessoaFisica = new PessoaFisica(
                nomes[i],
                $"{nomes[i].ToLower().Replace(" ", ".")}@email.com",
                GerarCpfValido(i),
                DateTime.Now.AddYears(-(20 + i)),
                enderecos[i].Id,
                $"(11) 9{8000 + i}-{1000 + i}",
                $"MG-{10000000 + i}"
            );
            pessoasFisicas.Add(pessoaFisica);
        }

        _context.PessoasFisicas.AddRange(pessoasFisicas);
        await _context.SaveChangesAsync();

        var pessoasJuridicas = new List<PessoaJuridica>();
        var nomesEmpresas = new[]
        {
            "Tech Solutions Ltda", "Comercial Alvorada", "Serviços Master", "Industria Brasil SA", "Distribuidora Central",
            "Consultoria Premium", "Transportes Rapido", "Alimentação Saudável", "Construções Modernas", "Marketing Digital Plus",
            "Logística Express", "Farmácia Popular", "Livraria Cultural", "Auto Peças Nacional", "Eletrônicos Premium",
            "Moda & Estilo", "Restaurante Sabor", "Hotel Conforto", "Academia Fitness", "Escola Futuro",
            "Clínica Saúde Total", "Advocacia & Direito", "Contabilidade Exata", "Engenharia Precisa", "Arquitetura Criativa",
            "Software House Tech", "Games & Diversão", "Eventos & Festas", "Turismo Aventura", "Imobiliária Prime"
        };

        for (int i = 0; i < 30; i++)
        {
            var pessoaJuridica = new PessoaJuridica(
                nomesEmpresas[i],
                $"contato@{nomesEmpresas[i].ToLower().Replace(" ", "").Replace("&", "")}.com.br",
                GerarCnpjValido(i),
                DateTime.Now.AddYears(-(5 + (i % 15))),
                enderecos[i].Id,
                i % 2 == 0 ? $"{nomesEmpresas[i].Split(' ')[0]}" : null,
                $"(11) 3{3000 + i}-{2000 + i}",
                i % 2 == 0 ? $"1234567890{i}" : null
            );
            pessoasJuridicas.Add(pessoaJuridica);
        }

        _context.PessoasJuridicas.AddRange(pessoasJuridicas);
        await _context.SaveChangesAsync();
    }
}
