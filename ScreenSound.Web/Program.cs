using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ScreenSound.Web;
using ScreenSound.Web.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// Cria uma única instância de HttpClient e a reutiliza
builder.Services.AddSingleton(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!)
    };
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    return httpClient;
});

builder.Services.AddScoped<ArtistaAPI>();
builder.Services.AddScoped<GeneroAPI>();
builder.Services.AddScoped<MusicaAPI>();
builder.Services.AddScoped<LoginAPI>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
