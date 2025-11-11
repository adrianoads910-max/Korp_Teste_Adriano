import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { LayoutComponent } from './layout/layout'; 

export const routes: Routes = [

  // ✅ Rotas públicas (sem layout)
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/user/login/login')
        .then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./pages/user/register/register')
        .then(m => m.RegisterComponent)
  },

  // ✅ Rotas protegidas (somente usuário logado)
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard], // ⛔ Bloqueia se não estiver logado
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
  },

  // ✅ fallback para qualquer rota inválida
  {
    path: '**',
    redirectTo: 'login'
  }
];
