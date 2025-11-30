using WalletHub.DTOs;
using WalletHub.Models;

using WalletHub.Services.Interface;
namespace WalletHub.Services
{
    public class ReporteService: IReporteService
    {
        private readonly IReporteRepository _repo;

        public ReporteService(IReporteRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> CrearReporteAsync(ReporteSolicitadoDTO dto, string idUsuario)
        {
            //  Calcular fechas según la lógica del negocio
            DateTime inicio;
            DateTime final;

            switch (dto.tipoPeriodo)
            {
                case ReporteSolicitadoDTO.TipoPeriodo.semana:
                    inicio = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
                    final = inicio.AddDays(6);
                    break;

                case ReporteSolicitadoDTO.TipoPeriodo.mes:
                    inicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    final = inicio.AddMonths(1).AddDays(-1);
                    break;

                case ReporteSolicitadoDTO.TipoPeriodo.año:
                    inicio = new DateTime(DateTime.Today.Year, 1, 1);
                    final = new DateTime(DateTime.Today.Year, 12, 31);
                    break;

                default:
                    throw new Exception("Tipo de periodo inválido.");
            }

            // El ID lo genera el repositorio
            string nuevoId = await _repo.GenerarIdReporteAsync();

            // Armar entidad COMPLETA AQUÍ (en el servicio)
            var reporte = new Reporte
            {
                idReporte = nuevoId,
                idUsuario = idUsuario,
                tipoArchivoRepo = dto.tipoArchivoRepo,
                inicioPeriodo = inicio,
                finalPeriodo = final,
                fechaCreacionRepo = DateTime.UtcNow,
                rutaArchivoRepo = "" //Hay que definir el metodo para generar la ruta
            };

            // Guardar usando el repositorio
            await _repo.CreateReporteAsync(reporte);

            return nuevoId;
        }

        Task<bool> IReporteService.EliminarReporteAsync(string idReporte, string idUsuario)
        {
            return _repo.DeleteReporteAsync(idReporte, idUsuario);
        }

        Task<ReporteDTO?> IReporteService.ObtenerReportePorIdAsync(string idReporte, string idUsuario)
        {
            return _repo.GetReporteByIdAsync(idReporte, idUsuario);
        }

        Task<IEnumerable<ReporteDTO>> IReporteService.ObtenerReportesPorUsuarioAsync(string idUsuario)
        {
            return _repo.GetReportesAsync( idUsuario);
        }
    }

    


}

