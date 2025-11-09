import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface NotaItem {
  codigoProduto: string;
  quantidade: number;
}

export interface NotaFiscal {
  numero?: number;
  status?: string;
  itens: NotaItem[];
}

@Injectable({
  providedIn: 'root'
})
export class NotasService {

  private apiUrl = "http://localhost:5208"; // FaturamentoService

  constructor(private http: HttpClient) {}

  criar(nota: NotaFiscal): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(`${this.apiUrl}/notas`, nota);
  }

  listar(): Observable<NotaFiscal[]> {
    return this.http.get<NotaFiscal[]>(`${this.apiUrl}/notas`);
  }

  imprimir(numero: number): Observable<any> {
    const headers = new HttpHeaders({
      "Idempotency-Key": crypto.randomUUID()
    });

    return this.http.post(`${this.apiUrl}/notas/${numero}/imprimir`, {}, { headers });
  }
}
