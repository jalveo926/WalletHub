namespace WalletHub.DTOs
{
    public class ReporteDTO
    {
        public string IdReporte { get; set; }
        public string UrlArchivo { get; set; } // URL accesible desde el front
        public string TipoArchivo { get; set; } // "PDF", "Excel", etc.
        public DateTime FechaCreacion { get; set; }
        public DateTime InicioPeriodo { get; set; }
        public DateTime FinalPeriodo { get; set; }
    }
}
