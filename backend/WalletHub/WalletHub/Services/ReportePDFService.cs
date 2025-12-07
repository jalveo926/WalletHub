using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WalletHub.Data.Interface;
using WalletHub.Services.Interface;
using WalletHub.DTOs;

public class ReportePDFService : IReportePDFService
{
    // Servicios necesarios
    private readonly ICalculosService _calculosService; // Servicio para cálculos de ingresos/gastos
    private readonly IReporteRepository _reporteRepository; // Repositorio para acceso a datos

    // Constructor con inyección de dependencias
    public ReportePDFService(
        IReporteRepository reporteRepository,
        ICalculosService calculosService)
    {
        _reporteRepository = reporteRepository; // Inicializar repositorio
        _calculosService = calculosService; // Inicializar servicio de cálculos
    }

    // ========================================================
    // GENERAR PDF POR PERIODO (USADO EN EL FRONT)
    // ========================================================
    public async Task<byte[]> GenerarReportePdfPorPeriodoAsync(
        string idUsuario,
        DateTime inicio,
        DateTime fin)
    {
        // Obtener datos financieros del periodo especificado
        var resumen = await _calculosService.ObtenerResumenAsync(idUsuario, inicio, fin);

        // Crear documento PDF con los datos obtenidos
        return CrearDocumentoPDF(
            titulo: "REPORTE POR PERIODO",
            inicio: inicio,
            fin: fin,
            resumen: resumen
        );
    }

    // ========================================================
    // GENERAR PDF POR ID DE REPORTE (OPCIONAL)
    // ========================================================
    public async Task<byte[]> GenerarReportePdfAsync(
        string idReporte,
        string idUsuario)
    {
        // Buscar reporte por ID y usuario
        var reporte = await _reporteRepository.GetReporteByIdInterno(idReporte, idUsuario);

        // Si no existe, retornar array vacío
        if (reporte == null)
            return Array.Empty<byte>();

        // Obtener resumen usando fechas guardadas en el reporte
        var resumen = await _calculosService.ObtenerResumenAsync(
            idUsuario,
            reporte.inicioPeriodo,
            reporte.finalPeriodo);

        // Crear PDF con datos del reporte existente
        return CrearDocumentoPDF(
            titulo: "REPORTE DETALLADO",
            inicio: reporte.inicioPeriodo,
            fin: reporte.finalPeriodo,
            resumen: resumen,
            idReporte: idReporte
        );
    }

