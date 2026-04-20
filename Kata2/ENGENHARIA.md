# Engenharia e Arquitetura - TaskFlow (Stack Profissional)

Este documento detalha as decisões técnicas e respostas de engenharia para o Kata 2.

## 1. Decisões de Arquitetura no Backend

Para a versão profissional em **C# / .NET 8**, adotei uma arquitetura baseada em **Camadas de Responsabilidade**:

*   **Camada de Controladores (Controllers):** O `TasksController` é responsável apenas por receber as requisições, validar o formato básico e retornar os códigos HTTP corretos (200, 201, 204, 404). Ele não conhece os detalhes de como os dados são salvos.
*   **Camada de Serviço (Services):** O `TaskService` encapsula toda a lógica de negócio e persistência. Ele gerencia a leitura e escrita no arquivo `database.json`. Esta separação permite que possamos migrar para um banco SQL apenas alterando o Service, sem tocar na API.
*   **Modelos e DTOs:** Utilizamos classes separadas para representar a entidade no banco (`TaskItem`) e os objetos de transferência de dados (`TaskRequest`, `TaskUpdateRequest`), garantindo que a API não exponha campos internos desnecessários.

## 2. Confiabilidade e Observabilidade em Produção

Para garantir que esta API seja confiável em um cenário real, implementaria:

1.  **Observabilidade com Logs Estruturados:** Utilização do `Serilog` para gravar logs em formato JSON. Isso permite rastrear erros e performance de forma muito mais eficiente do que simples textos.
2.  **Health Checks:** Implementação do middleware nativo de Health Checks do .NET. Isso permite que ferramentas de monitoramento (como Prometheus ou Kubernetes) saibam instantaneamente se o serviço está operante ou se houve falha no acesso ao sistema de arquivos.
3.  **Dependency Injection (DI):** Embora neste Kata tenhamos instanciado o service manualmente para agilidade, em produção usaríamos o Injetor de Dependência nativo do .NET para gerenciar o ciclo de vida dos objetos e facilitar testes unitários.

## 3. Escalabilidade: Múltiplos Usuários e Autenticação

Caso o sistema precisasse suportar múltiplos usuários autenticados, as mudanças seriam:

*   **Identidade:** Implementação do **ASP.NET Core Identity** com tokens **JWT**.
*   **Segurança:** Adição de um atributo `[Authorize]` nos controladores para proteger as rotas.
*   **Isolamento de Dados:**
    *   Cada tarefa teria um campo `UserId`.
    *   Todas as consultas no `TaskService` seriam modificadas para incluir `where UserId == currentUserId`.
*   **Banco de Dados:** Substituição do JSON por um banco relacional como **PostgreSQL**, permitindo transações seguras e suporte a milhares de acessos simultâneos.
