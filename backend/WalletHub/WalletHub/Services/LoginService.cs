using Microsoft.EntityFrameworkCore;
using WalletHub.Data;
using System.Net.Mail;
using WalletHub.Models;
using WalletHub.Data.Interface;

namespace WalletHub.Services
{
    public class LoginService
    {
        private readonly IUsuarioRepository _usuarioRepository; // Campo privado para acceder a la base de datos

        /// <summary>
        /// Constructor - Recibe el contexto de BD por inyección de dependencias
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public LoginService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario?> VerificarCorreoContrasena(string correo, string contrasena)
        {
            return await _usuarioRepository.GetByCorreoAndPasswordAsync(correo, contrasena);
        }
    }
}