    // ========================================================
    // PLANTILLA DE DISEÑO DEL PDF
    // ========================================================
    private byte[] CrearDocumentoPDF(
        string titulo,
        DateTime inicio,
        DateTime fin,
        CalculosDTO resumen,
        string? idReporte = null)
    {
        // Crear documento PDF usando QuestPDF
        return Document.Create(container =>
        {
            // Configurar página
            container.Page(page =>
            {
                page.Size(PageSizes.A4); // Tamaño carta A4
                page.Margin(30); // Margen de 30 unidades
                page.DefaultTextStyle(x => x.FontSize(11).FontColor("#333")); // Estilo de texto por defecto

                // ---------------- ENCABEZADO ------------------
                page.Header().Element(header =>
                {
                    header.Column(col =>
                    {
                        // Título principal del reporte
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text(titulo)
                                .SemiBold() // Negrita semi-gruesa
                                .FontSize(22) // Tamaño grande
                                .FontColor("#1A73E8"); // Color azul
                        });

                        // Línea separadora debajo del título
                        col.Item()
                           .PaddingTop(5) // Espacio superior
                           .BorderBottom(1) // Borde inferior
                           .BorderColor("#1A73E8"); // Color azul
                    });
                });

                // ---------------- CUERPO ------------------
                page.Content().PaddingVertical(15).Column(col =>
                {
                    col.Spacing(20); // Espacio entre elementos

                    // ---- Sección: Datos del periodo ----
                    col.Item().Border(1).BorderColor("#E0E0E0").Padding(10).Column(info =>
                    {
                        info.Spacing(5); // Espacio entre líneas

                        // Título de sección
                        info.Item().Text("Información del periodo")
                            .SemiBold().FontSize(14).FontColor("#1A73E8");

                        // Mostrar ID solo si existe
                        if (idReporte != null)
                            info.Item().Text($"ID reporte: {idReporte}");

                        // Fecha de inicio
                        info.Item().Text($"Desde: {inicio:dd/MM/yyyy}");

                        // Fecha de fin
                        info.Item().Text($"Hasta: {fin:dd/MM/yyyy}");
                    });

                    // ---- Sección: Resumen financiero ----
                    col.Item().Border(1).BorderColor("#E0E0E0").Padding(10).Column(res =>
                    {
                        res.Spacing(8); // Espacio entre elementos

                        // Título de sección
                        res.Item().Text("Resumen General")
                            .SemiBold().FontSize(14).FontColor("#1A73E8");

                        // Fila con tres columnas: Ingresos, Gastos, Saldo
                        res.Item().Row(r =>
                        {
                            // Columna: Total de ingresos en verde
                            r.RelativeItem().Text($"Ingresos\n{resumen.TotalIngresos:C}")
                                .FontSize(12).FontColor("#0F9D58");

                            // Columna: Total de gastos en rojo
                            r.RelativeItem().Text($"Gastos\n{resumen.TotalGastos:C}")
                                .FontSize(12).FontColor("#DB4437");

                            // Columna: Diferencia (saldo) en negrita
                            r.RelativeItem().Text($"Saldo\n{resumen.Diferencia:C}")
                                .FontSize(12).Bold();
                        });
                    });

                    // ---- Tabla: Gastos por categoría ----
                    if (resumen.GastosPorCategoria.Any()) // Solo si hay gastos
                    {
                        // Título de tabla
                        col.Item().PaddingTop(10).Text("Gastos por Categoría")
                            .SemiBold().FontSize(14).FontColor("#DB4437");

                        // Crear tabla
                        col.Item().Table(table =>
                        {
                            // Definir ancho de columnas: ID fijo, Categoría flexible, Total fijo
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID: 50 unidades
                                columns.RelativeColumn(); // Categoría: espacio restante
                                columns.ConstantColumn(90); // Total: 90 unidades
                            });

                            // Encabezados de tabla
                            table.Header(header =>
                            {
                                header.Cell().Text("ID").SemiBold();
                                header.Cell().Text("Categoría").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });

                            // Llenar filas con datos de gastos
                            foreach (var g in resumen.GastosPorCategoria)
                            {
                                table.Cell().Text(g.IdCategoria.ToString()); // ID categoría
                                table.Cell().Text(g.NombreCategoria); // Nombre categoría
                                table.Cell().Text(g.Total.ToString("C")).FontColor("#DB4437"); // Total en rojo
                            }
                        });
                    }

                    // ---- Tabla: Ingresos por categoría ----
                    if (resumen.IngresosPorCategoria.Any()) // Solo si hay ingresos
                    {
                        // Título de tabla
                        col.Item().PaddingTop(20).Text("Ingresos por Categoría")
                            .SemiBold().FontSize(14).FontColor("#0F9D58");

                        // Crear tabla
                        col.Item().Table(table =>
                        {
                            // Definir ancho de columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.RelativeColumn(); // Categoría
                                columns.ConstantColumn(90); // Total
                            });

                            // Encabezados de tabla
                            table.Header(header =>
                            {
                                header.Cell().Text("ID").SemiBold();
                                header.Cell().Text("Categoría").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });

                            // Llenar filas con datos de ingresos
                            foreach (var i in resumen.IngresosPorCategoria)
                            {
                                table.Cell().Text(i.IdCategoria.ToString()); // ID categoría
                                table.Cell().Text(i.NombreCategoria); // Nombre categoría
                                table.Cell().Text(i.Total.ToString("C")).FontColor("#0F9D58"); // Total en verde
                            }
                        });
                    }
                });

                // ---------------- PIE DE PÁGINA ------------------
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página "); // Texto fijo
                    x.CurrentPageNumber(); // Número de página actual
                    x.Span(" / "); // Separador
                    x.TotalPages(); // Total de páginas
                });
            });
        }).GeneratePdf(); // Generar y retornar bytes del PDF
    }
}