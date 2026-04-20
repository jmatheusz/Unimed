import csv
import os

def create_pedidos():
    data = [
        ['id_pedido', 'data_pedido', 'id_cliente', 'valor_total', 'status'],
        ['1001', '2023-01-10', 'C001', '250.50', 'Entregue'],
        ['1002', '15/01/2023', 'C002', '150,75', 'Cancelado'],
        ['1003', '2023-01-20 10:00:00', 'C003', '300.00', 'Entregue'],
        ['1004', '2023-01-25', 'C001', '120,00', 'Pendente'],
        ['1005', '', 'C004', '450.00', 'Entregue'],
        ['1006', '2023-02-01', 'C002', None, 'Entregue'],
    ]
    with open('pedidos.csv', 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        writer.writerows(data)

def create_clientes():
    data = [
        ['id_cliente', 'nome', 'cidade', 'estado', 'data_cadastro'],
        ['C001', 'João Silva', 'São Paulo', 'SP', '2022-01-01'],
        ['C002', 'Maria Oliveira', 'sao paulo', 'SP', '2022-02-15'],
        ['C003', 'Carlos Souza', 'SAO PAULO', 'SP', '2022-03-10'],
        ['C004', 'Ana Santos', 'Rio de Janeiro', 'RJ', '2022-04-20'],
        ['C005', 'Pedro Costa', 'rio de janeiro', 'RJ', '2022-05-05'],
    ]
    with open('clientes.csv', 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        writer.writerows(data)

def create_entregas():
    data = [
        ['id_entrega', 'id_pedido', 'data_prevista', 'data_realizada', 'status_entrega'],
        ['E001', '1001', '2023-01-15', '2023-01-14', 'Entregue'],
        ['E002', '1003', '2023-01-25', '2023-01-28', 'Entregue'],
        ['E003', '1005', '2023-02-10', '2023-02-10', 'Entregue'],
        ['E004', '1006', '2023-02-15', '', 'Em Trânsito'],
        ['E005', '9999', '2023-01-01', '2023-01-02', 'Entregue'],
    ]
    with open('entregas.csv', 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        writer.writerows(data)

if __name__ == "__main__":
    create_pedidos()
    create_clientes()
    create_entregas()
    print("Files created: pedidos.csv, clientes.csv, entregas.csv")
