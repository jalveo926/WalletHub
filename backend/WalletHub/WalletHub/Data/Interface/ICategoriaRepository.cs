using WalletHub.DTOs;
using WalletHub.Models;
namespace WalletHub.Data.Interface
{
    public interface ICategoriaRepository
    {
        public Task<string> AddCategoriaAsync(Categoria insertado);
        public Task<bool> DeleteCategoriaAsync(string idCategoria);
        public Task<bool> UpdateCategoriaAsync(string idCategoria, CategoriaDTO actualizado);

        public Task<Categoria?> GetCategoriaByID(string idCategoria);
        public Task<List<Categoria>> GetAllCategoriaAsync();

      
       
    }
    }
