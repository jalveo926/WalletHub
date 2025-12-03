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

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstadisticasController : ControllerBase
{
    private readonly IEstadisticasService _estadisticasService;

    public EstadisticasController(IEstadisticasService estadisticasService)
    {
        _estadisticasService = estadisticasService;
    }

    // Obtener las transacciones del periodo elegido divido en ingresos vs gastos
    [HttpGet("ingresos-vs-gastos")]
    public async Task<ActionResult<IngresosGastosDTO>> GetIngresosVsGastos(
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

            var resultado = await _estadisticasService.ObtenerIngresosVsGastosAsync(idUsuario, fechaInicio, fechaFin);

            if (resultado == null)
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
                mensaje = "Datos obtenidos correctamente.",
                datos = resultado
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
                mensaje = "Error interno en el servidor.",
                error = ex.Message
            });
        }
    }

    // Obtener gastos distribuidos por categoria del periodo elegido
    [HttpGet("gastos-por-categoria")]
    public async Task<ActionResult<List<TotalCategoriaDTO>>> GetGastosPorCategoria(
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

            var resultado = await _estadisticasService.ObtenerGastosPorCategoriaAsync(idUsuario, fechaInicio, fechaFin);

            if (resultado == null)
            {
                return NotFound(new
                {
                    exito = false,
                    mensaje = "No hay gastos para el periodo seleccionado."
                });
            }

            return Ok(new
            {
                exito = true,
                mensaje = "Datos obtenidos correctamente.",
                datos = resultado
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
                mensaje = "Error interno en el servidor.",
                error = ex.Message
            });
        }
    }

    // Obtener ingresos distribuidos por categoria del periodo elegido
    [HttpGet("ingresos-por-categoria")]
    public async Task<ActionResult<List<TotalCategoriaDTO>>> GetIngresosPorCategoria(
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

            var resultado = await _estadisticasService.ObtenerIngresosPorCategoriaAsync(idUsuario, fechaInicio, fechaFin);

            if (resultado == null)
            {
                return NotFound(new
                {
                    exito = false,
                    mensaje = "No hay ingresos para el periodo seleccionado."
                });
            }

            return Ok(new
            {
                exito = true,
                mensaje = "Datos obtenidos correctamente.",
                datos = resultado
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
                mensaje = "Error interno en el servidor.",
                error = ex.Message
            });
        }
    }
}
