using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;
public static class ArtistasExtensions
{
    public static void AddEndpointsArtistas(this WebApplication app)
    {
        app.MapGet("/Artistas", (DAL<Artista> dal) =>
        {
            return Results.Ok(dal.Listar());
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

        app.MapPost("/Artistas", ([FromBody] Artista artista, DAL<Artista> dal) =>
        {
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