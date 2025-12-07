using WalletHub.DTOs;

namespace WalletHub.Services.Interface
{
    public interface IEstadisticasService
    {
        Task<IngresosGastosDTO> ObtenerIngresosVsGastosAsync(
            string idUsuario,
            DateTime inicio,
            DateTime fin);

        Task<List<TotalCategoriaDTO>> ObtenerGastosPorCategoriaAsync(
            string idUsuario,
            DateTime inicio,
            DateTime fin);

        Task<List<TotalCategoriaDTO>> ObtenerIngresosPorCategoriaAsync(
            string idUsuario,
            DateTime inicio,
            DateTime fin);
    }
}
