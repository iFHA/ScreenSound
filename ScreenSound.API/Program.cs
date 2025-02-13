using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;

var builder = WebApplication.CreateBuilder(args);

// builder.Host.ConfigureAppConfiguration(config =>
// {
//     var settings = config.Build();
//     config.AddAzureAppConfiguration("Endpoint=https://screensound0-configuration.azconfig.io;Id=/mBx;Secret=4wnhT62bdNZj9CC4lkAXe2QUoEsRgnnUdXiTpJN2ciQRE68n5ixNJQQJ99BAACZoyfij5AKWAAACAZACo3K0");
// });

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ScreenSoundContext>((options) =>
{
    options
    .UseSqlServer(builder.Configuration["ConnectionStrings:ScreenSoundDB"])
            .UseLazyLoadingProxies();
});

builder.Services
.AddIdentityApiEndpoints<PessoaComAcesso>()
.AddEntityFrameworkStores<ScreenSoundContext>();

builder.Services.AddAuthorization();

builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();
builder.Services.AddTransient<DAL<Genero>>();
builder.Services.AddTransient<DAL<PessoaComAcesso>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "http://localhost:5160",
            builder.Configuration["FrontendUrl"] ?? "http://localhost:5057"])
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));

var app = builder.Build();

app.UseCors("wasm");
app.UseStaticFiles();

app.UseAuthorization();

app.AddEndpointsArtistas();
app.AddEndpointsMusicas();
app.AddEndpointsGeneros();

app.MapGroup("auth")
.MapIdentityApi<PessoaComAcesso>()
.WithTags("Autorização");

app.MapPost("auth/logout", async ([FromServices] SignInManager<PessoaComAcesso> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization()
.WithTags("Autorização");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();