using WalletHub.Models;
using WalletHub.DTOs;
namespace WalletHub.Services.Interface
{
    public interface ICategoriaService
    {
        public Task<string> AgregarCategoriaAsync(CategoriaDTO dto, string idUsuario);
        public Task<bool> EliminarCategoriaAsync(string idCategoria);
        public Task<bool> ActualizarCategoriaAsync(string idCategoria, CategoriaDTO actualizado);

        public Task<Categoria?> ObtenerCategoriaPorIdAsync(string idCategoria);

        public Task<List<Categoria?>> ObtenerCategoriasPorUsuario(string idCategoria);

        public Task<List<Categoria>> ObtenerCategoriasGlobales();
        public Task<List<Categoria>> ObtenerTodasCategoriasAsync();
       
    }
}
