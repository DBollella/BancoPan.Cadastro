using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Application.Interfaces;
using BancoPan.Cadastro.Application.Services;
using BancoPan.Cadastro.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BancoPan.Cadastro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoaFisicaController : ControllerBase
{
    private readonly IPessoaFisicaService _pessoaFisicaService;

    public PessoaFisicaController(IPessoaFisicaService pessoaFisicaService)
    {
        _pessoaFisicaService = pessoaFisicaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaFisicaDto>>> ObterTodos()
    {
        var pessoas = await _pessoaFisicaService.ObterTodosAsync();
        return Ok(pessoas);
    }

    [HttpGet("paginado")]
    public async Task<ActionResult<PagedResultDto<PessoaFisicaDto>>> ObterPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var parameters = new PaginationParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _pessoaFisicaService.ObterPaginadoAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PessoaFisicaDto>> ObterPorId(Guid id)
    {
        var pessoa = await _pessoaFisicaService.ObterPorIdAsync(id);

        if (pessoa == null)
            return NotFound(new { message = "Pessoa física não encontrada" });

        return Ok(pessoa);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<ActionResult<PessoaFisicaDto>> ObterPorCpf(string cpf)
    {
        var pessoa = await _pessoaFisicaService.ObterPorCpfAsync(cpf);

        if (pessoa == null)
            return NotFound(new { message = "Pessoa física não encontrada" });

        return Ok(pessoa);
    }

    [HttpPost]
    public async Task<ActionResult<PessoaFisicaDto>> Criar([FromBody] CriarPessoaFisicaDto dto)
    {
        try
        {
            var pessoa = await _pessoaFisicaService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PessoaFisicaDto>> Atualizar(Guid id, [FromBody] AtualizarPessoaFisicaDto dto)
    {
        try
        {
            var pessoa = await _pessoaFisicaService.AtualizarAsync(id, dto);

            if (pessoa == null)
                return NotFound(new { message = "Pessoa física não encontrada" });

            return Ok(pessoa);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var removido = await _pessoaFisicaService.RemoverAsync(id);

        if (!removido)
            return NotFound(new { message = "Pessoa física não encontrada" });

        return NoContent();
    }
}
