namespace ApiLibros.Model
{
    public class Prestamos
    {
		private int IdLibro {  get; set; }
        public string? Titlo { get; set; }
        public string? Autor { get; set; }
        public string? ISBN { get; set; }
        public int? EjemplaresTotales { get; set; }
        public string? EjemplaresDisponibles { get; set; }




    }
}
