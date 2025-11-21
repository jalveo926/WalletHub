using Microsoft.AspNetCore.Mvc;
using WalletHub.Servicios;
using WalletHub.Models;

namespace WalletHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrarUsuarioControlador : ControllerBase
    {
        private readonly RegistrarUsuario _registrarUsuario;

        public RegistrarUsuarioControlador(RegistrarUsuario registrarUsuario)
        {
            _registrarUsuario = registrarUsuario;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            if (_registrarUsuario.CorreoExiste(usuario.correoUsu))
                return BadRequest("El correo ya está registrado.");

            _registrarUsuario.RegistrarNuevoUsuario(usuario);
            return Ok("Usuario registrado exitosamente.");
        }
    }
}
