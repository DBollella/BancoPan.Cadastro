import { Endereco } from '../../endereco/models/endereco.model';

export interface PessoaJuridica {
  id: string;
  razaoSocial: string;
  nomeFantasia?: string;
  email: string;
  telefone?: string;
  cnpj: string;
  dataAbertura: string;
  inscricaoEstadual?: string;
  tempoAtuacao: number;
  enderecoId: string;
  endereco?: Endereco;
  criadoEm: string;
  atualizadoEm?: string;
}

export interface CriarPessoaJuridicaDto {
  razaoSocial: string;
  nomeFantasia?: string;
  email: string;
  telefone?: string;
  cnpj: string;
  dataAbertura: string;
  inscricaoEstadual?: string;
  enderecoId: string;
}

export interface AtualizarPessoaJuridicaDto {
  razaoSocial: string;
  nomeFantasia?: string;
  email: string;
  telefone?: string;
  dataAbertura: string;
  inscricaoEstadual?: string;
}
