# BancoPan.Cadastro 

## Para executar o Projeto ler: README EXECUTANDO O PROJETO.md

<img width="1170" height="671" alt="image" src="https://github.com/user-attachments/assets/d22d4c2e-f87a-491a-8d81-299a95143f8f" />

## Tecnologias Utilizadas Back-End
- **.NET 8**: Framework principal.
- **ASP.NET Core Web API**: Framework web.
- **Entity Framework Core 8**: ORM.
- **SQL Server**: Banco de dados relacional.
- **Redis**: Banco de dados não relacional utilizado para o cache.
- **xUnit**: Framework de testes.
- **NSubstitute**: Framework para mockar dependencias dos testes.
- **Swagger**: Documentação da API.

## Tecnologias Utilizadas Front-End
  - **Angular 21**: O Angular é um framework amplamente utilizado para criar aplicações web dinâmicas e interativas.
  - **PrimeNG 17**: Biblioteca de componentes Angular para interfaces elegantes e interativas.

## Integrações
- **ViaCep** Consulta de endereços para facilitar o preenchimento do usuario.

## Lazy Load
  Implementado paginação e cache.

## CRUD
  Criado implementação do CRUD para Endereço, Pessoa Fisica e Pessoa Juridica.

## Design Patterns Utilizados
### 1. Repository Pattern
Abstração da camada de acesso a dados, permitindo fácil substituição da implementação.

```csharp
public interface IRepository<T> where T : Entity
{
    Task<T?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<T>> ObterTodosAsync();
    Task<PagedResult<T>> ObterPaginadoAsync(PaginationParameters parameters);
    Task AdicionarAsync(T entity);
    Task AtualizarAsync(T entity);
    Task RemoverAsync(Guid id);
}
```

### 2. Unit of Work
Gerenciamento de transações e coordenação entre múltiplos repositórios.

```csharp
public interface IUnitOfWork : IDisposable
{
    IPessoaFisicaRepository PessoasFisicas { get; }
    IPessoaJuridicaRepository PessoasJuridicas { get; }
    IEnderecoRepository Enderecos { get; }
    Task<int> CommitAsync();
}
```

### 3. Dependency Injection
Inversão de controle para gerenciamento de dependências.

### 4. Template Method
Na classe base `Pessoa` que define a estrutura de validação e atualização, permitindo que `PessoaFisica` e `PessoaJuridica` especializem o comportamento.

## Melhorias Possíveis 

### 1. CQRS (Command Query Responsibility Segregation)
Separar as operações de leitura e escrita em diferentes modelos:
- **Vantagens**: Melhor performance, escalabilidade independente
- **Quando usar**: Sistemas com alta carga de leitura ou complexidade

### 2. Mediator Pattern (MediatR)
Desacoplar os controllers dos serviços de aplicação:
```csharp
public class CriarPessoaFisicaCommand : IRequest<PessoaFisicaDto>
{
    public CriarPessoaFisicaDto Dto { get; set; }
}
```




 
