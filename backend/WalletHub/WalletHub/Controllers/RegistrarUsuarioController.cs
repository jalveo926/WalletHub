using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Services;
using WalletHub.Services.Interface;

namespace WalletHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly RegistrarUsuarioService _registrarUsuarioService;
        private readonly IPasswordHashService _passwordHashService;

        public RegistroController(
            RegistrarUsuarioService registrarUsuarioService,
            IPasswordHashService passwordHashService)
        {
            _registrarUsuarioService = registrarUsuarioService;
            _passwordHashService = passwordHashService;
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDTO dto)
        {
            if (string.IsNullOrEmpty(dto.correoUsu) ||
                string.IsNullOrEmpty(dto.contrasena) ||
                string.IsNullOrEmpty(dto.nombreUsu))
            {
                return BadRequest(new
                {
                    mensaje = "Los campos no pueden estar vacíos."
                });
            }

            // Correo debe tener un @
            if (!dto.correoUsu.Contains("@"))
            {
                return BadRequest(new
                {
                    mensaje = "Debe ingresar un correo válido."
                });
            }

            if (dto.contrasena.Length < 12) // Contraseña con un mínimo de 12 caracteres
            {
                return BadRequest(new
                {
                    mensaje = "La contraseña debe tener al menos 12 caracteres."
                });
            } else if (!dto.contrasena.Any(char.IsUpper)) // Contraseña con al menos una mayúscula
            {
                return BadRequest(new
                {
                    mensaje = "La contraseña debe tener al menos una letra mayúscula."
                });
            } else if (!dto.contrasena.Any(char.IsDigit)) //contraseña con al menos 1 número
            {
                return BadRequest(new
                {
                    mensaje = "La contraseña debe tener al menos un número."
                });
            }

            try
            {
                // Hashea la contraseña antes de guardar
                string hashedPassword = _passwordHashService.HashPassword(dto.contrasena);

                var nuevoUsuario = new Models.Usuario
                {
                    nombreUsu = dto.nombreUsu,
                    correoUsu = dto.correoUsu,
                    pwHashUsu = hashedPassword
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
