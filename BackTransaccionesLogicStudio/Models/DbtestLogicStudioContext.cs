using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackTransaccionesLogicStudio.Models;

public partial class DbtestLogicStudioContext : DbContext
{
    public DbtestLogicStudioContext()
    {
    }

    public DbtestLogicStudioContext(DbContextOptions<DbtestLogicStudioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<TipoTransaccion> TipoTransaccions { get; set; }

    public virtual DbSet<TransaccionCabecera> TransaccionCabeceras { get; set; }

    public virtual DbSet<TransaccionDetalle> TransaccionDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07B74504B0");

            entity.ToTable("Categoria", "Productos");

            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07A7BEBABF");

            entity.ToTable("Producto", "Productos");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");
        });

        modelBuilder.Entity<TipoTransaccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tipo_Tra__3214EC07A216490B");

            entity.ToTable("Tipo_Transaccion", "Cajas");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TransaccionCabecera>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transacc__3214EC0701A8E92B");

            entity.ToTable("Transaccion_Cabecera", "Cajas");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdTipoTransaccion).HasColumnName("Id_Tipo_Transaccion");

            entity.HasOne(d => d.IdTipoTransaccionNavigation).WithMany(p => p.TransaccionCabeceras)
                .HasForeignKey(d => d.IdTipoTransaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cabecera_TipoTransaccion");
        });

        modelBuilder.Entity<TransaccionDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transacc__3214EC07B3581475");

            entity.ToTable("Transaccion_Detalle", "Cajas");

            entity.Property(e => e.Detalle).HasMaxLength(255);
            entity.Property(e => e.IdProducto).HasColumnName("Id_Producto");
            entity.Property(e => e.IdTransaccionCabecera).HasColumnName("Id_Transaccion_Cabecera");
            entity.Property(e => e.PrecioTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_Total");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_Unitario");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.TransaccionDetalles)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalle_Producto");

            entity.HasOne(d => d.IdTransaccionCabeceraNavigation).WithMany(p => p.TransaccionDetalles)
                .HasForeignKey(d => d.IdTransaccionCabecera)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalle_Cabecera");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
