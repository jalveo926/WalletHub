namespace WalletHub.Models
{
    public class Transaccion
    {
        public  string idTransaccion { get; set; } = string.Empty;
        public DateTime fechaTransac { get; set; }
        public decimal montoTransac { get; set; }
        public string descripcionTransac { get; set; }

        // FK Usuario
        public string idUsuario { get; set; }
        public Usuario Usuario { get; set; }

        // FK Categoria
        public string idCategoria { get; set; }
        public Categoria Categoria { get; set; }
    }
}
