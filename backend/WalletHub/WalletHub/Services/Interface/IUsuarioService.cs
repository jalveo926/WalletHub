using WalletHub.Models;

namespace WalletHub.Services.Interface
{
    public interface IUsuarioService
    {
        public Task<Usuario?> VerificarCorreoContrasena(string correo, string contrasena);

        public Task<Usuario?> RegistrarUsuarioAsync(Usuario usuario);
    }
}
