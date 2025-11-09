import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NotaFiscal } from '../models/nota-fiscal'; // ✅ usando o modelo correto

@Injectable({ providedIn: 'root' })
export class NotasService {

  private apiUrl = 'http://localhost:5208/notas';

  constructor(private http: HttpClient) {}

  criar(nota: NotaFiscal): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(`${this.apiUrl}`, nota);
  }

  listar(): Observable<NotaFiscal[]> {
    return this.http.get<NotaFiscal[]>(`${this.apiUrl}`);
  }

  imprimir(numero: number): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(`${this.apiUrl}/${numero}/imprimir`, {});
  }
}
