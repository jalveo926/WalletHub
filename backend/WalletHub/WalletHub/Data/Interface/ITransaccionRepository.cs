using WalletHub.DTOs;
using WalletHub.Models;

namespace WalletHub.Data.Interface
{
    //Esta interfaz es la que me va a permitir definir los métodos que voy a implementar en el repositorio de transacciones
    public interface ITransaccionRepository
        {
            public Task<IEnumerable<TransaccionDTO>> GetByCategoria(string categoria,string idUsuario);
            public Task<IEnumerable<TransaccionDTO>> GetTransaccionesPorUsuarioAsync(string idUsuario);
            public Task<TransaccionDTO> AddTransaccionAsync(RegistroTransaccionDTO dto, string idUsuario);
            public Task<bool> DeleteTransaccionAsync(string idTransaccion, string idUsuario);
            public Task<bool> UpdateTransaccionAsync(string idTransaccion, ActualizarTransaccionDTO editado, string idUsuario);
            public Task<List<Transaccion>> ObtenerPorUsuarioYFechas(string idUsuario, DateTime inicio, DateTime fin);
    }
    
}
