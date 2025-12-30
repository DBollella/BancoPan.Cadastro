import { CpfPipe } from './cpf.pipe';

describe('CpfPipe', () => {
  let pipe: CpfPipe;

  beforeEach(() => {
    pipe = new CpfPipe();
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should format a valid CPF', () => {
    const result = pipe.transform('11144477735');
    expect(result).toBe('111.444.777-35');
  });

  it('should return empty string for null', () => {
    const result = pipe.transform(null);
    expect(result).toBe('');
  });

  it('should return empty string for undefined', () => {
    const result = pipe.transform(undefined);
    expect(result).toBe('');
  });

  it('should return original value if length is not 11', () => {
    const result = pipe.transform('123456');
    expect(result).toBe('123456');
  });

  it('should handle already formatted CPF', () => {
    const result = pipe.transform('111.444.777-35');
    expect(result).toBe('111.444.777-35');
  });
});
