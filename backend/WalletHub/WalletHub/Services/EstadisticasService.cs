using WalletHub.Data.Interface;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services.Interface;

public class EstadisticasService : IEstadisticasService
{
    private readonly ITransaccionRepository _transaccionRepository;

    public EstadisticasService(ITransaccionRepository transaccionRepository)
    {
        _transaccionRepository = transaccionRepository;
    }

    public async Task<IngresosGastosDTO> ObtenerIngresosVsGastosAsync(
        string idUsuario,
        DateTime inicio,
        DateTime fin)
    {
        var transacciones = await _transaccionRepository
            .ObtenerPorUsuarioYFechas(idUsuario, inicio, fin);

        var totalIngresos = transacciones
            .Where(t => t.Categoria.tipoCateg == TipoCategoria.Ingreso)
            .Sum(t => t.montoTransac);

        var totalGastos = transacciones
            .Where(t => t.Categoria.tipoCateg == TipoCategoria.Gasto)
            .Sum(t => t.montoTransac);

        return new IngresosGastosDTO
        {
            TotalIngresos = totalIngresos,
            TotalGastos = totalGastos
        };
    }

    public async Task<List<TotalCategoriaDTO>> ObtenerGastosPorCategoriaAsync(
        string idUsuario,
        DateTime inicio,
        DateTime fin)
    {
        var transacciones = await _transaccionRepository
            .ObtenerPorUsuarioYFechas(idUsuario, inicio, fin);

        return transacciones
            .Where(t => t.Categoria.tipoCateg == TipoCategoria.Gasto)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();
    }

    public async Task<List<TotalCategoriaDTO>> ObtenerIngresosPorCategoriaAsync(
        string idUsuario,
        DateTime inicio,
        DateTime fin)
    {
        var transacciones = await _transaccionRepository
            .ObtenerPorUsuarioYFechas(idUsuario, inicio, fin);

        return transacciones
            .Where(t => t.Categoria.tipoCateg == TipoCategoria.Ingreso)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();
    }
}
