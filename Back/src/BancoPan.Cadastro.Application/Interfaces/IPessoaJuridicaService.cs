using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Domain.Common;

namespace BancoPan.Cadastro.Application.Interfaces;

public interface IPessoaJuridicaService
{
    Task<PessoaJuridicaDto?> ObterPorIdAsync(Guid id);
    Task<PessoaJuridicaDto?> ObterPorCnpjAsync(string cnpj);
    Task<IEnumerable<PessoaJuridicaDto>> ObterTodosAsync();
    Task<PagedResultDto<PessoaJuridicaDto>> ObterPaginadoAsync(PaginationParameters parameters);
    Task<PessoaJuridicaDto> CriarAsync(CriarPessoaJuridicaDto dto);
    Task<PessoaJuridicaDto?> AtualizarAsync(Guid id, AtualizarPessoaJuridicaDto dto);
    Task<bool> RemoverAsync(Guid id);
}
