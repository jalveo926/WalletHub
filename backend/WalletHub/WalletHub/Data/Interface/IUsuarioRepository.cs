using WalletHub.Models;

namespace WalletHub.Data.Interface
{
    public interface IUsuarioRepository
    {
        public Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string contrasena);

        public Task<Usuario?> RegistrarUsuarioAsync(Usuario usuario);

        public Task<Usuario?> GetUsuarioByCorreoAsync(string correo);
    }
}
