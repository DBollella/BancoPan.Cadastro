import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmationService, MessageService } from 'primeng/api';
import { PessoaJuridicaService } from '../../services/pessoa-juridica.service';
import { PessoaJuridica } from '../../models/pessoa-juridica.model';
import { CnpjPipe } from '../../../../shared/pipes/cnpj.pipe';

@Component({
  selector: 'app-pessoa-juridica-list',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, CardModule, CnpjPipe],
  templateUrl: './pessoa-juridica-list.component.html',
  styleUrl: './pessoa-juridica-list.component.scss'
})
export class PessoaJuridicaListComponent implements OnInit {
  pessoas: PessoaJuridica[] = [];
  loading = false;

  constructor(
    private pessoaJuridicaService: PessoaJuridicaService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    public router: Router
  ) {}

  ngOnInit() {
    this.loadPessoas();
  }

  loadPessoas() {
    this.loading = true;
    this.pessoaJuridicaService.getAll().subscribe({
      next: (data) => {
        this.pessoas = data;
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao carregar pessoas jurídicas'
        });
        this.loading = false;
      }
    });
  }

  confirmDelete(id: string) {
    this.confirmationService.confirm({
      message: 'Tem certeza que deseja excluir esta empresa?',
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.delete(id);
      }
    });
  }

  delete(id: string) {
    this.pessoaJuridicaService.delete(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: 'Pessoa jurídica excluída com sucesso'
        });
        this.loadPessoas();
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao excluir pessoa jurídica'
        });
      }
    });
  }
}
