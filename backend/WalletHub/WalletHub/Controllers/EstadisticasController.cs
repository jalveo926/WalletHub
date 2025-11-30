using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Services.Interface;

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
        [FromQuery] string idUsuario,
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin)
    {
        var resultado = await _estadisticasService
            .ObtenerIngresosVsGastosAsync(idUsuario, inicio, fin);

        return Ok(resultado);
    }

    // Obtener gastos distribuidos por categoria del periodo elegido
    [HttpGet("gastos-por-categoria")]
    public async Task<ActionResult<List<TotalCategoriaDTO>>> GetGastosPorCategoria(
        [FromQuery] string idUsuario,
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin)
    {
        var resultado = await _estadisticasService
            .ObtenerGastosPorCategoriaAsync(idUsuario, inicio, fin);

        return Ok(resultado);
    }

    // Obtener ingresos distribuidos por categoria del periodo elegido
    [HttpGet("ingresos-por-categoria")]
    public async Task<ActionResult<List<TotalCategoriaDTO>>> GetIngresosPorCategoria(
        [FromQuery] string idUsuario,
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin)
    {
        var resultado = await _estadisticasService
            .ObtenerIngresosPorCategoriaAsync(idUsuario, inicio, fin);

        return Ok(resultado);
    }
}
