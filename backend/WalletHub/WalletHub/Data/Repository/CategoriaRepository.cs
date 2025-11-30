using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WalletHub.Data;
using WalletHub.Data.Interface;
using WalletHub.DTOs;
using WalletHub.Models;
using WalletHub.Utils;
namespace WalletHub.Data.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        //Le da el contexto de la base de datos al repositorio para que se puedan hacer las operaciones
        private readonly ApplicationDbContext _context;
      

        public CategoriaRepository(ApplicationDbContext context){
            _context = context;
          
        }
        public async Task<string> AddCategoriaAsync(CategoriaDTO dto, string idUsuario)
        {
            // 1. Generar ID
            string nuevoId = await IdGenerator.GenerateIdAsync(
                _context.Categoria,
                "CA",
                "idCategoria"
            );

            // 2. Armar la entidad completa
            var categoria = new Categoria
            {
                idCategoria = nuevoId,
                idUsuario = idUsuario,
                nombreCateg = dto.nombreCateg,
                tipoCateg = dto.tipoCateg
            };

            // 3. Persistir
            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();

            return nuevoId;
        }


        public async Task<bool> DeleteCategoriaAsync(string idCategoria)
        {
            var categoria = await _context.Categoria.FindAsync(idCategoria);
            if (categoria == null)
            {
                return false;
            }
            _context.Categoria.Remove(categoria);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Categoria>> GetAllCategoriaAsync()
        {
            return await _context.Categoria
                .Include(c => c.Usuario)
                .Select(c => new Categoria
                {   
                    idCategoria = c.idCategoria,    
                    nombreCateg = c.nombreCateg,
                    tipoCateg = c.tipoCateg,
                })
                .ToListAsync();
        }



        public async Task<bool> UpdateCategoriaAsync(string idCategoria,CategoriaDTO actualizado)
        {
            var categoriaExistente = await _context.Categoria
                .FirstOrDefaultAsync(c => c.idCategoria == idCategoria);

            if (categoriaExistente == null)
            {
                return false;
            }

            categoriaExistente.nombreCateg = actualizado.nombreCateg;
            categoriaExistente.tipoCateg = actualizado.tipoCateg;

            _context.Categoria.Update(categoriaExistente);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Categoria?> GetCategoriaByID(string idCategoria)
        {
            return await _context.Categoria
                .FirstOrDefaultAsync(c => c.idCategoria == idCategoria);
        }

        public async Task<List<Categoria?>> GetCategoriasByUsuario(string idUsuario)
        {
            return await _context.Categoria
                .Where(c => c.idUsuario == idUsuario)
                .Select(c => new Categoria
                {
                    idCategoria = c.idCategoria,
                    nombreCateg = c.nombreCateg,
                    tipoCateg = c.tipoCateg,
                })
                .ToListAsync();
        }

        public async Task<List<Categoria>> GetCategoriasGlobales()
        {
            return await _context.Categoria
                .Where(c => c.idUsuario == null)
                .Select(c => new Categoria
                {
                    idCategoria = c.idCategoria,
                    nombreCateg = c.nombreCateg,
                    tipoCateg = c.tipoCateg,
                })
                .ToListAsync();
        }

    }
}
