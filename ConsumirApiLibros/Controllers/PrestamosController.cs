using ApiLibros.Services;
using ConsumirApiLibros.Models;
using ConsumirApiLibros.Services;
using Microsoft.AspNetCore.Mvc;


namespace ConsumirApiLibros.Controllers
{
    public class PrestamosController :Controller
    {
        private readonly IPrestamosService _prestamoService;

        public PrestamosController(IPrestamosService prestamoService)
        {
            _prestamoService = prestamoService ?? throw new ArgumentNullException(nameof(prestamoService));
        }

        [HttpGet ("Prestamos")]
        public async Task<IActionResult> Prestamos()
        {
            try
            {
              
                var success = await _prestamoService.LoginAsync("admin", "123");

                if (success)
                {
                    var prestamos = await _prestamoService.GetPrestamosAsync();
                    return View("~/Views/Prestamos/Prestamos.cshtml", prestamos);
                }
                else
                {
                    return View("~/Views/Prestamos/Prestamos.cshtml");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("Insertar")]
        public IActionResult Insertar()
        {
            return View("~/Views/Prestamos/Insertar.cshtml");
        }

        [HttpPost ("InsertarPrestamo")]
        public async Task<IActionResult> InsertarPrestamo(Prestamos prestamo) 
        {
            try
            {
                var success = await _prestamoService.LoginAsync("admin", "123");

                var response = await _prestamoService.InsertarPrestamos(prestamo);
                

                if (response)
                {
                    
                    TempData["ShowAlert"] = "¡El prestamo se añadió correctamente!";

                    return Insertar();

                }
                else
                {
                    TempData["ShowAlert"] = $"No se pudo registrar el préstamo.";
                    return Insertar();
                }
            }
            catch (Exception ex)
            {
                TempData["ShowAlert"] = $"Ocurrió un error al procesar su solicitud. {ex}";
                return Insertar();
            }
        }

        [HttpDelete ("EliminarPrestamo")]

        public async Task<IActionResult> EliminarPrestamo(int id) 
        {
            try
            {
                var success = await _prestamoService.LoginAsync("admin", "123");

                var response = await _prestamoService.EliminarPrestamo(id);

                if (!response) 
                {
                    TempData["ShowAlert"] = "Ocurrió un error al eliminar el prestamo :(";
                    return Insertar();
                }
                TempData["ShowAlert"] = "¡El prestamo se Eliminó correctamente!";
                return Insertar();

            }
            catch (Exception ex)
            {
                TempData["ShowAlert"] = $"Ocurrió un error al procesar su solicitud. {ex}";
                return Insertar();
            }
        }
    }
}
