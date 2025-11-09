import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ProdutosService, Produto } from '../../../services/produtos';

@Component({
  selector: 'app-cadastro-produto',
  standalone: true,
  templateUrl: './cadastro-produto.html',
  styleUrls: ['./cadastro-produto.scss'],
  imports: [
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
})
export class CadastroProdutoComponent {

  produto: Produto = {
    codigo: '',
    descricao: '',
    saldo: 0
  };

  constructor(private produtosService: ProdutosService) {}

  salvar() {                        // ✅ ESTE MÉTODO É O QUE FALTAVA
    this.produtosService.criar(this.produto).subscribe(() => {
      alert("Produto cadastrado com sucesso!");

      // limpa inputs após salvar
      this.produto = { codigo: '', descricao: '', saldo: 0 };
    });
  }
}
