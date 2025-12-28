# Script de Preparação do Ambiente - Banco Pan Cadastro
# Executa todos os passos necessários para rodar o docker-compose

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  PREPARAÇÃO DO AMBIENTE - BANCO PAN CADASTRO" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Verificar instalação do Docker
Write-Host "[1/8] Verificando instalação do Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "[OK] Docker instalado: $dockerVersion" -ForegroundColor Green
    } else {
        throw "Docker não encontrado"
    }
} catch {
    Write-Host "[ERRO] Docker não está instalado!" -ForegroundColor Red
    Write-Host "Por favor, instale o Docker Desktop de: https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
    pause
    exit 1
}
Write-Host ""

# 2. Verificar se Docker Desktop está rodando
Write-Host "[2/8] Verificando se Docker Desktop está rodando..." -ForegroundColor Yellow
try {
    docker info 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERRO] Docker Desktop não está rodando!" -ForegroundColor Red
        Write-Host "Por favor, inicie o Docker Desktop e aguarde até estar completamente iniciado." -ForegroundColor Yellow
        Write-Host "Pressione qualquer tecla quando o Docker Desktop estiver rodando..." -ForegroundColor Yellow
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

        # Verificar novamente
        docker info 2>&1 | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-Host "[ERRO] Docker Desktop ainda não está acessível." -ForegroundColor Red
            pause
            exit 1
        }
    }
    Write-Host "[OK] Docker Desktop está rodando" -ForegroundColor Green
} catch {
    Write-Host "[ERRO] Falha ao verificar Docker Desktop" -ForegroundColor Red
    pause
    exit 1
}
Write-Host ""

# 3. Limpar containers e volumes antigos
Write-Host "[3/8] Limpando containers e volumes antigos..." -ForegroundColor Yellow
docker-compose down -v 2>&1 | Out-Null
Write-Host "[OK] Ambiente limpo" -ForegroundColor Green
Write-Host ""

# 4. Verificar arquivos necessários
Write-Host "[4/8] Verificando arquivos necessários..." -ForegroundColor Yellow
if (-not (Test-Path "docker-compose.yml")) {
    Write-Host "[ERRO] Arquivo docker-compose.yml não encontrado!" -ForegroundColor Red
    pause
    exit 1
}
if (-not (Test-Path "Dockerfile")) {
    Write-Host "[ERRO] Arquivo Dockerfile do backend não encontrado!" -ForegroundColor Red
    pause
    exit 1
}

$frontendPath = "..\Front\banco-pan-cadastro"
if (-not (Test-Path $frontendPath)) {
    Write-Host "[AVISO] Pasta do frontend não encontrada em: $frontendPath" -ForegroundColor Yellow
    Write-Host "[AVISO] O frontend não será construído" -ForegroundColor Yellow
    $buildFrontend = $false
} else {
    if (-not (Test-Path "$frontendPath\Dockerfile")) {
        Write-Host "[AVISO] Dockerfile do frontend não encontrado" -ForegroundColor Yellow
        Write-Host "[AVISO] O frontend não será construído" -ForegroundColor Yellow
        $buildFrontend = $false
    } else {
        $buildFrontend = $true
    }
}
Write-Host "[OK] Arquivos do backend encontrados" -ForegroundColor Green
Write-Host ""

# 5. Fazer pull das imagens base necessárias
Write-Host "[5/8] Baixando imagens base do Docker Hub..." -ForegroundColor Yellow
Write-Host "Baixando SQL Server..." -ForegroundColor Cyan
docker pull mcr.microsoft.com/mssql/server:2019-latest
if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERRO] Falha ao baixar imagem do SQL Server" -ForegroundColor Red
    pause
    exit 1
}

Write-Host "Baixando Redis..." -ForegroundColor Cyan
docker pull redis:alpine
if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERRO] Falha ao baixar imagem do Redis" -ForegroundColor Red
    pause
    exit 1
}

Write-Host "[OK] Imagens base baixadas com sucesso" -ForegroundColor Green
Write-Host ""

# 6. Construir imagem do Frontend
if ($buildFrontend) {
    Write-Host "[6/8] Construindo imagem do Frontend..." -ForegroundColor Yellow
    Write-Host "Este processo pode demorar 5-10 minutos na primeira execução..." -ForegroundColor Cyan

    Push-Location $frontendPath
    docker build -t bancopan-frontend .
    $buildResult = $LASTEXITCODE
    Pop-Location

    if ($buildResult -ne 0) {
        Write-Host "[ERRO] Falha ao construir imagem do frontend!" -ForegroundColor Red
        Write-Host "Verifique os logs acima para mais detalhes." -ForegroundColor Yellow
        pause
        exit 1
    }
    Write-Host "[OK] Imagem do frontend construída com sucesso" -ForegroundColor Green
} else {
    Write-Host "[6/8] Pulando construção do Frontend (não disponível)..." -ForegroundColor Yellow
}
Write-Host ""

