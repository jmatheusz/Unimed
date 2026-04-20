# TaskFlow - Edição Profissional (Kata 2)

Este repositório contém a versão profissional do Painel de Tarefas, utilizando uma arquitetura moderna e escalável.

## Estrutura do Projeto

*   **`/backend`**: API REST desenvolvida em **ASP.NET Core (C#)**. Utiliza Controllers, Services e persistência em JSON.
*   **`/frontend`**: Aplicação Single Page (SPA) desenvolvida em **React + TypeScript** com Vite. Design premium e responsivo.
*   **`/scripts`**: Scripts de automação e análise em **Python**. Inclui um analisador de estatísticas de tarefas.

## Como Executar

### 1. Backend (.NET)
```bash
cd backend/TaskFlow.Api
dotnet run --urls=http://localhost:5000
```

### 2. Frontend (React)
```bash
cd frontend
npm install
npm run dev
```
Acesse em: `http://localhost:5173`

### 3. Análise de Dados (Python)
```bash
python scripts/analytics.py
```

## Engenharia e Decisões
- **C#/.NET**: Escolhido pela robustez, tipagem forte e facilidade de criação de APIs RESTful.
- **React/TS**: Garante uma interface reativa, modular e com segurança de tipos durante o desenvolvimento.
- **Python**: Utilizado para processamento de dados "off-line", simulando um cenário real de análise de métricas de produtividade.
