import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmationService, MessageService } from 'primeng/api';
import { EnderecoService } from '../../services/endereco.service';
import { Endereco } from '../../models/endereco.model';

@Component({
  selector: 'app-endereco-list',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, CardModule],
  templateUrl: './endereco-list.component.html',
  styleUrl: './endereco-list.component.scss'
})
export class EnderecoListComponent implements OnInit {
  enderecos: Endereco[] = [];
  loading = false;

  constructor(
    private enderecoService: EnderecoService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    public router: Router
  ) {}

  ngOnInit() {
    this.loadEnderecos();
  }

  loadEnderecos() {
    this.loading = true;
    this.enderecoService.getAll().subscribe({
      next: (data) => {
        this.enderecos = data;
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao carregar endereços'
        });
        this.loading = false;
      }
    });
  }

  confirmDelete(id: string) {
    this.confirmationService.confirm({
      message: 'Tem certeza que deseja excluir este endereço?',
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.delete(id);
      }
    });
  }

  delete(id: string) {
    this.enderecoService.delete(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: 'Endereço excluído com sucesso'
        });
        this.loadEnderecos();
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao excluir endereço'
        });
      }
    });
  }
}
