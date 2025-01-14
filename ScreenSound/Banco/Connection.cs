using Microsoft.Data.SqlClient;
using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    public class Connection
    {
        private SqlConnection Conn { get;set; }
        private string ConnectionString = "Data Source=localhost;Initial Catalog=ScreenSound;User ID=sa;Password=Dbzbt333@;TrustServerCertificate=True";
        public SqlConnection ObterConexao() {
            if(Conn == null) {
                Conn = new SqlConnection(ConnectionString);
                Conn.Open();
            }
            return Conn;
        }
        
    }
}