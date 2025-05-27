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
                var response = await _prestamoService.InsertarPrestamos(prestamo);

                if (response != null)
                {
                    var prestamos = await _prestamoService.GetPrestamosAsync();
                    TempData["Success"] = "¡Gracias! Su comentario fue enviado correctamente.";

                    return View("~/Views/Prestamos/Insertar.cshtml", prestamos);

                }
                else
                {
                    
                    return View("~/Views/Prestamos/Prestamos.cshtml");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al procesar su solicitud. Se ha notificado a soporte, {ex}";
                return View("~/Views/Prestamos/Prestamos.cshtml");
            }
        }
    }
}
