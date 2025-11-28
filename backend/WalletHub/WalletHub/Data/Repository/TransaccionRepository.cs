using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletHub.Data.Interface;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Utils;
namespace WalletHub.Data.Repository
{
    public class TransaccionRepository : ITransaccionRepository
    {
        //Le da el contexto de la base de datos al repositorio para que se puedan hacer las operaciones
        private readonly ApplicationDbContext _context; // Inyección de dependencia del DbContext

        public TransaccionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //Devuelve una lista de transacciones de la base de datos con el filtrado por categoría
        public async Task<IEnumerable<TransaccionDTO>> GetByCategoria(string nombreCategoria)
        {
            var resultado = await _context.Transaccion
                .Include(t => t.Categoria)
                .Where(t => t.Categoria.nombreCateg == nombreCategoria)
                .Select(t => new TransaccionDTO(nombreCategoria)
                {
                    fechaTransac = t.fechaTransac,
                    descripcionTransac = t.descripcionTransac,
                    montoTransac = t.montoTransac,
                    nombreCateg = t.Categoria.nombreCateg
                })
                .ToListAsync();

            return resultado;
        }

        public async Task<IEnumerable<TransaccionDTO>> GetAllTransaccionAsync()
        {
            var resultado = await _context.Transaccion
                .Include(t => t.Categoria)
                .Select(t => new TransaccionDTO(t.Categoria.nombreCateg)
                {
                    fechaTransac = t.fechaTransac,
                    descripcionTransac = t.descripcionTransac,
                    montoTransac = t.montoTransac,
                    nombreCateg = t.Categoria.nombreCateg
                })
                .ToListAsync();

            return resultado;
        }

        public async Task<TransaccionDTO> AddTransaccionAsync(RegistroTransaccionDTO dto, string idUsuario)
        {
            // 1. Buscar categoría del usuario por nombre
            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(c => c.nombreCateg == dto.nombreCateg
                                       && (c.idUsuario == idUsuario || string.IsNullOrEmpty(c.idUsuario))); // Permite categorias creadas por el usuario o globales del sistema (null)

            if (categoria == null)
                throw new ArgumentException($"La categoría '{dto.nombreCateg}' no existe para este usuario.");

            // 2. Generar ID de la transacción
            string nuevoId = await IdGenerator.GenerateIdAsync(
                _context.Transaccion,
                prefijo: "TR",
                idEntidad: "idTransaccion"
            );

            // 3. Crear transacción
            var transaccion = new Transaccion
            {
                idTransaccion = nuevoId,
                fechaTransac = DateTime.Now,
                montoTransac = dto.montoTransac,
                descripcionTransac = dto.descripcionTransac,
                idUsuario = idUsuario,
                idCategoria = categoria.idCategoria
            };

            _context.Transaccion.Add(transaccion);
            await _context.SaveChangesAsync();

            // 4. Devolver DTO
            return new TransaccionDTO(categoria.nombreCateg)
            {
                fechaTransac = transaccion.fechaTransac,
                montoTransac = transaccion.montoTransac,
                descripcionTransac = transaccion.descripcionTransac,
                nombreCateg = categoria.nombreCateg
            };
        }

        public async Task<bool> DeleteTransaccionAsync(string idTransaccion)
        {
            var transaccion = await _context.Transaccion.FindAsync(idTransaccion);

            if (transaccion == null)
                return false; 

            _context.Transaccion.Remove(transaccion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
