# Desafio Unimed - Katas 1, 2, 3 e 4

Esse repositório reúne as soluções que desenvolvi para os desafios propostos. Cada kata tem um foco diferente, passando por desenvolvimento web, análise de sistemas e também tratamento de dados.

## Informações do Candidato
- **Nome:** José Matheus De Oliveira Guimarães  
- **Telefone:** (81) 99289-8400  
- **Email:** jmatheusguimaraes07@gmail.com  

---

## 🛠 Tecnologias utilizadas e por quê

### **Katas 1 e 2 (Sistemas Web)**

- **Back-End:** .NET Core / C#  
  Escolhi usar C# com .NET porque é uma stack bem sólida e confiável. Como os sistemas envolvem dados importantes (tipo informações médicas e tarefas), achei melhor usar algo com tipagem forte e boa organização.

- **Front-End:** React (JavaScript/TypeScript)  
  Usei React porque facilita muito na hora de montar interfaces mais organizadas e reutilizáveis. Além disso, dá pra deixar tudo mais dinâmico e responsivo.

---

### **Kata 3 (Engenharia de Software)**

- **Foco:** análise de sistema legado e melhorias  
  Aqui a ideia foi pensar mais como resolver problemas reais. Trabalhei com a ideia de melhorar o sistema aos poucos (sem quebrar tudo), usando algo parecido com o *Strangler Pattern*.  
  Também considerei automação com GitHub Actions pra deixar o processo mais organizado.

---

### **Kata 4 (Pipeline de Dados)**

- **Stack:** Python + Pandas  
  Usei Python porque já tenho familiaridade e ele é muito bom pra trabalhar com dados.  
  O Pandas ajudou bastante na parte de ler os arquivos, tratar os dados e fazer os cálculos de forma mais rápida.

---

## 🚀 Como rodar o projeto

### **Katas 1 e 2**

Você precisa ter o **.NET** e o **Node.js** instalados.

**Back-End:**
```bash
cd kata-1/Back-End/src
# ou
cd kata-2/backend/TaskFlow.Api

dotnet run

cd frontend

npm install
npm start

Kata 4

É necessário ter o Python 3 instalado.

cd kata-4

pip install pandas

python pipeline.py

💬 O que eu faria diferente com mais tempo?
Implementaria Docker para padronizar o ambiente e facilitar a execução em qualquer máquina
Criaria mais testes automatizados (tanto no back quanto no Python) para evitar bugs
Adicionaria documentação da API com Swagger para facilitar o uso
Implementaria autenticação usando JWT
No kata 4, em vez de gerar só CSV, integraria com banco de dados e usaria ferramentas como Power BI ou Grafana para visualizar os dados melhor
