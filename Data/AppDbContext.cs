using System;
using System.Collections.Generic;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DangKyO> DangKyOs { get; set; }

    public virtual DbSet<KhuNha> KhuNhas { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<SinhVien> SinhViens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=QL_KY_TUC_XA;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DangKyO>(entity =>
        {
            entity.HasKey(e => e.MaDangKy).HasName("PK__DangKyO__BA90F02DCEEEC2C3");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.DangKyOs).HasConstraintName("FK__DangKyO__MaPhong__2D27B809");

            entity.HasOne(d => d.MaSvNavigation).WithMany(p => p.DangKyOs).HasConstraintName("FK__DangKyO__MaSV__2C3393D0");
        });

        modelBuilder.Entity<KhuNha>(entity =>
        {
            entity.HasKey(e => e.MaKhu).HasName("PK__KhuNha__3BDA934A3AB79536");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__Phong__20BD5E5B98CADE77");

            entity.Property(e => e.SoLuongHienTai).HasDefaultValue(0);

            entity.HasOne(d => d.MaKhuNavigation).WithMany(p => p.Phongs).HasConstraintName("FK__Phong__MaKhu__29572725");
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.MaSv).HasName("PK__SinhVien__2725081AD351945B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
