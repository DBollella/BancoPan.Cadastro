export interface Endereco {
  id: string;
  cep: string;
  logradouro: string;
  numero: string;
  complemento?: string;
  bairro: string;
  localidade: string;
  uf: string;
  estado: string;
  regiao: string;
  ibge: string;
  ddd: string;
}

export interface CriarEnderecoDto {
  cep: string;
  logradouro: string;
  numero: string;
  complemento?: string;
  bairro: string;
  localidade: string;
  uf: string;
  estado: string;
  regiao: string;
  ibge: string;
  ddd: string;
}

export interface AtualizarEnderecoDto {
  logradouro: string;
  numero: string;
  complemento?: string;
  bairro: string;
  localidade: string;
}

export interface ViaCepResponse {
  cep: string;
  logradouro: string;
  complemento: string;
  bairro: string;
  localidade: string;
  uf: string;
  estado: string;
  regiao: string;
  ibge: string;
  ddd: string;
}
