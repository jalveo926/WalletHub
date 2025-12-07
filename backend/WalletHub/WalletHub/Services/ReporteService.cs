using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services.Interface;
using static WalletHub.DTOs.ReporteSolicitadoDTO;

namespace WalletHub.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _repo; // Repositorio para persistencia de reportes

        // Constructor con inyección de dependencias
        public ReporteService(IReporteRepository repo)
        {
            _repo = repo; // Asignar repositorio inyectado
        }

        // ⚠ Ya NO se usa para PDFs por periodo
        //    Solo se mantiene para compatibilidad con reportes guardados
        public async Task<string> CrearReporteAsync(ReporteSolicitadoDTO dto, string idUsuario)
        {
            // Variables para almacenar fechas calculadas
            DateTime inicio;
            DateTime final;

            // Calcular rango de fechas según el tipo de periodo
            switch (dto.tipoPeriodo)
            {
                case TipoPeriodo.semana:
                    inicio = DateTime.Today.AddDays(-7); // Hace 7 días desde hoy
                    final = DateTime.Today; // Fecha actual
                    break;

                case TipoPeriodo.mes:
                    inicio = DateTime.Today.AddMonths(-1); // Hace 1 mes desde hoy
                    final = DateTime.Today; // Fecha actual
                    break;

                case TipoPeriodo.año:
                    inicio = DateTime.Today.AddYears(-1); // Hace 1 año desde hoy
                    final = DateTime.Today; // Fecha actual
                    break;

                default:
                    throw new Exception("Tipo de periodo inválido."); // Lanzar error para valores no válidos
            }

            // Generar ID único para el nuevo reporte
            string nuevoId = await _repo.GenerarIdReporteAsync();

            // Crear objeto Reporte con los datos calculados
            var reporte = new Reporte
            {
                idReporte = nuevoId, // ID generado
                idUsuario = idUsuario, // Usuario propietario
                inicioPeriodo = inicio, // Fecha inicio calculada
                finalPeriodo = final, // Fecha fin calculada
                fechaCreacionRepo = DateTime.UtcNow, // Fecha de creación en UTC
                rutaArchivoRepo = "" // Ruta vacía (se puede implementar después)
            };

            // Persistir reporte en la base de datos
            await _repo.CreateReporteAsync(reporte);

            return nuevoId; // Retornar ID del reporte creado
        }

        // Eliminar un reporte existente
        public Task<bool> EliminarReporteAsync(string idReporte, string idUsuario)
        {
            // Delegar eliminación al repositorio (valida usuario)
            return _repo.DeleteReporteAsync(idReporte, idUsuario);
        }

        // Obtener un reporte específico por su ID
        public Task<ReporteDTO?> ObtenerReportePorIdAsync(string idReporte, string idUsuario)
        {
            // Delegar búsqueda al repositorio (valida usuario)
            return _repo.GetReporteByIdAsync(idReporte, idUsuario);
        }

        // Obtener lista de todos los reportes de un usuario
        public Task<IEnumerable<ReporteDTO>> ObtenerReportesPorUsuarioAsync(string idUsuario)
        {
            // Delegar consulta al repositorio
            return _repo.GetReportesAsync(idUsuario);
        }
    }
}