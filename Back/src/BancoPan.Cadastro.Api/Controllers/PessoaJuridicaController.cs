using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Application.Interfaces;
using BancoPan.Cadastro.Application.Services;
using BancoPan.Cadastro.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BancoPan.Cadastro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoaJuridicaController : ControllerBase
{
    private readonly IPessoaJuridicaService _pessoaJuridicaService;

    public PessoaJuridicaController(IPessoaJuridicaService pessoaJuridicaService)
    {
        _pessoaJuridicaService = pessoaJuridicaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PessoaJuridicaDto>>> ObterTodos()
    {
        var pessoas = await _pessoaJuridicaService.ObterTodosAsync();
        return Ok(pessoas);
    }

    [HttpGet("paginado")]
    public async Task<ActionResult<PagedResultDto<PessoaJuridicaDto>>> ObterPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var parameters = new PaginationParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _pessoaJuridicaService.ObterPaginadoAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PessoaJuridicaDto>> ObterPorId(Guid id)
    {
        var pessoa = await _pessoaJuridicaService.ObterPorIdAsync(id);

        if (pessoa == null)
            return NotFound(new { message = "Pessoa jurídica não encontrada" });

        return Ok(pessoa);
    }

    [HttpGet("cnpj/{cnpj}")]
    public async Task<ActionResult<PessoaJuridicaDto>> ObterPorCnpj(string cnpj)
    {
        var pessoa = await _pessoaJuridicaService.ObterPorCnpjAsync(cnpj);

        if (pessoa == null)
            return NotFound(new { message = "Pessoa jurídica não encontrada" });

        return Ok(pessoa);
    }

    [HttpPost]
    public async Task<ActionResult<PessoaJuridicaDto>> Criar([FromBody] CriarPessoaJuridicaDto dto)
    {
        try
        {
            var pessoa = await _pessoaJuridicaService.CriarAsync(dto);
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
    public async Task<ActionResult<PessoaJuridicaDto>> Atualizar(Guid id, [FromBody] AtualizarPessoaJuridicaDto dto)
    {
        try
        {
            var pessoa = await _pessoaJuridicaService.AtualizarAsync(id, dto);

            if (pessoa == null)
                return NotFound(new { message = "Pessoa jurídica não encontrada" });

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
        var removido = await _pessoaJuridicaService.RemoverAsync(id);

        if (!removido)
            return NotFound(new { message = "Pessoa jurídica não encontrada" });

        return NoContent();
    }
}
