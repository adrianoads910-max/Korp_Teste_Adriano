
import { ProdutosService, Produto } from '../../../services/produtos';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-cadastro-produto',
  standalone: true,
  templateUrl: './cadastro-produto.html',
  styleUrls: ['./cadastro-produto.scss'],
  imports: [
    CommonModule,
    FormsModule
  ],
})
export class CadastroProdutoComponent {

  produto: Produto = {
    codigo: '',
    descricao: '',
    saldo: 0
  };

  constructor(private produtosService: ProdutosService) {}

  salvar() {
    this.produtosService.criar(this.produto).subscribe({
      next: () => {
        alert("✅ Produto cadastrado com sucesso!");
        this.produto = { codigo: '', descricao: '', saldo: 0 };  // limpa o formulário
      },
      error: (erro) => {
        alert("❌ Erro ao salvar: " + (erro?.error?.erro ?? erro.message));
      }
    });
  }
}
