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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
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
                entity.Property(e => e.idUsuario)
                      .HasMaxLength(5); //formato de id US000

                // Configurar propiedades requeridas
                entity.Property(e => e.nombreUsu)
                      .IsRequired() // basicamente not null
                      .HasMaxLength(50);

                entity.Property(e => e.correoUsu)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.pwHashUsu)
                      .IsRequired()
                      .HasMaxLength(200); // tamaño recomendado para hashes
            });
            // Configurar la tabla "Categoria"
            modelBuilder.Entity<Categoria>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Categoria");

                // Configurar la llave primaria
                entity.HasKey(e => e.idCategoria);
                entity.Property(e => e.idCategoria)
                      .HasMaxLength(5); //formato de id CA000

                // Configurar propiedades requeridas
                entity.Property(e => e.nombreCateg)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.tipoCateg)
                      .IsRequired()
                      .HasConversion<string>() // Convertir enum en string para guardarlo
                      .HasMaxLength(30);

                entity.Property(e => e.idUsuario) //habilita not null para categorias globales del sistema, las creadas por el usuario si seran marcadas con su id
                      .HasMaxLength(50);

                entity.HasOne(c => c.Usuario) //se define relación un usuario a muchas categorias
                      .WithMany(u => u.Categorias)
                      .HasForeignKey(c => c.idUsuario)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);   // ← Esto permite categorías globales

                entity.HasData(CategoriaSeed.categoriasGlobales); // Insertar las categorias globales al crear la tabla
            });
            // Configurar la tabla "Transaccion"
            modelBuilder.Entity<Transaccion>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Transaccion");

                // Configurar la llave primaria
                entity.HasKey(e => e.idTransaccion);
                entity.Property(e => e.idTransaccion)
                      .HasMaxLength(5); //formato de id TR000

                // Configurar propiedades requeridas
                entity.Property(e => e.fechaTransac)
                      .IsRequired();

                entity.Property(e => e.montoTransac)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)"); // Forma estandar para transacciones monetarias

                entity.Property(e => e.descripcionTransac)
                      .HasMaxLength(200);

                entity.Property(e => e.idUsuario)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.idCategoria)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasOne(t => t.Usuario) // se define relación un usuario a muchas transacciones
                      .WithMany(u => u.Transacciones)
                      .HasForeignKey(t => t.idUsuario)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Categoria) // se define relación una categoría a muchas transacciones
                      .WithMany(c => c.Transacciones)
                      .HasForeignKey(t => t.idCategoria)
                      .OnDelete(DeleteBehavior.Restrict); // no borra la transacción si se borra la categoría
            });
            // Configurar la tabla "Reporte"
            modelBuilder.Entity<Reporte>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Reporte");

                // Configurar la llave primaria
                entity.HasKey(e => e.idReporte);
                entity.Property(e => e.idReporte) // formato de id RE000
                      .HasMaxLength(5);

                // Configurar propiedades requeridas
                entity.Property(e => e.fechaCreacionRepo)
                      .IsRequired();

                entity.Property(e => e.inicioPeriodo)
                      .IsRequired();

                entity.Property(e => e.finalPeriodo)
                      .IsRequired();

                entity.Property(e => e.rutaArchivoRepo)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.Property(e => e.tipoArchivoRepo)
                      .IsRequired()
                      .HasConversion<string>() // Convertir enum en string para guardarlo
                      .HasMaxLength(20);

                entity.Property(e => e.idUsuario)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasOne(r => r.Usuario) // se define relación un usuario a muchos reportes
                      .WithMany(u => u.Reportes)
                      .HasForeignKey(r => r.idUsuario)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
