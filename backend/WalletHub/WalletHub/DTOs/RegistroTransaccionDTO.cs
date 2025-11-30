namespace WalletHub.DTOs
{
    // DTO para recibir datos para crear transacciones
    public class RegistroTransaccionDTO
    {
        public decimal montoTransac { get; set; }
        public string descripcionTransac { get; set; } = string.Empty;
        public DateOnly fechaTransac { get; set; } // Fecha de la transacción sin hora
        public string nombreCateg { get; set; } = string.Empty;
    }
}
