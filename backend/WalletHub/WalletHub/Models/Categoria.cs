namespace WalletHub.Models
{
    public class Categoria
    {
        public string idCategoria { get; set; } = string.Empty;
        public string nombreCateg { get; set; } = string.Empty;
        public TipoCategoria tipoCateg { get; set; }
        public string idUsuario { get; set; } //FK, no usamos string.Empty para admitir categorias creadas por el sistema (idUsuario = null)
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
