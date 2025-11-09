import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-layout',
  standalone: true,
  template: `
<mat-sidenav-container class="container">

  <mat-sidenav mode="side" opened class="sidenav">
    <mat-toolbar color="primary">Menu</mat-toolbar>

    <mat-nav-list>

      <mat-list-item>
        <span>Produtos</span>
      </mat-list-item>

      <a mat-list-item routerLink="/produtos/cadastrar">Cadastrar produto</a>
      <a mat-list-item routerLink="/produtos/listar">Listar produtos</a>

      <mat-divider></mat-divider>

      <mat-list-item>
        <span>Notas</span>
      </mat-list-item>

      <a mat-list-item routerLink="/notas/cadastrar">Cadastrar nota</a>
      <a mat-list-item routerLink="/notas/listar">Listar notas</a>

    </mat-nav-list>
  </mat-sidenav>

  <mat-sidenav-content>
    <mat-toolbar color="primary">
      <span>Korp - Sistema de Estoque / Faturamento</span>
    </mat-toolbar>

    <div class="content">
      <router-outlet></router-outlet>
    </div>

  </mat-sidenav-content>
</mat-sidenav-container>
`,
  styles: [`
    .container {
      height: 100vh;
    }

    .sidenav {
      width: 230px;
    }

    .content {
      padding: 20px;
    }
  `],
  imports: [
    RouterOutlet,
    RouterLink,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule
  ]
})
export class LayoutComponent {}
