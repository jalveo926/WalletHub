using WalletHub.DTOs;
using WalletHub.Models;

namespace WalletHub.Services.Interface
{
    public interface IUsuarioService
    {
        public Task<Usuario?> RegistrarUsuarioAsync(RegistrarDTO dto);
    }
}
