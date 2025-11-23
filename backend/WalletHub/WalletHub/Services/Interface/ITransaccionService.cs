using WalletHub.DTOs;
using WalletHub.Models;

namespace WalletHub.Services.Interface
{
    public interface ITransaccionService
    {
        public Task<IEnumerable<TransaccionDTO>> FiltrarCategoriaAsync(string categoria);
    }
}
