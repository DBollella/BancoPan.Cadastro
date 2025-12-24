using BancoPan.Cadastro.Application.DTOs;

namespace BancoPan.Cadastro.Application.Interfaces;

public interface IEnderecoService
{
    Task<EnderecoDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EnderecoDto>> ObterTodosAsync();
    Task<EnderecoDto?> ConsultarViaCepAsync(string cep);
    Task<EnderecoDto> CriarAsync(CriarEnderecoDto dto);
    Task<EnderecoDto?> AtualizarAsync(Guid id, AtualizarEnderecoDto dto);
    Task<bool> RemoverAsync(Guid id);
}
