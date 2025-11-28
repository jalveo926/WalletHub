namespace WalletHub.DTOs
{
    public class ActualizarTransaccionDTO
    {
        public DateTime? fechaTransac { get; set; }
        public decimal? montoTransac { get; set; }
        public string? descripcionTransac { get; set; }
        public string? nombreCateg { get; set; }

        public string idTransaccion { get; set; }
    }
}
