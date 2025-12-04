using WalletHub.Models;

namespace WalletHub.DTOs
{
    public class ReporteSolicitadoDTO
    {
        public TipoPeriodo tipoPeriodo { get; set; } // "semana", "mes", "año"
        public TipoArchivo tipoArchivoRepo { get; set; } // Pdf o Excel

        public enum TipoPeriodo
        {
            semana,
            mes,
            año
        }
    }
}
