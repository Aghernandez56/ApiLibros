using Microsoft.EntityFrameworkCore;
using ApiLibros.Model;

namespace ApiLibros.Data
{
    public class ControlLibroContext :DbContext
    {
        public ControlLibroContext(DbContextOptions<ControlLibroContext> options) : base(options) { }

        public DbSet<Prestamos> Prestamoss { get; set; }
    }
}
