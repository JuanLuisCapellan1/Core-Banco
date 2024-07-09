using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_Banco.Data;
using Core_Banco.Models;

namespace Core_Banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariosController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public BeneficiariosController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/Beneficiarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beneficiario>>> GetBeneficiarios()
        {
            return await _context.Beneficiarios.ToListAsync();
        }

        // GET: api/Beneficiarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Beneficiario>> GetBeneficiario(int id)
        {
            var beneficiario = await _context.Beneficiarios.FindAsync(id);

            if (beneficiario == null)
            {
                return NotFound();
            }

            return beneficiario;
        }

        // POST: api/Beneficiarios
        [HttpPost]
        public async Task<ActionResult<Beneficiario>> PostBeneficiario(Beneficiario.CreateDto beneficiarioDto)
        {
            var beneficiario = new Beneficiario
            {
                Nombre = beneficiarioDto.Nombre,
                CuentaID = beneficiarioDto.CuentaID,
                UsuarioID = beneficiarioDto.UsuarioID
            };

            _context.Beneficiarios.Add(beneficiario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBeneficiario), new { id = beneficiario.BeneficiarioID }, beneficiario);
        }

        // PUT: api/Beneficiarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeneficiario(int id, Beneficiario.UpdateDto beneficiarioDto)
        {
            var beneficiario = await _context.Beneficiarios.FindAsync(id);

            if (beneficiario == null)
            {
                return NotFound();
            }

            beneficiario.Nombre = beneficiarioDto.Nombre;
            beneficiario.CuentaID = beneficiarioDto.CuentaID;
            beneficiario.UsuarioID = beneficiarioDto.UsuarioID;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_context.Beneficiarios.Any(e => e.BeneficiarioID == id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Beneficiarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeneficiario(int id)
        {
            var beneficiario = await _context.Beneficiarios.FindAsync(id);
            if (beneficiario == null)
            {
                return NotFound();
            }

            _context.Beneficiarios.Remove(beneficiario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
