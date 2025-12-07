using WalletHub.Models;

namespace WalletHub.Data.Configurations
{
    public static class ConfigSeed
    {
        public static readonly List<Usuario> usuariosPrueba = new() //Lista estática de usuarios de prueba
        {
            new Usuario { idUsuario = "US001", nombreUsu = "Jesus", correoUsu = "jesus.alveo@utp.ac.pa", pwHashUsu = "AQAAAAIAAYagAAAAELTGLo7+TfUQMvP8jqKL5+BbQSorW9tghAnPuU2xuvpqifTarMSVYOKMm1V0Cq/XHw==" }, // pwHashUsu = "Jesus.12", generado usando PasswordHasher<T> de ASP.NET Identity
            new Usuario { idUsuario = "US002", nombreUsu = "Erick", correoUsu = "erick.hou@utp.ac.pa", pwHashUsu = "AQAAAAIAAYagAAAAEB45Z6Gu4V/N2CkXvobsT6xIH8lAkdIE7qsAlZJRFHi/tmy5u7G6uWhflAFuQvhh/w==" },// pwHashUsu = "Erick.45", lo lógica de hashes con Identity permite generar el hash de una contraseña en un proyecto y que sea interpretado en otro
            new Usuario { idUsuario = "US003", nombreUsu = "Roniel", correoUsu = "roniel.quintero@utp.ac.pa", pwHashUsu = "AQAAAAIAAYagAAAAEIDl+SkyIySpcBx88SBrCDMNZVlj/4Wfkqxcbbv1BNTNG10+6aOVCX2/JFHX+FrSsg==" }, // pwHashUsu = "Roniel.23", los hashes son portables y totalmente interoperables.
            new Usuario { idUsuario = "US004", nombreUsu = "Jessica", correoUsu = "jessica.zheng@utp.ac.pa", pwHashUsu = "AQAAAAIAAYagAAAAEF8fIe8K49ZHrRycUBl5W8tpc2d+vpxKQI00CG2JuxHSGLvo/hf/ZJCUiH9/ZeZYdQ==" } , // pwHashUsu = "Jessica.34"
        };
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
        public static readonly List<Transaccion> transaccionesPrueba = new() //Lista estática de transacciones de prueba
        {
            new Transaccion { idTransaccion = "TR001", montoTransac = 150.75m, fechaTransac = new DateTime(2025, 1, 15), descripcionTransac = "Compra de supermercado", idCategoria = "CA002", idUsuario = "US001" },
            new Transaccion { idTransaccion = "TR002", montoTransac = 2000.00m, fechaTransac = new DateTime(2025, 1, 31), descripcionTransac = "Pago de salario mensual", idCategoria = "CA007", idUsuario = "US001" },
            new Transaccion { idTransaccion = "TR003", montoTransac = 50.00m, fechaTransac = new DateTime(2025, 2, 5), descripcionTransac = "Taxi al trabajo", idCategoria = "CA003", idUsuario = "US002" },
            new Transaccion { idTransaccion = "TR004", montoTransac = 300.00m, fechaTransac = new DateTime(2025, 2, 20), descripcionTransac = "Cena con amigos", idCategoria = "CA006", idUsuario = "US003" },
            new Transaccion { idTransaccion = "TR005", montoTransac = 1200.00m, fechaTransac = new DateTime(2025, 3, 1), descripcionTransac = "Pago de salario mensual", idCategoria = "CA007", idUsuario = "US004" }
        };
    }
}
