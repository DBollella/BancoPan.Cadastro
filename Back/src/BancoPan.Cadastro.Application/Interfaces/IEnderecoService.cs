using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Domain.Common;

namespace BancoPan.Cadastro.Application.Interfaces;

public interface IEnderecoService
{
    Task<EnderecoDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EnderecoDto>> ObterTodosAsync();
    Task<PagedResultDto<EnderecoDto>> ObterPaginadoAsync(PaginationParameters parameters);
    Task<EnderecoDto?> ConsultarViaCepAsync(string cep);
    Task<EnderecoDto> CriarAsync(CriarEnderecoDto dto);
    Task<EnderecoDto?> AtualizarAsync(Guid id, AtualizarEnderecoDto dto);
    Task<bool> RemoverAsync(Guid id);
}
