using System;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;

namespace ScreenSound.Web.Services;

public class AuthAPI : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private bool IsAuthenticated = false;
    public AuthAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var pessoa = new ClaimsPrincipal();
        IsAuthenticated = false;
        // obtendo informações do usuário logado
        var response = await _httpClient.GetAsync("auth/manage/info");
        if (response.IsSuccessStatusCode)
        {
            var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
            Claim[] dados = [
                new Claim(ClaimTypes.Name, info.Email),
                new Claim(ClaimTypes.Email, info.Email),
            ];
            var identity = new ClaimsIdentity(dados, "cookies");
            pessoa = new ClaimsPrincipal(identity);
            IsAuthenticated = true;
        }
        return new AuthenticationState(pessoa);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest login)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login?useCookies=true", login);
        var sucesso = false;
        string[] erros = ["Login e/ou senha inválido(s)"];

        if (response.IsSuccessStatusCode)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return new LoginResponse(true, null);
        }
        return new LoginResponse(sucesso, erros);
    }
    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("/auth/logout", null);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<bool> VerificaAutenticado()
    {
        await GetAuthenticationStateAsync();
        return IsAuthenticated;
    }
}
