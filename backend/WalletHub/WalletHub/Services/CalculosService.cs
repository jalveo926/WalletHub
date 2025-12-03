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
        // Validación: fechas incorrectas
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
            return new CalculosDTO
            {
                TotalIngresos = 0,
                TotalGastos = 0,
                GastosPorCategoria = new List<TotalCategoriaDTO>(),
                IngresosPorCategoria = new List<TotalCategoriaDTO>()
            };
        }

        // Cálculo total de ingresos
        var totalIngresos = transacciones
            .Where(t => t.Categoria?.tipoCateg == TipoCategoria.Ingreso)
            .Sum(t => t.montoTransac);

        // Cálculo total de gastos
        var totalGastos = transacciones
            .Where(t => t.Categoria?.tipoCateg == TipoCategoria.Gasto)
            .Sum(t => t.montoTransac);

        // Validación defensiva de categoría nula
        var gastosPorCategoria = transacciones
            .Where(t => t.Categoria != null && t.Categoria.tipoCateg == TipoCategoria.Gasto)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();

        var ingresosPorCategoria = transacciones
            .Where(t => t.Categoria != null && t.Categoria.tipoCateg == TipoCategoria.Ingreso)
            .GroupBy(t => new { t.idCategoria, t.Categoria.nombreCateg })
            .Select(g => new TotalCategoriaDTO
            {
                IdCategoria = g.Key.idCategoria,
                NombreCategoria = g.Key.nombreCateg,
                Total = g.Sum(x => x.montoTransac)
            })
            .ToList();

        // Validación de coherencia de resultados
        if (totalIngresos < 0 || totalGastos < 0)
            throw new InvalidOperationException("Los totales no pueden ser negativos.");

        return new CalculosDTO
        {
            TotalIngresos = totalIngresos,
            TotalGastos = totalGastos,
            GastosPorCategoria = gastosPorCategoria,
            IngresosPorCategoria = ingresosPorCategoria
        };
    }

    public async Task<IngresosGastosDTO> ObtenerTotalesGeneralesAsync(string idUsuario) // servicio para obtener totales generales independientemente del periodo
    {
        if (string.IsNullOrWhiteSpace(idUsuario))
            throw new ArgumentException("El idUsuario no puede estar vacío.");

        var transacciones = await _transaccionRepository.GetTransaccionesPorUsuarioAsync(idUsuario);

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
            .Where(t => t.tipoCategoria == TipoCategoria.Ingreso.ToString())
            .Sum(t => t.montoTransac);

        var totalGastos = transacciones
            .Where(t => t.tipoCategoria == TipoCategoria.Gasto.ToString())
            .Sum(t => t.montoTransac);

        // Defensa extra: nunca deben ser negativos
        if (totalIngresos < 0 || totalGastos < 0)
            throw new InvalidOperationException("Los totales calculados no pueden ser negativos.");

        return new IngresosGastosDTO
        {
            TotalIngresos = totalIngresos,
            TotalGastos = totalGastos
        };
    }
}
