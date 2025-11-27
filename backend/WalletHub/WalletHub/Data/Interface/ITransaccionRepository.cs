using WalletHub.DTOs;
using WalletHub.Models;

namespace WalletHub.Data.Interface
{
    //Esta interfaz es la que me va a permitir definir los métodos que voy a implementar en el repositorio de transacciones
    public interface ITransaccionRepository
        {
            public Task<IEnumerable<TransaccionDTO>> GetByCategoria(string categoria);
            public Task<IEnumerable<TransaccionDTO>> GetAll();
            public Task<TransaccionDTO> AddTransaccionAsync(RegistroTransaccionDTO dto, string idUsuario);
    }
    }
