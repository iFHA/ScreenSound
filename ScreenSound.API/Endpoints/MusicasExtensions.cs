using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;
public static class MusicasExtensions
{
    public static void AddEndpointsMusicas(this WebApplication app)
    {
        app.MapGet("/Musicas", (DAL<Musica> dal) =>
        {
            return Results.Ok(dal.Listar());
        });

        app.MapGet("/Musicas/{Id}", (int Id, DAL<Musica> dal) =>
        {
            var result = dal.RecuperarPor(musica => musica.Id.Equals(Id));
            if (result is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);

        });

        app.MapPost("/Musicas", ([FromBody] MusicaRequest musicaRequest, DAL<Musica> dal) =>
        {
            var musica = new Musica(musicaRequest.Nome)
            {
                ArtistaId = musicaRequest.ArtistaId,
                AnoLancamento = musicaRequest.AnoLancamento,
                Generos = musicaRequest.Generos is not null ?
                GeneroRequestConverter(musicaRequest.Generos)
                : new List<Genero>()
            };
            dal.Adicionar(musica);
            return Results.Created();
        });

        app.MapPut("/Musicas/{Id}", (int Id, [FromBody] Musica musica, DAL<Musica> dal) =>
        {
            var musicaDb = dal.RecuperarPor(musica => musica.Id.Equals(Id));
            if (musicaDb is null)
            {
                return Results.NotFound();
            }
            musica.Id = musicaDb.Id;
            dal.Atualizar(musica);
            return Results.Ok(musica);
        });

        app.MapDelete("/Musicas/{Id}", (int Id, DAL<Musica> dal) =>
        {
            var musica = dal.RecuperarPor(musica => musica.Id.Equals(Id));
            if (musica is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(musica);
            return Results.NoContent();
        });
    }

    private static ICollection<Genero> GeneroRequestConverter(ICollection<GeneroRequest> generos)
    {
        return generos.Select(generoRequest => RequestToEntity(generoRequest)).ToList();
    }

    private static Genero RequestToEntity(GeneroRequest generoRequest)
    {
        return new Genero()
        {
            Nome = generoRequest.Nome,
            Descricao = generoRequest.Descricao
        };
    }
}