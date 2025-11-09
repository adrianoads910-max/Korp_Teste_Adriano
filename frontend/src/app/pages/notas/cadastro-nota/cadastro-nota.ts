import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { NotasService } from '../../../services/notas';

@Component({
  selector: 'app-cadastro-nota',
  standalone: true,
  imports: [
    FormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './cadastro-nota.html',
  styleUrls: ['./cadastro-nota.scss']
})
export class CadastroNotaComponent {

  codigoProduto = '';
  quantidade = 0;

  constructor(private notasService: NotasService) {}

  salvar() {
    this.notasService.criar({
      itens: [{ codigoProduto: this.codigoProduto, quantidade: this.quantidade }]
    }).subscribe(resp => {
      alert("Nota criada! Número: " + resp.numero);
    });
  }
}
