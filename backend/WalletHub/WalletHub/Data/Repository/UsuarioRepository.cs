using Microsoft.EntityFrameworkCore;
using WalletHub.Data.Interface;
using WalletHub.Models;

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

    }

}
