using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;

namespace ScreenSound.API.Endpoints;
public static class ArtistasExtensions
{
    public static void AddEndpointsArtistas(this WebApplication app)
    {
        app.MapGet("/Artistas", (DAL<Artista> dal) =>
        {
            var artistas = dal.Listar();
            var artistasResponse = artistas.Select(artista => new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil));
            return Results.Ok(artistasResponse);
        });

        app.MapGet("/Artistas/{nome}", (string nome, DAL<Artista> dal) =>
        {
            var result = dal.RecuperarPor(artista => artista.Nome.ToLower().Contains(nome.ToLower()));
            if (result is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);

        });

        app.MapPost("/Artistas", async ([FromServices] IHostEnvironment env, [FromBody] ArtistaRequest artistaRequest, DAL<Artista> dal) =>
        {
            var imagemArtista = "";
            if (artistaRequest.fotoPerfil is not null)
            {
                var nome = artistaRequest.nome.Trim();
                imagemArtista = DateTime.Now.ToString("ddMMyyyyhhss") + "." + nome + ".jpeg";
                var path = Path.Combine(env.ContentRootPath, "wwwroot", "FotosPerfil", imagemArtista);
                using MemoryStream ms = new MemoryStream(Convert.FromBase64String(artistaRequest.fotoPerfil));
                using FileStream fs = new FileStream(path, FileMode.Create);
                await ms.CopyToAsync(fs);
            }
            var artista = new Artista(artistaRequest.nome, artistaRequest.bio)
            {
                FotoPerfil = imagemArtista is not null ? $"/FotosPerfil/{imagemArtista}" : ""
            };
            dal.Adicionar(artista);
            return Results.Created();
        });

        app.MapPut("/Artistas", ([FromBody] Artista artista, DAL<Artista> dal) =>
        {
            if (artista is null)
            {
                return Results.NotFound();
            }
            dal.Atualizar(artista);
            return Results.Ok(artista);
        });

        app.MapDelete("/Artistas/{Id:int}", (int Id, DAL<Artista> dal) =>
        {
            var artista = dal.RecuperarPor(artista => artista.Id.Equals(Id));
            if (artista is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(artista);
            return Results.NoContent();
        });
    }
}