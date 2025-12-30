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
    path: 'pessoas-fisicas',
    loadComponent: () => import('./features/pessoas-fisicas/components/pessoa-fisica-list/pessoa-fisica-list.component').then(m => m.PessoaFisicaListComponent)
  },
  {
    path: 'pessoas-fisicas/novo',
    loadComponent: () => import('./features/pessoas-fisicas/components/pessoa-fisica-form/pessoa-fisica-form.component').then(m => m.PessoaFisicaFormComponent)
  },
  {
    path: 'pessoas-fisicas/:id/editar',
    loadComponent: () => import('./features/pessoas-fisicas/components/pessoa-fisica-form/pessoa-fisica-form.component').then(m => m.PessoaFisicaFormComponent)
  },
  {
    path: 'pessoas-juridicas',
    loadComponent: () => import('./features/pessoas-juridicas/components/pessoa-juridica-list/pessoa-juridica-list.component').then(m => m.PessoaJuridicaListComponent)
  },
  {
    path: 'pessoas-juridicas/novo',
    loadComponent: () => import('./features/pessoas-juridicas/components/pessoa-juridica-form/pessoa-juridica-form.component').then(m => m.PessoaJuridicaFormComponent)
  },
  {
    path: 'pessoas-juridicas/:id/editar',
    loadComponent: () => import('./features/pessoas-juridicas/components/pessoa-juridica-form/pessoa-juridica-form.component').then(m => m.PessoaJuridicaFormComponent)
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];
