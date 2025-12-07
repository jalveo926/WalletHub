using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services;
using WalletHub.Services.Interface;
namespace WalletHub.Controllers
{
    [Route("api/[controller]")] // Ruta base: api/Registro
    [ApiController] // Habilita comportamientos automáticos de validación
    public class RegistroController : ControllerBase
    {
        private readonly RegistrarUsuarioService _registrarUsuarioService; // Servicio de registro

        // Constructor con inyección de dependencias
        public RegistroController(
            RegistrarUsuarioService registrarUsuarioService)
        {
            _registrarUsuarioService = registrarUsuarioService; // Asignar servicio inyectado
        }

        // Endpoint: POST api/Registro/RegistrarUsuario
        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDTO dto) // Recibir datos desde body
        {
            try
            {
                // Llamar al servicio para registrar usuario en la BD
                var usuarioRegistrado = await _registrarUsuarioService.RegistrarUsuarioAsync(dto);

                // Retornar respuesta exitosa con datos del usuario creado
                return Ok(new
                {
                    mensaje = "Registro exitoso",
                    usuario = new
                    {
                        id = usuarioRegistrado.idUsuario, // ID generado
                        nombre = usuarioRegistrado.nombreUsu, // Nombre del usuario
                        correo = usuarioRegistrado.correoUsu // Correo electrónico
                    }
                });
            }
            catch (ArgumentException ex) // Errores de validación (ej: correo duplicado)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex) // Errores de lógica de negocio
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception) // Error genérico no controlado
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al procesar la solicitud" });
            }
        }
    }
}