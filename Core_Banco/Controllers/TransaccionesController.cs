using System;
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
    public class TransaccionesController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public TransaccionesController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/Transacciones
        [HttpGet]
        [Authorize(Roles = "Admin,User,Maintenance")]
        public async Task<ActionResult<IEnumerable<TransaccionDto>>> GetTransacciones()
        {
            var userPerfilId = GetUserPerfilId(); // Método para obtener el PerfilID del usuario

            var transacciones = await _context.Transacciones
                .Select(t => new TransaccionDto
                {
                    TransaccionID = t.TransaccionID,
                    CuentaID = t.CuentaID,
                    TipoTransaccionID = t.TipoTransaccionID,
                    Monto = t.Monto,
                    FechaTransaccion = t.FechaTransaccion,
                    BeneficiarioID = t.BeneficiarioID // Incluir BeneficiarioID en la vista DTO
                })
                .ToListAsync();

            // Restringir transacciones visibles para roles específicos
            if (userPerfilId == 2) // Mantenimiento
            {
                transacciones = transacciones.Where(t => t.TipoTransaccionID == 1).ToList(); // Solo tipo consulta
            }
            else if (userPerfilId == 3) // Usuario común
            {
                return Forbid("Usuarios con PerfilID 3 no pueden ver la lista completa de transacciones.");
            }

            return Ok(transacciones);
        }

        // GET: api/Transacciones/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,Maintenance")]
        public async Task<ActionResult<TransaccionDto>> GetTransaccion(int id)
        {
            var userPerfilId = GetUserPerfilId(); // Método para obtener el PerfilID del usuario

            var transaccion = await _context.Transacciones
                .Where(t => t.TransaccionID == id)
                .Select(t => new TransaccionDto
                {
                    TransaccionID = t.TransaccionID,
                    CuentaID = t.CuentaID,
                    TipoTransaccionID = t.TipoTransaccionID,
                    Monto = t.Monto,
                    FechaTransaccion = t.FechaTransaccion,
                    BeneficiarioID = t.BeneficiarioID // Incluir BeneficiarioID en la vista DTO
                })
                .FirstOrDefaultAsync();

            if (transaccion == null)
            {
                return NotFound(new { message = $"Transacción con ID {id} no encontrada." });
            }

            // Restringir detalles de transacción según el perfil del usuario
            if (userPerfilId == 2 && transaccion.TipoTransaccionID != 1) // Mantenimiento
            {
                return Forbid("Usuarios con PerfilID 2 no pueden ver transacciones que no sean de tipo consulta.");
            }
            else if (userPerfilId == 3) // Usuario común
            {
                return Forbid("Usuarios con PerfilID 3 no pueden ver detalles de transacciones.");
            }

            return Ok(transaccion);
        }

        // POST: api/Transacciones
        [HttpPost]
        [Authorize(Roles = "Admin,User,Maintenance")]
        public async Task<ActionResult<Transaccion>> PostTransaccion(CreateTransaccionDto transaccionDto)
        {
            var userPerfilId = GetUserPerfilId(); // Método para obtener el PerfilID del usuario

            if (userPerfilId == 3)
            {
                return Forbid("Usuarios con PerfilID 3 no pueden realizar esta acción.");
            }

            var cuenta = await _context.Cuentas.FindAsync(transaccionDto.CuentaID);
            if (cuenta == null)
            {
                return NotFound(new { message = $"Cuenta con ID {transaccionDto.CuentaID} no encontrada." });
            }

            if (transaccionDto.TipoTransaccionID == 1 || transaccionDto.TipoTransaccionID == 2) // Ingreso o Retiro de dinero
            {
                if (transaccionDto.TipoTransaccionID == 1) // Ingreso de dinero
                {
                    cuenta.Balance += transaccionDto.Monto;

                    var movimientoIngreso = new Movimiento
                    {
                        CuentaID = transaccionDto.CuentaID,
                        TipoTransaccionID = transaccionDto.TipoTransaccionID,
                        Monto = transaccionDto.Monto,
                        FechaTransaccion = DateTime.Now
                    };

                    _context.Movimientos.Add(movimientoIngreso);
                }
                else if (transaccionDto.TipoTransaccionID == 2) // Retiro de dinero
                {
                    if (cuenta.Balance < transaccionDto.Monto)
                    {
                        return BadRequest(new { message = "Saldo insuficiente en la cuenta." });
                    }
                    cuenta.Balance -= transaccionDto.Monto;

                    var movimientoRetiro = new Movimiento
                    {
                        CuentaID = transaccionDto.CuentaID,
                        TipoTransaccionID = transaccionDto.TipoTransaccionID,
                        Monto = -transaccionDto.Monto,
                        FechaTransaccion = DateTime.Now
                    };

                    _context.Movimientos.Add(movimientoRetiro);
                }
            }
            else if (transaccionDto.TipoTransaccionID == 3) // Transferencia
            {
                var cuentaDestino = await _context.Cuentas.FindAsync(transaccionDto.CuentaDestinoID);

                if (cuentaDestino == null)
                {
                    return NotFound(new { message = $"Cuenta de destino con ID {transaccionDto.CuentaDestinoID} no encontrada." });
                }

                if (cuenta.Balance < transaccionDto.Monto)
                {
                    return BadRequest(new { message = "Saldo insuficiente en la cuenta de origen." });
                }

                cuenta.Balance -= transaccionDto.Monto;
                cuentaDestino.Balance += transaccionDto.Monto;

                var movimientoRetiro = new Movimiento
                {
                    CuentaID = transaccionDto.CuentaID,
                    TipoTransaccionID = transaccionDto.TipoTransaccionID,
                    Monto = -transaccionDto.Monto,
                    FechaTransaccion = DateTime.Now
                };

                var movimientoIngreso = new Movimiento
                {
                    CuentaID = transaccionDto.CuentaDestinoID,
                    TipoTransaccionID = transaccionDto.TipoTransaccionID,
                    Monto = transaccionDto.Monto,
                    FechaTransaccion = DateTime.Now
                };

                _context.Movimientos.Add(movimientoRetiro);
                _context.Movimientos.Add(movimientoIngreso);
            }
            else if (transaccionDto.TipoTransaccionID == 4) // Transacción tipo 4 (Beneficiario)
            {
                var beneficiario = await _context.Beneficiarios
                    .Include(b => b.Cuenta)
                    .FirstOrDefaultAsync(b => b.BeneficiarioID == transaccionDto.BeneficiarioID);

                if (beneficiario == null || beneficiario.Cuenta == null)
                {
                    return NotFound(new { message = $"Beneficiario con ID {transaccionDto.BeneficiarioID} no encontrado o no tiene cuenta asociada." });
                }

                if (cuenta.Balance < transaccionDto.Monto)
                {
                    return BadRequest(new { message = "Saldo insuficiente en la cuenta de origen." });
                }

                cuenta.Balance -= transaccionDto.Monto;
                beneficiario.Cuenta.Balance += transaccionDto.Monto;

                var movimientoRetiro = new Movimiento
                {
                    CuentaID = transaccionDto.CuentaID,
                    TipoTransaccionID = transaccionDto.TipoTransaccionID,
                    Monto = -transaccionDto.Monto,
                    FechaTransaccion = DateTime.Now
                };

                var movimientoIngreso = new Movimiento
                {
                    CuentaID = beneficiario.Cuenta.CuentaID,
                    TipoTransaccionID = transaccionDto.TipoTransaccionID,
                    Monto = transaccionDto.Monto,
                    FechaTransaccion = DateTime.Now
                };

                _context.Movimientos.Add(movimientoRetiro);
                _context.Movimientos.Add(movimientoIngreso);
            }
            else
            {
                return BadRequest(new { message = "Tipo de transacción no válido." });
            }

            var transaccion = new Transaccion
            {
                CuentaID = transaccionDto.CuentaID,
                TipoTransaccionID = transaccionDto.TipoTransaccionID,
                Monto = transaccionDto.Monto,
                FechaTransaccion = DateTime.Now,
                BeneficiarioID = transaccionDto.BeneficiarioID // Asociar transacción con el beneficiario por su ID
            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaccion), new { id = transaccion.TransaccionID }, transaccion);
        }

        // PUT: api/Transacciones/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User,Maintenance")]
        public async Task<IActionResult> PutTransaccion(int id, UpdateTransaccionDto transaccionDto)
        {
            var userPerfilId = GetUserPerfilId(); // Método para obtener el PerfilID del usuario

            if (userPerfilId == 3)
            {
                return Forbid("Usuarios con PerfilID 3 no pueden realizar esta acción.");
            }

            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound(new { message = $"Transacción con ID {id} no encontrada." });
            }

            transaccion.CuentaID = transaccionDto.CuentaID;
            transaccion.TipoTransaccionID = transaccionDto.TipoTransaccionID;
            transaccion.Monto = transaccionDto.Monto;
            transaccion.FechaTransaccion = transaccionDto.FechaTransaccion;
            transaccion.BeneficiarioID = transaccionDto.BeneficiarioID;

            _context.Entry(transaccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransaccionExists(id))
                {
                    return NotFound(new { message = $"Transacción con ID {id} no encontrada." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Transacciones/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User,Maintenance")]
        public async Task<IActionResult> DeleteTransaccion(int id)
        {
            var userPerfilId = GetUserPerfilId(); // Método para obtener el PerfilID del usuario

            if (userPerfilId == 3)
            {
                return Forbid("Usuarios con PerfilID 3 no pueden realizar esta acción.");
            }

            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound(new { message = $"Transacción con ID {id} no encontrada." });
            }

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransaccionExists(int id)
        {
            return _context.Transacciones.Any(e => e.TransaccionID == id);
        }

        private int GetUserPerfilId()
        {
            // Implementar lógica para obtener el PerfilID del usuario actual
            // Esto puede incluir decodificar el token JWT o buscar en la base de datos
            return 1; // Este es un ejemplo estático. Cambiar por la lógica real.
        }
    }
}
