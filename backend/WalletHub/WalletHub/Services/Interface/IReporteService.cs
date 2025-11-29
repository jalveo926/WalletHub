using WalletHub.DTOs;
using WalletHub.Models;
namespace WalletHub.Services.Interface
{
    public interface IReporteService
    {
        public Task<string> CrearReporteAsync(ReporteSolicitadoDTO dto, string idUsuario);
        public Task<IEnumerable<Reporte>> ObtenerReportesPorUsuarioAsync(string idUsuario);

        public Task<Reporte?> ObtenerReportePorIdAsync(string idReporte, string idUsuario);
        public Task<bool> EliminarReporteAsync(string idReporte, string idUsuario);

    }
}
