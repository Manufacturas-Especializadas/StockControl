﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StockControl.Models;

public partial class StockControlContext : DbContext
{
    public StockControlContext(DbContextOptions<StockControlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entrada> Entradas { get; set; }

    public virtual DbSet<Historialinventario> Historialinventarios { get; set; }

    public virtual DbSet<Planner> Planners { get; set; }

    public virtual DbSet<Registrohistorico> Registrohistoricos { get; set; }

    public virtual DbSet<Salida> Salidas { get; set; }

    public virtual DbSet<Shoporder> Shoporders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entrada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ENTRADAS__3214EC077A17FDC6");

            entity.ToTable("ENTRADAS");

            entity.Property(e => e.Codigo).IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Historialinventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HISTORIA__3214EC079E5E6119");

            entity.ToTable("HISTORIALINVENTARIO");

            entity.Property(e => e.Codigo).IsUnicode(false);
            entity.Property(e => e.FechaEntrada).HasColumnType("datetime");
            entity.Property(e => e.FechaSalida).HasColumnType("datetime");
        });

        modelBuilder.Entity<Planner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PLANNER__3214EC071FE0ECE2");

            entity.ToTable("PLANNER");

            entity.Property(e => e.Cantidad).IsUnicode(false);
            entity.Property(e => e.Codigo).IsUnicode(false);
            entity.Property(e => e.Codigo2)
                .IsUnicode(false)
                .HasColumnName("Codigo_2");
            entity.Property(e => e.Codigo3)
                .IsUnicode(false)
                .HasColumnName("Codigo_3");
            entity.Property(e => e.Codigo4)
                .IsUnicode(false)
                .HasColumnName("Codigo_4");
            entity.Property(e => e.Codigo5)
                .IsUnicode(false)
                .HasColumnName("Codigo_5");
            entity.Property(e => e.Codigo6)
                .IsUnicode(false)
                .HasColumnName("Codigo_6");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
        });

        modelBuilder.Entity<Registrohistorico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__REGISTRO__3214EC07E4CD6FF2");

            entity.ToTable("REGISTROHISTORICO");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.PartNumber).IsUnicode(false);
        });

        modelBuilder.Entity<Salida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SALIDAS__3214EC076E93D310");

            entity.ToTable("SALIDAS");

            entity.Property(e => e.Codigo).IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Shoporder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SHOPORDE__3214EC0791B31FAB");

            entity.ToTable("SHOPORDER");

            entity.Property(e => e.Codigo).IsUnicode(false);
            entity.Property(e => e.FechaShopOrder).HasColumnType("datetime");
            entity.Property(e => e.ShopOrder1).HasColumnName("ShopOrder");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}