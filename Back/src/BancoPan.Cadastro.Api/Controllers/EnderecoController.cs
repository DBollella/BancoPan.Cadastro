using BancoPan.Cadastro.Application.DTOs;
using BancoPan.Cadastro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BancoPan.Cadastro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnderecoController : ControllerBase
{
    private readonly IEnderecoService _enderecoService;

    public EnderecoController(IEnderecoService enderecoService)
    {
        _enderecoService = enderecoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnderecoDto>>> ObterTodos()
    {
        var enderecos = await _enderecoService.ObterTodosAsync();
        return Ok(enderecos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnderecoDto>> ObterPorId(Guid id)
    {
        var endereco = await _enderecoService.ObterPorIdAsync(id);

        if (endereco == null)
            return NotFound(new { message = "Endereço não encontrado" });

        return Ok(endereco);
    }

    [HttpGet("consultar-cep/{cep}")]
    public async Task<ActionResult<EnderecoDto>> ConsultarCep(string cep)
    {
        var endereco = await _enderecoService.ConsultarViaCepAsync(cep);

        if (endereco == null)
            return NotFound(new { message = "CEP não encontrado" });

        return Ok(endereco);
    }

    [HttpPost]
    public async Task<ActionResult<EnderecoDto>> Criar([FromBody] CriarEnderecoDto dto)
    {
        try
        {
            var endereco = await _enderecoService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = endereco.Id }, endereco);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EnderecoDto>> Atualizar(Guid id, [FromBody] AtualizarEnderecoDto dto)
    {
        try
        {
            var endereco = await _enderecoService.AtualizarAsync(id, dto);

            if (endereco == null)
                return NotFound(new { message = "Endereço não encontrado" });

            return Ok(endereco);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var removido = await _enderecoService.RemoverAsync(id);

        if (!removido)
            return NotFound(new { message = "Endereço não encontrado" });

        return NoContent();
    }
}
