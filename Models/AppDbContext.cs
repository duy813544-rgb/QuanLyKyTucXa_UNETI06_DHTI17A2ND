
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;
    public DbSet<SinhVien> SinhViens { get; set; } = null!;
    public DbSet<LoaiPhong> LoaiPhongs { get; set; } = null!;
    public DbSet<PhongKTX> PhongKTXs { get; set; } = null!;
    public DbSet<DangKyKTX> DangKyKTXs { get; set; } = null!;
    public DbSet<PhanPhong> PhanPhongs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaiKhoan>(e =>
        {
            e.ToTable("TaiKhoan");
            e.HasKey(x => x.MaTaiKhoan);
            e.Property(x => x.MaTaiKhoan).HasColumnName("MaTaiKhoan");
            e.Property(x => x.TenDangNhap).HasMaxLength(50);
            e.HasIndex(x => x.TenDangNhap).IsUnique().HasDatabaseName("UQ_TaiKhoan_TenDangNhap");
            e.Property(x => x.MatKhau).HasMaxLength(100);
            e.Property(x => x.HoTen).HasMaxLength(100);
            e.Property(x => x.Email).HasMaxLength(100);
            e.Property(x => x.VaiTro).HasMaxLength(20);
            e.Property(x => x.TrangThai).HasMaxLength(20).HasDefaultValue("DangHoatDong");
        });

        modelBuilder.Entity<SinhVien>(e =>
        {
            e.ToTable("SinhVien");
            e.HasKey(x => x.MaSinhVien);
            e.Property(x => x.MaSinhVien).HasColumnName("MaSinhVien").HasMaxLength(20);
            e.Property(x => x.MaTaiKhoan).HasColumnName("MaTaiKhoan");
            e.HasIndex(x => x.MaTaiKhoan).IsUnique().HasDatabaseName("UQ_SinhVien_MaTaiKhoan");
            e.Property(x => x.HoTen).HasMaxLength(100);
            e.Property(x => x.NgaySinh).HasColumnType("date");
            e.Property(x => x.GioiTinh).HasMaxLength(10);
            e.Property(x => x.Lop).HasMaxLength(50);
            e.Property(x => x.Khoa).HasMaxLength(100);
            e.Property(x => x.SoDienThoai).HasMaxLength(15).IsUnicode(false);
            e.Property(x => x.Email).HasMaxLength(100);
            e.Property(x => x.DiaChi).HasMaxLength(200);
            e.Property(x => x.TrangThai).HasMaxLength(20).HasDefaultValue("DangHoatDong");


            e.HasOne(x => x.TaiKhoan).WithOne(x => x.SinhVien)
                .HasForeignKey<SinhVien>(x => x.MaTaiKhoan)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SinhVien_TaiKhoan");
        });

        modelBuilder.Entity<LoaiPhong>(e =>
        {
            e.ToTable("LoaiPhong");
            e.HasKey(x => x.MaLoaiPhong);
            e.Property(x => x.TenLoaiPhong).HasMaxLength(100);
            e.HasIndex(x => x.TenLoaiPhong).IsUnique().HasDatabaseName("UQ_LoaiPhong_Ten");
            e.Property(x => x.DonGiaThang).HasPrecision(18, 0);
            e.Property(x => x.MoTa).HasMaxLength(250);
            e.Property(x => x.TrangThai).HasMaxLength(20).HasDefaultValue("DangSuDung");
        });

        modelBuilder.Entity<PhongKTX>(e =>
        {
            e.ToTable("PhongKTX");
            e.HasKey(x => x.MaPhong);
            e.Property(x => x.TenPhong).HasMaxLength(50);
            e.HasIndex(x => x.TenPhong).IsUnique().HasDatabaseName("UQ_PhongKTX_Ten");
            e.Property(x => x.ToaNha).HasMaxLength(50);
            e.Property(x => x.GioiTinhPhong).HasMaxLength(10);
            e.Property(x => x.TrangThai).HasMaxLength(20).HasDefaultValue("DangSuDung");
            e.Property(x => x.MoTa).HasMaxLength(250);

            e.HasOne(x => x.LoaiPhong).WithMany(x => x.PhongKTXs)
                .HasForeignKey(x => x.MaLoaiPhong)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PhongKTX_LoaiPhong");
        });

        modelBuilder.Entity<DangKyKTX>(e =>
        {
            e.ToTable("DangKyKTX", tb => tb.UseSqlOutputClause(false));
            e.HasKey(x => x.MaDangKy);
            e.Property(x => x.MaSinhVien).HasMaxLength(20);
            e.Property(x => x.NgayDangKy).HasColumnType("date");
            e.Property(x => x.NgayBatDau).HasColumnType("date");
            e.Property(x => x.NgayKetThucDuKien).HasColumnType("date");
            e.Property(x => x.GhiChu).HasMaxLength(300);
            e.Property(x => x.TrangThai).HasMaxLength(20).HasDefaultValue("ChoDuyet");

            // Chỉ một đăng ký Chờ duyệt và một đăng ký Đã phân phòng cho mỗi sinh viên (chỉ mục lọc).
            e.HasIndex(x => x.MaSinhVien).IsUnique().HasDatabaseName("UX_DangKyKTX_ChoDuyet")
                .HasFilter("[TrangThai] = N'ChoDuyet'");
            e.HasIndex(x => x.MaSinhVien).IsUnique().HasDatabaseName("UX_DangKyKTX_DaPhanPhong")
                .HasFilter("[TrangThai] = N'DaPhanPhong'");

            e.HasOne(x => x.SinhVien).WithMany(x => x.DangKyKTXs)
                .HasForeignKey(x => x.MaSinhVien)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DangKyKTX_SinhVien");

            e.HasOne(x => x.NguyenVong).WithMany()
                .HasForeignKey(x => x.NguyenVongLoaiPhong)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DangKyKTX_LoaiPhong");
        });

        modelBuilder.Entity<PhanPhong>(e =>
        {
            e.ToTable("PhanPhong", tb => tb.UseSqlOutputClause(false));
            e.HasKey(x => x.MaPhanPhong);
            e.Property(x => x.NgayVao).HasColumnType("date");
            e.Property(x => x.NgayRa).HasColumnType("date");
            e.Property(x => x.TrangThai).HasMaxLength(20).HasDefaultValue("DangO");

            // Mỗi đăng ký chỉ có tối đa một bản ghi đang ở.
            e.HasIndex(x => x.MaDangKy).IsUnique().HasDatabaseName("UX_PhanPhong_DangO_MaDangKy")
                .HasFilter("[TrangThai] = N'DangO'");

            e.HasOne(x => x.DangKyKTX).WithMany(x => x.PhanPhongs)
                .HasForeignKey(x => x.MaDangKy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PhanPhong_DangKyKTX");

            e.HasOne(x => x.PhongKTX).WithMany(x => x.PhanPhongs)
                .HasForeignKey(x => x.MaPhong)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PhanPhong_PhongKTX");
        });
    }
}
