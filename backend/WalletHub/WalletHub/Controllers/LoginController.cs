using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletHub.Data;
using WalletHub.DTOs;
using WalletHub.Services;

namespace WalletHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly LoginService _loginServicio;

        public LoginController(LoginService loginServicio)
        {
            _loginServicio = loginServicio;
        }

        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginDTO dto) //Recomendable usar dto en vez del modelo
        {
            if (string.IsNullOrEmpty(dto.correoUsu) || string.IsNullOrEmpty(dto.Contrasena))
                return BadRequest(new
                {
                    mensaje = "Los campos no pueden estar vacíos."
                });

            try
            {          
                //Valida en la capa servicios si los datos son correctos o existentes
                var credencialesUsuario = await _loginServicio.VerificarCorreoContrasena(dto.correoUsu, dto.Contrasena);

                //Si no existe el usuario o la contraseña no coincide
                if (credencialesUsuario == null)
                    return BadRequest(new { mensaje = "Correo o contraseña incorrectos." });

                return Ok(new
                {
                    mensaje = "Login exitoso",
                    usuario = new
                    {
                        nombre = credencialesUsuario.nombreUsu,
                        correo = credencialesUsuario.correoUsu
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al procesar la solicitud",
                });
            }
            
        }
    }
}
