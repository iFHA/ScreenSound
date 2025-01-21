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

        app.MapPost("/Artistas", ([FromBody] ArtistaRequest artistaRequest, DAL<Artista> dal) =>
        {
            var artista = new Artista(artistaRequest.nome, artistaRequest.bio);
            dal.Adicionar(artista);
            return Results.Created();
        });

        app.MapPut("/Artistas/{Id}", (int Id, [FromBody] Artista artista, DAL<Artista> dal) =>
        {
            var artistaDb = dal.RecuperarPor(artista => artista.Id.Equals(Id));
            if (artistaDb is null)
            {
                return Results.NotFound();
            }
            artista.Id = artistaDb.Id;
            dal.Atualizar(artista);
            return Results.Ok(artista);
        });

        app.MapDelete("/Artistas/{Id}", (int Id, DAL<Artista> dal) =>
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