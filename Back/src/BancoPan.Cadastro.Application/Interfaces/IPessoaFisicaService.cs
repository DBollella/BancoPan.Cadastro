using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Domain.Common;

namespace BancoPan.Cadastro.Application.Interfaces;

public interface IPessoaFisicaService
{
    Task<PessoaFisicaDto?> ObterPorIdAsync(Guid id);
    Task<PessoaFisicaDto?> ObterPorCpfAsync(string cpf);
    Task<IEnumerable<PessoaFisicaDto>> ObterTodosAsync();
    Task<PagedResultDto<PessoaFisicaDto>> ObterPaginadoAsync(PaginationParameters parameters);
    Task<PessoaFisicaDto> CriarAsync(CriarPessoaFisicaDto dto);
    Task<PessoaFisicaDto?> AtualizarAsync(Guid id, AtualizarPessoaFisicaDto dto);
    Task<bool> RemoverAsync(Guid id);
}
