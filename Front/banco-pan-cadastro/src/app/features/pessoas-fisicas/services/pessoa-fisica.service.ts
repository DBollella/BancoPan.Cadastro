import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { PessoaFisica, CriarPessoaFisicaDto, AtualizarPessoaFisicaDto } from '../models/pessoa-fisica.model';
import { PagedResult } from '../../../shared/models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class PessoaFisicaService {
  private apiUrl = `${environment.apiUrl}/PessoaFisica`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<PessoaFisica[]> {
    return this.http.get<PessoaFisica[]>(this.apiUrl);
  }

  getPaginated(pageNumber: number, pageSize: number): Observable<PagedResult<PessoaFisica>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PagedResult<PessoaFisica>>(`${this.apiUrl}/paginado`, { params });
  }

  getById(id: string): Observable<PessoaFisica> {
    return this.http.get<PessoaFisica>(`${this.apiUrl}/${id}`);
  }

  getByCpf(cpf: string): Observable<PessoaFisica> {
    const cleanCpf = cpf.replace(/\D/g, '');
    return this.http.get<PessoaFisica>(`${this.apiUrl}/cpf/${cleanCpf}`);
  }

  create(dto: CriarPessoaFisicaDto): Observable<PessoaFisica> {
    return this.http.post<PessoaFisica>(this.apiUrl, dto);
  }

  update(id: string, dto: AtualizarPessoaFisicaDto): Observable<PessoaFisica> {
    return this.http.put<PessoaFisica>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
