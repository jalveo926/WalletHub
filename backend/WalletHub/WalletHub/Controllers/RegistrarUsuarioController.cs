using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Services;

namespace WalletHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrarUsuarioController : ControllerBase
    {
        private readonly RegistrarUsuarioService _registrarUsuarioService;

        public RegistrarUsuarioController(RegistrarUsuarioService registrarUsuarioService)
        {
            _registrarUsuarioService = registrarUsuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDTO dto)
        {
            if (string.IsNullOrEmpty(dto.correoUsu) ||
                string.IsNullOrEmpty(dto.Contrasena) ||
                string.IsNullOrEmpty(dto.nombreUsu))
            {
                return BadRequest(new
                {
                    mensaje = "Los campos no pueden estar vacíos."
                });
            }

            try
            {
                var nuevoUsuario = new Models.Usuario
                {
                    nombreUsu = dto.nombreUsu,
                    correoUsu = dto.correoUsu,
                    pwHashUsu = dto.Contrasena
                };

                var usuarioRegistrado = await _registrarUsuarioService.RegistrarUsuarioAsync(nuevoUsuario);

                if (usuarioRegistrado == null)
                    return BadRequest(new { mensaje = "El correo ya está registrado." });

                return Ok(new
                {
                    mensaje = "Registro exitoso",
                    usuario = new
                    {
                        id = usuarioRegistrado.idUsuario,
                        nombre = usuarioRegistrado.nombreUsu,
                        correo = usuarioRegistrado.correoUsu
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al procesar la solicitud"
                });
            }
        }
    }
}
