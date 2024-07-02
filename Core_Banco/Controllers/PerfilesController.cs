using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core_Banco.Data;
using Core_Banco.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requiere autenticación para todo el controlador
    public class PerfilesController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public PerfilesController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/Perfiles
        [HttpGet]
        [Authorize(Roles = "Admin,Mantenimiento,Usuario")]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfiles()
        {
            return await _context.Perfiles.ToListAsync();
        }

        // GET: api/Perfiles/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Mantenimiento,Usuario")]
        public async Task<ActionResult<Perfil>> GetPerfil(int id)
        {
            var perfil = await _context.Perfiles.FindAsync(id);

            if (perfil == null)
            {
                return NotFound();
            }

            return perfil;
        }

        // PUT: api/Perfiles/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPerfil(int id, UpdatePerfilDto updatePerfilDto)
        {
            if (id != updatePerfilDto.PerfilID)
            {
                return BadRequest();
            }

            var perfil = await _context.Perfiles.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            perfil.NombrePerfil = updatePerfilDto.NombrePerfil;
            perfil.Descripcion = updatePerfilDto.Descripcion;

            _context.Entry(perfil).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Perfiles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Perfil>> PostPerfil(CreatePerfilDto createPerfilDto)
        {
            // Verificar si el PerfilID ya existe
            if (PerfilExists(createPerfilDto.PerfilID))
            {
                return BadRequest("PerfilID ya existe.");
            }

            var perfil = new Perfil
            {
                PerfilID = createPerfilDto.PerfilID,
                NombrePerfil = createPerfilDto.NombrePerfil,
                Descripcion = createPerfilDto.Descripcion
            };

            _context.Perfiles.Add(perfil);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerfil", new { id = perfil.PerfilID }, perfil);
        }

        // DELETE: api/Perfiles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePerfil(int id)
        {
            var perfil = await _context.Perfiles.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            _context.Perfiles.Remove(perfil);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerfilExists(int id)
        {
            return _context.Perfiles.Any(e => e.PerfilID == id);
        }
    }
}
