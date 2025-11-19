using WalletHub.Models;

namespace WalletHub.Data.Configurations
{
    public static class CategoriaSeed
    {
        public static readonly List<Categoria> categoriasGlobales = new() //Lista estática de categorias globales del sistema
    {
        new Categoria { idCategoria = "CA001", nombreCateg = "Hogar", tipoCateg = TipoCategoria.Gasto, idUsuario = null! },
        new Categoria { idCategoria = "CA002", nombreCateg = "Alimentación", tipoCateg = TipoCategoria.Gasto, idUsuario = null! },
        new Categoria { idCategoria = "CA003", nombreCateg = "Transporte", tipoCateg = TipoCategoria.Gasto, idUsuario = null! },
        new Categoria { idCategoria = "CA004", nombreCateg = "Salud", tipoCateg = TipoCategoria.Gasto, idUsuario = null! },
        new Categoria { idCategoria = "CA005", nombreCateg = "Educación", tipoCateg = TipoCategoria.Gasto, idUsuario = null! },
        new Categoria { idCategoria = "CA006", nombreCateg = "Entretenimiento", tipoCateg = TipoCategoria.Gasto, idUsuario = null! },
        new Categoria { idCategoria = "CA007", nombreCateg = "Salario", tipoCateg = TipoCategoria.Ingreso, idUsuario = null! },
        new Categoria { idCategoria = "CA008", nombreCateg = "Emprendimiento Personal", tipoCateg = TipoCategoria.Ingreso, idUsuario = null! },
        new Categoria { idCategoria = "CA009", nombreCateg = "Inversiones", tipoCateg = TipoCategoria.Ingreso, idUsuario = null! },
        new Categoria { idCategoria = "CA010", nombreCateg = "Regalos", tipoCateg = TipoCategoria.Ingreso, idUsuario = null! }
    };
    }
}
