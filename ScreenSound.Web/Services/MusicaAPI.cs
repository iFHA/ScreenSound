using System;
using System.Net.Http.Json;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;

namespace ScreenSound.Web.Services;

public class MusicaAPI
{
    private readonly HttpClient _httpClient;
    public MusicaAPI(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<ICollection<MusicaResponse>?> GetMusicasAsync()
    {
        return await _httpClient.GetFromJsonAsync<ICollection<MusicaResponse>>("Musicas");
    }
    public async Task AddMusicaAsync(MusicaRequest musica)
    {
        await _httpClient.PostAsJsonAsync("Musicas", musica);
    }
    public async Task UpdateMusicaAsync(MusicaRequestEdit musica)
    {
        await _httpClient.PutAsJsonAsync($"Musicas/{musica.Id}", musica);
    }
    public async Task DeleteMusicaAsync(int musicaId)
    {
        await _httpClient.DeleteAsync($"Musicas/{musicaId}");
    }

    public async Task<MusicaResponse?> GetMusicaFromNameAsync(string nomeMusica)
    {
        return await _httpClient.GetFromJsonAsync<MusicaResponse>($"/Musicas/{nomeMusica}");
    }
}
