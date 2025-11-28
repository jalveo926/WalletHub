using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using WalletHub.Data.Interface;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services;
using WalletHub.Services.Interface;

namespace WalletHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly ITransaccionService _transaccionService;
        public TransaccionController(ITransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }

        [HttpGet("PorCategoria")]
        public async Task<IActionResult> FiltrarCategoria(string categoria)
        {

            //Validar que la categoria no sea nula o vacía
            if (string.IsNullOrEmpty(categoria) )
            {
                return BadRequest(new {
                   mensaje =  "La categoría no puede ser nula o vacía."
                });
            }

            try
                {
                    var transaccionesFiltradas = await _transaccionService.FiltrarCategoriaAsync(categoria);
                if (transaccionesFiltradas.IsNullOrEmpty()) {
                    return BadRequest(new
                    {
                        mensaje = "No hay transacciones por mostrar este filtro",
                        filtro = categoria
                    });
                }

                    return Ok(new
                {
                    mensaje = "Filtrado de categorias exitoso",
                    transaccionesFiltradas
                });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new {
                        mensaje = "Ocurrio un error inesperado"
                    } );
                }
        }

        [HttpGet("TodasTransacciones")]
        public async Task<IActionResult> ObtenerTodasTransacciones()
        {
            try
            {
                var todasTransacciones = await _transaccionService.ObtenerTodasTransaccionesAsync();
                if (todasTransacciones.IsNullOrEmpty())
                {
                    return BadRequest(new
                    {
                        mensaje = "No hay transacciones por mostrar este filtro"
                    });
                }

                return Ok(new { 
                    mensaje = "Transacciones obtenidas exitosamente",
                    todasTransacciones
                
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500,new { 
                    mensaje = "Error al obtener todas las transacciones.",

                } );
            }
        }

        [Authorize]
        [HttpGet("MisTransacciones")]
        public async Task<IActionResult> ObtenerMisTransacciones()
        {
            try
            {
                // 1. Obtener el ID del usuario desde el JWT
                var idUsuario = User.FindFirst("idUsuario")?.Value;

                if (string.IsNullOrEmpty(idUsuario))
                {
                    return Unauthorized(new
                    {
                        mensaje = "No se pudo obtener el usuario autenticado."
                    });
                }

                // 2. Obtener las transacciones de ese usuario
                var transacciones = await _transaccionService.ObtenerMisTransaccionesAsync(idUsuario);

                if (transacciones == null)
                {
                    return NotFound(new
                    {
                        mensaje = "No hay transacciones registradas para este usuario."
                    });
                }

                // 3. Respuesta OK
                return Ok(new
                {
                    mensaje = "Transacciones obtenidas exitosamente.",
                    transacciones
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las transacciones.",
                    detalle = ex.Message
                });
            }
        }

        [HttpPost("RegistrarTransaccion")]
        public async Task<IActionResult> RegistrarTransaccion([FromBody]RegistroTransaccionDTO dto)
        {
            try
            {
                string idUsuario = User.Claims.First(c => c.Type == "idUsuario").Value; 
                var nuevaTransaccion = await _transaccionService.RegistrarTransaccion(dto, idUsuario);
                return Ok(new
                {
                    mensaje = "Transacción registrada exitosamente",
                    transaccion = nuevaTransaccion
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = ex.Message
                });
            }
        }

        ///<summary>
        ///Elimina una transacción por su id.
        ///</summary>
        ///<param name="idTransaccion">ID de la transacción a eliminar</param>
        [HttpDelete("{idTransaccion}")]
        public async Task<IActionResult> EliminarTransaccion(string idTransaccion)
        {
            try
            {
                var resultado = await _transaccionService.EliminarTransaccionAsync(idTransaccion);

                if (!resultado)
                    return NotFound(new { mensaje = "Transacción no encontrada" });

                return Ok(new { mensaje = "Transacción eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar la transacción: " + ex.Message });
            }
        }

        [HttpPut("ActualizarTransaccion")]
        public async Task<IActionResult> ActualizarTransaccion([FromBody] ActualizarTransaccionDTO transaccionDTO, string idUsuario)
        {
            try
            {
                var actualizado = await _transaccionService.ActualizarTransaccionAsync(transaccionDTO, idUsuario);
                if (!actualizado)
                {
                    return new NotFoundObjectResult(new
                    {
                        mensaje = "La transacción no fue encontrada para actualizar."
                    });
                }

                return new OkObjectResult(new
                {
                    mensaje = "Transacción actualizada exitosamente",
                    actualizado = true
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al actualizar la transacción.",
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
