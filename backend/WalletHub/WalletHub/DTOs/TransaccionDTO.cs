namespace WalletHub.DTOs
{
    // DTO para devolver datos de transacciones
    public class TransaccionDTO
    {
        public DateTime fechaTransac { get; set; }
        public decimal montoTransac { get; set; }
        public string descripcionTransac { get; set; }
        public string nombreCateg { get; set; }

        public string tipoCategoria { get; set; }
       
    }
}
