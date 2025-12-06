using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WalletHub.Data.Interface;
using WalletHub.Services.Interface;
using WalletHub.DTOs;

public class ReportePDFService : IReportePDFService
{
    private readonly ICalculosService _calculosService;
    private readonly IReporteRepository _reporteRepository;

    // Inyección de dependencias: repositorio y cálculos
    public ReportePDFService(
        IReporteRepository reporteRepository,
        ICalculosService calculosService)
    {
        _reporteRepository = reporteRepository;
        _calculosService = calculosService;
    }

    // ========================================================
    // GENERAR PDF POR PERIODO (USADO EN EL FRONT)
    // ========================================================
    public async Task<byte[]> GenerarReportePdfPorPeriodoAsync(
        string idUsuario,
        DateTime inicio,
        DateTime fin)
    {
        // Obtener resumen de ingresos/gastos para el periodo
        var resumen = await _calculosService.ObtenerResumenAsync(idUsuario, inicio, fin);

        // Crear y devolver el PDF
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
        var reporte = await _reporteRepository.GetReporteByIdInterno(idReporte, idUsuario);
        if (reporte == null)
            return Array.Empty<byte>(); // Retornar vacío si no existe

        // Obtener resumen usando las fechas del reporte
        var resumen = await _calculosService.ObtenerResumenAsync(
            idUsuario,
            reporte.inicioPeriodo,
            reporte.finalPeriodo);

        // Crear y devolver el PDF
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
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);         // Tamaño A4
                page.Margin(30);                 // Margen
                page.DefaultTextStyle(x => x.FontSize(11).FontColor("#333")); // Estilo base

                // ---------------- ENCABEZADO ------------------
                page.Header().Element(header =>
                {
                    header.Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text(titulo)
                                .SemiBold()
                                .FontSize(22)
                                .FontColor("#1A73E8");
                        });

                        col.Item()
                           .PaddingTop(5)
                           .BorderBottom(1)
                           .BorderColor("#1A73E8");
                    });
                });

                // ---------------- CUERPO ------------------
                page.Content().PaddingVertical(15).Column(col =>
                {
                    col.Spacing(20);

                    // ---- Datos del periodo ----
                    col.Item().Border(1).BorderColor("#E0E0E0").Padding(10).Column(info =>
                    {
                        info.Spacing(5);

                        info.Item().Text("Información del periodo")
                            .SemiBold().FontSize(14).FontColor("#1A73E8");

                        if (idReporte != null)
                            info.Item().Text($"ID reporte: {idReporte}");

                        info.Item().Text($"Desde: {inicio:dd/MM/yyyy}");
                        info.Item().Text($"Hasta: {fin:dd/MM/yyyy}");
                    });

                    // ---- Resumen ----
                    col.Item().Border(1).BorderColor("#E0E0E0").Padding(10).Column(res =>
                    {
                        res.Spacing(8);
                        res.Item().Text("Resumen General")
                            .SemiBold().FontSize(14).FontColor("#1A73E8");

                        res.Item().Row(r =>
                        {
                            r.RelativeItem().Text($"Ingresos\n{resumen.TotalIngresos:C}")
                                .FontSize(12).FontColor("#0F9D58");

                            r.RelativeItem().Text($"Gastos\n{resumen.TotalGastos:C}")
                                .FontSize(12).FontColor("#DB4437");

                            r.RelativeItem().Text($"Saldo\n{resumen.Diferencia:C}")
                                .FontSize(12).Bold();
                        });
                    });

                    // ---- Tabla Gastos ----
                    if (resumen.GastosPorCategoria.Any())
                    {
                        col.Item().PaddingTop(10).Text("Gastos por Categoría")
                            .SemiBold().FontSize(14).FontColor("#DB4437");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.ConstantColumn(90);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").SemiBold();
                                header.Cell().Text("Categoría").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });

                            foreach (var g in resumen.GastosPorCategoria)
                            {
                                table.Cell().Text(g.IdCategoria.ToString());
                                table.Cell().Text(g.NombreCategoria);
                                table.Cell().Text(g.Total.ToString("C")).FontColor("#DB4437");
                            }
                        });
                    }

                    // ---- Tabla Ingresos ----
                    if (resumen.IngresosPorCategoria.Any())
                    {
                        col.Item().PaddingTop(20).Text("Ingresos por Categoría")
                            .SemiBold().FontSize(14).FontColor("#0F9D58");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.ConstantColumn(90);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").SemiBold();
                                header.Cell().Text("Categoría").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });

                            foreach (var i in resumen.IngresosPorCategoria)
                            {
                                table.Cell().Text(i.IdCategoria.ToString());
                                table.Cell().Text(i.NombreCategoria);
                                table.Cell().Text(i.Total.ToString("C")).FontColor("#0F9D58");
                            }
                        });
                    }
                });

                // ---------------- FOOTER ------------------
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf(); // Generar PDF final
    }
}
