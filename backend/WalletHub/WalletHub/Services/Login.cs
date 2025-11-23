using Microsoft.EntityFrameworkCore;
using WalletHub.Data;
using System.Net.Mail;
using WalletHub.Models;

namespace WalletHub.Services
{
    public class Login
    {
        private readonly ApplicationDbContext _context; // Campo privado para acceder a la base de datos

        /// <summary>
        /// Constructor - Recibe el contexto de BD por inyección de dependencias
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        public Login(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> LoginAsync(string correo, string contrasena)
        {
            if (!ValidarCorreo(correo) ||string.IsNullOrEmpty(contrasena))
                return null;

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.correoUsu == correo);
            if (usuario == null) 
                return null;

            if (usuario.pwHashUsu != contrasena)
                return null;

            return usuario;
        }

        private bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return false;

            try
            {
                var mail = new MailAddress(correo);
                return mail.Address == correo; //Validar que tenga un arroba
            }
            catch { 
                return false; 
            }
        }
            
    }
}
