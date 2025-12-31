import { Endereco } from '../../endereco/models/endereco.model';

export interface PessoaFisica {
  id: string;
  nome: string;
  email: string;
  telefone?: string;
  cpf: string;
  dataNascimento: string;
  rg?: string;
  idade: number;
  enderecoId: string;
  endereco?: Endereco;
  criadoEm: string;
  atualizadoEm?: string;
}

export interface CriarPessoaFisicaDto {
  nome: string;
  email: string;
  telefone?: string;
  cpf: string;
  dataNascimento: string;
  rg?: string;
  enderecoId: string;
}

export interface AtualizarPessoaFisicaDto {
  nome: string;
  email: string;
  telefone?: string;
  dataNascimento: string;
  rg?: string;
}
