using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletHub.DTOs;
using WalletHub.Services.Interface;

namespace WalletHub.Controllers
{
    // Controlador para gestionar la creación, consulta y descarga de reportes
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _reporteServicio;
        private readonly IReportePDFService _reportePdfServicio;

        // Inyección de servicios de dominio (reportes y generación de PDF)
        public ReporteController(
            IReporteService reporteServicio,
            IReportePDFService reportePdfServicio)
        {
            _reporteServicio = reporteServicio;
            _reportePdfServicio = reportePdfServicio;
        }

        // Crea un nuevo reporte para el usuario autenticado
        [HttpPost("AgregarReporte")]
        public async Task<IActionResult> CrearReporte([FromBody] ReporteSolicitadoDTO dto)
        {
            // Validación básica del cuerpo de la petición
            if (dto == null)
            {
                return BadRequest(new
                {
                    mensaje = "Especifique los datos correspondientes."
                });
            }

            // Validación del enum tipoPeriodo (semana, mes o año)
            if (!Enum.TryParse<ReporteSolicitadoDTO.TipoPeriodo>(
                    dto.tipoPeriodo.ToString(),
                    ignoreCase: true,
                    out var tipoValido))
            {
                return BadRequest(new
                {
                    mensaje = "El tipo de periodo no es válido. Debe ser: semana, mes o año."
                });
            }

            dto.tipoPeriodo = tipoValido;

            try
            {
                // Obtiene el id del usuario desde los claims del token
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return Unauthorized();
                }

                // Crea el reporte y devuelve el identificador generado
                var nuevoIdReporte = await _reporteServicio.CrearReporteAsync(dto, idUsuario);

                return Ok(new
                {
                    mensaje = "Reporte creado exitosamente",
                    idReporte = nuevoIdReporte
                });
            }
            catch (Exception)
            {
                // Error genérico de servidor al crear el reporte
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al crear el reporte"
                });
            }
        }

        // Elimina un reporte específico del usuario autenticado
        [HttpDelete("EliminarReporte/{idReporte}")]
        public async Task<IActionResult> EliminarReporte(string idReporte)
        {
            try
            {
                // Valida que el usuario esté autenticado
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return Unauthorized();
                }

                // Intenta eliminar el reporte asociado al usuario
                var eliminado = await _reporteServicio.EliminarReporteAsync(idReporte, idUsuario);
                if (!eliminado)
                {
                    // No se encontró el reporte o no pertenece al usuario
                    return NotFound(new
                    {
                        mensaje = "El reporte no existe."
                    });
                }

                return Ok(new
                {
                    mensaje = "Reporte eliminado exitosamente."
                });
            }
            catch (Exception)
            {
                // Error genérico de servidor al eliminar
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al eliminar el reporte"
                });
            }
        }

        // Obtiene todos los reportes pertenecientes al usuario autenticado
        [HttpGet("MisReportes")]
        public async Task<IActionResult> ObtenerReportesPorUsuario()
        {
            try
            {
                // Valida que el usuario esté autenticado
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return Unauthorized();
                }

                // Recupera la lista de reportes del usuario
                var reportes = await _reporteServicio.ObtenerReportesPorUsuarioAsync(idUsuario);

                // Si no hay reportes, devuelve lista vacía con mensaje informativo
                if (reportes == null || !reportes.Any())
                {
                    return Ok(new
                    {
                        mensaje = "No se encontraron reportes para este usuario.",
                        reportes = new List<object>()
                    });
                }

                return Ok(new
                {
                    mensaje = "Reportes obtenidos exitosamente",
                    reportes
                });
            }
            catch (Exception)
            {
                // Error genérico de servidor al listar
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al obtener los reportes"
                });
            }
        }

        // Obtiene la información de un solo reporte por su id
        [HttpGet("ObtenerReporteUnico/{idReporte}")]
        public async Task<IActionResult> ObtenerReportePorId(string idReporte)
        {
            try
            {
                // Valida que el usuario esté autenticado
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return Unauthorized();
                }

                // Busca el reporte asociado al usuario
                var reporte = await _reporteServicio.ObtenerReportePorIdAsync(idReporte, idUsuario);
                if (reporte == null)
                {
                    return NotFound(new
                    {
                        mensaje = "El reporte no existe."
                    });
                }

                // Devuelve el reporte encontrado
                return Ok(reporte);
            }
            catch (Exception)
            {
                // Error genérico de servidor al consultar
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al obtener el reporte"
                });
            }
        }

        // Genera y descarga el PDF de un reporte para el usuario autenticado
        [HttpGet("DescargarPdf/{idReporte}")]
        public async Task<IActionResult> DescargarPdf(string idReporte)
        {
            try
            {
                // Valida que el usuario esté autenticado
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return Unauthorized();
                }

                // Genera el PDF del reporte (byte[])
                var pdfBytes = await _reportePdfServicio.GenerarReportePdfAsync(idReporte, idUsuario);

                // Si no se pudo generar contenido
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return NotFound(new
                    {
                        mensaje = "No fue posible generar el PDF para este reporte."
                    });
                }

                // Devuelve el archivo PDF para descarga
                var nombreArchivo = $"Reporte_{idReporte}.pdf";
                return File(pdfBytes, "application/pdf", nombreArchivo);
            }
            catch (Exception)
            {
                // Error genérico de servidor al generar o enviar el PDF
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al generar o descargar el PDF del reporte"
                });
            }
        }
    }
}
