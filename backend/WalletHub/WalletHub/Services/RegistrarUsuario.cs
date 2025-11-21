using WalletHub.Data;
using WalletHub.Models;
using System.Security.Cryptography;
using System.Text;

namespace WalletHub.Servicios
{
    public class RegistrarUsuario
    {
        private readonly ApplicationDbContext _dbContext;

        public RegistrarUsuario(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CorreoExiste(string correo)
        {
            return _dbContext.Usuario.Any(u => u.correoUsu == correo);
        }

        public void RegistrarNuevoUsuario(Usuario usuario)
        {
            usuario.idUsuario = $"US{(_dbContext.Usuario.Count() + 1):D3}";
            usuario.pwHashUsu = HashPassword(usuario.pwHashUsu);
            usuario.Categorias = new List<Categoria>();
            usuario.Transacciones = new List<Transaccion>();
            usuario.Reportes = new List<Reporte>();
            _dbContext.Usuario.Add(usuario);
            _dbContext.SaveChanges();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