# 7. Construir e inicializar containers do Backend
Write-Host "[7/8] Construindo e iniciando containers do Backend..." -ForegroundColor Yellow
Write-Host "Este processo pode demorar 3-5 minutos na primeira execução..." -ForegroundColor Cyan
Write-Host ""
docker-compose up -d --build
if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "[ERRO] Falha ao construir ou iniciar containers!" -ForegroundColor Red
    Write-Host "Verifique os logs acima para mais detalhes." -ForegroundColor Yellow
    pause
    exit 1
}
Write-Host ""
Write-Host "[OK] Containers iniciados" -ForegroundColor Green
Write-Host ""

# 8. Aguardar containers ficarem saudáveis
Write-Host "[8/8] Aguardando containers ficarem saudáveis..." -ForegroundColor Yellow
Write-Host "Isso pode levar até 2 minutos..." -ForegroundColor Cyan
Start-Sleep -Seconds 15

# Verificar status dos containers
Write-Host ""
Write-Host "Status dos containers:" -ForegroundColor White
docker ps --format "table {{.Names}}`t{{.Status}}" | Select-String "bancopan"
Write-Host ""

# Verificar se a API está respondendo
Write-Host "Testando API..." -ForegroundColor Yellow
Start-Sleep -Seconds 5
$apiHealthy = $false
for ($i = 1; $i -le 3; $i++) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5000/health" -UseBasicParsing -TimeoutSec 5 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            Write-Host "[OK] API está respondendo" -ForegroundColor Green
            $apiHealthy = $true
            break
        }
    } catch {
        if ($i -lt 3) {
            Write-Host "[AVISO] Tentativa $i/3 - Aguardando API iniciar..." -ForegroundColor Yellow
            Start-Sleep -Seconds 10
        }
    }
}

if (-not $apiHealthy) {
    Write-Host "[AVISO] API ainda não está respondendo após 3 tentativas" -ForegroundColor Yellow
    Write-Host "Verifique os logs com: docker-compose logs api" -ForegroundColor Cyan
}

# Verificar se o Frontend está respondendo (se foi construído)
if ($buildFrontend) {
    Write-Host ""
    Write-Host "Testando Frontend..." -ForegroundColor Yellow
    Start-Sleep -Seconds 3
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:4200" -UseBasicParsing -TimeoutSec 5 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            Write-Host "[OK] Frontend está respondendo" -ForegroundColor Green
        }
    } catch {
        Write-Host "[AVISO] Frontend ainda não está respondendo" -ForegroundColor Yellow
        Write-Host "Verifique os logs com: docker-compose logs frontend" -ForegroundColor Cyan
    }
}
Write-Host ""

# Resumo
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  AMBIENTE PREPARADO COM SUCESSO!" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Serviços disponíveis:" -ForegroundColor White
Write-Host "  - API Backend:    http://localhost:5000" -ForegroundColor Green
Write-Host "  - Swagger UI:     http://localhost:5000" -ForegroundColor Green
if ($buildFrontend) {
    Write-Host "  - Frontend:       http://localhost:4200" -ForegroundColor Green
}
Write-Host "  - SQL Server:     localhost:1433" -ForegroundColor Green
Write-Host "  - Redis:          localhost:6379" -ForegroundColor Green
Write-Host ""
Write-Host "Credenciais SQL Server:" -ForegroundColor White
Write-Host "  - Usuário: sa" -ForegroundColor Cyan
Write-Host "  - Senha: BancoPan@2025" -ForegroundColor Cyan
Write-Host ""
Write-Host "Comandos úteis:" -ForegroundColor White
Write-Host "  Ver logs:              docker-compose logs -f [service_name]" -ForegroundColor Cyan
Write-Host "  Parar containers:      docker-compose down" -ForegroundColor Cyan
Write-Host "  Parar e limpar dados:  docker-compose down -v" -ForegroundColor Cyan
Write-Host ""
Write-Host "Imagens Docker criadas:" -ForegroundColor White
docker images | Select-String "bancopan"
Write-Host ""
pause
