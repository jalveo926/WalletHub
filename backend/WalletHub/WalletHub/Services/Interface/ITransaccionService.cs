using WalletHub.DTOs;
using WalletHub.Models;

namespace WalletHub.Services.Interface
{
    public interface ITransaccionService
    {
        public Task<IEnumerable<TransaccionDTO>> FiltrarCategoriaAsync(string categoria);
        public Task<IEnumerable<TransaccionDTO>> ObtenerTodasTransaccionesAsync();
        public Task<TransaccionDTO> RegistrarTransaccion(RegistroTransaccionDTO dto, string idUsuario);

        public Task<bool> EliminarTransaccionAsync(string idTransaccion);
        public Task<bool> ActualizarTransaccionAsync(ActualizarTransaccionDTO editado, string idUsuario);
    }
}
