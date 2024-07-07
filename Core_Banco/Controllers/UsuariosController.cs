using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core_Banco.Data;
using Core_Banco.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Core_Banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly Core_BancoContext _context;

        public UsuariosController(Core_BancoContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        [Authorize(Roles = "Admin,User,Maintenance")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,Maintenance")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // GET: api/Usuarios/cliente/5
        [HttpGet("cliente/{clienteId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosByClienteId(int clienteId)
        {
            var usuarios = await _context.Usuarios.Where(u => u.ClienteID == clienteId).ToListAsync();

            if (usuarios == null || !usuarios.Any())
            {
                return NotFound();
            }

            return usuarios;
        }

        // POST: api/Usuarios
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Usuario>> PostUsuario(CreateUserDto createUserDto)
        {
            var usuario = new Usuario
            {
                NombreUsuario = createUserDto.NombreUsuario,
                Contraseña = createUserDto.Contraseña,
                PerfilID = createUserDto.PerfilID,
                ClienteID = createUserDto.ClienteID, // Asigna ClienteID
                FechaCreacion = DateTime.Now,
                UltimoAcceso = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioID }, usuario);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUsuario(int id, UpdateUserDto updateUserDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.NombreUsuario = updateUserDto.NombreUsuario;
            usuario.Contraseña = updateUserDto.Contraseña;
            usuario.PerfilID = updateUserDto.PerfilID;
            usuario.ClienteID = updateUserDto.ClienteID; // Asigna ClienteID

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioID == id);
        }
    }
}