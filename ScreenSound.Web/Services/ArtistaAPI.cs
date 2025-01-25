using System;
using System.Net.Http.Json;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;

namespace ScreenSound.Web.Services;

public class ArtistaAPI
{
    private readonly HttpClient _httpClient;
    public ArtistaAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>("Artistas");
    }
    public async Task AddArtistaAsync(ArtistaRequest artista)
    {
        await _httpClient.PostAsJsonAsync("Artistas", artista);
    }
    public async Task UpdateArtistaAsync(ArtistaRequestEdit artista)
    {
        await _httpClient.PutAsJsonAsync("Artistas", artista);
    }
    public async Task DeleteArtistaAsync(int artistaId)
    {
        await _httpClient.DeleteAsync($"Artistas/{artistaId}");
    }

    public async Task<ArtistaResponse?> GetArtistaFromNameAsync(string nomeArtista)
    {
        return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"/Artistas/{nomeArtista}");
    }
}
