using System;
using System.Windows.Forms;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Dialog nhỏ dùng chung để chọn ngày kết thúc mới khi gia hạn hợp đồng (UC07),
    /// dùng cả cho Nhân viên (trong FrmQuanLyHopDong) và Sinh viên (trong FrmGiaHanHopDong).
    /// </summary>
    public partial class FrmChonNgayGiaHan : Form
    {
        public DateOnly NgayKetThucMoi { get; private set; }

        public FrmChonNgayGiaHan(string moTaHopDong, DateOnly? ngayKetThucHienTai)
        {
            InitializeComponent();
            lblThongTin.Text = moTaHopDong;

            // Mặc định đề xuất gia hạn thêm 6 tháng kể từ ngày kết thúc hiện tại
            // (hoặc từ hôm nay nếu hợp đồng chưa có ngày kết thúc).
            DateTime moc = ngayKetThucHienTai.HasValue
                ? ngayKetThucHienTai.Value.ToDateTime(TimeOnly.MinValue)
                : DateTime.Today;

            dtpNgayKetThucMoi.MinDate = DateTime.Today;
            dtpNgayKetThucMoi.Value = moc > DateTime.Today ? moc.AddMonths(6) : DateTime.Today.AddMonths(6);
        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            NgayKetThucMoi = DateOnly.FromDateTime(dtpNgayKetThucMoi.Value);
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
