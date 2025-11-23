using Microsoft.EntityFrameworkCore;
using WalletHub.Data.Interface;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Services.Interface;
namespace WalletHub.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _transaccionRepository;
        public TransaccionService(ITransaccionRepository transaccionRepository)
        {
            _transaccionRepository = transaccionRepository;
        }
        // Aquí irían los métodos para manejar la lógica de negocio relacionada con las transacciones

        public async Task<IEnumerable<TransaccionDTO>> FiltrarCategoriaAsync(string categoria)
        {
            try
            {
                var transaccionesFiltradas = await _transaccionRepository.GetByCategoria(categoria);
                return transaccionesFiltradas;
            }
            catch (ArgumentNullException ex)
            {
                // Manejo de errores, logging, etc.
                throw new Exception("Error al filtrar transacciones por categoría.", ex);

            }
            catch (Exception ex)
            {

                throw new Exception("Error al filtrar transacciones por categoría.", ex);
            }
        }
    }
}
