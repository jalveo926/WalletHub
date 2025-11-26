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
                      .HasColumnName("idUsuario")
                      .ValueGeneratedNever() // Deshabilitar generación automática
                      .HasMaxLength(5); //formato de id US000

                // Configurar propiedades requeridas
                entity.Property(e => e.nombreUsu)
                      .IsRequired() // basicamente not null
                      .HasColumnName("nombreUsu")
                      .HasMaxLength(50);

                entity.Property(e => e.correoUsu)
                      .IsRequired()
                      .HasColumnName("correoUsu")
                      .HasMaxLength(30);

                entity.Property(e => e.pwHashUsu)
                      .IsRequired()
                      .HasColumnName("pwHashUsu")
                      .HasMaxLength(200); // tamaño recomendado para hashes

                entity.HasData(ConfigSeed.usuariosPrueba); // Insertar los usuarios de prueba al crear la tabla
            });
            // Configurar la tabla "Categoria"
            modelBuilder.Entity<Categoria>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Categoria");

                // Configurar la llave primaria
                entity.HasKey(e => e.idCategoria);
                entity.Property(e => e.idCategoria)
                      .HasColumnName("idCategoria")
                      .ValueGeneratedNever() // Deshabilitar generación automática
                      .HasMaxLength(5); //formato de id CA000

                // Configurar propiedades requeridas
                entity.Property(e => e.nombreCateg)
                      .IsRequired()
                      .HasColumnName("nombreCateg")
                      .HasMaxLength(50);

                entity.Property(e => e.tipoCateg)
                      .IsRequired()
                      .HasConversion<string>() // Convertir enum en string para guardarlo
                      .HasColumnName("tipoCateg")
                      .HasMaxLength(30);

                entity.Property(e => e.idUsuario) //habilita not null para categorias globales del sistema, las creadas por el usuario si seran marcadas con su id
                      .HasColumnName("idUsuario")
                      .HasMaxLength(5);

                entity.HasOne(c => c.Usuario) //se define relación un usuario a muchas categorias
                      .WithMany(u => u.Categorias)
                      .HasForeignKey(c => c.idUsuario)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false)   // ← Esto permite categorías globales
                      .HasConstraintName("Categoria_idUsuario_FK");

                entity.HasData(ConfigSeed.categoriasGlobales); // Insertar las categorias globales al crear la tabla
            });
            // Configurar la tabla "Transaccion"
            modelBuilder.Entity<Transaccion>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Transaccion");

                // Configurar la llave primaria
                entity.HasKey(e => e.idTransaccion);
                entity.Property(e => e.idTransaccion)
                      .HasColumnName("idTransaccion")
                      .ValueGeneratedNever() // Deshabilitar generación automática
                      .HasMaxLength(5); //formato de id TR000

                // Configurar propiedades requeridas
                entity.Property(e => e.fechaTransac)
                      .IsRequired()
                      .HasColumnName("fechaTransac");

                entity.Property(e => e.montoTransac)
                      .IsRequired()
                      .HasColumnName("montoTransac")
                      .HasColumnType("decimal(18,2)"); // Forma estandar para transacciones monetarias

                entity.Property(e => e.descripcionTransac)
                      .HasColumnName("descripcionTransac")
                      .HasMaxLength(200);

                entity.Property(e => e.idUsuario)
                      .IsRequired()
                      .HasColumnName("idUsuario")
                      .HasMaxLength(5);

                entity.Property(e => e.idCategoria)
                      .IsRequired()
                      .HasColumnName("idCategoria")
                      .HasMaxLength(5);

                entity.HasOne(t => t.Usuario) // se define relación un usuario a muchas transacciones
                      .WithMany(u => u.Transacciones)
                      .HasForeignKey(t => t.idUsuario)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("Transaccion_idUsuario_FK");

                entity.HasOne(t => t.Categoria) // se define relación una categoría a muchas transacciones
                      .WithMany(c => c.Transacciones)
                      .HasForeignKey(t => t.idCategoria)
                      .OnDelete(DeleteBehavior.Restrict) // no borra la transacción si se borra la categoría
                      .HasConstraintName("Transaccion_idCategoria_FK");

                entity.HasData(ConfigSeed.transaccionesPrueba); // Insertar las transacciones de prueba al crear la tabla
            });
            // Configurar la tabla "Reporte"
            modelBuilder.Entity<Reporte>(entity =>
            {
                // Nombre de la tabla en la base de datos
                entity.ToTable("Reporte");

                // Configurar la llave primaria
                entity.HasKey(e => e.idReporte);
                entity.Property(e => e.idReporte) 
                      .HasColumnName("idReporte")
                      .ValueGeneratedNever() // Deshabilitar generación automática
                      .HasMaxLength(5); // formato de id RE000

                // Configurar propiedades requeridas
                entity.Property(e => e.fechaCreacionRepo)
                      .IsRequired()
                      .HasColumnName("fechaCreacionRepo");

                entity.Property(e => e.inicioPeriodo)
                      .IsRequired()
                      .HasColumnName("inicioPeriodo");

                entity.Property(e => e.finalPeriodo)
                      .IsRequired()
                      .HasColumnName("finalPeriodo");

                entity.Property(e => e.rutaArchivoRepo)
                      .IsRequired()
                      .HasColumnName("rutaArchivoRepo")
                      .HasMaxLength(300);

                entity.Property(e => e.tipoArchivoRepo)
                      .IsRequired()
                      .HasColumnName("tipoArchivoRepo")
                      .HasConversion<string>() // Convertir enum en string para guardarlo
                      .HasMaxLength(20);

                entity.Property(e => e.idUsuario)
                      .IsRequired()
                      .HasColumnName("idUsuario")
                      .HasMaxLength(5);

                entity.HasOne(r => r.Usuario) // se define relación un usuario a muchos reportes
                      .WithMany(u => u.Reportes)
                      .HasForeignKey(r => r.idUsuario)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("Reporte_idUsuario_FK");
            });
        }
    }
}
