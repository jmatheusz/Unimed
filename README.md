# Desafio Unimed - katas 1, 2, 3 e 4

Este repositório contém as soluções desenvolvidas para os desafios propostos, abrangendo desde o desenvolvimento de sistemas médicos até análise de dados financeiros e planejamento de infraestrutura.

## Informações do Candidato
- **Nome:** José Matheus De Oliveira Guimarães
- **Telefone:** (81) 99289-8400
- **Email:** jmatheusguimaraes07@gmail.com

---

## 🛠 Stack(s) Utilizada(s) e Justificativa

### **katas 1 e 2 (Sistemas Web)**
- **Back-End:** .NET Core / C#
  - *Justificativa:* Escolhido por ser uma plataforma extremamente robusta, com tipagem forte e alta performance, ideal para sistemas que exigem integridade de dados (como triagem médica e gestão de tarefas).
- **Front-End:** React (JavaScript/TypeScript)
  - *Justificativa:* Utilizado para criar interfaces dinâmicas, responsivas e baseadas em componentes, garantindo uma melhor experiência do usuário.

### **kata 3 (Engenharia e Processos)**
- **Foco:** Diagnóstico de Sistemas Legados e Planejamento.
  - *Justificativa:* A análise focou em estratégias de **Refatoração Incremental (Strangler Pattern)** e automação de processos via **GitHub Actions**, priorizando a estabilidade sem interromper a operação.

### **kata 4 (Data Pipeline)**
- **Stack:** Python + Pandas
  - *Justificativa:* O Python é a linguagem líder em ciência de dados. A biblioteca **Pandas** foi escolhida pela facilidade de manipulação de grandes volumes de dados (CSVs), limpeza de strings e cálculos estatísticos eficientes.

---

## 🚀 Instruções para Executar Localmente

### **kata 1 e kata 2**
Certifique-se de ter o **.NET SDK** e o **Node.js** instalados.

1.  **Back-End:**
    - Navegue até a pasta do backend (`cd kata-1/Back-End/src` ou `cd kata-2/backend/TaskFlow.Api`).
    - Execute: `dotnet run`
2.  **Front-End:**
    - Navegue até a pasta `frontend`.
    - Instale as dependências: `npm install`
    - Execute: `npm start`

### **kata 4**
Certifique-se de ter o **Python 3.x** instalado.

1.  Navegue até a pasta `kata-4`.
2.  Instale a biblioteca Pandas:
    ```bash
    pip install pandas
    ```
3.  Execute o pipeline:
    ```bash
    python pipeline.py
    ```
    *Isso gerará o arquivo `relatorio_consolidado.csv` e exibirá os indicadores no terminal.*

---

## 💬 Comentários Livres: O que eu faria diferente com mais tempo?

1.  **Containerização:** Implementaria **Docker** em todos os projetos para garantir que o ambiente de execução seja idêntico em qualquer máquina, facilitando o deploy.
2.  **Testes Automatizados:** Ampliaria a cobertura de testes unitários e de integração (xUnit no C# e Pytest no Python) para garantir que novas funcionalidades não quebrem o que já existe.
3.  **Documentação de API:** Utilizaria **Swagger/OpenAPI** no Back-End para fornecer uma documentação interativa dos endpoints, facilitando a integração com o front-end.
4.  **Autenticação Centralizada:** Implementaria um sistema de autenticação via **JWT** unificado para os sistemas.
5.  **Monitoramento:** No kata-4, em vez de apenas CSVs, eu integraria com um banco de dados SQL e utilizaria ferramentas como **PowerBI ou Grafana** para visualizar os indicadores em tempo real.
