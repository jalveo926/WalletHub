using WalletHub.DTOs;

namespace WalletHub.Services.Interface
{
    public interface ICalculosService
    {
        public Task<CalculosDTO> ObtenerResumenAsync(
            string idUsuario,
            DateTime inicio,
            DateTime fin);
        public Task<IngresosGastosDTO> ObtenerTotalesGeneralesAsync(string idUsuario);
    }
}
