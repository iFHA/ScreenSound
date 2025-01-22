using System;
using System.Net.Http.Json;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;

namespace ScreenSound.Web.Services;

public class ArtistaAPI
{
    private readonly HttpClient _httpClient;
    public ArtistaAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>("Artistas");
    }
    public async Task AddArtistaAsync(ArtistaRequest artista)
    {
        await _httpClient.PostAsJsonAsync("Artistas", artista);
    }
}
