using WalletHub.Data.Interface;
using WalletHub.Data.Repository;
using WalletHub.Models;
using System.Threading.Tasks;
using WalletHub.Services.Interface;

namespace WalletHub.Services
{
    public class RegistrarUsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public RegistrarUsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario?> RegistrarUsuarioAsync(Usuario usuario)
        {
            var usuarioRegistrado = await _usuarioRepository.RegistrarUsuarioAsync(usuario);
            return usuarioRegistrado;
        }

    }
}
