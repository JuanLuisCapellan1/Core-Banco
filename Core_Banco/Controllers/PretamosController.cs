using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core_Banco.Data;
using Core_Banco.Models;

namespace Core_Banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requiere autenticación para todo el controlador
    public class PrestamosController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public PrestamosController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/Prestamos
        [HttpGet]
        [Authorize(Roles = "Admin,Mantenimiento,Usuario")]
        public async Task<ActionResult<IEnumerable<PrestamoDto>>> GetPrestamos()
        {
            return await _context.Prestamos
                .Include(p => p.Cliente)
                .Select(p => new PrestamoDto
                {
                    PrestamoId = p.PrestamoId,
                    ClienteId = p.ClienteId,
                    Monto = p.Monto,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaVencimiento = p.FechaVencimiento,
                    ClienteNombre = p.Cliente.Nombre,
                    ClienteDocumentoIdentidad = p.Cliente.DocumentoIdentidad
                })
                .ToListAsync();
        }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Mantenimiento,Usuario")]
        public async Task<ActionResult<PrestamoDto>> GetPrestamo(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Cliente)
                .Select(p => new PrestamoDto
                {
                    PrestamoId = p.PrestamoId,
                    ClienteId = p.ClienteId,
                    Monto = p.Monto,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaVencimiento = p.FechaVencimiento,
                    ClienteNombre = p.Cliente.Nombre,
                    ClienteDocumentoIdentidad = p.Cliente.DocumentoIdentidad
                })
                .FirstOrDefaultAsync(p => p.PrestamoId == id);

            if (prestamo == null)
            {
                return NotFound(new { message = $"Préstamo con ID {id} no encontrado." });
            }

            return prestamo;
        }

        // GET: api/Prestamos/Cliente/5
        [HttpGet("Cliente/{clienteId}")]
        [Authorize(Roles = "Admin,Mantenimiento,Usuario")]
        public async Task<ActionResult<IEnumerable<PrestamoDto>>> GetPrestamosByCliente(int clienteId)
        {
            var prestamos = await _context.Prestamos
                .Where(p => p.ClienteId == clienteId)
                .Include(p => p.Cliente)
                .Select(p => new PrestamoDto
                {
                    PrestamoId = p.PrestamoId,
                    ClienteId = p.ClienteId,
                    Monto = p.Monto,
                    FechaPrestamo = p.FechaPrestamo,
                    FechaVencimiento = p.FechaVencimiento,
                    ClienteNombre = p.Cliente.Nombre,
                    ClienteDocumentoIdentidad = p.Cliente.DocumentoIdentidad
                })
                .ToListAsync();

            if (prestamos == null || prestamos.Count == 0)
            {
                return NotFound(new { message = $"No se encontraron préstamos para el cliente con ID {clienteId}." });
            }

            return Ok(prestamos);
        }

        // POST: api/Prestamos
        [HttpPost]
        [Authorize(Roles = "Admin,Mantenimiento")]
        public async Task<ActionResult<Prestamo>> PostPrestamo(CreatePrestamoDto prestamoDto)
        {
            var prestamo = new Prestamo
            {
                ClienteId = prestamoDto.ClienteId,
                Monto = prestamoDto.Monto,
                FechaPrestamo = prestamoDto.FechaPrestamo,
                FechaVencimiento = prestamoDto.FechaVencimiento
            };

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrestamo), new { id = prestamo.PrestamoId }, prestamo);
        }

        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound(new { message = $"Préstamo con ID {id} no encontrado." });
            }

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Préstamo eliminado correctamente.", prestamo });
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.PrestamoId == id);
        }
    }
}
