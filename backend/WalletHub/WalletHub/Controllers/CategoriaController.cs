using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services.Interface;
namespace WalletHub.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet("TodasLasCategorias")]
        public async Task<IActionResult> ObtenerTodasCategorias()
        {
            try
            {
                var categorias = await _categoriaService.ObtenerTodasCategoriasAsync();
                return new OkObjectResult(new
                {
                    mensaje = "Categorías obtenidas exitosamente",
                    categorias = categorias.Select(c => new
                    {
                        c.idCategoria,
                        c.nombreCateg,
                        c.tipoCateg
                    })
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al obtener las categorías"
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpPut("ActualizarCategoria/{idCategoria}")]
        public async Task<IActionResult> ActualizarCategoria(string idCategoria,[FromBody] CategoriaDTO categoriaDto)
        {
            if (categoriaDto == null || string.IsNullOrEmpty(categoriaDto.nombreCateg))
            {
                return new BadRequestObjectResult(new
                {
                    mensaje = "Los datos de la categoría y su id no pueden estar vacíos."
                });
            }
            try
            {
                var actualizado = await _categoriaService.ActualizarCategoriaAsync(idCategoria,categoriaDto);
                if (!actualizado)
                {
                    return new NotFoundObjectResult(new
                    {
                        mensaje = "La categoría no fue encontrada para actualizar."
                    });
                }

                return new OkObjectResult(new
                {
                    mensaje = "Categoría actualizada exitosamente",
                    actualizado = true
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new
                {
                    mensaje = "Ocurrió un error al actualizar la categoría"
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpPost("AgregarCategoria")]
        public async Task<IActionResult> AgregarCategoriaAsync([FromBody] CategoriaDTO dto)
        {
            if (dto == null)
                return BadRequest(new { mensaje = "Los datos enviados están vacíos." });

            try
            {
                string idUsuario = User.Claims.First(c => c.Type == "idUsuario").Value;

                string nuevoId = await _categoriaService.AgregarCategoriaAsync(dto, idUsuario);

                return Ok(new
                {
                    mensaje = "Categoría agregada exitosamente",
                    idCategoria = nuevoId
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al agregar la categoría" });
            }

        }

        [HttpDelete("EliminarCategoria/{idCategoria}")]
        public async Task<IActionResult> EliminarCategoria(string idCategoria)
        {
            try
            {
                var idUsuarioToken = User.Claims.First(c => c.Type == "idUsuario").Value;

                // Buscar categoría en BD
                var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(idCategoria);

                if (categoria == null)
                {
                    return NotFound(new
                    {
                        mensaje = "La categoría no existe."
                    });
                }

                // Verificar que el usuario dueño coincide con el del JWT
                if (categoria.idUsuario != idUsuarioToken)
                {
                    return StatusCode(403, new
                    { //403 Forbidden no tiene permisos
                        mensaje = "No tienes permiso para eliminar esta categoría." });

                } 

                // Si coincide, sí puede eliminarla
                await _categoriaService.EliminarCategoriaAsync(idCategoria);

                return Ok(new
                {
                    mensaje = "Categoría eliminada exitosamente",
                    eliminado = true
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al eliminar la categoría"
                });
            }
        }

        [HttpGet("CategoriasPorUsuario")]
        public async Task<IActionResult> ObtenerCategoriasPorUsuario()
        {
            try
            {
                var idUsuarioToken = User.Claims.First(c => c.Type == "idUsuario").Value;
                var categorias = await _categoriaService.ObtenerCategoriasPorUsuario(idUsuarioToken);

                if (categorias.IsNullOrEmpty()) {
                    return Ok(new
                    {
                        mensaje = "No has creado ninguna categoria"
                    });
                }

                return Ok(new
                {
                    mensaje = "Categorías obtenidas exitosamente",
                    categorias = categorias.Select(c => new
                    {
                        c.idCategoria,
                        c.nombreCateg,
                        c.tipoCateg
                    })

                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al obtener las categorías del usuario"
                });
            }
        }

        [HttpGet("CategoriasGlobales")]
        public async Task<IActionResult> ObtenerCategoriasGlobales() {
            try
            {
                
                var categorias = await _categoriaService.ObtenerCategoriasGlobales();
                return Ok(new
                {
                    mensaje = "Categorías obtenidas exitosamente",
                    categorias = categorias.Select(c => new
                    {
                        c.idCategoria,
                        c.nombreCateg,
                        c.tipoCateg
                    })
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al obtener las categorías globales"
                });
            }

        }

        
    }
}
