using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    public class ScreenSoundContext: DbContext
    {
        public DbSet<Artista> Artistas { get; set; }
        private string ConnectionString = "Data Source=localhost;Initial Catalog=ScreenSound;User ID=sa;Password=Dbzbt333@;TrustServerCertificate=True";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }        
    }
}