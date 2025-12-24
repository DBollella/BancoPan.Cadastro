using BancoPan.Cadastro.Application.DTOs;

namespace BancoPan.Cadastro.Application.Interfaces;

public interface IPessoaFisicaService
{
    Task<PessoaFisicaDto?> ObterPorIdAsync(Guid id);
    Task<PessoaFisicaDto?> ObterPorCpfAsync(string cpf);
    Task<IEnumerable<PessoaFisicaDto>> ObterTodosAsync();
    Task<PessoaFisicaDto> CriarAsync(CriarPessoaFisicaDto dto);
    Task<PessoaFisicaDto?> AtualizarAsync(Guid id, AtualizarPessoaFisicaDto dto);
    Task<bool> RemoverAsync(Guid id);
}
