import json
import os

def analyze_tasks():
    # Path to the database file in the backend folder
    db_path = os.path.join(os.path.dirname(__file__), '..', 'backend', 'TaskFlow.Api', 'database.json')
    
    if not os.path.exists(db_path):
        print("[-] Arquivo de banco de dados não encontrado. Crie algumas tarefas primeiro!")
        return

    try:
        with open(db_path, 'r', encoding='utf-8') as f:
            tasks = json.load(f)
    except Exception as e:
        print(f"[-] Erro ao ler o banco de dados: {e}")
        return

    total = len(tasks)
    if total == 0:
        print("[!] Nenhuma tarefa para analisar.")
        return

    completed = len([t for t in tasks if t.get('completed', t.get('Completed', False))])
    pending = total - completed
    
    # Divisão por prioridade
    priorities = {}
    for t in tasks:
        p = t.get('priority', t.get('Priority', 'Média'))
        priorities[p] = priorities.get(p, 0) + 1

    print("="*30)
    print(" ANÁLISE DE DADOS TASKFLOW ")
    print("="*30)
    print(f"Total de Tarefas:   {total}")
    print(f"Concluídas:        {completed} ({ (completed/total)*100:.1f}%)")
    print(f"Pendentes:          {pending}")
    print("-" * 30)
    print("Divisão por Prioridade:")
    for p, count in priorities.items():
        print(f" - {p}: {count}")
    print("="*30)

if __name__ == "__main__":
    analyze_tasks()
