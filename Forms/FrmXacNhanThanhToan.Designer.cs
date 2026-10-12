using DoAnNhom06.Helpers;

namespace DoAnNhom06.Forms
{
    partial class FrmXacNhanThanhToan
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private DataGridView dgvChoXacNhan;
        private Label lblSoLuong;
        private ComboBox cboTrangThai;

        private Button btnXacNhan;
        private Button btnTuChoi;
        private Button btnLamMoi;
        private Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
       

        private void InitializeComponent()
        {
            lblTitle = TaoNhanTieuDe("XÁC NHẬN THANH TOÁN", 900);

            dgvChoXacNhan = new DataGridView();
            lblSoLuong = new Label();
            cboTrangThai = new ComboBox();

            btnXacNhan = TaoNut("Xác nhận", KieuNut.ThanhCong);
            btnTuChoi = TaoNut("Từ chối", KieuNut.NguyHiem);
            btnLamMoi = TaoNut("Làm mới", KieuNut.ChinhDien);
            btnDong = TaoNut("Đóng", KieuNut.Phu);

            ((System.ComponentModel.ISupportInitialize)dgvChoXacNhan).BeginInit();
            SuspendLayout();

            ClientSize = new Size(900, 560);
            Text = "Xác nhận thanh toán";

            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 giao dịch chờ xác nhận";
            lblSoLuong.Location = new Point(20, 62);
            lblSoLuong.Size = new Size(400, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            // ComboBox lọc trạng thái
            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Location = new Point(670, 60);
            cboTrangThai.Size = new Size(210, 30);
            cboTrangThai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;

            dgvChoXacNhan.Name = "dgvChoXacNhan";
            //dgvChoXacNhan.Location = new Point(20, 100);
            dgvChoXacNhan.Location = new Point(20, 105);
            dgvChoXacNhan.Size = new Size(860, 380);
            dgvChoXacNhan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GridStyle.ApDungChiDoc(dgvChoXacNhan);

            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(120, 38);
            btnXacNhan.Location = new Point(20, 500);
            btnXacNhan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnXacNhan.Click += btnXacNhan_Click;

            btnTuChoi.Name = "btnTuChoi";
            btnTuChoi.Size = new Size(120, 38);
            btnTuChoi.Location = new Point(150, 500);
            btnTuChoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnTuChoi.Click += btnTuChoi_Click;

            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(120, 38);
            btnLamMoi.Location = new Point(280, 500);
            btnLamMoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLamMoi.Click += btnLamMoi_Click;

            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(770, 500);
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblSoLuong);
            Controls.Add(cboTrangThai);
            Controls.Add(dgvChoXacNhan);
            Controls.Add(btnXacNhan);
            Controls.Add(btnTuChoi);
            Controls.Add(btnLamMoi);
            Controls.Add(btnDong);

            Load += FrmXacNhanThanhToan_Load;

            ((System.ComponentModel.ISupportInitialize)dgvChoXacNhan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
