import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmationService, MessageService } from 'primeng/api';
import { PessoaFisicaService } from '../../services/pessoa-fisica.service';
import { PessoaFisica } from '../../models/pessoa-fisica.model';
import { CpfPipe } from '../../../../shared/pipes/cpf.pipe';

@Component({
  selector: 'app-pessoa-fisica-list',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, CardModule, CpfPipe],
  templateUrl: './pessoa-fisica-list.component.html',
  styleUrl: './pessoa-fisica-list.component.scss'
})
export class PessoaFisicaListComponent implements OnInit {
  pessoas: PessoaFisica[] = [];
  loading = false;

  constructor(
    private pessoaFisicaService: PessoaFisicaService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    public router: Router
  ) {}

  ngOnInit() {
    this.loadPessoas();
  }

  loadPessoas() {
    this.loading = true;
    this.pessoaFisicaService.getAll().subscribe({
      next: (data) => {
        this.pessoas = data;
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao carregar pessoas físicas'
        });
        this.loading = false;
      }
    });
  }

  confirmDelete(id: string) {
    this.confirmationService.confirm({
      message: 'Tem certeza que deseja excluir esta pessoa?',
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.delete(id);
      }
    });
  }

  delete(id: string) {
    this.pessoaFisicaService.delete(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: 'Pessoa física excluída com sucesso'
        });
        this.loadPessoas();
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao excluir pessoa física'
        });
      }
    });
  }
}
