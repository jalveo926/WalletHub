using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletHub.Data;
using WalletHub.Models;
using WalletHub.Services;

namespace WalletHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly Login _loginServicio;

        public LoginController(Login loginServicio)
        {
            _loginServicio = loginServicio;
        }

        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginDto dto) //Recomendable usar dto en vez del modelo
        {
            try
            {
                if (string.IsNullOrEmpty(dto.correoUsu) || string.IsNullOrEmpty(dto.pwHashUsu))
                    return BadRequest(new
                    {
                        mensaje = "Los campos no pueden estar vacíos."
                    });

                //Valida en la capa servicios si los datos son correctos o existentes
                var usuario = await _loginServicio.LoginAsync(dto.correoUsu, dto.pwHashUsu);

                //Si no existe el usuario o la contraseña no coincide
                if (usuario == null)
                    return BadRequest(new { mensaje = "Correo o contraseña incorrectos." });

                return Ok(new
                {
                    mensaje = "Login exitoso",
                    usuario = new
                    {
                        id = usuario.idUsuario,
                        nombre = usuario.nombreUsu,
                        correo = usuario.correoUsu
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al procesar la solicitud",
                    error = ex.Message
                });
            }
            
        }
    }
}
