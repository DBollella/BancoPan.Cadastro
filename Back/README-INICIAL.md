# README INICIAL - Banco Pan Cadastro

Guia rápido para iniciar o ambiente de desenvolvimento usando Docker Compose.

## Pré-requisitos

- **Docker Desktop** instalado e rodando
- Portas disponíveis: `1433`, `5000`, `4200`, `6379`

## Iniciar Ambiente - FORMA MAIS RÁPIDA

```bash
cd Back
PREPARAR-AMBIENTE.bat
```

O script faz tudo automaticamente: verifica Docker, limpa ambiente antigo, constrói imagens e inicia todos os serviços.

## Iniciar Ambiente - FORMA MANUAL

```bash
# 1. Build do Frontend
cd "Front/banco-pan-cadastro"
docker build -t bancopan-frontend .

# 2. Iniciar todos os serviços
cd ../../Back
docker-compose up -d --build
```

## Acessar os Serviços

| Serviço | URL/Host | Credenciais |
|---------|----------|-------------|
| Frontend | http://localhost:4200 | - |
| API Backend | http://localhost:5000 | - |
| SQL Server | `localhost:1433` | User: `sa` / Password: `BancoPan@2025` |
| Redis | `localhost:6379` | - |

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
