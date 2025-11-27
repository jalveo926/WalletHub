using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WalletHub.Data;
using WalletHub.Data.Interface;
using WalletHub.Models;
using WalletHub.DTOs;
namespace WalletHub.Data.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        //Le da el contexto de la base de datos al repositorio para que se puedan hacer las operaciones
        private readonly ApplicationDbContext _context;
      

        public CategoriaRepository(ApplicationDbContext context){
            _context = context;
          
        }
        public async Task<string> AddCategoriaAsync(Categoria insertado)
        {
            _context.Categoria.Add(insertado);
            await _context.SaveChangesAsync();
            return insertado.idCategoria; //ID generado
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

        public async Task<List<CategoriaDTO>> GetAllCategoriaAsync()
        {
            return await _context.Categoria
                .Include(c => c.Usuario)
                .Select(c => new CategoriaDTO
                {
                    idCategoria = c.idCategoria,
                    nombreCateg = c.nombreCateg,
                    tipoCateg = c.tipoCateg,
                    correoUsu = c.Usuario != null ? c.Usuario.correoUsu : string.Empty
                })
                .ToListAsync();
        }



        public async Task<bool> UpdateCategoriaAsync(CategoriaDTO actualizado)
        {
            var categoriaExistente = await _context.Categoria
                .FirstOrDefaultAsync(c => c.idCategoria == actualizado.idCategoria);

            if (categoriaExistente == null)
            {
                return false;
            }

            categoriaExistente.nombreCateg = actualizado.nombreCateg;
            categoriaExistente.tipoCateg = actualizado.tipoCateg;

            _context.Categoria.Update(categoriaExistente);
            return await _context.SaveChangesAsync() > 0;
        }



    }
}
