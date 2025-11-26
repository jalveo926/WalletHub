using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using WalletHub.Data.Interface;
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

        [HttpGet("categoria")]
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

        [HttpGet]
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
    }
}
