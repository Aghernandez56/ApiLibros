using ConsumirApiLibros.Models;

namespace ApiLibros.Services
{
    public interface IPrestamosService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<List<Prestamos>> GetPrestamosAsync();
        Task<bool> InsertarPrestamos(Prestamos prestamo);
        Task<bool> EliminarPrestamo(int id);
    }
}
