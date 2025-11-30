using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WalletHub.Data.Interface;
using WalletHub.Models;
using WalletHub.Services.Interface;

public class ReportePDFService : IReportePDFService
{
    private readonly IReporteRepository _reporteRepository;

    public ReportePDFService(IReporteRepository reporteRepository)
    {
        _reporteRepository = reporteRepository;
    }

    public async Task<byte[]> GenerarReportePdfAsync(string idReporte, string idUsuario)
    {
        Reporte reporte = await _reporteRepository
            .GetReporteByIdInterno(idReporte, idUsuario);

        if (reporte == null)
            throw new InvalidOperationException("El reporte no existe o no pertenece al usuario.");

        // QuestPDF genera directamente el byte[]
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

                    col.Item().Text($"ID reporte: {reporte.idReporte}");
                    col.Item().Text(
                        $"Fecha de creación: {reporte.fechaCreacionRepo:dd/MM/yyyy HH:mm}");
                    col.Item().Text(
                        $"Periodo: {reporte.inicioPeriodo:dd/MM/yyyy} - {reporte.finalPeriodo:dd/MM/yyyy}");
                    col.Item().Text($"Tipo de archivo: {reporte.tipoArchivoRepo}");

                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                    col.Item().Text("Aquí podrías agregar más datos del reporte...");
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();   // devuelve byte[]

        return pdfBytes;
    }
}
