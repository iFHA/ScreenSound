using System;
using System.Net.Http.Json;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;

namespace ScreenSound.Web.Services;

public class AuthAPI
{
    private readonly HttpClient _httpClient;
    public AuthAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest login)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", login);
        var sucesso = false;
        string[] erros = ["Login e/ou senha inválido(s)"];

        if (response.IsSuccessStatusCode)
        {
            return new LoginResponse(true, null);
        }
        return new LoginResponse(sucesso, erros);
    }
}
