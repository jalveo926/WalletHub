namespace WalletHub.Services.Interface
{
    public interface IReportePDFService
    {
        Task<byte[]> GenerarReportePdfAsync(string idReporte, string idUsuario);
    }
}
