using WalletHub.Models;
using WalletHub.DTOs;
public interface IReporteRepository
{
    Task<string> GenerarIdReporteAsync();
    Task CreateReporteAsync(Reporte reporte);
    Task<IEnumerable<ReporteDTO>> GetReportesAsync(string idUsuario); //Obtener los reportes de un usuario especifico
    Task<ReporteDTO?> GetReporteByIdAsync(string idReporte, string idUsuario);
    Task<bool> DeleteReporteAsync(string idReporte, string idUsuario);
    Task<Reporte?> GetReporteByIdInterno(string idReporte, string idUsuario);
}
