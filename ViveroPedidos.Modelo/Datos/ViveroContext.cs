using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViveroPedidos.Modelo.Entidades;

namespace ViveroPedidos.Modelo.Datos
{
    internal class ViveroContext : DbContext
    {
        public DbSet<Planta> Plantas => Set<Planta>();
        public DbSet<Pedido> Pedido => Set<Pedido>();
        public DbSet<PedidoDetalle> PedidoDetalle => Set<PedidoDetalle>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=ViveroPedidosDb;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Planta>(e =>
            {
                e.ToTable("Plantas");
                e.HasKey(x => x.Id);

                e.Property(x => x.Codigo)
                    .HasMaxLength(20)
                    .IsRequired();
                e.HasIndex(x => x.Codigo).IsUnique();

                e.Property(x => x.NombreComun)
                    .HasMaxLength(120)
                    .IsRequired();

                e.Property(x => x.Especie)
                    .HasMaxLength(120)
                    .IsRequired();

                e.Property(x => x.Disponible)
                    .IsRequired();
            });

            modelBuilder.Entity<Pedido>(e =>
            {
                e.ToTable("Pedidos");
                e.HasKey(x => x.Id);

                e.Property(x => x.Numero).IsRequired();
                e.HasIndex(x => x.Numero).IsUnique();

                
                e.Property(x => x.Fecha)
                    .HasColumnType("date")
                    .IsRequired();

                e.Property(x => x.SolicitanteNombre)
                    .HasMaxLength(120)
                    .IsRequired();

                e.Property(x => x.SolicitanteDocumento)
                    .HasMaxLength(30)
                    .IsRequired();

                e.Property(x => x.FechaRetiro)
                    .HasColumnType("date")
                    .IsRequired();

                e.Property(x => x.CantidadItems).IsRequired();

                e.HasMany(x => x.Detalles)
                    .WithOne(d => d.Pedido)
                    .HasForeignKey(d => d.PedidoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PedidoDetalle>(e =>
            {
                e.ToTable("PedidoDetalles");
                e.HasKey(x => x.Id);

                e.Property(x => x.NombreHistorico)
                    .HasMaxLength(120)
                    .IsRequired();

                e.Property(x => x.Cantidad).IsRequired();

                e.HasOne(x => x.Planta)
                    .WithMany(p => p.Detalles)
                    .HasForeignKey(x => x.PlantaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
