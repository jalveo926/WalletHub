using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using WalletHub.Models;
using WalletHub.Data.Configurations;
namespace WalletHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor que recibe las opciones de configuración
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Este constructor es necesario para la inyección de dependencias
        }

        /// <summary>
        /// Representa la tabla "Usuario" en la base de datos
        /// DbSet es como una "colección" que permite hacer consultas
        /// </summary>
        public DbSet<Usuario> Usuario { get; set; }
        /// <summary>
        /// Representa la tabla "Categoria" en la base de datos
        /// DbSet es como una "colección" que permite hacer consultas
        /// </summary>
        public DbSet<Categoria> Categoria { get; set; }
        /// <summary>
        /// Representa la tabla "Transaccion" en la base de datos
        /// DbSet es como una "colección" que permite hacer consultas
        /// </summary>
        public DbSet<Transaccion> Transaccion { get; set; }
        /// <summary>
        /// Representa la tabla "Reporte" en la base de datos
        /// DbSet es como una "colección" que permite hacer consultas
        /// </summary>
        public DbSet<Reporte> Reporte { get; set; }

        /// <summary>
        /// Método para configurar el modelo de datos
        /// Aquí podemos personalizar cómo se crean las tablas
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la tabla "Usuario"
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Usuario");

                // Configurar la llave primaria
                entity.HasKey(e => e.idUsuario);

                // Configurar propiedades requeridas
                entity.Property(e => e.nombreUsu)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.correoUsu)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.pwHashUsu)
                      .IsRequired()
                      .HasMaxLength(200);
            });
            // Configurar la tabla "Categoria"
            modelBuilder.Entity<Categoria>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Categoria");

                // Configurar la llave primaria
                entity.HasKey(e => e.idCategoria);

                // Configurar propiedades requeridas
                entity.Property(e => e.nombreCateg)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.tipoCateg)
                      .HasConversion<string>() // Convertir enum en string para guardarlo
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.idUsuario)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasData(CategoriaSeed.categoriasGlobales); // Insertar las categorias globales al crear la tabla
            });
            // Configurar la tabla "Transaccion"
            modelBuilder.Entity<Transaccion>(entity =>
            {

            });
            // Configurar la tabla "Reporte"
            modelBuilder.Entity<Reporte>(entity =>
            {

            });
        }
    }
}
