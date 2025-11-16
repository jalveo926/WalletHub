namespace WalletHub.Models
{
    public class Transaccion
    {
        public  int idTransaccion { get; set; } //El tipo int ya obliga a que tengan un valor
        public string fechaTransac { get; set; }
        public string montoTransac { get; set; }
        public string descripcionTransac { get; set; }
    }
}
