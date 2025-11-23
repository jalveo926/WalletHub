namespace WalletHub.DTOs
{
    public class TransaccionDTO
    {
        public DateTime fechaTransac { get; set; }
        public decimal montoTransac { get; set; }
        public string descripcionTransac { get; set; }
        public string nombreCateg { get; set; }

        public TransaccionDTO(string categoria) { 
            nombreCateg = categoria;
        }

        public TransaccionDTO() { }
    }
}
