using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PoSBackend.Models;

namespace PoSBackend.Context
{
    public partial class PoSDbContext : DbContext
    {
        public PoSDbContext()
        {
        }

        public PoSDbContext(DbContextOptions<PoSDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<item> items { get; set; } = null!;
        public virtual DbSet<order> orders { get; set; } = null!;
        public virtual DbSet<receipt> receipts { get; set; } = null!;
        public virtual DbSet<unit> units { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=pointofsale;uid=root;password=css225", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<item>(entity =>
            {
                entity.ToTable("item");

                entity.HasIndex(e => e.unit, "fk_item_unit_idx");

                entity.Property(e => e.code).HasMaxLength(45);

                entity.Property(e => e.name).HasMaxLength(45);

                entity.Property(e => e.price).HasPrecision(19, 2);

                entity.HasOne(d => d.unitNavigation)
                    .WithMany(p => p.items)
                    .HasForeignKey(d => d.unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_item_unit");
            });

            modelBuilder.Entity<order>(entity =>
            {
                entity.ToTable("order");

                entity.HasIndex(e => e.item_id, "fk_order_item_idx");

                entity.HasIndex(e => e.receipt_id, "fk_order_receipt_idx");

                entity.HasIndex(e => e.unit_id, "fk_order_unit_idx");

                entity.Property(e => e.item_code).HasMaxLength(45);

                entity.Property(e => e.item_discount_amount).HasPrecision(19, 2);

                entity.Property(e => e.item_discount_percent).HasPrecision(5, 2);

                entity.Property(e => e.item_name).HasMaxLength(45);

                entity.Property(e => e.item_price).HasPrecision(19, 2);

                entity.Property(e => e.item_total_amount).HasPrecision(19, 2);

                entity.Property(e => e.item_unit).HasMaxLength(45);

                entity.HasOne(d => d.receipt)
                    .WithMany(p => p.orders)
                    .HasForeignKey(d => d.receipt_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_receipt");

                entity.HasOne(d => d.unit)
                    .WithMany(p => p.orders)
                    .HasForeignKey(d => d.unit_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_unit");
            });

            modelBuilder.Entity<receipt>(entity =>
            {
                entity.ToTable("receipt");

                entity.Property(e => e.code).HasMaxLength(45);

                entity.Property(e => e.grand_total).HasPrecision(19, 2);

                entity.Property(e => e.subtotal).HasPrecision(19, 2);

                entity.Property(e => e.total_amount).HasPrecision(19, 2);

                entity.Property(e => e.total_discount_amount).HasPrecision(19, 2);

                entity.Property(e => e.trade_discount).HasPrecision(19, 2);
            });

            modelBuilder.Entity<unit>(entity =>
            {
                entity.ToTable("unit");

                entity.Property(e => e.name).HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
