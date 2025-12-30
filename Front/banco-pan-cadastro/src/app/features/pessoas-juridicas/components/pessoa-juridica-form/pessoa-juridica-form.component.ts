import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { InputMaskModule } from 'primeng/inputmask';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { DividerModule } from 'primeng/divider';
import { MessageService } from 'primeng/api';
import { switchMap } from 'rxjs/operators';
import { PessoaJuridicaService } from '../../services/pessoa-juridica.service';
import { EnderecoService } from '../../../enderecos/services/endereco.service';
import { CriarPessoaJuridicaDto, AtualizarPessoaJuridicaDto } from '../../models/pessoa-juridica.model';
import { CriarEnderecoDto } from '../../../enderecos/models/endereco.model';
import { cnpjValidator } from '../../../../shared/validators/cnpj.validator';

interface UF {
  sigla: string;
  nome: string;
}

@Component({
  selector: 'app-pessoa-juridica-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    InputTextModule,
    InputMaskModule,
    ButtonModule,
    CalendarModule,
    DropdownModule,
    DividerModule
  ],
  templateUrl: './pessoa-juridica-form.component.html',
  styleUrl: './pessoa-juridica-form.component.scss'
})
export class PessoaJuridicaFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  pessoaId?: string;
  loading = false;
  loadingCep = false;

  ufs: UF[] = [
    { sigla: 'AC', nome: 'Acre' }, { sigla: 'AL', nome: 'Alagoas' }, { sigla: 'AP', nome: 'Amapá' },
    { sigla: 'AM', nome: 'Amazonas' }, { sigla: 'BA', nome: 'Bahia' }, { sigla: 'CE', nome: 'Ceará' },
    { sigla: 'DF', nome: 'Distrito Federal' }, { sigla: 'ES', nome: 'Espírito Santo' },
    { sigla: 'GO', nome: 'Goiás' }, { sigla: 'MA', nome: 'Maranhão' }, { sigla: 'MT', nome: 'Mato Grosso' },
    { sigla: 'MS', nome: 'Mato Grosso do Sul' }, { sigla: 'MG', nome: 'Minas Gerais' },
    { sigla: 'PA', nome: 'Pará' }, { sigla: 'PB', nome: 'Paraíba' }, { sigla: 'PR', nome: 'Paraná' },
    { sigla: 'PE', nome: 'Pernambuco' }, { sigla: 'PI', nome: 'Piauí' }, { sigla: 'RJ', nome: 'Rio de Janeiro' },
    { sigla: 'RN', nome: 'Rio Grande do Norte' }, { sigla: 'RS', nome: 'Rio Grande do Sul' },
    { sigla: 'RO', nome: 'Rondônia' }, { sigla: 'RR', nome: 'Roraima' }, { sigla: 'SC', nome: 'Santa Catarina' },
    { sigla: 'SP', nome: 'São Paulo' }, { sigla: 'SE', nome: 'Sergipe' }, { sigla: 'TO', nome: 'Tocantins' }
  ];

  constructor(
    private fb: FormBuilder,
    private pessoaJuridicaService: PessoaJuridicaService,
    private enderecoService: EnderecoService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.initForm();

    this.pessoaId = this.route.snapshot.params['id'];
    if (this.pessoaId) {
      this.isEditMode = true;
      this.loadPessoa();
    }
  }

  initForm() {
    this.form = this.fb.group({
      razaoSocial: ['', [Validators.required]],
      nomeFantasia: [''],
      email: ['', [Validators.required, Validators.email]],
      telefone: [''],
      cnpj: ['', [Validators.required, cnpjValidator()]],
      dataAbertura: ['', [Validators.required]],
      inscricaoEstadual: [''],
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

  loadPessoa() {
    if (!this.pessoaId) return;

    this.loading = true;
    this.pessoaJuridicaService.getById(this.pessoaId).subscribe({
      next: (pessoa) => {
        this.form.patchValue({
          razaoSocial: pessoa.razaoSocial,
          nomeFantasia: pessoa.nomeFantasia,
          email: pessoa.email,
          telefone: pessoa.telefone,
          cnpj: pessoa.cnpj,
          dataAbertura: new Date(pessoa.dataAbertura),
          inscricaoEstadual: pessoa.inscricaoEstadual
        });

        if (pessoa.endereco) {
          this.form.patchValue({
            cep: pessoa.endereco.cep,
            logradouro: pessoa.endereco.logradouro,
            numero: pessoa.endereco.numero,
            complemento: pessoa.endereco.complemento,
            bairro: pessoa.endereco.bairro,
            localidade: pessoa.endereco.localidade,
            uf: pessoa.endereco.uf,
            estado: pessoa.endereco.estado,
            regiao: pessoa.endereco.regiao,
            ibge: pessoa.endereco.ibge,
            ddd: pessoa.endereco.ddd
          });
        }
        this.loading = false;
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: 'Erro ao carregar pessoa jurídica'
        });
        this.loading = false;
        this.router.navigate(['/pessoas-juridicas']);
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

    if (this.isEditMode && this.pessoaId) {
      const dto: AtualizarPessoaJuridicaDto = {
        razaoSocial: this.form.value.razaoSocial,
        nomeFantasia: this.form.value.nomeFantasia,
        email: this.form.value.email,
        telefone: this.form.value.telefone,
        dataAbertura: this.formatDate(this.form.value.dataAbertura),
        inscricaoEstadual: this.form.value.inscricaoEstadual
      };

      this.pessoaJuridicaService.update(this.pessoaId, dto).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Sucesso',
            detail: 'Pessoa jurídica atualizada com sucesso'
          });
          this.router.navigate(['/pessoas-juridicas']);
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao atualizar pessoa jurídica'
          });
          this.loading = false;
        }
      });
    } else {
      const enderecoDto: CriarEnderecoDto = {
        cep: this.form.value.cep,
        logradouro: this.form.value.logradouro,
        numero: this.form.value.numero,
        complemento: this.form.value.complemento,
        bairro: this.form.value.bairro,
        localidade: this.form.value.localidade,
        uf: this.form.value.uf,
        estado: this.form.value.estado,
        regiao: this.form.value.regiao,
        ibge: this.form.value.ibge,
        ddd: this.form.value.ddd
      };

      this.enderecoService.create(enderecoDto).pipe(
        switchMap((endereco) => {
          const pessoaDto: CriarPessoaJuridicaDto = {
            razaoSocial: this.form.value.razaoSocial,
            nomeFantasia: this.form.value.nomeFantasia,
            email: this.form.value.email,
            telefone: this.form.value.telefone,
            cnpj: this.form.value.cnpj.replace(/\D/g, ''),
            dataAbertura: this.formatDate(this.form.value.dataAbertura),
            inscricaoEstadual: this.form.value.inscricaoEstadual,
            enderecoId: endereco.id
          };
          return this.pessoaJuridicaService.create(pessoaDto);
        })
      ).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Sucesso',
            detail: 'Pessoa jurídica criada com sucesso'
          });
          this.router.navigate(['/pessoas-juridicas']);
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: 'Erro ao criar pessoa jurídica'
          });
          this.loading = false;
        }
      });
    }
  }

  formatDate(date: Date): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  cancel() {
    this.router.navigate(['/pessoas-juridicas']);
  }
}
