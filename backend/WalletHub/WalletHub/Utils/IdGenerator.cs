using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WalletHub.Utils
{
    public class IdGenerator
    {
        /// <summary>
        /// Este método genera un nuevo ID con un prefijo específico para una entidad en una base de datos utilizando Entity Framework Core.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="prefijo"></param>
        /// <param name="propertyName"></param>
        /// <param name="padding"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<string> GenerateIdAsync<T>( // Método de generacion de IDs con prefijos
        DbSet<T> dbSet,
        string prefijo, // "US", "CA", "TR", "RE"
        string idEntidad, // "idUsuario", "idCategoria", "idTransaccion", "idReporte"
        int padding = 3,
        int maxLength = 5) where T : class
        {
            var lastEntity = await dbSet
                .OrderByDescending(e => EF.Property<string>(e, idEntidad)) // Ordena de mayor a menor para obtener el último ID
                .FirstOrDefaultAsync(); // Toma el primero (el mayor)

            int siguienteNumId = 1; // Valor inicial si no hay entidades previas

            if (lastEntity != null) // si existe una entidad previa
            {
                string lastId = EF.Property<string>(lastEntity, idEntidad); // Obtiene dinámicamente el valor del ID en lugar de especificar "idUsuario" u otro
                string numId = lastId.Substring(prefijo.Length);
                siguienteNumId = int.Parse(numId) + 1;
            }

            string newId = $"{prefijo}{siguienteNumId.ToString().PadLeft(padding, '0')}";

            // 🚨 Validación para evitar overflow del tamaño máximo
            if (newId.Length > maxLength)
                throw new Exception($"No se pueden generar más IDs para el prefijo {prefijo}. Límite alcanzado.");

            return newId;
        }
    }
}
