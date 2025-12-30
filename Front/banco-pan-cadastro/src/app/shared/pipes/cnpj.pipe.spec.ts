import { CnpjPipe } from './cnpj.pipe';

describe('CnpjPipe', () => {
  let pipe: CnpjPipe;

  beforeEach(() => {
    pipe = new CnpjPipe();
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should format a valid CNPJ', () => {
    const result = pipe.transform('11222333000181');
    expect(result).toBe('11.222.333/0001-81');
  });

  it('should return empty string for null', () => {
    const result = pipe.transform(null);
    expect(result).toBe('');
  });

  it('should return empty string for undefined', () => {
    const result = pipe.transform(undefined);
    expect(result).toBe('');
  });

  it('should return original value if length is not 14', () => {
    const result = pipe.transform('123456');
    expect(result).toBe('123456');
  });

  it('should handle already formatted CNPJ', () => {
    const result = pipe.transform('11.222.333/0001-81');
    expect(result).toBe('11.222.333/0001-81');
  });
});
