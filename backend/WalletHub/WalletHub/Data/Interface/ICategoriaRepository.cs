using WalletHub.DTOs;
using WalletHub.Models;
namespace WalletHub.Data.Interface
{
    public interface ICategoriaRepository
    {
        public Task<string> AddCategoriaAsync(CategoriaDTO dto, string idUsuario);
        public Task<bool> DeleteCategoriaAsync(string idCategoria);
        public Task<bool> UpdateCategoriaAsync(string idCategoria, CategoriaDTO actualizado);

        public Task<Categoria?> GetCategoriaByID(string idCategoria); // Si entregas una categoria que no existe retorna null
        
        public Task<List<Categoria?>> GetCategoriasByUsuario(string idUsuario);
        public Task<List<Categoria>> GetCategoriasGlobales();  //Las que tienen idUsuario null

        public Task<List<Categoria>> GetAllCategoriaAsync();

      
       
    }
    }
