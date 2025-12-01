using System.Collections.Generic;

namespace WalletHub.DTOs
{
    public class CalculosDTO
    {
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Diferencia => TotalIngresos - TotalGastos;

        public List<TotalCategoriaDTO> GastosPorCategoria { get; set; } = new();
        public List<TotalCategoriaDTO> IngresosPorCategoria { get; set; } = new();
    }
}
