import { FormControl } from '@angular/forms';
import { cpfValidator } from './cpf.validator';

describe('CPF Validator', () => {
  const validator = cpfValidator();

  it('should validate a valid CPF', () => {
    const control = new FormControl('11144477735');
    const result = validator(control);
    expect(result).toBeNull();
  });

  it('should validate a valid CPF with formatting', () => {
    const control = new FormControl('111.444.777-35');
    const result = validator(control);
    expect(result).toBeNull();
  });

  it('should invalidate CPF with all same digits', () => {
    const control = new FormControl('111.111.111-11');
    const result = validator(control);
    expect(result).toEqual({ cpfInvalid: true });
  });

  it('should invalidate CPF with wrong check digits', () => {
    const control = new FormControl('111.444.777-00');
    const result = validator(control);
    expect(result).toEqual({ cpfInvalid: true });
  });

  it('should invalidate CPF with wrong length', () => {
    const control = new FormControl('111.444.777');
    const result = validator(control);
    expect(result).toEqual({ cpfInvalid: true });
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
