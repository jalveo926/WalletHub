namespace WalletHub.DTOs
{
    public class ReporteSolicitadoDTO
    {
        public TipoPeriodo tipoPeriodo { get; set; } // Tipo de periodo solicitado (semana, mes, año, todo)
        public DateTime? inicio { get; set; } // Fecha de inicio opcional
        public DateTime? fin { get; set; } // Fecha de fin opcional

        // Enum para los tipos de periodo
        public enum TipoPeriodo
        {
            semana, // Últimos 7 días
            mes, // Último mes
            año, // Último año
            todo // Todo el historial
        }
    }
}
