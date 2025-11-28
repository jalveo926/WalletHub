using WalletHub.Data.Interface;
using WalletHub.Services.Interface;
using WalletHub.Models;
using WalletHub.DTOs;

namespace WalletHub.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<string> AgregarCategoriaAsync(Categoria insertado)
        {
            if (insertado == null)
                throw new ArgumentNullException(nameof(insertado), "La categoría no puede ser nula.");

            if (string.IsNullOrWhiteSpace(insertado.nombreCateg))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            return await _categoriaRepository.AddCategoriaAsync(insertado);
        }

        public async Task<bool> EliminarCategoriaAsync(string idCategoria)
        {
            if (string.IsNullOrWhiteSpace(idCategoria))
                throw new ArgumentException("El ID de categoría no puede estar vacío.");

            var existe = await _categoriaRepository.GetCategoriaByID(idCategoria);
            if (existe == null)
                throw new KeyNotFoundException("La categoría no existe.");

            return await _categoriaRepository.DeleteCategoriaAsync(idCategoria);
        }

        public async Task<bool> ActualizarCategoriaAsync(string idCategoria, CategoriaDTO editado)
        {
            if (string.IsNullOrWhiteSpace(idCategoria))
                throw new ArgumentException("El ID de categoría no puede estar vacío.");

            if (editado == null)
                throw new ArgumentNullException(nameof(editado), "Los datos enviados están vacíos.");

            if (string.IsNullOrWhiteSpace(editado.nombreCateg))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            var existe = await _categoriaRepository.GetCategoriaByID(idCategoria);
            if (existe == null)
                throw new KeyNotFoundException("La categoría no existe para ser actualizada.");

            return await _categoriaRepository.UpdateCategoriaAsync(idCategoria, editado);
        }

        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(string idCategoria)
        {
            if (string.IsNullOrWhiteSpace(idCategoria))
                throw new ArgumentException("El ID de categoría no puede estar vacío.");

            return await _categoriaRepository.GetCategoriaByID(idCategoria)
                   ?? throw new KeyNotFoundException("La categoría solicitada no existe.");
        }

        public async Task<List<Categoria>> ObtenerTodasCategoriasAsync()
        {
            var lista = await _categoriaRepository.GetAllCategoriaAsync();

            if (lista == null)
                throw new InvalidOperationException("No se pudieron obtener las categorías.");

            return lista;
        }

        public async Task<List<Categoria>> ObtenerCategoriasPorUsuario(string idUsuario)
        {
            if (string.IsNullOrWhiteSpace(idUsuario))
                throw new ArgumentException("El ID del usuario no puede estar vacío.");

            var lista = await _categoriaRepository.GetCategoriasByUsuario(idUsuario);

            return lista ?? new List<Categoria>();
        }

        public async Task<List<Categoria>> ObtenerCategoriasGlobales()
        {
            var lista = await _categoriaRepository.GetCategoriasGlobales();

            return lista ?? new List<Categoria>(); //Si la lista es null devuelve una lista vacía y si no es null devuelve la lista
        }
    }
}
