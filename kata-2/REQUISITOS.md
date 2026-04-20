# Análise de Requisitos - Kata 2 (Painel de Tarefas)

## 1. Ambiguidades e Informações Faltantes

Abaixo estão listadas 3 ambiguidades identificadas nos requisitos informais fornecidos pelo cliente:

### A. Persistência e Durabilidade
*   **Pergunta ao Cliente:** "As tarefas devem ser salvas permanentemente em um banco de dados ou podem ser temporárias (perdidas ao reiniciar o sistema)?"
*   **Decisão na ausência de resposta:** Utilizaremos um arquivo local `database.json` para persistência. Isso garante que os dados sobrevivam a reinicializações do servidor sem a complexidade de configurar um servidor de banco de dados externo para este Kata.

### B. Escopo de Status (Situação)
*   **Pergunta ao Cliente:** "Além de 'Pendente' e 'Concluída', existem outros estados como 'Em Andamento' ou 'Bloqueada'?"
*   **Decisão na ausência de resposta:** Implementaremos apenas dois estados: `pending` (pendente) e `completed` (concluída), mapeando diretamente o que foi solicitado informalmente ("ver só as pendentes ou só as concluídas").

### C. Detalhes da Tarefa (Campos)
*   **Pergunta ao Cliente:** "As tarefas terão apenas um título, ou precisamos de campos como 'Descrição', 'Data de Entrega' e 'Responsável'?"
*   **Decisão na ausência de resposta:** A tarefa terá apenas `id`, `title`, `status` (boolean/string) e um campo opcional de `priority` (preparando para o futuro), mantendo a simplicidade solicitada.

---

## 2. Requisitos Formais

### Requisitos Funcionais (RF)
*   **RF01:** O sistema deve permitir a criação de novas tarefas informando um título.
*   **RF02:** O sistema deve listar todas as tarefas cadastradas.
*   **RF03:** O sistema deve permitir marcar uma tarefa como "Concluída" ou retornar para "Pendente".
*   **RF04:** O sistema deve permitir a exclusão definitiva de uma tarefa.
*   **RF05:** O sistema deve permitir filtrar a listagem por status (Todas, Pendentes ou Concluídas).

### Requisitos Não Funcionais (RNF)
*   **RNF01:** A interface deve ser responsiva e funcional em diferentes tamanhos de tela.
*   **RNF02:** A API deve seguir os padrões REST e retornar códigos HTTP adequados (ex: 201 para criação, 404 para item não encontrado).
*   **RNF03:** O tempo de resposta da API para operações básicas deve ser inferior a 200ms em condições normais.
*   **RNF04:** Os dados das tarefas devem ser persistidos localmente.

---

## 3. Estratégia para o Backlog (Prioridade)

O requisito de **prioridade** foi marcado pelo cliente como algo que "pode ficar pra depois". No meu backlog, eu o trataria da seguinte forma:

1.  **Iteração Atual (MVP):** Foco total nos RFs de CRUD e Filtro. No código da API, já incluirei um campo `priority` no modelo de dados (padrão: "Média") para facilitar a expansão futura sem necessidade de migração de dados complexa.
2.  **Próxima Iteração:** Adição do campo de seleção de prioridade no formulário de criação e exibição de badges coloridos (ex: Vermelho para Alta, Azul para Baixa) na listagem.
3.  **Refinamento:** Implementação de ordenação automática na lista baseada na prioridade.
