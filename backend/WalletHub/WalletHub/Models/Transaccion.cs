namespace WalletHub.Models
{
    public class Transaccion
    {
        public  string idTransaccion { get; set; } = string.Empty;
        public DateTime fechaTransac { get; set; }
        public decimal montoTransac { get; set; }
        public string descripcionTransac { get; set; } 
        public string idUsuario { get; set; } = string.Empty; //FK
        public string idCategoria { get; set; } = string.Empty; //FK
    }
}
