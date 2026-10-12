namespace DoAnNhom06.Forms
{
    partial class FrmQuanLyHopDong
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private Button btnLamMoi;
        private Button btnXemTatCa;

        private DataGridView dgvHopDong;
        private Label lblSoLuong;

        private Button btnGiaHan;
        private Button btnChamDut;
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
            lblTitle = new Label();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            btnLamMoi = new Button();

            dgvHopDong = new DataGridView();
            lblSoLuong = new Label();

            btnGiaHan = new Button();
            btnChamDut = new Button();
            btnDong = new Button();
            btnXemTatCa = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvHopDong).BeginInit();
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 620);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý hợp đồng ở ký túc xá";
            BackColor = Color.WhiteSmoke;
            MinimumSize = new Size(850, 500);

            lblTitle.Text = "QUẢN LÝ HỢP ĐỒNG Ở KÝ TÚC XÁ";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(1000, 40);
            lblTitle.Location = new Point(0, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblTimKiem.Text = "Tìm kiếm (mã SV / họ tên / mã phòng):";
            lblTimKiem.Location = new Point(20, 62);
            lblTimKiem.Size = new Size(230, 25);
            lblTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Location = new Point(250, 58);
            txtTimKiem.Size = new Size(280, 27);
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;

            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.Location = new Point(540, 57);
            btnTimKiem.BackColor = Color.RoyalBlue;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnTimKiem.Click += btnTimKiem_Click;

            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(100, 30);
            btnLamMoi.Location = new Point(650, 57);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnLamMoi.Click += btnLamMoi_Click;

            btnXemTatCa.Name = "btnXemTatCa";
            btnXemTatCa.Text = "Xem tất cả";
            btnXemTatCa.Size = new Size(120, 30);
            btnXemTatCa.Location = new Point(760, 57);
            btnXemTatCa.BackColor = Color.MediumSeaGreen;
            btnXemTatCa.ForeColor = Color.White;
            btnXemTatCa.FlatStyle = FlatStyle.Flat;
            btnXemTatCa.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnXemTatCa.Click += btnXemTatCa_Click;

            dgvHopDong.Name = "dgvHopDong";
            dgvHopDong.Location = new Point(20, 100);
            dgvHopDong.Size = new Size(960, 440);
            dgvHopDong.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHopDong.ReadOnly = true;
            dgvHopDong.AllowUserToAddRows = false;
            dgvHopDong.AllowUserToDeleteRows = false;
            dgvHopDong.MultiSelect = false;
            dgvHopDong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHopDong.BackgroundColor = Color.White;

            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 hợp đồng";
            lblSoLuong.Location = new Point(20, 550);
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btnGiaHan.Name = "btnGiaHan";
            btnGiaHan.Text = "Gia hạn";
            btnGiaHan.Size = new Size(120, 38);
            btnGiaHan.Location = new Point(480, 570);
            btnGiaHan.BackColor = Color.SeaGreen;
            btnGiaHan.ForeColor = Color.White;
            btnGiaHan.FlatStyle = FlatStyle.Flat;
            btnGiaHan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGiaHan.Click += btnGiaHan_Click;

            btnChamDut.Name = "btnChamDut";
            btnChamDut.Text = "Chấm dứt";
            btnChamDut.Size = new Size(120, 38);
            btnChamDut.Location = new Point(610, 570);
            btnChamDut.BackColor = Color.IndianRed;
            btnChamDut.ForeColor = Color.White;
            btnChamDut.FlatStyle = FlatStyle.Flat;
            btnChamDut.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnChamDut.Click += btnChamDut_Click;

            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(870, 570);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(btnLamMoi);
            Controls.Add(btnLamMoi);
            Controls.Add(dgvHopDong);
            Controls.Add(lblSoLuong);
            Controls.Add(btnGiaHan);
            Controls.Add(btnChamDut);
            Controls.Add(btnDong);

            Load += FrmQuanLyHopDong_Load;

            ((System.ComponentModel.ISupportInitialize)dgvHopDong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
