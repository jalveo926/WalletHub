using WalletHub.Models;
using WalletHub.DTOs;
namespace WalletHub.Services.Interface
{
    public interface ICategoriaService
    {
        public Task<string> AgregarCategoriaAsync(Categoria insertado);
        public Task<bool> EliminarCategoriaAsync(string idCategoria);
        public Task<bool> ActualizarCategoriaAsync(string idCategoria, CategoriaDTO actualizado);

        public Task<Categoria?> ObtenerCategoriaPorIdAsync(string idCategoria);
        public Task<List<Categoria>> ObtenerTodasCategoriasAsync();
       
    }
}
