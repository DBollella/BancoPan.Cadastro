import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EnderecoService } from './endereco.service';
import { Endereco, CriarEnderecoDto, ViaCepResponse } from '../models/endereco.model';
import { environment } from '../../../../environments/environment';

describe('EnderecoService', () => {
  let service: EnderecoService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EnderecoService]
    });
    service = TestBed.inject(EnderecoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAll', () => {
    it('should return an array of enderecos', () => {
      const mockEnderecos: Endereco[] = [
        {
          id: '1',
          cep: '01310-100',
          logradouro: 'Avenida Paulista',
          numero: '1578',
          bairro: 'Bela Vista',
          localidade: 'São Paulo',
          uf: 'SP',
          estado: 'São Paulo',
          regiao: 'Sudeste',
          ibge: '3550308',
          ddd: '11'
        }
      ];

      service.getAll().subscribe(enderecos => {
        expect(enderecos.length).toBe(1);
        expect(enderecos).toEqual(mockEnderecos);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/enderecos`);
      expect(req.request.method).toBe('GET');
      req.flush(mockEnderecos);
    });
  });

  describe('getById', () => {
    it('should return a single endereco', () => {
      const mockEndereco: Endereco = {
        id: '1',
        cep: '01310-100',
        logradouro: 'Avenida Paulista',
        numero: '1578',
        bairro: 'Bela Vista',
        localidade: 'São Paulo',
        uf: 'SP',
        estado: 'São Paulo',
        regiao: 'Sudeste',
        ibge: '3550308',
        ddd: '11'
      };

      service.getById('1').subscribe(endereco => {
        expect(endereco).toEqual(mockEndereco);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/enderecos/1`);
      expect(req.request.method).toBe('GET');
      req.flush(mockEndereco);
    });
  });

  describe('consultarCep', () => {
    it('should call the CEP API and return address data', () => {
      const mockResponse: ViaCepResponse = {
        cep: '01310-100',
        logradouro: 'Avenida Paulista',
        complemento: '',
        bairro: 'Bela Vista',
        localidade: 'São Paulo',
        uf: 'SP',
        estado: 'São Paulo',
        regiao: 'Sudeste',
        ibge: '3550308',
        ddd: '11'
      };

      service.consultarCep('01310-100').subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/enderecos/consultar-cep/01310100`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });

    it('should remove non-numeric characters from CEP', () => {
      const mockResponse: ViaCepResponse = {
        cep: '01310-100',
        logradouro: 'Avenida Paulista',
        complemento: '',
        bairro: 'Bela Vista',
        localidade: 'São Paulo',
        uf: 'SP',
        estado: 'São Paulo',
        regiao: 'Sudeste',
        ibge: '3550308',
        ddd: '11'
      };

      service.consultarCep('01310-100').subscribe();

      const req = httpMock.expectOne(`${environment.apiUrl}/enderecos/consultar-cep/01310100`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });
  });

  describe('create', () => {
    it('should create a new endereco', () => {
      const dto: CriarEnderecoDto = {
        cep: '01310-100',
        logradouro: 'Avenida Paulista',
        numero: '1578',
        bairro: 'Bela Vista',
        localidade: 'São Paulo',
        uf: 'SP',
        estado: 'São Paulo',
        regiao: 'Sudeste',
        ibge: '3550308',
        ddd: '11'
      };

      const mockEndereco: Endereco = { id: '1', ...dto };

      service.create(dto).subscribe(endereco => {
        expect(endereco).toEqual(mockEndereco);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/enderecos`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(dto);
      req.flush(mockEndereco);
    });
  });

  describe('delete', () => {
    it('should delete an endereco', () => {
      service.delete('1').subscribe();

      const req = httpMock.expectOne(`${environment.apiUrl}/enderecos/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush({});
    });
  });
});
