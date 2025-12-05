using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services;
using WalletHub.Services.Interface;

namespace WalletHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly RegistrarUsuarioService _registrarUsuarioService;

        public RegistroController(
            RegistrarUsuarioService registrarUsuarioService)
        {
            _registrarUsuarioService = registrarUsuarioService;
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDTO dto)
        {
            try
            {
                var usuarioRegistrado = await _registrarUsuarioService.RegistrarUsuarioAsync(dto);

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
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }
    }
}
