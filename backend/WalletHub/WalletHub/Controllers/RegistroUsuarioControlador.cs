using Microsoft.AspNetCore.Mvc;
using WalletHub.Data;
using WalletHub.Models;
using System.Security.Cryptography;
using System.Text;

namespace WalletHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroUsuarioControlador : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RegistroUsuarioControlador(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

        [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            if (_dbContext.Usuario.Any(u => u.correoUsu == usuario.correoUsu))
                return BadRequest("El correo ya está registrado.");

            usuario.idUsuario = $"US{(_dbContext.Usuario.Count() + 1):D3}";
            usuario.pwHashUsu = HashPassword(usuario.pwHashUsu);

            usuario.Categorias = new List<Categoria>();
            usuario.Transacciones = new List<Transaccion>();
            usuario.Reportes = new List<Reporte>();

            _dbContext.Usuario.Add(usuario);
            _dbContext.SaveChanges();

            return Ok("Usuario registrado exitosamente.");
        }
    }
}
