using BancoPan.Cadastro.Application.DTOs;

namespace BancoPan.Cadastro.Application.Interfaces;

public interface IPessoaJuridicaService
{
    Task<PessoaJuridicaDto?> ObterPorIdAsync(Guid id);
    Task<PessoaJuridicaDto?> ObterPorCnpjAsync(string cnpj);
    Task<IEnumerable<PessoaJuridicaDto>> ObterTodosAsync();
    Task<PessoaJuridicaDto> CriarAsync(CriarPessoaJuridicaDto dto);
    Task<PessoaJuridicaDto?> AtualizarAsync(Guid id, AtualizarPessoaJuridicaDto dto);
    Task<bool> RemoverAsync(Guid id);
}
