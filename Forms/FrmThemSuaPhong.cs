using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    public partial class FrmThemSuaPhong : Form
    {
        // Item hiển thị trong ComboBox chọn khu nhà, ghi đè ToString() để hiện "Mã - Tên".
        private sealed class KhuNhaItem
        {
            public string MaKhu { get; }
            public string TenKhu { get; }

            public KhuNhaItem(string maKhu, string tenKhu)
            {
                MaKhu = maKhu;
                TenKhu = tenKhu;
            }

            public override string ToString() => $"{MaKhu} - {TenKhu}";
        }

        // null = đang thêm mới, khác null = đang sửa (giữ mã phòng đang sửa)
        private readonly string? _maPhongDangSua;

        /// <summary>
        /// Chế độ thêm mới.
        /// </summary>
        public FrmThemSuaPhong()
        {
            InitializeComponent();
            _maPhongDangSua = null;
            Text = "Thêm phòng";
            TaiDanhSachKhu();
        }

        /// <summary>
        /// Chế độ sửa, truyền vào phòng cần sửa.
        /// </summary>
        public FrmThemSuaPhong(Phong phong)
        {
            InitializeComponent();
            _maPhongDangSua = phong.MaPhong;
            Text = "Sửa phòng";
            TaiDanhSachKhu();

            txtMaPhong.Text = phong.MaPhong;
            txtMaPhong.Enabled = false; // không cho đổi mã phòng khi sửa

            if (phong.MaKhu != null)
            {
                foreach (var obj in cboKhuNha.Items)
                {
                    if (obj is KhuNhaItem item && item.MaKhu == phong.MaKhu)
                    {
                        cboKhuNha.SelectedItem = obj;
                        break;
                    }
                }
            }

            numSoLuongToiDa.Value = phong.SoLuongToiDa ?? 0;
            numSoLuongHienTai.Value = phong.SoLuongHienTai ?? 0;
        }

        private void TaiDanhSachKhu()
        {
            using var db = new AppDbContext();
            var danhSachKhu = db.KhuNhas.OrderBy(k => k.MaKhu).ToList();

            cboKhuNha.Items.Clear();
            foreach (var khu in danhSachKhu)
            {
                cboKhuNha.Items.Add(new KhuNhaItem(khu.MaKhu, khu.TenKhu));
            }

            cboKhuNha.Enabled = cboKhuNha.Items.Count > 0;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maPhong = txtMaPhong.Text.Trim();

            if (string.IsNullOrEmpty(maPhong))
            {
                MessageBox.Show("Vui lòng nhập Mã phòng.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? maKhuDaChon = (cboKhuNha.SelectedItem as KhuNhaItem)?.MaKhu;

            using var db = new AppDbContext();

            if (_maPhongDangSua == null)
            {
                // Thêm mới -> kiểm tra trùng mã phòng
                bool daTonTai = db.Phongs.Any(p => p.MaPhong == maPhong);
                if (daTonTai)
                {
                    MessageBox.Show("Mã phòng đã tồn tại, vui lòng chọn mã khác.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var phongMoi = new Phong
                {
                    MaPhong = maPhong,
                    MaKhu = maKhuDaChon,
                    SoLuongToiDa = (int)numSoLuongToiDa.Value,
                    SoLuongHienTai = (int)numSoLuongHienTai.Value
                };

                db.Phongs.Add(phongMoi);
            }
            else
            {
                var phong = db.Phongs.FirstOrDefault(p => p.MaPhong == _maPhongDangSua);
                if (phong == null)
                {
                    MessageBox.Show("Không tìm thấy phòng để cập nhật (có thể đã bị xóa).",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                phong.MaKhu = maKhuDaChon;
                phong.SoLuongToiDa = (int)numSoLuongToiDa.Value;
                phong.SoLuongHienTai = (int)numSoLuongHienTai.Value;
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
