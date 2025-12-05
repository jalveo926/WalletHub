using WalletHub.Data.Interface;
using WalletHub.Data.Repository;
using WalletHub.Models;
using System.Threading.Tasks;
using WalletHub.Services.Interface;
using WalletHub.DTOs;

namespace WalletHub.Services
{
    public class RegistrarUsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHashService _passwordHashService;

        public RegistrarUsuarioService(IUsuarioRepository usuarioRepository, IPasswordHashService passwordHashService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task<Usuario?> RegistrarUsuarioAsync(RegistrarDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.nombreUsu) ||
        string.IsNullOrWhiteSpace(dto.correoUsu) ||
        string.IsNullOrWhiteSpace(dto.contrasena))
            {
                throw new ArgumentException("Los campos no pueden estar vacíos.");
            }

            if (!dto.correoUsu.Contains("@"))
                throw new ArgumentException("Debe ingresar un correo válido.");

            if (dto.contrasena.Length < 12)
                throw new ArgumentException("La contraseña debe tener al menos 12 caracteres.");

            if (!dto.contrasena.Any(char.IsUpper))
                throw new ArgumentException("La contraseña debe tener al menos una letra mayúscula.");

            if (!dto.contrasena.Any(char.IsDigit))
                throw new ArgumentException("La contraseña debe tener al menos un número.");

            // Hash de contraseña
            string hashedPassword = _passwordHashService.HashPassword(dto.contrasena);

            // Crear entidad usuario
            var usuario = new Usuario
            {
                nombreUsu = dto.nombreUsu,
                correoUsu = dto.correoUsu,
                pwHashUsu = hashedPassword
            };

            // Guardar en BD
            var usuarioRegistrado = await _usuarioRepository.RegistrarUsuarioAsync(usuario);

            if (usuarioRegistrado == null)
                throw new InvalidOperationException("El correo ya está registrado.");

            return usuarioRegistrado;
        }

    }
}
