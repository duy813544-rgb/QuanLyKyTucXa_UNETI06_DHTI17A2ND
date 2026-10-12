using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    public partial class FrmThemSuaSinhVien : Form
    {
        // Tiền tố mã sinh viên tự sinh, ví dụ SV001, SV002...
        private const string TienToMaSv = "SV";

        // null = đang thêm mới, khác null = đang sửa (giữ mã sinh viên đang sửa)
        private readonly string? _maSvDangSua;

        /// <summary>
        /// Chế độ thêm mới.
        /// </summary>
        public FrmThemSuaSinhVien()
        {
            InitializeComponent();
            _maSvDangSua = null;
            Text = "Thêm hồ sơ sinh viên";

            // Tự sinh Mã sinh viên mới (không trùng) và khóa ô lại, không cho gõ tay.
            using var db = new AppDbContext();
            txtMaSv.Text = MaSoHelper.SinhMaMoi(db.SinhViens.Select(sv => sv.MaSv), TienToMaSv);
            txtMaSv.ReadOnly = true;
            txtMaSv.BackColor = Color.WhiteSmoke;
        }

        /// <summary>
        /// Chế độ sửa, truyền vào sinh viên cần sửa.
        /// </summary>
        public FrmThemSuaSinhVien(SinhVien sinhVien)
        {
            InitializeComponent();
            _maSvDangSua = sinhVien.MaSv;
            Text = "Sửa hồ sơ sinh viên";

            txtMaSv.Text = sinhVien.MaSv;
            txtMaSv.ReadOnly = true; // không cho đổi mã sinh viên khi sửa
            txtMaSv.BackColor = Color.WhiteSmoke;
            txtHoTen.Text = sinhVien.HoTen;
            txtLop.Text = sinhVien.Lop;
            txtSoDienThoai.Text = sinhVien.SoDienThoai;
            txtQueQuan.Text = sinhVien.QueQuan;

            if (sinhVien.NgaySinh.HasValue)
            {
                dtpNgaySinh.Checked = true;
                dtpNgaySinh.Value = sinhVien.NgaySinh.Value.ToDateTime(TimeOnly.MinValue);
            }
            else
            {
                dtpNgaySinh.Checked = false;
            }

            if (!string.IsNullOrEmpty(sinhVien.GioiTinh) && cboGioiTinh.Items.Contains(sinhVien.GioiTinh))
            {
                cboGioiTinh.SelectedItem = sinhVien.GioiTinh;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSv.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();

            if (string.IsNullOrEmpty(maSv) || string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã sinh viên và Họ tên.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();

            if (_maSvDangSua == null)
            {
                // Thêm mới -> kiểm tra trùng mã sinh viên
                bool daTonTai = db.SinhViens.Any(sv => sv.MaSv == maSv);
                if (daTonTai)
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại, vui lòng chọn mã khác.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var svMoi = new SinhVien
                {
                    MaSv = maSv,
                    HoTen = hoTen,
                    NgaySinh = dtpNgaySinh.Checked ? DateOnly.FromDateTime(dtpNgaySinh.Value) : null,
                    GioiTinh = cboGioiTinh.SelectedItem as string,
                    Lop = txtLop.Text.Trim(),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    QueQuan = txtQueQuan.Text.Trim()
                };

                db.SinhViens.Add(svMoi);
            }
            else
            {
                var sv = db.SinhViens.FirstOrDefault(s => s.MaSv == _maSvDangSua);
                if (sv == null)
                {
                    MessageBox.Show("Không tìm thấy sinh viên để cập nhật (có thể đã bị xóa).",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                sv.HoTen = hoTen;
                sv.NgaySinh = dtpNgaySinh.Checked ? DateOnly.FromDateTime(dtpNgaySinh.Value) : null;
                sv.GioiTinh = cboGioiTinh.SelectedItem as string;
                sv.Lop = txtLop.Text.Trim();
                sv.SoDienThoai = txtSoDienThoai.Text.Trim();
                sv.QueQuan = txtQueQuan.Text.Trim();
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
