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
    public class TipoCuentaController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public TipoCuentaController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/TipoCuenta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCuenta>>> GetTipoCuentas()
        {
            return await _context.TipoCuentas.ToListAsync();
        }

        // GET: api/TipoCuenta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCuenta>> GetTipoCuenta(int id)
        {
            var tipoCuenta = await _context.TipoCuentas.FindAsync(id);

            if (tipoCuenta == null)
            {
                return NotFound();
            }

            return tipoCuenta;
        }

        // POST: api/TipoCuenta
        [HttpPost]
        public async Task<ActionResult<TipoCuenta>> PostTipoCuenta(TipoCuenta.CreateTipoCuentaDto createTipoCuentaDto)
        {
            var tipoCuenta = new TipoCuenta
            {
                Nombre = createTipoCuentaDto.Nombre
            };

            _context.TipoCuentas.Add(tipoCuenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoCuenta", new { id = tipoCuenta.TipoCuentaID }, tipoCuenta);
        }

        // PUT: api/TipoCuenta/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoCuenta(int id, TipoCuenta.UpdateTipoCuentaDto updateTipoCuentaDto)
        {
            var tipoCuenta = await _context.TipoCuentas.FindAsync(id);
            if (tipoCuenta == null)
            {
                return NotFound();
            }

            tipoCuenta.Nombre = updateTipoCuentaDto.Nombre;

            _context.Entry(tipoCuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCuentaExists(id))
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

        // DELETE: api/TipoCuenta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoCuenta(int id)
        {
            var tipoCuenta = await _context.TipoCuentas.FindAsync(id);
            if (tipoCuenta == null)
            {
                return NotFound();
            }

            _context.TipoCuentas.Remove(tipoCuenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoCuentaExists(int id)
        {
            return _context.TipoCuentas.Any(e => e.TipoCuentaID == id);
        }
    }
}
