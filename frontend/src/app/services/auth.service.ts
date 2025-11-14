import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environments';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private apiUrl = environment.faturamentoApi; //'http://localhost:5208'; // FaturamentoService

  constructor(private http: HttpClient) {}

  login(email: string, senha: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/login`, { email, senha });
  }

  register(email: string, senha: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, { email, senha });
  }

  logout() {
  localStorage.removeItem("authenticated");
  localStorage.removeItem("email");
  }

  isLogged(): boolean {
    return localStorage.getItem("authenticated") == "true";
  }

  
}
