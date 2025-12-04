using Microsoft.EntityFrameworkCore;                        
namespace WalletHub.Utils
{
    public class IdGenerator
    {
        public static async Task<string> GenerateIdAsync<T>( 
            DbSet<T> dbSet,                                  // Tabla sobre la que se genera el ID
            string prefijo,                                  // Prefijo del ID (US, CA, etc.)
            string idEntidad,                                // Nombre de la propiedad ID
            int padding = 3,                                 // Dígitos numéricos con ceros a la izquierda
            int maxLength = 5) where T : class              
        {
            var lastEntity = await dbSet                     // Consulta la tabla
                .OrderByDescending(e =>                      
                    EF.Property<string>(e, idEntidad))       
                .FirstOrDefaultAsync();                      // Toma el último registro o null

            int siguienteNumId = 1;                         

            if (lastEntity != null)                          
            {
                var propertyInfo = lastEntity                
                    .GetType()
                    .GetProperty(idEntidad);                 

                if (propertyInfo == null)                    
                    throw new Exception(
                        $"La propiedad {idEntidad} no existe en {typeof(T).Name}.");

                string lastId = propertyInfo                 // Obtiene el valor de la propiedad ID
                    .GetValue(lastEntity)?
                    .ToString()
                    ?? throw new Exception("El último ID es nulo."); 

                string numId = lastId.Substring(prefijo.Length); // Parte numérica sin prefijo

                if (!int.TryParse(numId, out var numeroActual))  // Intenta convertir a entero
                    throw new Exception(
                        $"El ID '{lastId}' no es válido para el prefijo {prefijo}.");

                siguienteNumId = numeroActual + 1;               
            }

            string newId = $"{prefijo}{                         // Arma el nuevo ID
                siguienteNumId.ToString()                       // Convierte número a texto
                .PadLeft(padding, '0')}";                       // Rellena con ceros a la izquierda

            if (newId.Length > maxLength)                       // Si supera la longitud máxima
                throw new Exception(
                    $"No se pueden generar más IDs para {prefijo}.");

            return newId;                                       // Devuelve el nuevo ID
        }
    }
}
