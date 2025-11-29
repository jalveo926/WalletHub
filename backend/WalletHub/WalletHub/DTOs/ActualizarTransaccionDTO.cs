namespace WalletHub.DTOs
{
    public class ActualizarTransaccionDTO
    {
        public DateOnly? fechaTransac { get; set; }
        public decimal? montoTransac { get; set; }
        public string? descripcionTransac { get; set; }
        public string? nombreCateg { get; set; }
    }
}
