using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Dialog nhỏ dùng để chọn phòng mới khi Quản trị viên thực hiện Phân phòng (UC15) -
    /// chuyển một sinh viên đang ở từ phòng hiện tại sang phòng khác còn chỗ trống.
    /// </summary>
    public partial class FrmChonPhongMoi : Form
    {
        // Item hiển thị trong ComboBox chọn phòng, ghi đè ToString() để hiện
        // "Mã phòng - Khu nhà (còn trống x/y)".
        private sealed class PhongItem
        {
            public string MaPhong { get; }
            private readonly string _hienThi;

            public PhongItem(string maPhong, string hienThi)
            {
                MaPhong = maPhong;
                _hienThi = hienThi;
            }

            public override string ToString() => _hienThi;
        }

        public string? MaPhongMoiDaChon { get; private set; }

        public FrmChonPhongMoi(string moTa, IEnumerable<Phong> danhSachPhongCoTheChon)
        {
            InitializeComponent();
            lblThongTin.Text = moTa;

            foreach (var p in danhSachPhongCoTheChon)
            {
                int toiDa = p.SoLuongToiDa ?? 0;
                int hienTai = p.SoLuongHienTai ?? 0;
                string tenKhu = p.MaKhuNavigation?.TenKhu ?? p.MaKhu ?? "";
                string hienThi = $"{p.MaPhong} - {tenKhu} (còn trống {toiDa - hienTai}/{toiDa})";
                cboPhongMoi.Items.Add(new PhongItem(p.MaPhong, hienThi));
            }

            cboPhongMoi.Enabled = cboPhongMoi.Items.Count > 0;
            if (cboPhongMoi.Items.Count > 0)
            {
                cboPhongMoi.SelectedIndex = 0;
            }
        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            if (cboPhongMoi.SelectedItem is not PhongItem item)
            {
                ThongBao.CanhBao("Vui lòng chọn một phòng còn chỗ trống.");
                return;
            }

            MaPhongMoiDaChon = item.MaPhong;
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
