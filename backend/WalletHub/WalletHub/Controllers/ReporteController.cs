using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Services.Interface;

namespace WalletHub.Controllers
{
    [Authorize] 
    [Route("api/[controller]")] 
    [ApiController] 
    public class ReporteController : ControllerBase
    {
        private readonly IReportePDFService _reportePdfServicio; // Servicio para generar PDFs

        // Constructor con inyección de dependencias
        public ReporteController(IReportePDFService reportePdfServicio)
        {
            _reportePdfServicio = reportePdfServicio; // Asignar servicio inyectado
        }

        // ============================================
        // DESCARGAR PDF DIRECTAMENTE POR PERIODO
        // ============================================
        [HttpPost("DescargarPdfPeriodo")]
        public async Task<IActionResult> DescargarPdfPorPeriodo([FromBody] ReporteSolicitadoDTO dto)
        {
            // Validar que el DTO no sea nulo
            if (dto == null)
                return BadRequest("Debe enviar un tipo de periodo.");

            // Extraer ID de usuario del token JWT
            var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;

            // Verificar que el usuario esté autenticado
            if (string.IsNullOrEmpty(idUsuario))
                return Unauthorized();

            DateTime hoy = DateTime.UtcNow; // Fecha actual en UTC
            DateTime inicio; // Variable para fecha de inicio

            // Calcular fecha de inicio según el tipo de periodo
            switch (dto.tipoPeriodo)
            {
                case ReporteSolicitadoDTO.TipoPeriodo.semana:
                    inicio = hoy.AddDays(-7); // Últimos 7 días
                    break;

                case ReporteSolicitadoDTO.TipoPeriodo.mes:
                    inicio = hoy.AddMonths(-1); // Último mes
                    break;

                case ReporteSolicitadoDTO.TipoPeriodo.año:
                    inicio = hoy.AddYears(-1); // Último año
                    break;

                case ReporteSolicitadoDTO.TipoPeriodo.todo:
                    inicio = new DateTime(2000, 1, 1); // Desde el año 2000
                    break;

                default:
                    // Retornar error si el periodo no es válido
                    return BadRequest("Periodo inválido. Use: semana, mes, año o todo.");
            }

            // Llamar al servicio para generar el PDF
            var pdfBytes = await _reportePdfServicio.GenerarReportePdfPorPeriodoAsync(
                idUsuario,
                inicio,
                hoy
            );

            // Verificar que el PDF se haya generado correctamente
            if (pdfBytes == null || pdfBytes.Length == 0)
                return StatusCode(500, "No fue posible generar el PDF.");

            // Crear nombre de archivo con timestamp
            string nombreArchivo = $"Reporte_{dto.tipoPeriodo}_{DateTime.Now:yyyyMMddHHmm}.pdf";

            // Retornar archivo PDF para descarga
            return File(pdfBytes, "application/pdf", nombreArchivo);
        }

        // ============================================
        // DESCARGAR PDF POR ID DE REPORTE (OPCIONAL)
        // ============================================
        [HttpGet("DescargarPdf/{idReporte}")]
        public async Task<IActionResult> DescargarPdf(string idReporte)
        {
            // Extraer ID de usuario del token JWT
            var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;

            // Verificar autenticación del usuario
            if (string.IsNullOrEmpty(idUsuario))
                return Unauthorized();

            // Generar PDF usando el ID del reporte guardado
            var pdfBytes = await _reportePdfServicio.GenerarReportePdfAsync(idReporte, idUsuario);

            // Validar que el PDF se haya generado
            if (pdfBytes == null)
                return NotFound("No fue posible generar el PDF para este reporte.");

            // Crear nombre de archivo con el ID del reporte
            string nombreArchivo = $"Reporte_{idReporte}.pdf";

            // Retornar archivo PDF para descarga
            return File(pdfBytes, "application/pdf", nombreArchivo);
        }
    }
}