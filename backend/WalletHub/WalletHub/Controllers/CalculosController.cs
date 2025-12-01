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

[ApiController]
[Route("api/[controller]")]
public class CalculosController : ControllerBase
{
    private readonly ICalculosService _calculosService;

    public CalculosController(ICalculosService calculosService)
    {
        _calculosService = calculosService;
    }

    // GET: api/Calculos/resumen
    [HttpGet("resumen")]
    public async Task<ActionResult<CalculosDTO>> GetResumen(
        [FromQuery] DateOnly inicio,
        [FromQuery] DateOnly fin)
    {
        try
        {
            DateTime fechaInicio = inicio.ToDateTime(TimeOnly.MinValue);       // 00:00:00
            DateTime fechaFin = fin.ToDateTime(TimeOnly.MaxValue);            // 23:59:59.9999

            var claim = User.Claims.FirstOrDefault(c => c.Type == "idUsuario");
            if (claim == null)
            {
                return Unauthorized(new
                {
                    exito = false,
                    mensaje = "No se pudo obtener el id del usuario del token."
                });
            }

            string idUsuario = claim.Value;

            var resumen = await _calculosService.ObtenerResumenAsync(idUsuario, fechaInicio, fechaFin);

            if (resumen == null)
            {
                return NotFound(new
                {
                    exito = false,
                    mensaje = "No hay datos disponibles para el periodo seleccionado."
                });
            }

            return Ok(new
            {
                exito = true,
                mensaje = "Traído con éxito",
                datos = resumen
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                exito = false,
                mensaje = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            // Errores lógicos o estado inesperado en la aplicación -> 500 (consistente con tu estilo)
            return StatusCode(500, new
            {
                exito = false,
                mensaje = "Error en la operación.",
                error = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                exito = false,
                mensaje = "Error interno en el servidor",
                error = ex.Message
            });
        }
    }
}
