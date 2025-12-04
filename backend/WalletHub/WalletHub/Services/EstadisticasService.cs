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
        if (inicio == default || fin == default)
            throw new ArgumentException("Las fechas 'inicio' y 'fin' deben ser válidas.");

        if (inicio > fin)
            throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha fin.");

        // Obtener transacciones
        var transacciones = await _transaccionRepository.ObtenerPorUsuarioYFechas(idUsuario, inicio, fin);

        if (transacciones == null)
            throw new InvalidOperationException("No se pudieron obtener las transacciones del usuario.");

        if (!transacciones.Any())
        {
            return new IngresosGastosDTO
            {
                TotalIngresos = 0,
                TotalGastos = 0
            };
        }

        var totalIngresos = transacciones
            .Where(t => t.Categoria?.tipoCateg == TipoCategoria.Ingreso)
            .Sum(t => t.montoTransac);

        var totalGastos = transacciones
            .Where(t => t.Categoria?.tipoCateg == TipoCategoria.Gasto)
            .Sum(t => t.montoTransac);

        if (totalIngresos < 0 || totalGastos < 0)
            throw new InvalidOperationException("Los totales no pueden ser negativos.");

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
        if (inicio == default || fin == default)
            throw new ArgumentException("Las fechas 'inicio' y 'fin' deben ser válidas.");

        if (inicio > fin)
            throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha fin.");

        var transacciones = await _transaccionRepository
            .ObtenerPorUsuarioYFechas(idUsuario, inicio, fin);

        if (transacciones == null)
            throw new InvalidOperationException("No se pudieron obtener las transacciones del usuario.");

        if (!transacciones.Any())
            return new List<TotalCategoriaDTO>();

        var resultados = transacciones
            .Where(t => t.Categoria != null && t.Categoria.tipoCateg == TipoCategoria.Gasto)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();

        return resultados;
    }

    public async Task<List<TotalCategoriaDTO>> ObtenerIngresosPorCategoriaAsync(
        string idUsuario,
        DateTime inicio,
        DateTime fin)
    {
        if (inicio == default || fin == default)
            throw new ArgumentException("Las fechas 'inicio' y 'fin' deben ser válidas.");

        if (inicio > fin)
            throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha fin.");

        var transacciones = await _transaccionRepository
            .ObtenerPorUsuarioYFechas(idUsuario, inicio, fin);

        if (transacciones == null)
            throw new InvalidOperationException("No se pudieron obtener las transacciones del usuario.");

        if (!transacciones.Any())
            return new List<TotalCategoriaDTO>();

        var resultados = transacciones
            .Where(t => t.Categoria != null && t.Categoria.tipoCateg == TipoCategoria.Ingreso)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();

        return resultados;
    }
}
