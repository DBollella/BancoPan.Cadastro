import { FormControl } from '@angular/forms';
import { cnpjValidator } from './cnpj.validator';

describe('CNPJ Validator', () => {
  const validator = cnpjValidator();

  it('should validate a valid CNPJ', () => {
    const control = new FormControl('11222333000181');
    const result = validator(control);
    expect(result).toBeNull();
  });

  it('should validate a valid CNPJ with formatting', () => {
    const control = new FormControl('11.222.333/0001-81');
    const result = validator(control);
    expect(result).toBeNull();
  });

  it('should invalidate CNPJ with all same digits', () => {
    const control = new FormControl('11.111.111/1111-11');
    const result = validator(control);
    expect(result).toEqual({ cnpjInvalid: true });
  });

  it('should invalidate CNPJ with wrong check digits', () => {
    const control = new FormControl('11.222.333/0001-00');
    const result = validator(control);
    expect(result).toEqual({ cnpjInvalid: true });
  });

  it('should invalidate CNPJ with wrong length', () => {
    const control = new FormControl('11.222.333');
    const result = validator(control);
    expect(result).toEqual({ cnpjInvalid: true });
  });

  it('should return null for empty value', () => {
    const control = new FormControl('');
    const result = validator(control);
    expect(result).toBeNull();
  });

  it('should return null for null value', () => {
    const control = new FormControl(null);
    const result = validator(control);
    expect(result).toBeNull();
  });
});
