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
    public class ClientesController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public ClientesController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        public async Task<IActionResult> PutCliente(int id, UpdateClienteDto clienteDto)
        {
            var userPerfilId = GetUserPerfilId();

            if (userPerfilId == 2)
            {
                return Forbid("Usuarios con PerfilID 2 no pueden realizar esta acción.");
            }

            var existingCliente = await _context.Clientes.FindAsync(id);

            if (existingCliente == null)
            {
                return NotFound(new { message = $"Cliente con ID {id} no encontrado." });
            }

            existingCliente.Nombre = clienteDto.Nombre;
            existingCliente.Apellido = clienteDto.Apellido;
            existingCliente.DocumentoIdentidad = clienteDto.DocumentoIdentidad;
            existingCliente.FechaRegistro = clienteDto.FechaRegistro;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Cliente actualizado correctamente.", cliente = existingCliente });
        }

        // POST: api/Clientes
        [HttpPost]
        [Authorize(Roles = "Admin,Mantenimiento")]
        public async Task<ActionResult<Cliente>> PostCliente(CreateClienteDto clienteDto)
        {
            var userPerfilId = GetUserPerfilId();

            if (userPerfilId == 2)
            {
                return Forbid("Usuarios con PerfilID 2 no pueden realizar esta acción.");
            }

            var cliente = new Cliente
            {
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                DocumentoIdentidad = clienteDto.DocumentoIdentidad,
                FechaRegistro = clienteDto.FechaRegistro
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var userPerfilId = GetUserPerfilId();

            if (userPerfilId == 2 || userPerfilId == 3)
            {
                return Forbid("Usuarios con PerfilID 2 y 3 no pueden realizar esta acción.");
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cliente eliminado correctamente.", cliente });
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }

        private int GetUserPerfilId()
        {
            var userName = User.Identity.Name;
            var user = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);
            return user?.PerfilID ?? 0;
        }
    }
}
