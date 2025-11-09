import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'produtos/listar',
    pathMatch: 'full'
  },

  // ✅ ROTAS DE PRODUTOS
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

  // ✅ ROTAS DE NOTAS
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
  },
];
