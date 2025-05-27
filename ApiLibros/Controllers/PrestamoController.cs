using Microsoft.AspNetCore.Mvc;
using ApiLibros.Model;
using ApiLibros.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;


namespace ApiLibros.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Prestamos")]
    public class PrestamoController : ControllerBase
    {
       private readonly PrestamosContext _context;
        public PrestamoController(PrestamosContext context) 
        {
            _context = context;
        }

        [HttpGet("Todos")]
        public async Task<ActionResult<List<Prestamos>>> GetPrestamos()
        {
            try
            {
                var prestauser = await _context.Prestamos.FromSqlRaw("EXEC VerAllPrestamos")
                .ToListAsync();
                return Ok(prestauser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        [HttpGet("prestamo/{idprestamo}")]
        public async Task<ActionResult<List<Prestamos>>> GetIdPrestamo(int idprestamo)
        {
            try
            {
                var prestamo = await _context.Prestamos.FromSqlRaw("EXEC VerPrestamoPorId  @IdPrestamo", new SqlParameter("@IdPrestamo", idprestamo))
                .ToListAsync();

                if (prestamo == null)
                    return NotFound();

                return Ok(prestamo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        [HttpGet("usuario/{usuarioid}")]
        public async Task<ActionResult<List<Prestamos>>> GetPrestamosId(int usuarioid)
        {
            try
            {
                var prestauser = await _context.Prestamos.FromSqlRaw("EXEC VerPrestamos @UsuarioId", new SqlParameter("@UsuarioId", usuarioid))
                .ToListAsync();
                return Ok(prestauser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        [HttpPatch("Prestamo")]
        public async Task<ActionResult<List<Prestamos>>> UpdatePrestamo( int prestamoid, [FromBody] string estado)
        {
            try
            {
                var response = await _context.Database.ExecuteSqlRawAsync(
                  "Exec UpdatePrestamo @p0, @p1",
                  prestamoid, estado
                    //prestamoid,prestamo?.Estado ?? "Prestado"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Prestamos>>> CrearPrestamos(Prestamos prestamo)
        {
            try
            {
                var response = await _context.Database.ExecuteSqlRawAsync(
                  "Exec InsertarPrestamo @p0, @p1, @p2",
                   prestamo?.UsuarioId, prestamo?.LibroId, prestamo?.FechaDevolucion
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{prestamoid}")]
        public async Task<IActionResult> DeletePrestamo( int prestamoid)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("Exec DeletePrestamo @p0", prestamoid);
                return Ok($"Prestamo con ID {prestamoid} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
