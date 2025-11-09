export interface NotaItem {
  codigoProduto: string;
  quantidade: number;
}

export interface NotaFiscal {
  numero?: number;
  status?: 'Aberta' | 'Fechada';
  itens: NotaItem[];  // ✅ obrigatório
}
