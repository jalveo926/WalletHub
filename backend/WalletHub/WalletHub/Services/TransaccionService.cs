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
                throw new Exception("No hay registros por categoría que mostrar.", ex);

            }
            catch (Exception ex)
            {

                throw new Exception("Error al filtrar transacciones por categoría.", ex);
            }
        }

        public async Task<IEnumerable<TransaccionDTO>> ObtenerTodasTransaccionesAsync()
        {
            try
            {
                var todasTransacciones = await _transaccionRepository.GetAll();
                return todasTransacciones;
            }
            catch (ArgumentNullException ex) {
                throw new Exception("No hay registros que mostrar.", ex);
            }
            catch (Exception ex)
            {
                // Manejo de errores, logging, etc.
                throw new Exception("Error al obtener todas las transacciones.", ex);
            }
        }

        public async Task<TransaccionDTO> RegistrarTransaccion(RegistroTransaccionDTO dto, string idUsuario)
        {
            // VALIDACIONES PREVIAS
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos de la transacción son obligatorios.");

            if (dto.montoTransac <= 0)
                throw new ArgumentException("El monto debe ser mayor que 0.", nameof(dto.montoTransac));

            if (string.IsNullOrWhiteSpace(dto.descripcionTransac))
                throw new ArgumentException("La descripción es obligatoria.", nameof(dto.descripcionTransac));

            if (string.IsNullOrWhiteSpace(dto.nombreCateg))
                throw new ArgumentException("La categoría es obligatoria.", nameof(dto.nombreCateg));

            if (string.IsNullOrWhiteSpace(idUsuario))
                throw new ArgumentException("El ID del usuario es obligatorio.", nameof(idUsuario));
            try
            {
                var nuevaTransaccion = await _transaccionRepository.AddTransaccionAsync(dto, idUsuario);
                return nuevaTransaccion;
            }
            catch (Exception ex)
            {
                // Manejo de errores, logging, etc.
                throw new Exception("Error al registrar la transacción.", ex);
            }
        }
    }
}
