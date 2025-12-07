using System;

namespace WalletHub.Services.Interface
{
    public interface IReportePDFService
    {
        // Genera un PDF para un usuario en un rango de fechas específico
        Task<byte[]> GenerarReportePdfPorPeriodoAsync(
            string idUsuario,   // ID del usuario
            DateTime inicio,    // Fecha de inicio del periodo
            DateTime fin        // Fecha de fin del periodo
        );

        // Genera un PDF de un reporte específico por su ID
        Task<byte[]> GenerarReportePdfAsync(
            string idReporte,   // ID del reporte
            string idUsuario    // ID del usuario propietario
        );
    }
}
