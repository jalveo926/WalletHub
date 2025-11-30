using WalletHub.Data.Interface;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services.Interface;

public class CalculosService : ICalculosService
{
    private readonly ITransaccionRepository _transaccionRepository;

    public CalculosService(ITransaccionRepository transaccionRepository)
    {
        _transaccionRepository = transaccionRepository;
    }

    public async Task<CalculosDTO> ObtenerResumenAsync(
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

        var gastosPorCategoria = transacciones
            .Where(t => t.Categoria.tipoCateg == TipoCategoria.Gasto)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();

        var ingresosPorCategoria = transacciones
            .Where(t => t.Categoria.tipoCateg == TipoCategoria.Ingreso)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();

        return new CalculosDTO
        {
            TotalIngresos = totalIngresos,
            TotalGastos = totalGastos,
            GastosPorCategoria = gastosPorCategoria,
            IngresosPorCategoria = ingresosPorCategoria
        };
    }
}
