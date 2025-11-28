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

        public async Task<bool> ActualizarCategoriaAsync(string idCategoria,CategoriaDTO editado)
        {
            return await _categoriaRepository.UpdateCategoriaAsync(idCategoria,editado);
        }

        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(string idCategoria)
        {
            return await _categoriaRepository.GetCategoriaByID(idCategoria);
        }
        public async Task<List<Categoria>> ObtenerTodasCategoriasAsync()
        {
            return await _categoriaRepository.GetAllCategoriaAsync();
        }
      
    }
}
