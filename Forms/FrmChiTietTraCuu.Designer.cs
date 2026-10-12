using DoAnNhom06.Helpers;

namespace DoAnNhom06.Forms
{
    partial class FrmChiTietTraCuu
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private TableLayoutPanel tblThongTin;
        private Label lblLienQuan;
        private DataGridView dgvLienQuan;
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
            lblTitle = TaoNhanTieuDe("CHI TIẾT", 760);
            tblThongTin = new TableLayoutPanel();
            lblLienQuan = new Label();
            dgvLienQuan = new DataGridView();
            btnDong = TaoNut("Đóng", KieuNut.Phu);

            ((System.ComponentModel.ISupportInitialize)dgvLienQuan).BeginInit();
            SuspendLayout();

            ClientSize = new Size(760, 480);
            Text = "Chi tiết";
            BackColor = Color.White;
            MinimumSize = new Size(680, 400);

            // ----- Tiêu đề -----
            lblTitle.ForeColor = Color.FromArgb(60, 64, 67);
            lblTitle.Location = new Point(0, 14);

            // ----- Bảng thông tin dạng nhãn : giá trị, số dòng tùy theo loại dữ liệu -----
            tblThongTin.Name = "tblThongTin";
            tblThongTin.Location = new Point(24, 62);
            tblThongTin.Size = new Size(712, 160);
            tblThongTin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tblThongTin.ColumnCount = 2;
            tblThongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            tblThongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tblThongTin.AutoSize = true;
            tblThongTin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblThongTin.BackColor = Color.White;

            // ----- Khu vực dữ liệu liên quan (hợp đồng của SV, danh sách ở của phòng...) -----
            lblLienQuan.Name = "lblLienQuan";
            lblLienQuan.Text = "";
            lblLienQuan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLienQuan.ForeColor = Color.FromArgb(60, 64, 67);
            lblLienQuan.Location = new Point(24, 232);
            lblLienQuan.Size = new Size(712, 22);
            lblLienQuan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblLienQuan.Visible = false;

            dgvLienQuan.Name = "dgvLienQuan";
            dgvLienQuan.Location = new Point(24, 258);
            dgvLienQuan.Size = new Size(712, 150);
            dgvLienQuan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLienQuan.Visible = false;
            GridStyle.ApDungHienDai(dgvLienQuan, Color.RoyalBlue);
            dgvLienQuan.CellContentClick += dgvLienQuan_CellContentClick;

            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(626, 430);
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(tblThongTin);
            Controls.Add(lblLienQuan);
            Controls.Add(dgvLienQuan);
            Controls.Add(btnDong);

            ((System.ComponentModel.ISupportInitialize)dgvLienQuan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
