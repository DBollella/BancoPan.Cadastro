using BancoPan.Cadastro.Domain.Interfaces;
using BancoPan.Cadastro.Domain.Models;
using System.Text.Json;

namespace BancoPan.Cadastro.Infra.ExternalServices;

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://viacep.com.br/ws";

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ViaCepResponse?> ConsultarCepAsync(string cep)
    {
        try
        {
            var cepLimpo = cep.Replace("-", "").Replace(".", "").Trim();

            if (string.IsNullOrWhiteSpace(cepLimpo) || cepLimpo.Length != 8)
                return null;

            var response = await _httpClient.GetAsync($"{BaseUrl}/{cepLimpo}/json/");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return null;

            var viaCepResponse = JsonSerializer.Deserialize<ViaCepResponse>(content);

            if (viaCepResponse != null && !string.IsNullOrWhiteSpace(viaCepResponse.Cep))
                return viaCepResponse;

            return null;
        }
        catch
        {
            return null;
        }
    }
}
