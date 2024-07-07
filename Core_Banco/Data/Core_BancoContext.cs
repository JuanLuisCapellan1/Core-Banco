using Microsoft.EntityFrameworkCore;
using Core_Banco.Models;

namespace Core_Banco.Data
{
    public class Core_BancoContext : DbContext
    {
        public Core_BancoContext(DbContextOptions<Core_BancoContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; } = default!;
        public DbSet<Cuenta> Cuentas { get; set; } = default!;
        public DbSet<TipoTransaccion> TiposTransaccion { get; set; } = default!;
        public DbSet<Transaccion> Transacciones { get; set; } = default!;
        public DbSet<Prestamo> Prestamos { get; set; } = default!;
        public DbSet<Movimiento> Movimientos { get; set; } = default!;
        public DbSet<Perfil> Perfiles { get; set; } = default!;
        public DbSet<TipoCuenta> TipoCuentas { get; set; }
        public DbSet<Beneficiario> Beneficiarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Cuenta>().ToTable("Cuentas");
            modelBuilder.Entity<TipoTransaccion>().ToTable("TiposTransaccion");
            modelBuilder.Entity<Transaccion>().ToTable("Transacciones");
            modelBuilder.Entity<Perfil>().ToTable("Perfiles");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<TipoCuenta>().ToTable("TipoCuenta");
            modelBuilder.Entity<Beneficiario>().ToTable("Beneficiarios");

            // Configuración para que el UsuarioId se autogenere y se incremente de 1 en 1
            modelBuilder.Entity<Usuario>()
                .Property(u => u.UsuarioID)
                .ValueGeneratedOnAdd();

            // Seed initial data
            modelBuilder.Entity<TipoTransaccion>().HasData(
                new TipoTransaccion { TipoTransaccionID = 1, Nombre = "Deposito", Descripcion = "Depósito de fondos" },
                new TipoTransaccion { TipoTransaccionID = 2, Nombre = "Retiro", Descripcion = "Retiro de fondos" },
                new TipoTransaccion { TipoTransaccionID = 3, Nombre = "Transaccion", Descripcion = "de cuenta a cuenta" },
                new TipoTransaccion { TipoTransaccionID = 4, Nombre = "Beneficiario", Descripcion = "Transacción a beneficiario" }
            );

            // Configuración de la relación entre Beneficiario y Cuenta
            modelBuilder.Entity<Beneficiario>()
                .HasOne(b => b.Cuenta)
                .WithMany()
                .HasForeignKey(b => b.CuentaID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
