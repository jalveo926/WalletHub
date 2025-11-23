namespace WalletHub.Models
{
    public class Categoria
    {
        public string idCategoria { get; set; } = string.Empty;
        public string nombreCateg { get; set; } = string.Empty;
        public TipoCategoria tipoCateg { get; set; }

        // FK Usuario
        public string? idUsuario { get; set; } // Puede ser null si es una categoría global
        public Usuario Usuario { get; set; }

        // Relación 1 a muchas Transacciones
        public ICollection<Transaccion> Transacciones { get; set; }
    }

    /// <summary>
    /// Representa los tipos de categoria que puede tener.
    /// Usamos un enum para limitar los valores posibles (Ingreso, Gasto)
    /// Esto ayuda a evitar errores de tipo, facilita la validación y 
    /// EF Core puede convertirlo a string al guardarlo en la base de datos.
    /// </summary>
    public enum TipoCategoria
    {
        Ingreso,
        Gasto
    }
}
