using WalletHub.Data.Interface;
using WalletHub.Services.Interface;
using WalletHub.Models;
using WalletHub.DTOs;

namespace WalletHub.Services
{
    public class CategoriaService : ICategoriaService
    {   
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriaService(ICategoriaRepository categoriaRepository) {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<string> AgregarCategoriaAsync(Categoria insertado)
        {
            return await _categoriaRepository.AddCategoriaAsync(insertado);
        }
        public async Task<bool> EliminarCategoriaAsync(string idCategoria)
        {
            return await _categoriaRepository.DeleteCategoriaAsync(idCategoria);
        }

        public async Task<bool> ActualizarCategoriaAsync(CategoriaDTO editado)
        {
            return await _categoriaRepository.UpdateCategoriaAsync(editado);
        }

        public async Task<List<CategoriaDTO>> ObtenerTodasCategoriasAsync()
        {
            return await _categoriaRepository.GetAllCategoriaAsync();
        }
      
    }
}
