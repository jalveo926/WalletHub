using WalletHub.Models;

namespace WalletHub.DTOs
{
    public class CategoriaDTO
    {
        public string nombreCateg { get; set; }
        public TipoCategoria tipoCateg { get; set; }
       
        public string correoUsu { get; set; } // Para identificar al usuario propietario de la categoría
        public string idCategoria { get; set; } // Para actualizaciones
    }

   
}
