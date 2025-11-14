import { Component, NgModule } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink ],
  standalone: true,
  templateUrl: './login.html',
})
export class LoginComponent {

  email = "";
  senha = "";
  loading = false;

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.loading = true;

    this.authService.login(this.email, this.senha).subscribe({
      next: (response) => {
        console.log("✅ Login realizado!");
        localStorage.setItem("authenticated", "true");

        this.loading = false;

        // 👉 Redireciona para o layout após login
        this.router.navigate(['/produtos/listar']);
      },
      error: (err) => {
        this.loading = false;
        alert("❌ Credenciais inválidas");
      }
    });
  }
}
