using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using WalletHub.Models;
namespace WalletHub.Data
{
    public class TransaccionDbContext : DbContext
    {
        public TransaccionDbContext(DbContextOptions<TransaccionDbContext> options) : base(options) { }

        public required DbSet<Transaccion> Transaccion { get; set; }
    {
    }
}
