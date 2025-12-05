using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WalletHub.Data.Interface;
using WalletHub.Models;
using WalletHub.Services.Interface;
using WalletHub.DTOs;

public class ReportePDFService : IReportePDFService
{
    private readonly IReporteRepository _reporteRepository;
    private readonly ICalculosService _calculosService;

    public ReportePDFService(
        IReporteRepository reporteRepository, 
        ICalculosService calculosService)
    {
        _reporteRepository = reporteRepository;
        _calculosService = calculosService;
    }

    public async Task<byte[]> GenerarReportePdfAsync(string idReporte, string idUsuario)
    {
        var reporte = await _reporteRepository.GetReporteByIdInterno(idReporte, idUsuario);

        if (reporte == null)
            throw new InvalidOperationException("El reporte no existe o no pertenece al usuario.");

        // Usamos el periodo del reporte
        var resumen = await _calculosService.ObtenerResumenAsync(
            idUsuario,
            reporte.inicioPeriodo,
            reporte.finalPeriodo
        );

        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("REPORTE DE TRANSACCIONES")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // Datos generales del reporte
                    col.Item().Text($"ID reporte: {reporte.idReporte}");
                    col.Item().Text($"Fecha de creación: {reporte.fechaCreacionRepo:dd/MM/yyyy HH:mm}");
                    col.Item().Text($"Periodo: {reporte.inicioPeriodo:dd/MM/yyyy} - {reporte.finalPeriodo:dd/MM/yyyy}");
                    col.Item().Text($"Tipo de archivo: {reporte.tipoArchivoRepo}");

                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                    // Resumen general
                    col.Item().Text("Resumen del periodo").SemiBold().FontSize(14);
                    col.Item().Text($"Total ingresos: {resumen.TotalIngresos:C}");
                    col.Item().Text($"Total gastos: {resumen.TotalGastos:C}");
                    col.Item().Text($"Diferencia: {resumen.Diferencia:C}");

                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                    // Gastos por categoría
                    if (resumen.GastosPorCategoria.Any())
                    {
                        col.Item().Text("Gastos por categoría").SemiBold().FontSize(14);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn();
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").SemiBold();
                                header.Cell().Text("Categoría").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });

                            foreach (var g in resumen.GastosPorCategoria)
                            {
                                table.Cell().Text(g.IdCategoria);
                                table.Cell().Text(g.NombreCategoria);
                                table.Cell().Text(g.Total.ToString("C"));
                            }
                        });
                    }

                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                    // Ingresos por categoría
                    if (resumen.IngresosPorCategoria.Any())
                    {
                        col.Item().Text("Ingresos por categoría").SemiBold().FontSize(14);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn();
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").SemiBold();
                                header.Cell().Text("Categoría").SemiBold();
                                header.Cell().Text("Total").SemiBold();
                            });

                            foreach (var i in resumen.IngresosPorCategoria)
                            {
                                table.Cell().Text(i.IdCategoria);
                                table.Cell().Text(i.NombreCategoria);
                                table.Cell().Text(i.Total.ToString("C"));
                            }
                        });
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();

        return pdfBytes;
    }
}
