using Microsoft.AspNetCore.Mvc;
using WalletHub.Services.Interface;
using WalletHub.DTOs;
namespace WalletHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObtenerTodasCategorias()
        {
            try
            {
                var categorias = await _categoriaService.ObtenerTodasCategoriasAsync();
                return new OkObjectResult(new
                {
                    mensaje = "Categorías obtenidas exitosamente",
                    categorias
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

        [HttpPut("ActualizarCategoria")]
        public async Task<IActionResult> ActualizarCategoria([FromBody] CategoriaDTO categoriaDto)
        {
            if (categoriaDto == null || string.IsNullOrEmpty(categoriaDto.nombreCateg) || string.IsNullOrEmpty(categoriaDto.idCategoria))
            {
                return new BadRequestObjectResult(new
                {
                    mensaje = "Los datos de la categoría y su id no pueden estar vacíos."
                });
            }
            try
            {
                var actualizado = await _categoriaService.ActualizarCategoriaAsync(categoriaDto);
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
    }
}
