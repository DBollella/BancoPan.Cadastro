import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmationService, MessageService } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { EnderecoService } from '../../services/endereco.service';
import { Endereco } from '../../models/endereco.model';

@Component({
  selector: 'app-endereco-list',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, CardModule, TooltipModule],
  templateUrl: './endereco-list.component.html',
  styleUrl: './endereco-list.component.scss'
})
export class EnderecoListComponent implements OnInit {
  enderecos: Endereco[] = [];
  loading = false;
  totalRecords = 0;
  rows = 10;
  first = 0;
  rowsPerPageOptions = [5, 10, 20, 30, 50, 100];

  constructor(
    private enderecoService: EnderecoService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    public router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadEnderecos({ first: 0, rows: this.rows });
  }

  loadEnderecos(event: TableLazyLoadEvent) {
    this.loading = true;

    const pageNumber = (event.first! / event.rows!) + 1;
    const pageSize = event.rows || 10;

    this.first = event.first || 0;
    this.rows = event.rows || 10;

    this.enderecoService.getPaginated(pageNumber, pageSize)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (data) => {
          console.log('Dados recebidos:', data);
          this.enderecos = data.items;
          this.totalRecords = data.totalCount;
        },
        error: (error) => {
          console.error('Erro ao carregar endereços:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao carregar endereços'
          });
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
        this.loadEnderecos({ first: this.first, rows: this.rows });
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
