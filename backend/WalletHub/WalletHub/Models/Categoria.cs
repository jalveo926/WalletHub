namespace WalletHub.Models
{
    public class Categoria
    {
        public string idCategoria { get; set; } = string.Empty;
        public string nombreCateg { get; set; } = string.Empty;
        public string tipoCateg { get; set; } = string.Empty;
        public string idUsuario { get; set; } //FK, no usamos string.Empty para admitir categorias creadas por el sistema (idUsuario = null)
    }
}
