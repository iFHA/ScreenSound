using Microsoft.Data.SqlClient;
using ScreenSound.Modelos;

namespace ScreenSound.Banco;
public class ArtistaDAL
{

    public IEnumerable<Artista> Listar()
    {
        var lista = new List<Artista>();
        using var conn = new Connection().ObterConexao();
        string sql = "select * from Artistas";
        SqlCommand command = new SqlCommand(sql, conn);
        using SqlDataReader dataReader = command.ExecuteReader();

        while(dataReader.Read()) {
            string nomeArtista = Convert.ToString(dataReader["Nome"]);
            string bioArtista = Convert.ToString(dataReader["Bio"]);
            int idArtista = Convert.ToInt32(dataReader["Id"]);
            Artista artista = new Artista(nomeArtista, bioArtista) { Id = idArtista };
            lista.Add(artista);
        }
        return lista;
    }

    public void Adicionar(Artista artista)
    {
        using var conn = new Connection().ObterConexao();
        string sql = "insert into Artistas (Nome, FotoPerfil, Bio) VALUES (@nome, @perfilPadrao, @bio)";
        SqlCommand command = new SqlCommand(sql, conn);
        command.Parameters.AddWithValue("@nome", artista.Nome);
        command.Parameters.AddWithValue("@perfilPadrao", artista.FotoPerfil);
        command.Parameters.AddWithValue("@bio", artista.Bio);

        int affectedRows = command.ExecuteNonQuery();
        Console.WriteLine($"Linhas afetadas: {affectedRows}");
    }
    public void Atualizar(int id, Artista artista)
    {
        using var conn = new Connection().ObterConexao();
        string sql = "update Artistas set Nome = @nome, FotoPerfil = @foto, Bio = @bio WHERE Id = @id";
        SqlCommand command = new SqlCommand(sql, conn);
        command.Parameters.AddWithValue("@nome", artista.Nome);
        command.Parameters.AddWithValue("@foto", artista.FotoPerfil);
        command.Parameters.AddWithValue("@bio", artista.Bio);
        command.Parameters.AddWithValue("@id", id);

        int affectedRows = command.ExecuteNonQuery();
        Console.WriteLine($"Linhas afetadas: {affectedRows}");
    }
    public void Deletar(int id)
    {
        using var conn = new Connection().ObterConexao();
        string sql = "delete Artistas WHERE Id = @id";
        SqlCommand command = new SqlCommand(sql, conn);
        command.Parameters.AddWithValue("@id", id);

        int affectedRows = command.ExecuteNonQuery();
        Console.WriteLine($"Linhas afetadas: {affectedRows}");
    }

}
