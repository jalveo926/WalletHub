namespace WalletHub.Models
{
    public class Reporte
    {
        public int idReporte { get; set; }
        public string rutaArchivoRepo { get; set; } = string.Empty; //Se inicializa como cadena vacía para evitar null
        public TipoArchivo tipoArchivoRepo { get; set; }
        public DateTime fechaCreacionRepo { get; set; }
        public DateTime periodoRepo { get; set; }
    }

    /// <summary>
    /// Representa los tipos de archivo que un reporte puede tener.
    /// Usamos un enum para limitar los valores posibles (PDF, Excel, etc.)
    /// Esto ayuda a evitar errores de tipo, facilita la validación y 
    /// EF Core puede convertirlo a string al guardarlo en la base de datos.
    /// </summary>
    public enum TipoArchivo
    {
        Pdf,
        Excel
    }
}
