import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'pessoa-fisica/novo',
    loadComponent: () => import('./features/pessoas-fisicas/components/pessoa-fisica-form/pessoa-fisica-form.component').then(m => m.PessoaFisicaFormComponent)
  },
  {
    path: 'pessoa-fisica/:id/editar',
    loadComponent: () => import('./features/pessoas-fisicas/components/pessoa-fisica-form/pessoa-fisica-form.component').then(m => m.PessoaFisicaFormComponent)
  },
  {
    path: 'pessoa-fisica',
    loadComponent: () => import('./features/pessoas-fisicas/components/pessoa-fisica-lista/pessoa-fisica-lista.component').then(m => m.PessoaFisicaListaComponent)
  },
  {
    path: 'pessoa-juridica/novo',
    loadComponent: () => import('./features/pessoas-juridicas/components/pessoa-juridica-form/pessoa-juridica-form.component').then(m => m.PessoaJuridicaFormComponent)
  },
  {
    path: 'pessoa-juridica/:id/editar',
    loadComponent: () => import('./features/pessoas-juridicas/components/pessoa-juridica-form/pessoa-juridica-form.component').then(m => m.PessoaJuridicaFormComponent)
  },
  {
    path: 'pessoa-juridica',
    loadComponent: () => import('./features/pessoas-juridicas/components/pessoa-juridica-lista/pessoa-juridica-lista.component').then(m => m.PessoaJuridicaListaComponent)
  },
  {
    path: 'endereco',
    loadComponent: () => import('./features/endereco/components/endereco-list/endereco-list.component').then(m => m.EnderecoListComponent)
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];
