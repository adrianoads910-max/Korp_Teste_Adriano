import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { ProdutosService, Produto } from '../../../services/produtos';

@Component({
  selector: 'app-lista-produtos',
  standalone: true,
  imports: [
    MatCardModule,
    MatTableModule
  ],
  templateUrl: './lista-produtos.html',
  styleUrls: ['./lista-produtos.scss']
})
export class ListaProdutosComponent implements OnInit {

  produtos: Produto[] = [];
  colunas = ['codigo', 'descricao', 'saldo'];

  constructor(private produtosService: ProdutosService) {}

  ngOnInit(): void {               // ✅ Agora o OnInit foi implementado
    this.produtosService.listar().subscribe((result) => {
      this.produtos = result;
    });
  }
}
