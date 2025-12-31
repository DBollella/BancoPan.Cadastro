import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmationService, MessageService } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { PessoaFisicaService } from '../../services/pessoa-fisica.service';
import { PessoaFisica } from '../../models/pessoa-fisica.model';
import { CpfPipe } from '../../../../shared/pipes/cpf.pipe';

@Component({
  selector: 'app-pessoa-fisica-lista',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, CardModule, TooltipModule, CpfPipe],
  templateUrl: './pessoa-fisica-lista.component.html',
  styleUrl: './pessoa-fisica-lista.component.scss'
})
export class PessoaFisicaListaComponent implements OnInit {
  pessoas: PessoaFisica[] = [];
  loading = false;

  constructor(
    private pessoaFisicaService: PessoaFisicaService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    public router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadPessoas();
  }

  loadPessoas() {
    this.loading = true;
    this.pessoaFisicaService.getAll()
      .pipe(
        finalize(() => this.loading = false)
      )
      .subscribe({
        next: (data) => {
          console.log('Dados recebidos:', data);
          this.pessoas = data;
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.error('Erro ao carregar pessoas físicas:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao carregar pessoas físicas'
          });
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
