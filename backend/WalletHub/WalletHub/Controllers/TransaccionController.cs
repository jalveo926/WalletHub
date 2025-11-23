using Microsoft.AspNetCore.Mvc;
using WalletHub.Data.Interface;
using WalletHub.Services.Interface;
using WalletHub.Services;
using System.Threading.Tasks;

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
            if (string.IsNullOrEmpty(categoria))
            {
                return BadRequest("La categoría no puede ser nula o vacía.");
            }

            try
                {
                    var transaccionesFiltradas = await _transaccionService.FiltrarCategoriaAsync(categoria);
                    return Ok(transaccionesFiltradas);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error al filtrar transacciones por categoría.");
                }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodasTransacciones()
        {
            try
            {
                var todasTransacciones = await _transaccionService.ObtenerTodasTransaccionesAsync();
                return Ok(todasTransacciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener todas las transacciones.");
            }
        }
    }
}
