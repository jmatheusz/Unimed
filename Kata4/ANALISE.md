# Análise Técnica — Kata 4: Pipeline de Relatório

Este documento detalha as decisões de engenharia de dados tomadas durante a construção do pipeline de transformação financeira/logística.

## 1. Decisões de Tratamento de Dados

### Registros Órfãos
**Decisão:** Registros em `entregas.csv` que não possuem um correspondente em `pedidos.csv` foram **descartados**.
**Justificativa:** No contexto de um relatório de desempenho de pedidos, uma entrega sem pedido é um dado inconsistente que não pode ser atribuído a um cliente ou valor financeiro. Utilizamos um `left join` a partir da tabela de pedidos para garantir a integridade.

### Normalização de Cidades
**Decisão:** Implementação de uma função de "ASCII folding" (remoção de acentos) combinada com caixa baixa e remoção de espaços extras.
**Exemplo:** `São Paulo` -> `sao paulo`.
**Justificativa:** Isso garante que variações de grafia e erros de digitação comuns não fragmentem os indicadores de volume por cidade.

### Datas Inconsistentes
**Decisão:** Uso do parâmetro `format='mixed'` do Pandas para inferir dinamicamente múltiplos formatos (ISO, Brasileiro, Timestamps).
**Justificativa:** Permite que o pipeline seja resiliente a mudanças nos sistemas de exportação sem necessidade de regex complexos para cada coluna.

### Valores Nulos
**Decisão:** Linhas com campos obrigatórios nulos (`id_pedido`, `valor_total`) foram removidas.
**Justificativa:** Dados financeiros incompletos geram relatórios de ticket médio e faturamento errôneos.

---

## 2. Idempotência
**Sim, o pipeline é idempotente.**
Ao rodar o script múltiplas vezes com os mesmos arquivos de entrada, o resultado final (`relatorio_consolidado.csv`) será idêntico. Isso ocorre porque o script:
1. Lê os arquivos originais sem modificá-los.
2. Sobrescreve o arquivo de saída ao invés de anexar dados.
3. Não possui estados persistentes entre execuções ou dependência de horários do sistema para cálculos de transformação (exceto se o dado original mudar).

---

## 3. Escalabilidade (10 Milhões de Linhas)
Se o volume de dados crescesse para 10 milhões de linhas diariamente, a abordagem atual em Pandas (que carrega tudo em memória) falharia. Eu mudaria para:
1.  **Distributed Computing:** Utilizaria **Apache Spark (PySpark)** ou **Dask** para processar os dados em clusters.
2.  **Incremental Loading:** Em vez de processar todo o histórico, o pipeline processaria apenas os arquivos "Delta" (novos e alterados) do dia.
3.  **Data Lake/Storage:** Armazenaria os dados em formato **Parquet** (colunar e comprimido) em vez de CSV, acelerando drasticamente as leituras.
4.  **Database Staging:** Realizaria o processamento pesado (Joins e Agregações) diretamente em um Data Warehouse (como BigQuery ou Snowflake) usando SQL, que é altamente otimizado para grandes volumes.

---

## 4. Estratégia de Testes de Qualidade
Para garantir a confiabilidade contínua, eu implementaria:
1.  **Testes de Schema:** Verificar se as colunas esperadas existem e possuem o tipo de dado correto (ex: `valor_total` deve ser float).
2.  **Testes de Range:** Garantir que `data_realizada` não seja anterior a `data_pedido` (violação lógica).
3.  **Testes de Unicidade:** Validar que não existem `id_pedido` duplicados no relatório consolidado.
4.  **Expectativas de Dados (Great Expectations):** Definir regras como "o percentual de valores nulos na coluna X não pode ultrapassar 5%".
