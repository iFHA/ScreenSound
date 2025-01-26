using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ScreenSound.Web;
using ScreenSound.Web.Services;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthAPI>();
builder.Services.AddScoped<AuthAPI>(sp => (AuthAPI)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<ArtistaAPI>();
builder.Services.AddScoped<GeneroAPI>();
builder.Services.AddScoped<MusicaAPI>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
