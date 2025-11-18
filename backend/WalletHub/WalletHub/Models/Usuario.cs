namespace WalletHub.Models
{
    public class Usuario
    {
        public int idUsuario {  get; set; }
        public string nombreUsu { get; set; } = string.Empty;
        public string correoUsu { get; set; } = string.Empty;
        public string pwHashUsu { get; set; } = string.Empty;
    }
}
