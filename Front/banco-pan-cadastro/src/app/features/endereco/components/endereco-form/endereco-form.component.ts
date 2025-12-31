import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { InputMaskModule } from 'primeng/inputmask';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { MessageService } from 'primeng/api';
import { EnderecoService } from '../../services/endereco.service';
import { CriarEnderecoDto, AtualizarEnderecoDto } from '../../models/endereco.model';

interface UF {
  sigla: string;
  nome: string;
}

@Component({
  selector: 'app-endereco-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    InputTextModule,
    InputMaskModule,
    ButtonModule,
    DropdownModule
  ],
  templateUrl: './endereco-form.component.html',
  styleUrl: './endereco-form.component.scss'
})
export class EnderecoFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  enderecoId?: string;
  loading = false;
  loadingCep = false;

  ufs: UF[] = [
    { sigla: 'AC', nome: 'Acre' },
    { sigla: 'AL', nome: 'Alagoas' },
    { sigla: 'AP', nome: 'Amapá' },
    { sigla: 'AM', nome: 'Amazonas' },
    { sigla: 'BA', nome: 'Bahia' },
    { sigla: 'CE', nome: 'Ceará' },
    { sigla: 'DF', nome: 'Distrito Federal' },
    { sigla: 'ES', nome: 'Espírito Santo' },
    { sigla: 'GO', nome: 'Goiás' },
    { sigla: 'MA', nome: 'Maranhão' },
    { sigla: 'MT', nome: 'Mato Grosso' },
    { sigla: 'MS', nome: 'Mato Grosso do Sul' },
    { sigla: 'MG', nome: 'Minas Gerais' },
    { sigla: 'PA', nome: 'Pará' },
    { sigla: 'PB', nome: 'Paraíba' },
    { sigla: 'PR', nome: 'Paraná' },
    { sigla: 'PE', nome: 'Pernambuco' },
    { sigla: 'PI', nome: 'Piauí' },
    { sigla: 'RJ', nome: 'Rio de Janeiro' },
    { sigla: 'RN', nome: 'Rio Grande do Norte' },
    { sigla: 'RS', nome: 'Rio Grande do Sul' },
    { sigla: 'RO', nome: 'Rondônia' },
    { sigla: 'RR', nome: 'Roraima' },
    { sigla: 'SC', nome: 'Santa Catarina' },
    { sigla: 'SP', nome: 'São Paulo' },
    { sigla: 'SE', nome: 'Sergipe' },
    { sigla: 'TO', nome: 'Tocantins' }
  ];

  constructor(
    private fb: FormBuilder,
    private enderecoService: EnderecoService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.initForm();

    this.enderecoId = this.route.snapshot.params['id'];
    if (this.enderecoId) {
      this.isEditMode = true;
      this.loadEndereco();
    }
  }

  initForm() {
    this.form = this.fb.group({
      cep: ['', [Validators.required]],
      logradouro: ['', [Validators.required]],
      numero: ['', [Validators.required]],
      complemento: [''],
      bairro: ['', [Validators.required]],
      localidade: ['', [Validators.required]],
      uf: ['', [Validators.required]],
      estado: ['', [Validators.required]],
      regiao: ['', [Validators.required]],
      ibge: ['', [Validators.required]],
      ddd: ['', [Validators.required]]
    });
  }

  buscarCep() {
    const cep = this.form.get('cep')?.value?.replace(/\D/g, '');
    if (cep && cep.length === 8) {
      this.consultarCep(cep);
    }
  }

  onCepComplete(event: any) {
    const cep = event.value?.replace(/\D/g, '');
    if (cep && cep.length === 8) {
      this.consultarCep(cep);
    }
  }

  consultarCep(cep: string) {
    this.loadingCep = true;
    this.enderecoService.consultarCep(cep).subscribe({
      next: (data) => {
        this.form.patchValue({
          logradouro: data.logradouro,
          bairro: data.bairro,
          localidade: data.localidade,
          uf: data.uf,
          estado: data.estado,
          regiao: data.regiao,
          ibge: data.ibge,
          ddd: data.ddd
        });
        this.loadingCep = false;
        this.messageService.add({
          severity: 'success',
          summary: 'CEP encontrado',
          detail: 'Endereço preenchido automaticamente'
        });
      },
      error: () => {
        this.loadingCep = false;
        this.messageService.add({
          severity: 'warn',
          summary: 'CEP não encontrado',
          detail: 'Preencha os campos manualmente'
        });
      }
    });
  }

  loadEndereco() {
    if (!this.enderecoId) return;

    this.loading = true;
    this.enderecoService.getById(this.enderecoId).subscribe({
      next: (endereco) => {
        this.form.patchValue(endereco);
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao carregar endereço'
        });
        this.loading = false;
        this.router.navigate(['/enderecos']);
      }
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return;
    }

    this.loading = true;

    if (this.isEditMode && this.enderecoId) {
      const dto: AtualizarEnderecoDto = {
        logradouro: this.form.value.logradouro,
        numero: this.form.value.numero,
        complemento: this.form.value.complemento,
        bairro: this.form.value.bairro,
        localidade: this.form.value.localidade
      };

      this.enderecoService.update(this.enderecoId, dto).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Sucesso',
            detail: 'Endereço atualizado com sucesso'
          });
          this.router.navigate(['/enderecos']);
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao atualizar endereço'
          });
          this.loading = false;
        }
      });
    } else {
      const dto: CriarEnderecoDto = this.form.value;

      this.enderecoService.create(dto).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Sucesso',
            detail: 'Endereço criado com sucesso'
          });
          this.router.navigate(['/enderecos']);
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao criar endereço'
          });
          this.loading = false;
        }
      });
    }
  }

  cancel() {
    this.router.navigate(['/enderecos']);
  }
}
