using System;
using System.Net.Http.Json;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;

namespace ScreenSound.Web.Services;

public class LoginAPI
{
    private readonly HttpClient _httpClient;
    public LoginAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<bool> LoginAsync(LoginRequest login)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", login);

        if (response.IsSuccessStatusCode)
        {
            // Desserializa o conteúdo da resposta como LoginResponse
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (loginResponse is not null)
            {
                // Adicionando o bearer token no cabeçalho das futuras requisições
                var authToken = loginResponse.accessToken;
                _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                return true;
            }
        }
        return false;
    }
}
