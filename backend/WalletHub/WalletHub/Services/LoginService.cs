using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using WalletHub.Data;
using WalletHub.Data.Interface;
using WalletHub.Models;
using WalletHub.Services.Interface;

namespace WalletHub.Services
{
    public class LoginService
    {
        private readonly IUsuarioRepository _usuarioRepository; // Campo privado para acceder a la base de datos
        private readonly IPasswordHashService _passwordHashService;

        /// <summary>
        /// Constructor - Recibe el contexto de BD por inyección de dependencias
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public LoginService(IUsuarioRepository usuarioRepository, IPasswordHashService passwordHashService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task<Usuario?> VerificarCorreoContrasena(string correo, string contrasena)
        {
            // 1. Buscar usuario por correo
            var usuario = await _usuarioRepository.GetUsuarioByCorreoAsync(correo);
            if (usuario == null) return null;

            // 2. Verificar contraseña contra el hash almacenado
            bool ok = _passwordHashService.VerifyPassword(usuario.pwHashUsu, contrasena);
            return ok ? usuario : null;
        }
    }
}
