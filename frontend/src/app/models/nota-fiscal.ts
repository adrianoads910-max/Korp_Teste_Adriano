export interface NotaItem {
  codigoProduto: string;
  quantidade: number;
}

/** Enum igual ao do backend */
export enum StatusNota {
  Aberta = 0,
  Fechada = 1,
  Cancelado = 2
}

export interface NotaFiscal {
  numero?: number;
  status: StatusNota;      
  itens: NotaItem[];        
}
