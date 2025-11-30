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
        public async Task<IEnumerable<TransaccionDTO>> GetByCategoria(string nombreCategoria, string idUsuario)
        {
            var resultado = await _context.Transaccion
                .Include(t => t.Categoria)
                .Where(t => t.idUsuario == idUsuario && t.Categoria.nombreCateg == nombreCategoria)
                .Select(t => new TransaccionDTO()
                {
                    fechaTransac = t.fechaTransac,
                    descripcionTransac = t.descripcionTransac,
                    montoTransac = t.montoTransac,
                    nombreCateg = t.Categoria.nombreCateg,
                    tipoCategoria = t.Categoria.tipoCateg.ToString()
                })
                .ToListAsync();

            return resultado;
        }

        public async Task<IEnumerable<TransaccionDTO>> GetTransaccionesPorUsuarioAsync(string idUsuario)
        {
            if (string.IsNullOrWhiteSpace(idUsuario))
                throw new ArgumentException("El idUsuario no puede estar vacío.");

            var resultado = await _context.Transaccion
                .Include(t => t.Categoria)
                .Where(t => t.idUsuario == idUsuario)
                .OrderByDescending(t => t.fechaTransac)
                .Select(t => new TransaccionDTO()
                {
                    fechaTransac = t.fechaTransac,
                    montoTransac = t.montoTransac,
                    descripcionTransac = t.descripcionTransac,
                    nombreCateg = t.Categoria.nombreCateg,
                    tipoCategoria = t.Categoria.tipoCateg.ToString()
                })
                .ToListAsync();

            return resultado;
        }

        public async Task<TransaccionDTO> AddTransaccionAsync(RegistroTransaccionDTO dto, string idUsuario)
        {
            // 1. Buscar categoría del usuario o global
            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(c =>
                    c.nombreCateg.ToLower() == dto.nombreCateg.ToLower() &&
                    (c.idUsuario == idUsuario || c.idUsuario == null)
                );

            if (categoria == null)
                throw new ArgumentException($"La categoría '{dto.nombreCateg}' no existe para este usuario.");

            // 2. Generar ID
            string nuevoId = await IdGenerator.GenerateIdAsync(
                _context.Transaccion,
                "TR",
                "idTransaccion"
            );

            // 3. Crear fecha con hora
            var fechaConHora = dto.fechaTransac.ToDateTime(TimeOnly.MinValue);

            // 4. Crear transacción
            var transaccion = new Transaccion
            {
                idTransaccion = nuevoId,
                fechaTransac = fechaConHora,
                montoTransac = dto.montoTransac,
                descripcionTransac = dto.descripcionTransac,
                idUsuario = idUsuario,
                idCategoria = categoria.idCategoria
            };

            _context.Transaccion.Add(transaccion);
            await _context.SaveChangesAsync();

            // 5. Respuesta DTO
            return new TransaccionDTO()
            {
                fechaTransac = transaccion.fechaTransac,
                montoTransac = transaccion.montoTransac,
                descripcionTransac = transaccion.descripcionTransac,
                nombreCateg = categoria.nombreCateg,
                tipoCategoria = categoria.tipoCateg.ToString()
            };
        }


        public async Task<bool> DeleteTransaccionAsync(string idTransaccion, string idUsuario)
        {
            var transaccion = await _context.Transaccion
                .FirstOrDefaultAsync(t => t.idTransaccion == idTransaccion && t.idUsuario == idUsuario);

            if (transaccion == null)
                return false; 

            _context.Transaccion.Remove(transaccion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTransaccionAsync(string idTransaccion, ActualizarTransaccionDTO actualizado, string idUsuario)
        {
            //Verifica si la transacción existe
            var transaccionExistente = await _context.Transaccion
                .FirstOrDefaultAsync(t => t.idTransaccion == idTransaccion && t.idUsuario == idUsuario);

            if (transaccionExistente == null)
            {
                return false;
            }
            
            //Cambia los valores de los campos editados por el usuario
            if (actualizado.montoTransac.HasValue)
            {
                transaccionExistente.montoTransac = actualizado.montoTransac.Value;
            }

            if (!string.IsNullOrWhiteSpace(actualizado.descripcionTransac))
            {
                transaccionExistente.descripcionTransac = actualizado.descripcionTransac;
            }

            if (actualizado.fechaTransac.HasValue)
            {
                transaccionExistente.fechaTransac = actualizado.fechaTransac.Value.ToDateTime(TimeOnly.MinValue);
            }

            //Verifica si la categoria existe
            if (!string.IsNullOrEmpty(actualizado.nombreCateg))
            {
                var categoria = await _context.Categoria
                    .FirstOrDefaultAsync(c => c.nombreCateg == actualizado.nombreCateg && (c.idUsuario == idUsuario || c.idUsuario == null));

                if (categoria == null)
                    throw new ArgumentException($"La categoría '{actualizado.nombreCateg}' no existe para este usuario.");

                transaccionExistente.idCategoria = categoria.idCategoria;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<TransaccionDTO>> GetTransaccionesByIngresoGasto( TipoCategoria tipoCateg,string idUsuario)
        {
            var resultado = await _context.Transaccion
                .Include(t => t.Categoria)
                .Where(t => t.idUsuario == idUsuario
                         && t.Categoria.tipoCateg == tipoCateg)
                .Select(t => new TransaccionDTO()
                {
                    fechaTransac = t.fechaTransac,
                    descripcionTransac = t.descripcionTransac,
                    montoTransac = t.montoTransac,
                    nombreCateg = t.Categoria.nombreCateg,
                    tipoCategoria = t.Categoria.tipoCateg.ToString()
                })
                .ToListAsync();

            return resultado;
        }
    }
}
