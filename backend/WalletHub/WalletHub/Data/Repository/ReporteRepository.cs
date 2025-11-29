using WalletHub.Data;
using WalletHub.Data.Interface;
using WalletHub.Models;
using WalletHub.Utils;
using WalletHub.DTOs;
using Microsoft.EntityFrameworkCore;

public class ReporteRepository : IReporteRepository
{
    private readonly ApplicationDbContext _context;

    public ReporteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerarIdReporteAsync()
    {
        return await IdGenerator.GenerateIdAsync(
            _context.Reporte,
            "RE",
            "idReporte"
        );
    }

    public async Task CreateReporteAsync(Reporte reporte)
    {
        _context.Reporte.Add(reporte);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Reporte>> GetReportesAsync(string idUsuario)
    {
        return await _context.Reporte
            .Where(r => r.idUsuario == idUsuario)
            .ToListAsync();
    }

    public async Task<Reporte?> GetReporteByIdAsync(string idReporte, string idUsuario)
    {
        return await _context.Reporte
            .FirstOrDefaultAsync(r => r.idReporte == idReporte && r.idUsuario == idUsuario);
    }

    public async Task<bool> DeleteReporteAsync(string idReporte, string idUsuario)
    {
        var reporte = await GetReporteByIdAsync(idReporte, idUsuario);

        if (reporte == null)
            return false;

        _context.Reporte.Remove(reporte);
        await _context.SaveChangesAsync();
        return true;
    }
}
