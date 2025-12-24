using BancoPan.Cadastro.Domain.Models;

namespace BancoPan.Cadastro.Domain.Interfaces;

public interface IViaCepService
{
    Task<ViaCepResponse?> ConsultarCepAsync(string cep);
}
