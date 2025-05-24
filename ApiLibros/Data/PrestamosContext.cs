using ApiLibros.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Data
{
    public class PrestamosContext:DbContext
    {
        public PrestamosContext(DbContextOptions<PrestamosContext> options) : base(options) { }

        public DbSet<Prestamos> Prestamos { get; set; }
    }
}
