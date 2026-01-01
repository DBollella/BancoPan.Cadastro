import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmationService, MessageService } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { PessoaJuridicaService } from '../../services/pessoa-juridica.service';
import { PessoaJuridica } from '../../models/pessoa-juridica.model';
import { CnpjPipe } from '../../../../shared/pipes/cnpj.pipe';

@Component({
  selector: 'app-pessoa-juridica-lista',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, CardModule, TooltipModule, CnpjPipe],
  templateUrl: './pessoa-juridica-lista.component.html',
  styleUrl: './pessoa-juridica-lista.component.scss'
})
export class PessoaJuridicaListaComponent implements OnInit {
  pessoas: PessoaJuridica[] = [];
  loading = false;
  totalRecords = 0;
  rows = 10;
  first = 0;
  rowsPerPageOptions = [5, 10, 20, 30, 50, 100];

  constructor(
    private pessoaJuridicaService: PessoaJuridicaService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    public router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadPessoas({ first: 0, rows: this.rows });
  }

  loadPessoas(event: TableLazyLoadEvent) {
    this.loading = true;

    const pageNumber = (event.first! / event.rows!) + 1;
    const pageSize = event.rows || 10;

    this.first = event.first || 0;
    this.rows = event.rows || 10;

    this.pessoaJuridicaService.getPaginated(pageNumber, pageSize)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (data) => {
          console.log('Dados recebidos:', data);
          this.pessoas = data.items;
          this.totalRecords = data.totalCount;
        },
        error: (error) => {
          console.error('Erro ao carregar pessoas jurídicas:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao carregar pessoas jurídicas'
          });
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
        this.loadPessoas({ first: this.first, rows: this.rows });
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
