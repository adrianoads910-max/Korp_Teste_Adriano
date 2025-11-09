import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { NotasService, NotaFiscal } from '../../../services/notas';

@Component({
  selector: 'app-lista-notas',
  standalone: true,
  imports: [
    MatCardModule,
    MatTableModule
  ],
  templateUrl: './lista-notas.html',
  styleUrls: ['./lista-notas.scss']
})
export class ListaNotasComponent implements OnInit {

  notas: NotaFiscal[] = [];
  colunas = ['numero', 'status'];

  constructor(private notasService: NotasService) {}

  ngOnInit(): void {
    this.notasService.listar().subscribe(data => {
      this.notas = data;
    });
  }
}
