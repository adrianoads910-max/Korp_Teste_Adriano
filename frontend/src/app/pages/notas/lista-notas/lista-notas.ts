import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotasService } from '../../../services/notas';
import { NotaFiscal } from '../../../models/nota-fiscal';

@Component({
  selector: 'app-lista-notas',
  standalone: true,
  imports: [CommonModule], // ✅ necessário para ngFor
  templateUrl: './lista-notas.html',
  styleUrls: ['./lista-notas.scss']
})
export class ListaNotasComponent implements OnInit {

  notas: NotaFiscal[] = [];

  constructor(private notasService: NotasService) {}

  ngOnInit(): void {
    this.notasService.listar().subscribe((data: NotaFiscal[]) => {
      this.notas = data;
    });
  }
}
