import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { PessoaJuridica, CriarPessoaJuridicaDto, AtualizarPessoaJuridicaDto } from '../models/pessoa-juridica.model';
import { PagedResult } from '../../../shared/models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class PessoaJuridicaService {
  private apiUrl = `${environment.apiUrl}/PessoaJuridica`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<PessoaJuridica[]> {
    return this.http.get<PessoaJuridica[]>(this.apiUrl);
  }

  getPaginated(pageNumber: number, pageSize: number): Observable<PagedResult<PessoaJuridica>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PagedResult<PessoaJuridica>>(`${this.apiUrl}/paginado`, { params });
  }

  getById(id: string): Observable<PessoaJuridica> {
    return this.http.get<PessoaJuridica>(`${this.apiUrl}/${id}`);
  }

  getByCnpj(cnpj: string): Observable<PessoaJuridica> {
    const cleanCnpj = cnpj.replace(/\D/g, '');
    return this.http.get<PessoaJuridica>(`${this.apiUrl}/cnpj/${cleanCnpj}`);
  }

  create(dto: CriarPessoaJuridicaDto): Observable<PessoaJuridica> {
    return this.http.post<PessoaJuridica>(this.apiUrl, dto);
  }

  update(id: string, dto: AtualizarPessoaJuridicaDto): Observable<PessoaJuridica> {
    return this.http.put<PessoaJuridica>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
