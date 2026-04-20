import pandas as pd
import unicodedata
import os

def normalize_text(text):
    if pd.isna(text):
        return text
    text = str(text)
    text = unicodedata.normalize('NFKD', text).encode('ASCII', 'ignore').decode('ASCII')
    return text.lower().strip()

def calculate_indicators(df):
    print("\n" + "="*30)
    print("PARTE B: INDICADORES CONSOLIDADOS")
    print("="*30)
    
    print("\n1. Total de pedidos por status:")
    print(df['status_pedido'].value_counts())
    
    print("\n2. Ticket médio por estado:")
    print(df.groupby('estado')['valor_total'].mean())
    
    entregues = df.dropna(subset=['data_realizada_entrega']).copy()
    if not entregues.empty:
        no_prazo = (entregues['atraso_dias'] <= 0).sum()
        com_atraso = (entregues['atraso_dias'] > 0).sum()
        total = len(entregues)
        print("\n3. Performance de Entrega:")
        print(f"No prazo: {(no_prazo/total)*100:.1f}%")
        print(f"Com atraso: {(com_atraso/total)*100:.1f}%")
    else:
        print("\n3. Performance de Entrega: Dados insuficientes.")

    print("\n4. Top 3 cidades com maior volume de pedidos:")
    print(df['cidade_normalizada'].value_counts().head(3))
    
    atrasados = df[df['atraso_dias'] > 0]
    if not atrasados.empty:
        media_atraso = atrasados['atraso_dias'].mean()
        print(f"\n5. Média de atraso para pedidos atrasados: {media_atraso:.1f} dias")
    else:
        print("\n5. Média de atraso para pedidos atrasados: N/A (sem atrasos registrados)")

def run_pipeline():
    print("Iniciando Pipeline...")
    
    try:
        df_pedidos = pd.read_csv('pedidos.csv')
        df_clientes = pd.read_csv('clientes.csv')
        df_entregas = pd.read_csv('entregas.csv')
    except Exception as e:
        print(f"Erro ao ler os arquivos: {e}")
        return

    print("Limpando datas...")
    date_cols_pedidos = ['data_pedido']
    date_cols_clientes = ['data_cadastro']
    date_cols_entregas = ['data_prevista', 'data_realizada']

    for col in date_cols_pedidos:
        df_pedidos[col] = pd.to_datetime(df_pedidos[col], format='mixed', errors='coerce')
    
    for col in date_cols_clientes:
        df_clientes[col] = pd.to_datetime(df_clientes[col], format='mixed', errors='coerce')

    for col in date_cols_entregas:
        df_entregas[col] = pd.to_datetime(df_entregas[col], format='mixed', errors='coerce')

    print("Limpando moedas...")
    if 'valor_total' in df_pedidos.columns:
        df_pedidos['valor_total'] = df_pedidos['valor_total'].astype(str).str.replace(',', '.')
        df_pedidos['valor_total'] = pd.to_numeric(df_pedidos['valor_total'], errors='coerce')

    print("Tratando nulos...")
    df_pedidos = df_pedidos.dropna(subset=['id_pedido', 'id_cliente', 'valor_total'])

    print("Normalizando nomes de cidades...")
    df_clientes['cidade_normalizada'] = df_clientes['cidade'].apply(normalize_text)

    print("Mesclando conjuntos de dados...")
    consolidado = pd.merge(df_pedidos, df_clientes, on='id_cliente', how='left')
    consolidado = pd.merge(consolidado, df_entregas, on='id_pedido', how='left')

    print("Calculando atrasos de entrega...")
    consolidado['atraso_dias'] = (consolidado['data_realizada'] - consolidado['data_prevista']).dt.days

    final_cols = {
        'id_pedido': 'id_pedido',
        'nome': 'nome_cliente',
        'cidade_normalizada': 'cidade_normalizada',
        'estado': 'estado',
        'valor_total': 'valor_total',
        'status': 'status_pedido',
        'data_pedido': 'data_pedido',
        'data_prevista': 'data_prevista_entrega',
        'data_realizada': 'data_realizada_entrega',
        'atraso_dias': 'atraso_dias',
        'status_entrega': 'status_entrega'
    }
    
    df_final = consolidado[list(final_cols.keys())].rename(columns=final_cols)

    output_file = 'relatorio_consolidado.csv'
    df_final.to_csv(output_file, index=False, encoding='utf-8')
    print(f"Pipeline concluído! Relatório consolidado salvo em {output_file}")
    
    calculate_indicators(df_final)

if __name__ == "__main__":
    run_pipeline()
