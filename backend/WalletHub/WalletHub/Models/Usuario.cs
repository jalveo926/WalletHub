namespace WalletHub.Models
{
    public class Usuario
    {
        public string idUsuario {  get; set; } = string.Empty;
        public string nombreUsu { get; set; } = string.Empty;
        public string correoUsu { get; set; } = string.Empty;
        public string pwHashUsu { get; set; } = string.Empty;

        // Relación 1 a muchas Categorías
        public ICollection<Categoria> Categorias { get; set; }

        // Relación 1 a muchas Transacciones
        public ICollection<Transaccion> Transacciones { get; set; }

        // Relación 1 a muchos Reportes
        public ICollection<Reporte> Reportes { get; set; }
    }
}
