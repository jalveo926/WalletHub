using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services.Interface;
using static WalletHub.DTOs.ReporteSolicitadoDTO;

namespace WalletHub.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _repo;

        // Inyección del repositorio de reportes
        public ReporteService(IReporteRepository repo)
        {
            _repo = repo;
        }

        // ⚠ Ya NO se usa para PDFs por periodo
        //    Solo se mantiene para compatibilidad con reportes guardados
        public async Task<string> CrearReporteAsync(ReporteSolicitadoDTO dto, string idUsuario)
        {
            // Calcular fechas según el periodo solicitado
            DateTime inicio;
            DateTime final;

            switch (dto.tipoPeriodo)
            {
                case TipoPeriodo.semana:
                    inicio = DateTime.Today.AddDays(-7);
                    final = DateTime.Today;
                    break;
                case TipoPeriodo.mes:
                    inicio = DateTime.Today.AddMonths(-1);
                    final = DateTime.Today;
                    break;
                case TipoPeriodo.año:
                    inicio = DateTime.Today.AddYears(-1);
                    final = DateTime.Today;
                    break;
                default:
                    throw new Exception("Tipo de periodo inválido."); // Error si el tipo no es válido
            }

            // Generar un nuevo ID para el reporte
            string nuevoId = await _repo.GenerarIdReporteAsync();

            // Crear objeto Reporte
            var reporte = new Reporte
            {
                idReporte = nuevoId,
                idUsuario = idUsuario,
                inicioPeriodo = inicio,
                finalPeriodo = final,
                fechaCreacionRepo = DateTime.UtcNow,
                rutaArchivoRepo = "" // Ruta del archivo, se puede implementar luego
            };

            // Guardar el reporte en la base de datos
            await _repo.CreateReporteAsync(reporte);

            return nuevoId; // Devolver ID del reporte creado
        }

        // Eliminar un reporte por ID y usuario
        public Task<bool> EliminarReporteAsync(string idReporte, string idUsuario)
        {
            return _repo.DeleteReporteAsync(idReporte, idUsuario);
        }

        // Obtener un reporte por ID y usuario
        public Task<ReporteDTO?> ObtenerReportePorIdAsync(string idReporte, string idUsuario)
        {
            return _repo.GetReporteByIdAsync(idReporte, idUsuario);
        }

        // Obtener todos los reportes de un usuario
        public Task<IEnumerable<ReporteDTO>> ObtenerReportesPorUsuarioAsync(string idUsuario)
        {
            return _repo.GetReportesAsync(idUsuario);
        }
    }
}
