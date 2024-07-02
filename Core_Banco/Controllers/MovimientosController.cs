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
    public class MovimientosController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public MovimientosController(Core_BancoContext context)
        {
            _context = context;
        }

        // POST: api/Movimientos
        [HttpPost]
        [Authorize(Roles = "Admin,Mantenimiento")]
        public async Task<ActionResult<Movimiento>> PostMovimiento(CreateMovimientoDto movimientoDto)
        {
            var userPerfilId = GetUserPerfilId();

            if (userPerfilId == 3)
            {
                return Forbid("Usuarios con PerfilID 3 no pueden realizar esta acción.");
            }

            var cuenta = await _context.Cuentas.FindAsync(movimientoDto.CuentaID);
            if (cuenta == null)
            {
                return NotFound(new { message = $"Cuenta con ID {movimientoDto.CuentaID} no encontrada." });
            }

            var tipoTransaccion = await _context.TiposTransaccion.FindAsync(movimientoDto.TipoTransaccionID);
            if (tipoTransaccion == null)
            {
                return NotFound(new { message = $"Tipo de Transacción con ID {movimientoDto.TipoTransaccionID} no encontrado." });
            }

            var movimiento = new Movimiento
            {
                CuentaID = movimientoDto.CuentaID,
                TipoTransaccionID = movimientoDto.TipoTransaccionID,
                Monto = movimientoDto.Monto,
                FechaTransaccion = movimientoDto.FechaTransaccion
            };

            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovimientosPorCuenta), new { cuentaId = movimiento.CuentaID }, movimiento);
        }

        // GET: api/Movimientos/Cuenta/5
        [HttpGet("Cuenta/{cuentaId}")]
        [Authorize(Roles = "Admin,Mantenimiento,Usuario")]
        public async Task<ActionResult<IEnumerable<Movimiento>>> GetMovimientosPorCuenta(int cuentaId)
        {
            var movimientos = await _context.Movimientos
                .Where(m => m.CuentaID == cuentaId)
                .Include(m => m.TipoTransaccion) // Incluir TipoTransaccion si es necesario
                .ToListAsync();

            if (movimientos == null || movimientos.Count == 0)
            {
                return NotFound(new { message = $"No se encontraron movimientos para la cuenta con ID {cuentaId}." });
            }

            return Ok(movimientos);
        }

        private int GetUserPerfilId()
        {
            var userName = User.Identity.Name;
            var user = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);
            return user?.PerfilID ?? 0;
        }
    }
}
