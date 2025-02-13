﻿namespace ScreenSound.Modelos;

public class Artista
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Bio { get; set; }
    public string FotoPerfil { get; set; }
    public virtual ICollection<Musica> Musicas { get; set; } = new List<Musica>();
    public virtual ICollection<AvaliacaoArtista> Avaliacoes { get; set; } = new List<AvaliacaoArtista>();
    public Artista() { Nome = ""; Bio = ""; FotoPerfil = ""; }
    public Artista(string nome, string bio)
    {
        Nome = nome;
        Bio = bio;
        FotoPerfil = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png";
    }

    public void AdicionarMusica(Musica musica)
    {
        Musicas.Add(musica);
    }

    public void ExibirDiscografia()
    {
        Console.WriteLine($"Discografia do artista {Nome}");
        foreach (var musica in Musicas)
        {
            Console.WriteLine($"Música: {musica.Nome}, ano de lançamento: {musica.AnoLancamento}");
        }
    }

    public override string ToString()
    {
        return $@"Id: {Id}
            Nome: {Nome}
            Foto de Perfil: {FotoPerfil}
            Bio: {Bio}";
    }

    public void AdicionarNota(int pessoaId, double nota)
    {
        nota = Math.Clamp(nota, 1, 5);
        Avaliacoes.Add(new AvaliacaoArtista() { ArtistaId = this.Id, PessoaId = pessoaId, Nota = nota });
    }
}