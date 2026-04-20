# Plano de Estabilização e Modernização Técnica - Kata 3

**Contexto:** Sistema de E-commerce / Logística (5 anos de operação, ~800 pedidos/dia).
**Objetivo:** Diagnóstico de incidentes críticos e proposta de plano de ação para recuperação da saúde do sistema e confiabilidade da operação.

---

## Seção 1 — Diagnóstico Detalhado

Abaixo, os 5 problemas identificados classificados por impacto e urgência.

| Problema | Causa Raiz Mais Provável | Risco Técnico e de Negócio | Classificação (Eisenhower) |
| :--- | :--- | :--- | :--- |
| **Lentidão no Checkout (8-12s)** | Consultas SQL ineficientes (falta de índices) ou processamento síncrono de tarefas pesadas. | **Risco:** Abandono de compra e sobrecarga de infraestrutura. | **Urgente e Importante** |
| **Pedidos Duplicados** | Falta de controle de concorrência ou mecanismos de idempotência no banco de dados. | **Risco:** Prejuízo financeiro direto, estornos e perda de confiança do cliente. | **Urgente e Importante** |
| **Hotfix direto em Produção** | Ausência de processos de CI/CD e falta de restrição de acesso ao ambiente produtivo. | **Risco:** Instabilidade sistêmica e falta de rastreabilidade de bugs. | **Urgente e Importante** |
| **Arquivo de Negócio Gigante (+4k linhas)** | Acúmulo de lógica (Complexidade Ciclomática alta) sem separação de responsabilidades. | **Risco:** Manutenção extremamente lenta e alto risco de regressões indesejadas. | **Importante (Prioridade Médica)** |
| **Ausência de Testes** | Cultura de desenvolvimento focada apenas em entregas rápidas, negligenciando qualidade. | **Risco:** Ciclo de feedback lento e insegurança total para qualquer refatoração. | **Importante (Base da Estratégia)** |

---

## Seção 2 — Plano de Ação Prioritário

Para estabilizar o sistema, focaremos nestas três frentes iniciais:

### 1. Eliminação de Pedidos Duplicados (Integridade)
*   **Ação Técnica:** Implementar `Unique Constraints` no banco de dados para chaves de transação e introduzir o conceito de "Idempotency Key" nas requisições da API.
*   **Estimativa de Esforço:** 2 dias (Inclui análise de logs para identificar o ponto exato da falha).
*   **Critério de Sucesso:** Zero novos pedidos duplicados registrados nos logs de auditoria após 7 dias.

### 2. Otimização de Performance (Experiência do Usuário)
*   **Ação Técnica:** Realizar *Profiling* do endpoint lento para encontrar a "query gargalo". Aplicar índices ou transformar processamentos pesados em assíncronos.
*   **Estimativa de Esforço:** 3 a 4 dias.
*   **Critério de Sucesso:** Tempo médio de resposta reduzido para menos de 1 segundo (P95 < 1s).

### 3. Implementação de Fluxo de Trabalho Seguro (Processo)
*   **Ação Técnica:** Configuração de um pipeline básico (GitHub Actions) que impeça deploys sem revisão. Proibir alterações diretas no servidor de produção via restrição de credenciais.
*   **Estimativa de Esforço:** 1 dia.
*   **Critério de Sucesso:** Todas as alterações de código registradas via Pull Request com histórico documentado.

---

## Seção 3 — Decisão de Arquitetura: Refatoração vs. Reescrita

**Decisão: Opção A — Refatoração Incremental**

**Justificativa:**
Como o sistema está em produção e processando um volume considerável de pedidos sem o suporte de testes automatizados, a reescrita total (Opção B) apresenta um risco de negócio inaceitável. Minha escolha pela **Refatoração Incremental** baseia-se em:

1.  **Conhecimento do Domínio:** Refatorar aos poucos permite que o time entenda as regras de negócio "escondidas" no código de 5 anos antes de alterá-las.
2.  **Mitigação de Risco:** Utilizaremos o **Padrão Estrangulador** (Strangler Pattern), extraindo módulos pequenos e testando-os individualmente. Se algo falhar, o impacto é isolado e fácil de reverter.
3.  **Valor Contínuo:** O negócio não pode parar por meses para uma reescrita. Com a refatoração, entregamos melhorias de estabilidade a cada sprint.

---

## Seção 4 — Requisitos Não Funcionais (RNFs) Negligenciados

1.  **Confiabilidade (Reliability):** Comprometida pela duplicidade de pedidos. O sistema não garante a consistência do estado financeiro.
    *   **Métrica:** Taxa de Duplicidade (Alvo: 0%).
2.  **Eficiência de Desempenho (Performance):** Comprometida pela latência de 12s. O sistema não escala para o horário de pico.
    *   **Métrica:** Tempo de Resposta (Response Time). Alvo: < 800ms.
3.  **Manutenibilidade (Maintainability):** Comprometida pelo arquivo gigante e falta de testes. O custo de mudança é altíssimo.
    *   **Métrica:** Cobertura de Testes e MTTR (Mean Time to Repair). Alvo: Redução progressiva do tempo de correção de bugs.

---

## Conclusão

Este plano foca inicialmente em **estabilizar a operação** e **proteger os dados**, criando em seguida a base técnica necessária para que o sistema volte a ser escalável e fácil de manter por toda a equipe.
