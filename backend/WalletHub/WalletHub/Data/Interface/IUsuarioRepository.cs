using WalletHub.Models;

namespace WalletHub.Data.Interface
{
    public interface IUsuarioRepository
    {
        public Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string contrasena);
    }
}
