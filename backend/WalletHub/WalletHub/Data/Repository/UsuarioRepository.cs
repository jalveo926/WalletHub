using Microsoft.EntityFrameworkCore;
using WalletHub.Data.Interface;
using WalletHub.Models;
using WalletHub.Utils;

namespace WalletHub.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context; // Campo privado para acceder a la base de datos

        /// <summary>
        /// Constructor - Recibe el contexto de BD por inyección de dependencias
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string contrasena)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
                return null;

            /*Las credenciales son el correo y contraseña*/
            var credencialesUsuario = await _context.Usuario.FirstOrDefaultAsync(u => u.correoUsu == correo);
            if (credencialesUsuario == null)
                return null;

            if (credencialesUsuario.pwHashUsu != contrasena)
                return null;

            return credencialesUsuario;
        }

        public async Task<Usuario?> RegistrarUsuarioAsync(Usuario usuario)
        {
            var existeUsuario = await _context.Usuario.FirstOrDefaultAsync(u => u.correoUsu == usuario.correoUsu);
            if (existeUsuario != null)
            {
                // El correo ya está registrado
                return null;
            }

            // Generar ID para el nuevo usuario
            usuario.idUsuario = await IdGenerator.GenerateIdAsync(
                _context.Usuario,  
                "US",              
                "idUsuario",        
                padding: 3,
                maxLength: 5
            );

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

    }

}
