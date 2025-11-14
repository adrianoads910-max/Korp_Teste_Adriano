import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html'
})
export class RegisterComponent {

  email = '';
  senha = '';

  constructor(private authService: AuthService, private router: Router) {}

  registrar() {
    this.authService.register(this.email, this.senha).subscribe({
      next: () => {
        alert("✅ Usuário registrado com sucesso!");
        this.router.navigate(['/login']);
      },
      error: (erro) => {
        alert("❌ Erro ao registrar: " + erro.error);
      }
    });
  }
}
