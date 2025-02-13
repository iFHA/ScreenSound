using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;
public static class GenerosExtensions
{
    public static void AddEndpointsGeneros(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("generos").RequireAuthorization();

        groupBuilder.MapGet("", (DAL<Genero> dal) =>
        {
            return Results.Ok(dal.Listar());
        });

        groupBuilder.MapGet("PorNome/{Nome}", (string Nome, DAL<Genero> dal) =>
        {
            var result = dal.RecuperarPor(genero => genero.Nome.ToLower().Contains(Nome.ToLower()));
            if (result is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);

        });

        groupBuilder.MapGet("{Id}", (int Id, DAL<Genero> dal) =>
        {
            var result = dal.RecuperarPor(genero => genero.Id.Equals(Id));
            if (result is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);

        });

        groupBuilder.MapPost("", ([FromBody] GeneroRequest generoRequest, DAL<Genero> dal) =>
        {
            var genero = new Genero()
            {
                Nome = generoRequest.Nome,
                Descricao = generoRequest.Descricao
            };
            dal.Adicionar(genero);
            return Results.Created();
        });

        groupBuilder.MapPut("{Id}", (int Id, [FromBody] Genero genero, DAL<Genero> dal) =>
        {
            var generoDb = dal.RecuperarPor(genero => genero.Id.Equals(Id));
            if (generoDb is null)
            {
                return Results.NotFound();
            }
            genero.Id = generoDb.Id;
            dal.Atualizar(genero);
            return Results.Ok(genero);
        });

        groupBuilder.MapDelete("{Id}", (int Id, DAL<Genero> dal) =>
        {
            var genero = dal.RecuperarPor(genero => genero.Id.Equals(Id));
            if (genero is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(genero);
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