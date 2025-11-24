using WalletHub.Data.Interface;
using WalletHub.Models;
using Microsoft.EntityFrameworkCore;
using WalletHub.DTOs;
namespace WalletHub.Data.Repository
{
    public class RegistrarUsuRepository : IRegistrarUsuRepository
    {
        //Le da el contexto de la base de datos al repositorio para que se puedan hacer las operaciones
        private readonly ApplicationDbContext _context; // Inyección de dependencia del DbContext

        public RegistrarUsuRepository(ApplicationDbContext context)
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

        public async Task<IEnumerable<TransaccionDTO>> GetAll()
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
    }
}
