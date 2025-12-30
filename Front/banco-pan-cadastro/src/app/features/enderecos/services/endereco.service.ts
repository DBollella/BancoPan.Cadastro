import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Endereco, CriarEnderecoDto, AtualizarEnderecoDto, ViaCepResponse } from '../models/endereco.model';

@Injectable({
  providedIn: 'root'
})
export class EnderecoService {
  private apiUrl = `${environment.apiUrl}/enderecos`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Endereco[]> {
    return this.http.get<Endereco[]>(this.apiUrl);
  }

  getById(id: string): Observable<Endereco> {
    return this.http.get<Endereco>(`${this.apiUrl}/${id}`);
  }

  consultarCep(cep: string): Observable<ViaCepResponse> {
    const cleanCep = cep.replace(/\D/g, '');
    return this.http.get<ViaCepResponse>(`${this.apiUrl}/consultar-cep/${cleanCep}`);
  }

  create(dto: CriarEnderecoDto): Observable<Endereco> {
    return this.http.post<Endereco>(this.apiUrl, dto);
  }

  update(id: string, dto: AtualizarEnderecoDto): Observable<Endereco> {
    return this.http.put<Endereco>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
