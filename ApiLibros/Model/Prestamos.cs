using System.ComponentModel.DataAnnotations;

namespace ApiLibros.Model
{
    public class Prestamos
    {
        [Key]
        public int IdPrestamo {  get; set; }
        public int? UsuarioId { get; set; }
        public int? LibroId { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string? Estado { get; set; }




    }
}
