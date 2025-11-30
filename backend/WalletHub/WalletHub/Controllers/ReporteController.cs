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
        private readonly IReporteService _reporteServicio;
        private readonly IReportePDFService _reportePdfServicio;

        public ReporteController(IReporteService reporteServicio,
                                 IReportePDFService reportePdfServicio)
        {
            _reporteServicio = reporteServicio;
            _reportePdfServicio = reportePdfServicio;
        }

        [HttpPost("AgregarReporte")]
        public async Task<IActionResult> CrearReporte([FromBody] ReporteSolicitadoDTO dto)
        {
            if (dto == null)
            {
                return new BadRequestObjectResult(new
                {
                    mensaje = "Especifique los datos correspondientes."
                });
            }

            if (!Enum.TryParse<ReporteSolicitadoDTO.TipoPeriodo>(
                    dto.tipoPeriodo.ToString(),
                    ignoreCase: true,
                    out var tipoValido))
            {
                return new BadRequestObjectResult(new
                {
                    mensaje = "El tipo de periodo no es válido. Debe ser: semana, mes o año."
                });
            }

            dto.tipoPeriodo = tipoValido;

            try
            {
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return new UnauthorizedResult();
                }

                var nuevoIdReporte = await _reporteServicio.CrearReporteAsync(dto, idUsuario);

                return new OkObjectResult(new
                {
                    mensaje = "Reporte creado exitosamente",
                    idReporte = nuevoIdReporte
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al crear el reporte"
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpDelete("EliminarReporte/{idReporte}")]
        public async Task<IActionResult> EliminarReporte(string idReporte)
        {
            try
            {
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return new UnauthorizedResult();
                }

                var eliminado = await _reporteServicio.EliminarReporteAsync(idReporte, idUsuario);
                if (!eliminado)
                {
                    return new NotFoundObjectResult(new
                    {
                        mensaje = "El reporte no existe."
                    });
                }

                return new OkObjectResult(new
                {
                    mensaje = "Reporte eliminado exitosamente."
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al eliminar el reporte"
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpGet("MisReportes")]
        public async Task<IActionResult> ObtenerReportesPorUsuario()
        {
            try
            {
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return new UnauthorizedResult();
                }

                var reportes = await _reporteServicio.ObtenerReportesPorUsuarioAsync(idUsuario);

                if (reportes == null || !reportes.Any())
                {
                    return new OkObjectResult(new
                    {
                        mensaje = "No se encontraron reportes para este usuario.",
                        reportes = new List<object>()
                    });
                }

                return new OkObjectResult(new
                {
                    mensaje = "Reportes obtenidos exitosamente",
                    reportes
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al obtener los reportes"
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpGet("ObtenerReporteUnico/{idReporte}")]
        public async Task<IActionResult> ObtenerReportePorId(string idReporte)
        {
            try
            {
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return new UnauthorizedResult();
                }

                var reporte = await _reporteServicio.ObtenerReportePorIdAsync(idReporte, idUsuario);
                if (reporte == null)
                {
                    return new NotFoundObjectResult(new
                    {
                        mensaje = "El reporte no existe."
                    });
                }

                return new OkObjectResult(reporte);
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al obtener el reporte"
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpGet("DescargarPdf/{idReporte}")]
        public async Task<IActionResult> DescargarPdf(string idReporte)
        {
            try
            {
                var idUsuario = User.Claims.FirstOrDefault(c => c.Type == "idUsuario")?.Value;
                if (string.IsNullOrEmpty(idUsuario))
                {
                    return new UnauthorizedResult();
                }

                var pdfBytes = await _reportePdfServicio.GenerarReportePdfAsync(idReporte, idUsuario);

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return new NotFoundObjectResult(new
                    {
                        mensaje = "No fue posible generar el PDF para este reporte."
                    });
                }

                var nombreArchivo = $"Reporte_{idReporte}.pdf";
                return File(pdfBytes, "application/pdf", nombreArchivo);
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al generar o descargar el PDF del reporte"
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
