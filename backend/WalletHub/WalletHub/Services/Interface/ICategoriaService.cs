using WalletHub.Models;
using WalletHub.DTOs;
namespace WalletHub.Services.Interface
{
    public interface ICategoriaService
    {
        public Task<string> AgregarCategoriaAsync(Categoria insertado);
        public Task<bool> EliminarCategoriaAsync(string idCategoria);
        public Task<bool> ActualizarCategoriaAsync(CategoriaDTO editado);
        public Task<List<CategoriaDTO>> ObtenerTodasCategoriasAsync();
       
    }
}
