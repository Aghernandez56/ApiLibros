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

                    return RedirectToAction("Prestamos", "Prestamos");

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



        [HttpGet("PrestamoId")]
        public async Task<IActionResult> PrestamoId(int id)
        {
            try
            {

                var success = await _prestamoService.LoginAsync("admin", "123");
                var prestamo = await _prestamoService.GetPrestamoIdAsync(id);

                if (prestamo == null)
                {
                    TempData["ShowAlert"] = "No se encontró el préstamo.";
                    return RedirectToAction("Prestamos");
                }

                return View("~/Views/Prestamos/Detalles.cshtml", prestamo);

            }
            catch (Exception)
            {

                throw;
            }
        }

        //[HttpGet ("Detalles")]
        //public async Task<IActionResult> Detalles(int id)
        //{
        //    var prestamos = await _prestamoService.GetPrestamosAsync();
        //    var prestamo = prestamos.FirstOrDefault(p => p.IdPrestamo == id);

        //    if (prestamo != null)
        //    {
        //        return View("~/Views/Prestamos/Detalles.cshtml", prestamo);
        //    }

        //    TempData["ShowAlert"] = "No se encontró el préstamo.";
        //    return RedirectToAction("Prestamos", "Prestamos");
        //}


        [HttpPost ("EliminarPrestamo")]

        public async Task<IActionResult> EliminarPrestamo(int id) 
        {
            try
            {
                var success = await _prestamoService.LoginAsync("admin", "123");

                var response = await _prestamoService.EliminarPrestamo(id);

                if (!response) 
                {
                    TempData["ShowAlert"] = "Ocurrió un error al eliminar el prestamo :( ";
                    return RedirectToAction("PrestamoId", "Prestamos", new { id = id });
                }
                TempData["ShowAlert"] = "¡El prestamo se Eliminó correctamente!";
                return RedirectToAction("Prestamos", "Prestamos");

            }
            catch (Exception ex)
            {
                TempData["ShowAlert"] = $"Ocurrió un error al procesar su solicitud. {ex}";
                return RedirectToAction("Prestamos", "Prestamos");
            }
        }


        [HttpPost ("ActualizarPrestamo")]
        public async Task<IActionResult> ActualizarPrestamo(Prestamos prestamo) 
        {
           
            try
            {
                var success = await _prestamoService.LoginAsync("admin", "123");

                Prestamos presta = new Prestamos { IdPrestamo = prestamo.IdPrestamo, Estado = prestamo.Estado };
                var response = await _prestamoService.ActualizarPrestamo(prestamo);

                if (!response) 
                {
                    TempData["ShowAlert"] = "Ocurrió un error al editar el prestamo :( ";
                    return RedirectToAction("PrestamoId", "Prestamos", new { id = presta.IdPrestamo });
                }
                TempData["ShowAlert"] = "¡El prestamo se Editó correctamente!";
                return RedirectToAction("Prestamos", "Prestamos");
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
