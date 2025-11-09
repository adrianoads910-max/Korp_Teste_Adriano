import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'produtos/listar',
        pathMatch: 'full'
      },
      {
        path: 'produtos/cadastrar',
        loadComponent: () =>
          import('./pages/produtos/cadastro-produto/cadastro-produto')
            .then(m => m.CadastroProdutoComponent)
      },
      {
        path: 'produtos/listar',
        loadComponent: () =>
          import('./pages/produtos/lista-produtos/lista-produtos')
            .then(m => m.ListaProdutosComponent)
      },
      {
        path: 'notas/cadastrar',
        loadComponent: () =>
          import('./pages/notas/cadastro-nota/cadastro-nota')
            .then(m => m.CadastroNotaComponent)
      },
      {
        path: 'notas/listar',
        loadComponent: () =>
          import('./pages/notas/lista-notas/lista-notas')
            .then(m => m.ListaNotasComponent)
      }
    ]
  }
];
