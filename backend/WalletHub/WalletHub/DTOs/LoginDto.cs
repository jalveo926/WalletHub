namespace WalletHub.DTOs
{
    public class LoginDTO
    {
        public string correoUsu { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty; //Cambiar cuando tengamos el servicio de hash
    }
}
