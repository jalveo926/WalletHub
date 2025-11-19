using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using WalletHub.Models;
namespace WalletHub.Data
{
    public class ReporteDbContext : DbContext
    {
        public ReporteDbContext(DbContextOptions<ReporteDbContext> options) : base(options) { }
        public required DbSet<Reporte> Reporte { get; set; }
    }
}
