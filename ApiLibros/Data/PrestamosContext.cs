using ApiLibros.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Data
{
    public class PrestamosContext
    {
        public PrestamosContext(DbContextOptions<PrestamosContext> options) : base(options) { }

        public DbSet<Prestamos> Prestamoss { get; set; }
    }
}
