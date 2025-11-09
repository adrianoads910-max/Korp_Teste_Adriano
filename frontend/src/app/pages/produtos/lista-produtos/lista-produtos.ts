
import { ProdutosService, Produto } from '../../../services/produtos';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-lista-produtos',
  standalone: true,
  templateUrl: './lista-produtos.html',
  styleUrls: ['./lista-produtos.scss'],
  imports: [ CommonModule ]
})
export class ListaProdutosComponent implements OnInit {

  produtos: Produto[] = [];

  constructor(private produtosService: ProdutosService) {}

  ngOnInit(): void {
    this.produtosService.listar().subscribe((result) => {
      this.produtos = result;
    });
  }
}
