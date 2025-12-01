using WalletHub.DTOs;

namespace WalletHub.Services.Interface
{
    public interface ICalculosService
    {
        Task<CalculosDTO> ObtenerResumenAsync(
            string idUsuario,
            DateTime inicio,
            DateTime fin);
    }
}
