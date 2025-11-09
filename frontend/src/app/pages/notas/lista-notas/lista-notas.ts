import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotasService } from '../../../services/notas';
import { NotaFiscal, StatusNota } from '../../../models/nota-fiscal';

@Component({
  selector: 'app-lista-notas',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './lista-notas.html'
})
export class ListaNotasComponent implements OnInit {

  notas: NotaFiscal[] = [];

  // ✅ expõe o enum StatusNota para usar no HTML
  StatusNota = StatusNota;

  constructor(private notasService: NotasService) {}

  ngOnInit(): void {
    this.notasService.listar().subscribe((data) => {
      this.notas = data;
    });
  }

  // ✅ método chamado pelo botão "Imprimir"
  imprimir(numero: number): void {
    this.notasService.imprimir(numero).subscribe({
      next: (resp) => {
        alert(`✅ Nota impressa e fechada!`);
        this.ngOnInit(); // recarrega a lista automaticamente
      },
      error: (erro) => {
        alert("❌ Erro ao imprimir nota: " + (erro?.error?.erro ?? erro.message));
      }
    });
  }


  cancelar(numero: number) {
    if (!confirm("Deseja cancelar esta nota?")) return;

    this.notasService.cancelar(numero).subscribe({
      next: () => {
        alert("✅ Nota cancelada com sucesso!");
        this.ngOnInit(); // recarrega lista
      },
      error: (erro) => {
        alert("❌ Erro: " + (erro?.error?.erro ?? erro.message));
      }
    });
}
}
