namespace WalletHub.Models
{
    public class Transaccion
    {
        public  int idTransaccion { get; set; } //El tipo int ya obliga a que tengan un valor
        public DateTime fechaTransac { get; set; }
        public decimal montoTransac { get; set; }
        public string descripcionTransac { get; set; } 
    }
}
