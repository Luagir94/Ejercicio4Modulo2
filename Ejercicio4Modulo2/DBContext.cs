using Microsoft.EntityFrameworkCore;

namespace Ejercicio4Modulo2;

public class DBContext : DbContext
{
    public DbSet<Ventas_Mensuales> Ventas_Mensuales { get; set; }
    public DbSet<Rechazos> Rechazos { get; set; }
    
    public DbSet<Parametria> Parametria { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=master;User Id=sa;Password=Password12345;");
    }
}