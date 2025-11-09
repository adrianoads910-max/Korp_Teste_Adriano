import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NotasService } from '../../../services/notas';
import { NotaFiscal, NotaItem, StatusNota } from '../../../models/nota-fiscal';

@Component({
  selector: 'app-cadastro-nota',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './cadastro-nota.html',
  styleUrls: ['./cadastro-nota.scss']
})
export class CadastroNotaComponent {

  codigoProduto = '';
  quantidade = 1;
  itens: NotaItem[] = [];

  constructor(private notasService: NotasService) {}

  adicionarItem(): void {
    if (!this.codigoProduto || this.quantidade <= 0) {
      alert("Preencha código e quantidade.");
      return;
    }

    this.itens.push({
      codigoProduto: this.codigoProduto,
      quantidade: this.quantidade
    });

    this.codigoProduto = '';
    this.quantidade = 1;
  }

  removerItem(index: number): void {
    this.itens.splice(index, 1);
  }

  salvar(): void {
    if (this.itens.length === 0) {
      alert("Adicione pelo menos 1 item!");
      return;
    }

    const novaNota: NotaFiscal = {
      status: StatusNota.Aberta, 
      itens: this.itens
    };

    this.notasService.criar(novaNota).subscribe({
      next: (resp: NotaFiscal) => {
        alert(`✅ Nota criada! Número: ${resp.numero}`);
        this.itens = [];
        this.codigoProduto = '';
        this.quantidade = 1;
      },
      error: (erro) => {
        console.error("Erro ao criar nota:", erro);
        alert("❌ Erro ao criar nota: " + (erro?.error?.erro ?? erro.message));
      }
    });
  }
}
