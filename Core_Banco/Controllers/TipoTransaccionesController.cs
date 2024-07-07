using Core_Banco.Data;
using Core_Banco.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requiere autenticación para todo el controlador
    public class TipoTransaccionesController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public TipoTransaccionesController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/TipoTransacciones
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<TipoTransaccionDto>>> GetTipoTransacciones()
        {
            var tipos = await _context.TiposTransaccion
                .Select(t => new TipoTransaccionDto
                {
                    TipoTransaccionID = t.TipoTransaccionID,
                    Nombre = t.Nombre,
                    Descripcion = t.Descripcion
                })
                .ToListAsync();

            return Ok(tipos);
        }

        // GET: api/TipoTransacciones/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<TipoTransaccionDto>> GetTipoTransaccion(int id)
        {
            var tipo = await _context.TiposTransaccion
                .Where(t => t.TipoTransaccionID == id)
                .Select(t => new TipoTransaccionDto
                {
                    TipoTransaccionID = t.TipoTransaccionID,
                    Nombre = t.Nombre,
                    Descripcion = t.Descripcion
                })
                .FirstOrDefaultAsync();

            if (tipo == null)
            {
                return NotFound(new { message = $"TipoTransaccion con ID {id} no encontrada." });
            }

            return Ok(tipo);
        }

        // PUT: api/TipoTransacciones/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTipoTransaccion(int id, UpdateTipoTransaccionDto tipoDto)
        {
            var existingTipo = await _context.TiposTransaccion.FindAsync(id);

            if (existingTipo == null)
            {
                return NotFound(new { message = $"TipoTransaccion con ID {id} no encontrada." });
            }

            existingTipo.Nombre = tipoDto.Nombre;
            existingTipo.Descripcion = tipoDto.Descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoTransaccionExists(id))
                {
                    return NotFound(new { message = $"TipoTransaccion con ID {id} no encontrada." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "TipoTransaccion actualizada correctamente.", tipo = existingTipo });
        }

        // POST: api/TipoTransacciones
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TipoTransaccion>> PostTipoTransaccion(CreateTipoTransaccionDto tipoDto)
        {
            var tipo = new TipoTransaccion
            {
                Nombre = tipoDto.Nombre,
                Descripcion = tipoDto.Descripcion
            };

            _context.TiposTransaccion.Add(tipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTipoTransaccion), new { id = tipo.TipoTransaccionID }, tipo);
        }

        // DELETE: api/TipoTransacciones/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTipoTransaccion(int id)
        {
            var tipo = await _context.TiposTransaccion.FindAsync(id);
            if (tipo == null)
            {
                return NotFound(new { message = $"TipoTransaccion con ID {id} no encontrada." });
            }

            _context.TiposTransaccion.Remove(tipo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "TipoTransaccion eliminada correctamente.", tipo });
        }

        private bool TipoTransaccionExists(int id)
        {
            return _context.TiposTransaccion.Any(e => e.TipoTransaccionID == id);
        }
    }
}
