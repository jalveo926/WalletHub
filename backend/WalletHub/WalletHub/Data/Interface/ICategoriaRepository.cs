using WalletHub.DTOs;
using WalletHub.Models;
namespace WalletHub.Data.Interface
{
    public interface ICategoriaRepository
    {
        public Task<string> AddCategoriaAsync(Categoria insertado);
        public Task<bool> DeleteCategoriaAsync(string idCategoria);
        public Task<bool> UpdateCategoriaAsync(CategoriaDTO editado);
        public Task<List<CategoriaDTO>> GetAllCategoriaAsync();

      
       
    }
    }
