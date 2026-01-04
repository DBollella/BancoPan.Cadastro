# README EXECUTANDO O PROJETO - Banco Pan Cadastro

Guia rápido para iniciar o ambiente de desenvolvimento usando Docker Compose.

## Pré-requisitos

- **Docker Desktop** instalado e rodando
- Portas disponíveis: `1433`, `5000`, `4200`, `6379`

## Iniciar Ambiente - FORMA MAIS RÁPIDA

Obs: Executar comandos no PowerShell

```bash
cd Back
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
.\PREPARAR-AMBIENTE
```
O script faz tudo automaticamente: verifica Docker, limpa ambiente antigo, constrói imagens e inicia todos os serviços.

Ao subir a API base de dados é atualizada com a estrutura atual das tabelas.

Para facilitar a navegação do sistema, adicionei 2 Endpoints para a criação de massa de dados:

```bash
cd Back
iwr -Uri "http://localhost:5000/api/Seed/execute" -Method POST -ContentType "application/json"
iwr -Uri "http://localhost:5000/api/Seed/execute-1000-pessoas-fisicas" -Method POST -ContentType "application/json"
```

## Iniciar Ambiente - FORMA MANUAL

# 1. Build do Frontend
cd "Front/banco-pan-cadastro"
docker build -t bancopan-frontend .

# 2. Iniciar todos os serviços
cd ../../Back
docker-compose up -d --build
```

## Acessar os Serviços

| Serviço    | URL/Host              | Credenciais                            |  
|------------|-----------------------|----------------------------------------|
| Frontend   | http://localhost:4200 |                     -                  |
| API Backend| http://localhost:5000 |                     -                  |
| SQL Server | `localhost:1433`      | User: `sa` / Password: `BancoPan@2025` |
| Redis      | `localhost:6379`      |                     -                  |

**Ambiente**: QA (`ASPNETCORE_ENVIRONMENT=QA`)

## Comandos Úteis

```bash
# Ver logs
docker-compose logs -f

# Ver status
docker-compose ps

# Parar serviços
docker-compose down

# Parar e remover dados
docker-compose down -v

# Apenas SQL Server (dev)
docker-compose -f docker-compose.dev.yml up -d
```

## Troubleshooting

**Porta em uso:**
```bash
netstat -ano | findstr :5000
```

**Erro de conexão SQL Server:**
```bash
docker-compose logs sqlserver
```

**Recomeçar do zero:**
```bash
docker-compose down -v
docker image prune -a
cd "../Front/banco-pan-cadastro"
docker build -t bancopan-frontend .
cd "../../Back"
docker-compose up -d --build
```
