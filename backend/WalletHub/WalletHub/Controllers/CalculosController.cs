using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
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
        [FromQuery] string idUsuario,
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fin)
    {
        var resumen = await _calculosService.ObtenerResumenAsync(idUsuario, inicio, fin);
        return Ok(resumen);
    }
}
