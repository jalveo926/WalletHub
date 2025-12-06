using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Services.Interface;

namespace WalletHub.Controllers
{
    [Authorize] // Protege todas las rutas del controlador; solo usuarios autenticados pueden acceder
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IReportePDFService _reportePdfServicio;

        // Inyección del servicio que genera los PDFs
        public ReporteController(IReportePDFService reportePdfServicio)
        {
            _reportePdfServicio = reportePdfServicio;
        }

        // ============================================
        // DESCARGAR PDF DIRECTAMENTE POR PERIODO
        // ============================================
        [HttpPost("DescargarPdfPeriodo")]
        public async Task<IActionResult> DescargarPdfPorPeriodo([FromBody] ReporteSolicitadoDTO dto)
        {
            if (dto == null)
                return BadRequest("Debe enviar un tipo de periodo."); // Validación del DTO

            var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
            if (string.IsNullOrEmpty(idUsuario))
                return Unauthorized(); // Validación de usuario autenticado

            DateTime hoy = DateTime.UtcNow;
            DateTime inicio;

            // Determinar fecha de inicio según tipo de periodo
            switch (dto.tipoPeriodo)
            {
                case ReporteSolicitadoDTO.TipoPeriodo.semana:
                    inicio = hoy.AddDays(-7);
                    break;
                case ReporteSolicitadoDTO.TipoPeriodo.mes:
                    inicio = hoy.AddMonths(-1);
                    break;
                case ReporteSolicitadoDTO.TipoPeriodo.año:
                    inicio = hoy.AddYears(-1);
                    break;
                case ReporteSolicitadoDTO.TipoPeriodo.todo:   // Periodo completo desde el año 2000
                    inicio = new DateTime(2000, 1, 1);
                    break;
                default:
                    return BadRequest("Periodo inválido. Use: semana, mes, año o todo.");
            }

            // Generar PDF con los datos del periodo
            var pdfBytes = await _reportePdfServicio.GenerarReportePdfPorPeriodoAsync(
                idUsuario,
                inicio,
                hoy
            );

            if (pdfBytes == null || pdfBytes.Length == 0)
                return StatusCode(500, "No fue posible generar el PDF."); // Error si el PDF no se generó

            string nombreArchivo = $"Reporte_{dto.tipoPeriodo}_{DateTime.Now:yyyyMMddHHmm}.pdf";

            // Retornar el PDF como archivo descargable
            return File(pdfBytes, "application/pdf", nombreArchivo);
        }

        // ============================================
        // DESCARGAR PDF POR ID DE REPORTE (OPCIONAL)
        // ============================================
        [HttpGet("DescargarPdf/{idReporte}")]
        public async Task<IActionResult> DescargarPdf(string idReporte)
        {
            var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
            if (string.IsNullOrEmpty(idUsuario))
                return Unauthorized(); // Verificar usuario autenticado

            var pdfBytes = await _reportePdfServicio.GenerarReportePdfAsync(idReporte, idUsuario);

            if (pdfBytes == null)
                return NotFound("No fue posible generar el PDF para este reporte."); // PDF no encontrado

            string nombreArchivo = $"Reporte_{idReporte}.pdf";

            // Retornar PDF generado
            return File(pdfBytes, "application/pdf", nombreArchivo);
        }
    }
}
