using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WalletHub.DTOs;
using WalletHub.Services;

namespace WalletHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginServicio;
        private readonly IConfiguration _config;

        public LoginController(LoginService loginServicio, IConfiguration config)
        {
            _loginServicio = loginServicio;
            _config = config;
        }

        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginDTO dto) //Recomendable usar dto en vez del modelo
        {
            if (string.IsNullOrEmpty(dto.correoUsu) || string.IsNullOrEmpty(dto.Contrasena))
                return BadRequest(new
                {
                    mensaje = "Los campos no pueden estar vacíos."
                });

            try
            {          
                //Valida en la capa servicios si los datos son correctos o existentes
                var credencialesUsuario = await _loginServicio.VerificarCorreoContrasena(dto.correoUsu, dto.Contrasena);

                //Si no existe el usuario o la contraseña no coincide
                if (credencialesUsuario == null)
                    return BadRequest(new { mensaje = "Correo o contraseña incorrectos." });

                var claims = new[]
                {
                    
                    new Claim("idUsuario", credencialesUsuario.idUsuario),
                    new Claim(ClaimTypes.Name, credencialesUsuario.nombreUsu),
                    new Claim(ClaimTypes.Email, credencialesUsuario.correoUsu)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); //Convierte la llave secreta del appsetting en una llave real
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Algoritmo de encriptación y que llave se usará

                var token = new  JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token); //Genera el token en formato string para que el front lo pueda usar

                return Ok(new
                {
                    mensaje = "Login exitoso",
                    token = tokenString,
                    usuario = new
                    {
                        nombre = credencialesUsuario.nombreUsu,
                        correo = credencialesUsuario.correoUsu
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al procesar la solicitud",
                });
            }
            
        }
    }
}
