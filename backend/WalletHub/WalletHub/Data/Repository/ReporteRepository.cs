using WalletHub.Data;
using WalletHub.Data.Interface;
using WalletHub.Models;
using WalletHub.Utils;
using WalletHub.DTOs;
using Microsoft.EntityFrameworkCore;

// Repositorio encargado de las operaciones sobre la entidad Reporte
public class ReporteRepository : IReporteRepository
{
    private readonly ApplicationDbContext _context;

    // Recibe el DbContext por inyección de dependencias
    public ReporteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Genera un nuevo id de reporte con el prefijo "RE"
    public async Task<string> GenerarIdReporteAsync()
    {
        return await IdGenerator.GenerateIdAsync(
            _context.Reporte,
            "RE",
            "idReporte"
        );
    }

    // Inserta un nuevo reporte en base de datos
    public async Task CreateReporteAsync(Reporte reporte)
    {
        _context.Reporte.Add(reporte);
        await _context.SaveChangesAsync();
    }

    // Obtiene todos los reportes asociados a un usuario (como DTO)
    public async Task<IEnumerable<ReporteDTO>> GetReportesAsync(string idUsuario)
    {
        return await _context.Reporte
            .Where(r => r.idUsuario == idUsuario)
            .Select(r => new ReporteDTO
            {
                IdReporte = r.idReporte,
                UrlArchivo = r.rutaArchivoRepo,    // Ruta o URL que hayas guardado
                TipoArchivo = r.tipoArchivoRepo.ToString(),
                FechaCreacion = r.fechaCreacionRepo,
                InicioPeriodo = r.inicioPeriodo,
                FinalPeriodo = r.finalPeriodo
            })
            .ToListAsync();
    }

    // Obtiene un reporte puntual del usuario (como DTO)
    public async Task<ReporteDTO?> GetReporteByIdAsync(string idReporte, string idUsuario)
    {
        return await _context.Reporte
            .Where(r => r.idReporte == idReporte && r.idUsuario == idUsuario)
            .Select(r => new ReporteDTO
            {
                IdReporte = r.idReporte,
                UrlArchivo = r.rutaArchivoRepo,
                TipoArchivo = r.tipoArchivoRepo.ToString(),
                FechaCreacion = r.fechaCreacionRepo,
                InicioPeriodo = r.inicioPeriodo,
                FinalPeriodo = r.finalPeriodo
            })
            .FirstOrDefaultAsync();
    }

    // Obtiene la entidad Reporte completa para uso interno (por ejemplo, generación de PDF)
    public async Task<Reporte?> GetReporteByIdInterno(string idReporte, string idUsuario)
    {
        return await _context.Reporte
            .FirstOrDefaultAsync(r => r.idReporte == idReporte && r.idUsuario == idUsuario);
    }

    // Elimina un reporte del usuario si existe
    public async Task<bool> DeleteReporteAsync(string idReporte, string idUsuario)
    {
        // Reutiliza el método interno para obtener la entidad
        var reporte = await GetReporteByIdInterno(idReporte, idUsuario);

        if (reporte == null)
            return false;

        _context.Reporte.Remove(reporte);
        await _context.SaveChangesAsync();
        return true;
    }
}
