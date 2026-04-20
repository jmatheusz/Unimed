# Análise Técnica - Sistema de Fila de Triagem

## 1. Escolha da Estrutura de Dados
Para a modelagem da fila, utilizei uma coleção `IEnumerable<Patient>` processada via **LINQ (Language Integrated Query)**. 
- **Por que?** Em C#, o LINQ oferece uma sintaxe expressiva e declarativa para ordenação (`OrderBy`, `ThenBy`). Como o requisito pedia para "receber uma lista e retornar a fila ordenada", uma estrutura de lista (`List<T>`) ou coleção enumerável é a mais adequada para representar o estado momentâneo da fila antes da triagem.
- **Alternativa**: Se o sistema precisasse de inserções e remoções constantes de alta performance mantendo a ordem, uma **Priority Queue (Fila de Prioridade)** seria mais eficiente, mas para uma ordenação única de uma lista recebida, o LINQ é mais legível e sustentável.

## 2. Complexidade de Tempo
A complexidade de tempo do algoritmo de ordenação do LINQ (baseado no Timsort ou Quicksort/Introsort dependendo da versão do .NET) é **O(n log n)** no caso médio.
- **Com 1 milhão de pacientes**: O algoritmo continuaria sendo O(n log n), o que é eficiente para a maioria dos casos. No entanto, o custo de memória para manter 1 milhão de objetos em RAM e o tempo de processamento começariam a ser significativos. Em um cenário real dessa escala, a ordenação deveria ser delegada ao **Banco de Dados** (via índices) ou processada em lotes (batch).

## 3. Interação entre as Regras 4 e 5
As regras 4 e 5 são aplicadas sequencialmente no cálculo da "Urgência Efetiva".
- **Caso: Paciente de 15 anos e urgência MÉDIA**:
  - A **Regra 4** (Idosos 60+) não se aplica.
  - A **Regra 5** (Menores de 18) se aplica, ganhando +1 nível de prioridade.
  - **Resultado**: O paciente passa de **MÉDIA** para **ALTA**.
- **Nota**: Como as regras de idade são mutuamente exclusivas (não se pode ter <18 e >=60 ao mesmo tempo), não há conflito direto de idade, mas a Regra 5 é mais abrangente pois se aplica a *qualquer* urgência inicial.

## 4. Extensibilidade (6ª Regra)
O código foi estruturado de forma a isolar a lógica de negócio no método `CalculateEffectiveUrgency`.
- **Como adicionar uma nova regra?** Basta inserir a nova validação lógica dentro desse método.
- **Escalabilidade de Código**: Se a clínica passasse a ter dezenas de regras, eu evoluiria a implementação para o padrão **Specification Pattern** ou um **Rule Engine**, onde cada regra seria uma classe separada, permitindo adicionar ou remover regras sem modificar o serviço principal (respeitando o princípio Open/Closed do SOLID).
