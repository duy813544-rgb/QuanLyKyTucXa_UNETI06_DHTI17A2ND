using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Controls;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC10 - Tra cứu thông tin: dùng chung cho cả Sinh viên và Nhân viên, nhưng phạm vi
    /// dữ liệu khác nhau theo vai trò:
    ///   - Nhân viên: tra cứu tự do theo từ khóa trên Sinh viên / Phòng / Hợp đồng (như cũ).
    ///   - Sinh viên: KHÔNG được gõ từ khóa để dò thông tin của người khác - chỉ xem được
    ///     đúng hồ sơ và hợp đồng của CHÍNH mình, còn Phòng thì xem được danh sách chung
    ///     (không phải dữ liệu cá nhân nên không cần giới hạn).
    ///
    /// Giao diện được làm mới theo phong cách thanh tìm kiếm Google: loại tra cứu chọn
    /// bằng các "chip" bo tròn thay cho ComboBox, ô nhập từ khóa bo tròn hết cỡ có icon
    /// kính lúp, và bảng kết quả có tiêu đề cột tô màu nhấn thay vì bảng lưới mặc định.
    /// </summary>
    public partial class FrmTraCuuThongTin : BaseAppForm
    {
        private const string LoaiSinhVien = "Sinh viên";
        private const string LoaiPhong = "Phòng";
        private const string LoaiHopDong = "Hợp đồng";

        private const string LoaiThongTinCuaToi = "Thông tin của tôi";
        private const string LoaiHopDongCuaToi = "Hợp đồng của tôi";

        private readonly TaiKhoan _taiKhoanDangNhap;
        private bool LaSinhVien => _taiKhoanDangNhap.Quyen == VaiTro.SinhVien;

        private readonly List<ChipButton> _chips = new();
        private string _loaiDangChon = "";

        /// <summary>Loại thực thể của bảng kết quả đang hiển thị: "SinhVien" | "Phong" | "HopDong".
        /// Dùng để biết phải tra thêm chi tiết theo bảng nào khi bấm nút "Xem chi tiết".</summary>
        private string _loaiKetQuaHienTai = "";

        private const string ColXemChiTiet = "colXemChiTiet";

        public FrmTraCuuThongTin(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;

            // QUAN TRỌNG: không được gọi ThemCotXemChiTiet() ngay sau khi gán DataSource,
            // vì lúc đó DataGridView có thể CHƯA sinh xong các cột dữ liệu tự động (việc này
            // xảy ra bất đồng bộ, nhất là khi form vừa Load và control chưa có Handle) - nếu
            // gọi sớm, cột nút sẽ bị chèn vào ĐẦU bảng thay vì cuối. Vì code lấy khóa chính
            // từ Cells[0] nên tra cứu sẽ luôn báo "không tìm thấy" một cách sai lệch.
            // Gắn vào sự kiện DataBindingComplete đảm bảo cột nút luôn được thêm SAU KHI các
            // cột tự sinh đã có đầy đủ, nên luôn nằm đúng vị trí cuối cùng.
            dgvKetQua.DataBindingComplete += (_, _) => ThemCotXemChiTiet();
        }

        private void FrmTraCuuThongTin_Load(object sender, EventArgs e)
        {
            try
            {
                if (LaSinhVien)
                {
                    // Sinh viên: không có ô từ khóa, chỉ chọn xem "của tôi" hay xem Phòng chung.
                    TaoChip(new[] { LoaiThongTinCuaToi, LoaiHopDongCuaToi, LoaiPhong });
                    pnlSearchRow.Visible = false;
                    TimKiem();
                }
                else
                {
                    TaoChip(new[] { LoaiSinhVien, LoaiPhong, LoaiHopDong });
                    pnlSearchRow.Visible = true;
                    dgvKetQua.DataSource = null;
                    lblKetQua.Text = "Chọn loại, nhập từ khóa rồi bấm \"Tìm kiếm\".";
                }
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        /// <summary>
        /// Thêm cột nút "Xem chi tiết" vào cuối bảng kết quả (bên phải cột cuối cùng,
        /// ví dụ QueQuan). Vì dgvKetQua.DataSource được gán lại mỗi lần tìm kiếm nên
        /// AutoGenerateColumns sẽ tự xóa hết cột cũ - phải gọi hàm này lại sau MỖI lần
        /// gán DataSource mới thì cột nút mới xuất hiện lại.
        /// </summary>
        private void ThemCotXemChiTiet()
        {
            if (dgvKetQua.Columns.Count == 0) return; // chưa có cột dữ liệu nào thì chưa thêm nút

            if (dgvKetQua.Columns.Contains(ColXemChiTiet))
            {
                // Cột đã tồn tại từ trước (ví dụ do gọi lại nhiều lần) - chỉ cần đảm bảo
                // nó luôn hiển thị ở vị trí CUỐI CÙNG, phòng trường hợp cột dữ liệu tự
                // sinh xuất hiện sau và đẩy nó lệch vị trí.
                dgvKetQua.Columns[ColXemChiTiet].DisplayIndex = dgvKetQua.Columns.Count - 1;
                return;
            }

            var cot = new DataGridViewButtonColumn
            {
                Name = ColXemChiTiet,
                HeaderText = "",
                Text = "Xem chi tiết",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 110,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgvKetQua.Columns.Add(cot);
            // Bắt buộc nằm cuối cùng, không phụ thuộc vào thứ tự Index lúc Add().
            cot.DisplayIndex = dgvKetQua.Columns.Count - 1;
        }

        /// <summary>Hiện lỗi đầy đủ (kèm stack trace) thay vì để ứng dụng bị crash.</summary>
        private void BaoLoi(Exception ex)
        {
            dgvKetQua.DataSource = null;
            lblKetQua.Text = "Đã xảy ra lỗi khi tra cứu (xem chi tiết trong hộp thoại).";
            MessageBox.Show(ex.ToString(), "Lỗi khi tra cứu thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>Tạo các chip chọn loại tra cứu và chọn sẵn chip đầu tiên.</summary>
        private void TaoChip(string[] danhSach)
        {
            pnlChips.Controls.Clear();
            _chips.Clear();

            foreach (var loai in danhSach)
            {
                var chip = new ChipButton { Text = loai, AccentColor = Color.RoyalBlue };
                int rong = TextRenderer.MeasureText(loai, chip.Font).Width + 36;
                chip.Size = new Size(rong, 36);
                chip.Click += (_, _) => ChonChip(loai);

                pnlChips.Controls.Add(chip);
                _chips.Add(chip);
            }

            _loaiDangChon = danhSach[0];
            CapNhatTrangThaiChip();
            CanhGiuaChip();
        }

        /// <summary>Căn giữa toàn bộ hàng chip theo chiều ngang của pnlChips (gọi lại khi Form đổi kích thước).</summary>
        private void CanhGiuaChip()
        {
            if (_chips.Count == 0) return;

            const int khoangCach = 10;
            int tongRong = _chips.Sum(c => c.Width) + khoangCach * (_chips.Count - 1);
            int x = Math.Max(0, (pnlChips.Width - tongRong) / 2);

            foreach (var chip in _chips)
            {
                chip.Location = new Point(x, (pnlChips.Height - chip.Height) / 2);
                x += chip.Width + khoangCach;
            }
        }

        private void CapNhatTrangThaiChip()
        {
            foreach (var chip in _chips)
                chip.DaChon = chip.Text == _loaiDangChon;
        }

        private void ChonChip(string loai)
        {
            try
            {
                _loaiDangChon = loai;
                CapNhatTrangThaiChip();

                if (LaSinhVien)
                {
                    // Sinh viên không có từ khóa để nhập: đổi loại là tự tải kết quả ngay.
                    TimKiem();
                    return;
                }

                // Nhân viên: đổi loại tra cứu -> xóa từ khóa và kết quả cũ, chờ nhập từ khóa mới
                // rồi bấm "Tìm kiếm" - không tự liệt kê lại toàn bộ.
                searchBox.Text = string.Empty;
                dgvKetQua.DataSource = null;
                lblKetQua.Text = "Nhập từ khóa rồi bấm \"Tìm kiếm\".";
                searchBox.Input.Focus();
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        private void TimKiem()
        {
            string loai = _loaiDangChon;

            if (LaSinhVien)
            {
                TimKiemChoSinhVien(loai);
                return;
            }

            string tuKhoa = searchBox.Text.Trim();

            // Đây là màn hình TRA CỨU: bắt buộc phải nhập từ khóa mới tìm,
            // không tự liệt kê toàn bộ dữ liệu (tránh trùng với các màn hình Danh sách).
            if (string.IsNullOrEmpty(tuKhoa))
            {
                dgvKetQua.DataSource = null;
                lblKetQua.Text = "Vui lòng nhập từ khóa để tra cứu.";
                searchBox.Input.Focus();
                return;
            }

            try
            {
                using var db = new AppDbContext();
                int soKetQua = 0;

                switch (loai)
                {
                    case LoaiSinhVien:
                        {
                            var ds = db.SinhViens
                                .Where(sv => sv.MaSv.Contains(tuKhoa)
                                    || sv.HoTen.Contains(tuKhoa))
                                .OrderBy(sv => sv.MaSv)
                                .ToList();

                            soKetQua = ds.Count;
                            _loaiKetQuaHienTai = "SinhVien";
                            dgvKetQua.DataSource = ds.Select(sv => new
                            {
                                MaSV = sv.MaSv,
                                HoTen = sv.HoTen,
                                NgaySinh = sv.NgaySinh.HasValue ? sv.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                                GioiTinh = sv.GioiTinh,
                                Lop = sv.Lop,
                                SoDienThoai = sv.SoDienThoai,
                                QueQuan = sv.QueQuan
                            }).ToList();
                            break;
                        }

                    case LoaiPhong:
                        {
                            var ds = db.Phongs
                                .Include(p => p.MaKhuNavigation)
                                .Include(p => p.MaLoaiNavigation)
                                .Where(p => p.MaPhong.Contains(tuKhoa)
                                    || (p.MaKhuNavigation != null && p.MaKhuNavigation.TenKhu.Contains(tuKhoa)))
                                .OrderBy(p => p.MaPhong)
                                .ToList();

                            soKetQua = ds.Count;
                            _loaiKetQuaHienTai = "Phong";
                            dgvKetQua.DataSource = ds.Select(p => new
                            {
                                MaPhong = p.MaPhong,
                                Khu = p.MaKhuNavigation?.TenKhu,
                                LoaiPhong = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.TenLoai : "",
                                SoLuongHienTai = p.SoLuongHienTai ?? 0,
                                SoLuongToiDa = p.SoLuongToiDa ?? 0,
                                TinhTrang = (p.SoLuongHienTai ?? 0) < (p.SoLuongToiDa ?? 0) ? "Còn chỗ" : "Đã đầy"
                            }).ToList();
                            break;
                        }

                    case LoaiHopDong:
                    default:
                        {
                            var ds = db.HopDongs
                                .Include(h => h.MaSvNavigation)
                                .Where(h => h.MaHopDong.Contains(tuKhoa)
                                    || h.MaSv.Contains(tuKhoa)
                                    || (h.MaSvNavigation != null && h.MaSvNavigation.HoTen.Contains(tuKhoa)))
                                .OrderByDescending(h => h.NgayLap)
                                .ToList();

                            soKetQua = ds.Count;
                            _loaiKetQuaHienTai = "HopDong";
                            dgvKetQua.DataSource = ds.Select(h => new
                            {
                                MaHopDong = h.MaHopDong,
                                MaSV = h.MaSv,
                                HoTen = h.MaSvNavigation?.HoTen,
                                MaPhong = h.MaPhong,
                                NgayLap = h.NgayLap.ToString("dd/MM/yyyy"),
                                NgayHetHan = h.NgayHetHan.ToString("dd/MM/yyyy"),
                                TrangThai = h.TrangThai
                            }).ToList();
                            break;
                        }
                }

                // Cột nút "Xem chi tiết" được thêm tự động qua sự kiện DataBindingComplete
                // của dgvKetQua (đăng ký trong constructor) - đảm bảo nó luôn được thêm SAU
                // khi các cột dữ liệu tự sinh đã có đầy đủ, nên luôn nằm đúng vị trí cuối cùng.

                // Luồng ngoại lệ: không có dữ liệu phù hợp.
                lblKetQua.Text = soKetQua > 0
                    ? $"Tìm thấy {soKetQua} kết quả."
                    : "Không có dữ liệu phù hợp.";
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        /// <summary>Tra cứu dành riêng cho Sinh viên: chỉ thấy hồ sơ/hợp đồng của chính mình.</summary>
        private void TimKiemChoSinhVien(string loai)
        {
            if (loai != LoaiPhong && string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                dgvKetQua.DataSource = null;
                lblKetQua.Text = "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào. " +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.";
                return;
            }

            try
            {
                using var db = new AppDbContext();
                string maSv = _taiKhoanDangNhap.MaSv ?? "";
                int soKetQua;

                switch (loai)
                {
                    case LoaiHopDongCuaToi:
                        {
                            var ds = db.HopDongs
                                .Where(h => h.MaSv == maSv)
                                .OrderByDescending(h => h.NgayLap)
                                .ToList();

                            soKetQua = ds.Count;
                            _loaiKetQuaHienTai = "HopDong";
                            dgvKetQua.DataSource = ds.Select(h => new
                            {
                                MaHopDong = h.MaHopDong,
                                MaPhong = h.MaPhong,
                                NgayLap = h.NgayLap.ToString("dd/MM/yyyy"),
                                NgayHetHan = h.NgayHetHan.ToString("dd/MM/yyyy"),
                                TrangThai = h.TrangThai
                            }).ToList();
                            break;
                        }

                    case LoaiPhong:
                        {
                            // Thông tin phòng không phải dữ liệu cá nhân nên sinh viên xem được toàn bộ.
                            var ds = db.Phongs
                                .Include(p => p.MaKhuNavigation)
                                .Include(p => p.MaLoaiNavigation)
                                .OrderBy(p => p.MaPhong)
                                .ToList();

                            soKetQua = ds.Count;
                            _loaiKetQuaHienTai = "Phong";
                            dgvKetQua.DataSource = ds.Select(p => new
                            {
                                MaPhong = p.MaPhong,
                                Khu = p.MaKhuNavigation?.TenKhu,
                                LoaiPhong = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.TenLoai : "",
                                SoLuongHienTai = p.SoLuongHienTai ?? 0,
                                SoLuongToiDa = p.SoLuongToiDa ?? 0,
                                TinhTrang = (p.SoLuongHienTai ?? 0) < (p.SoLuongToiDa ?? 0) ? "Còn chỗ" : "Đã đầy"
                            }).ToList();
                            break;
                        }

                    case LoaiThongTinCuaToi:
                    default:
                        {
                            var ds = db.SinhViens.Where(sv => sv.MaSv == maSv).ToList();

                            soKetQua = ds.Count;
                            _loaiKetQuaHienTai = "SinhVien";
                            dgvKetQua.DataSource = ds.Select(sv => new
                            {
                                MaSV = sv.MaSv,
                                HoTen = sv.HoTen,
                                NgaySinh = sv.NgaySinh.HasValue ? sv.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                                GioiTinh = sv.GioiTinh,
                                Lop = sv.Lop,
                                SoDienThoai = sv.SoDienThoai,
                                QueQuan = sv.QueQuan
                            }).ToList();
                            break;
                        }
                }

                // Cột nút "Xem chi tiết" được thêm tự động qua sự kiện DataBindingComplete
                // của dgvKetQua (đăng ký trong constructor).

                lblKetQua.Text = soKetQua > 0
                    ? $"Tìm thấy {soKetQua} kết quả."
                    : "Không có dữ liệu phù hợp.";
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void txtTuKhoa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TimKiem();
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>Bấm nút "Xem chi tiết" trên một dòng kết quả -> mở hộp thoại chi tiết.</summary>
        private void dgvKetQua_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvKetQua.Columns[e.ColumnIndex].Name != ColXemChiTiet) return;

            try
            {
                // Lấy khóa chính theo TÊN CỘT (MaSV / MaPhong / MaHopDong tùy loại kết quả -
                // xem các Select() phía trên) thay vì theo vị trí Cells[0]. Cách cũ giả định
                // cột đầu tiên luôn là khóa chính, nhưng vị trí cột nút "Xem chi tiết" có thể
                // lệch (xem ThemCotXemChiTiet), nên dựa vào tên cột là cách chắc chắn hơn.
                string tenCotKhoa = _loaiKetQuaHienTai switch
                {
                    "SinhVien" => "MaSV",
                    "Phong" => "MaPhong",
                    _ => "MaHopDong"
                };

                if (!dgvKetQua.Columns.Contains(tenCotKhoa)) return;

                var maKhoa = dgvKetQua.Rows[e.RowIndex].Cells[tenCotKhoa].Value?.ToString();
                if (string.IsNullOrEmpty(maKhoa)) return;

                MoChiTiet(maKhoa);
            }
            catch (Exception ex)
            {
                BaoLoi(ex);
            }
        }

        /// <summary>Tải đầy đủ thông tin (+ dữ liệu liên quan) của một bản ghi rồi hiện hộp thoại chi tiết.</summary>
        private void MoChiTiet(string maKhoa)
        {
            using var db = new AppDbContext();

            switch (_loaiKetQuaHienTai)
            {
                case "SinhVien":
                    {
                        var sv = db.SinhViens.FirstOrDefault(s => s.MaSv == maKhoa);
                        if (sv == null)
                        {
                            MessageBox.Show("Không tìm thấy sinh viên này (có thể đã bị xóa).",
                                "Xem chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var thongTin = new List<(string, string)>
                        {
                            ("Mã sinh viên", sv.MaSv),
                            ("Họ tên", sv.HoTen),
                            ("Ngày sinh", sv.NgaySinh.HasValue ? sv.NgaySinh.Value.ToString("dd/MM/yyyy") : ""),
                            ("Giới tính", sv.GioiTinh ?? ""),
                            ("Lớp", sv.Lop ?? ""),
                            ("Số điện thoại", sv.SoDienThoai ?? ""),
                            ("Quê quán", sv.QueQuan ?? "")
                        };

                        var dsHopDong = db.HopDongs
                            .Where(h => h.MaSv == maKhoa)
                            .OrderByDescending(h => h.NgayLap)
                            .Select(h => new
                            {
                                MaHopDong = h.MaHopDong,
                                MaPhong = h.MaPhong,
                                NgayLap = h.NgayLap.ToString("dd/MM/yyyy"),
                                NgayHetHan = h.NgayHetHan.ToString("dd/MM/yyyy"),
                                TrangThai = h.TrangThai
                            })
                            .ToList()
                            .Cast<object>()
                            .ToList();

                        using var frmSv = new FrmChiTietTraCuu($"Chi tiết sinh viên: {sv.MaSv}",
                            thongTin, "Hợp đồng liên quan", dsHopDong);
                        frmSv.ShowDialog(this);
                        break;
                    }

                case "Phong":
                    {
                        var phong = db.Phongs
                            .Include(p => p.MaKhuNavigation)
                            .Include(p => p.MaLoaiNavigation)
                            .FirstOrDefault(p => p.MaPhong == maKhoa);
                        if (phong == null)
                        {
                            MessageBox.Show("Không tìm thấy phòng này (có thể đã bị xóa).",
                                "Xem chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var thongTin = new List<(string, string)>
                        {
                            ("Mã phòng", phong.MaPhong),
                            ("Khu", phong.MaKhuNavigation?.TenKhu ?? ""),
                            ("Loại phòng", phong.MaLoaiNavigation?.TenLoai ?? "(chưa phân loại)"),
                            ("Đơn giá", phong.MaLoaiNavigation?.DonGia.HasValue == true
                                ? phong.MaLoaiNavigation.DonGia.Value.ToString("N0") + " đ/tháng" : ""),
                            ("Số lượng hiện tại", (phong.SoLuongHienTai ?? 0).ToString()),
                            ("Số lượng tối đa", (phong.SoLuongToiDa ?? 0).ToString()),
                            ("Tình trạng", (phong.SoLuongHienTai ?? 0) < (phong.SoLuongToiDa ?? 0) ? "Còn chỗ" : "Đã đầy")
                        };

                        var dsSinhVien = db.DangKyOs
                            .Include(d => d.MaSvNavigation)
                            .Where(d => d.MaPhong == maKhoa && d.TrangThai == TrangThaiDangKy.DaDuyet)
                            .OrderBy(d => d.MaSvNavigation != null ? d.MaSvNavigation.HoTen : "")
                            .Select(d => new
                            {
                                MaSV = d.MaSv,
                                HoTen = d.MaSvNavigation != null ? d.MaSvNavigation.HoTen : "",
                                NgayBatDau = d.NgayBatDau.HasValue ? d.NgayBatDau.Value.ToString("dd/MM/yyyy") : "",
                                NgayKetThuc = d.NgayKetThuc.HasValue ? d.NgayKetThuc.Value.ToString("dd/MM/yyyy") : "Chưa xác định"
                            })
                            .ToList()
                            .Cast<object>()
                            .ToList();

                        using var frmPhong = new FrmChiTietTraCuu($"Chi tiết phòng: {phong.MaPhong}",
                            thongTin, "Sinh viên đang ở trong phòng", dsSinhVien);
                        frmPhong.ShowDialog(this);
                        break;
                    }

                case "HopDong":
                default:
                    {
                        var hopDong = db.HopDongs
                            .Include(h => h.MaSvNavigation)
                            .FirstOrDefault(h => h.MaHopDong == maKhoa);
                        if (hopDong == null)
                        {
                            MessageBox.Show("Không tìm thấy hợp đồng này (có thể đã bị xóa).",
                                "Xem chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var thongTin = new List<(string, string)>
                        {
                            ("Mã hợp đồng", hopDong.MaHopDong),
                            ("Mã sinh viên", hopDong.MaSv),
                            ("Họ tên sinh viên", hopDong.MaSvNavigation?.HoTen ?? ""),
                            ("Mã phòng", hopDong.MaPhong),
                            ("Ngày lập", hopDong.NgayLap.ToString("dd/MM/yyyy")),
                            ("Ngày hết hạn", hopDong.NgayHetHan.ToString("dd/MM/yyyy")),
                            ("Trạng thái", hopDong.TrangThai)
                        };

                        var dsHoaDon = db.HoaDons
                            .Where(hd => hd.MaHopDong == maKhoa)
                            .OrderByDescending(hd => hd.Nam).ThenByDescending(hd => hd.Thang)
                            .Select(hd => new
                            {
                                MaHoaDon = hd.MaHoaDon,
                                ThangNam = $"{hd.Thang:00}/{hd.Nam}",
                                TongTien = hd.TongTien.ToString("N0") + " đ",
                                NgayLap = hd.NgayLap.ToString("dd/MM/yyyy"),
                                TrangThai = hd.TrangThai
                            })
                            .ToList()
                            .Cast<object>()
                            .ToList();

                        using var frmHd = new FrmChiTietTraCuu($"Chi tiết hợp đồng: {hopDong.MaHopDong}",
                            thongTin, "Hóa đơn liên quan", dsHoaDon, coNutXemHoaDon: true);
                        frmHd.ShowDialog(this);
                        break;
                    }
            }
        }
    }
}
