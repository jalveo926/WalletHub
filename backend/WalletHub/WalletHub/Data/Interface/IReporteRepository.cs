using WalletHub.Models;
using WalletHub.DTOs;
public interface IReporteRepository
{
    Task<string> GenerarIdReporteAsync();
    Task CreateReporteAsync(Reporte reporte);
    Task<IEnumerable<Reporte>> GetReportesAsync(string idUsuario); //Obtener los reportes de un usuario especifico
    Task<Reporte?> GetReporteByIdAsync(string idReporte, string idUsuario);
    Task<bool> DeleteReporteAsync(string idReporte, string idUsuario);
}
