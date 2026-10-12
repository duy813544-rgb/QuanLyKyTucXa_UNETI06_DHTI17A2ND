namespace DoAnNhom06.Forms
{
    partial class FrmPhanPhong
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitle = new Label();

            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            btnLamMoi = new Button();

            dgvDangO = new DataGridView();

            lblSoLuong = new Label();

            btnChuyenPhong = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvDangO).BeginInit();
            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(980, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Phân phòng";
            this.BackColor = Color.WhiteSmoke;
            this.MinimumSize = new Size(820, 500);

            // lblTitle
            lblTitle.Text = "PHÂN PHÒNG - CHUYỂN SINH VIÊN ĐANG Ở SANG PHÒNG KHÁC";
            lblTitle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(980, 40);
            lblTitle.Location = new Point(0, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // lblTimKiem
            lblTimKiem.Text = "Tìm kiếm (mã SV / họ tên / mã phòng):";
            lblTimKiem.Location = new Point(20, 62);
            lblTimKiem.Size = new Size(240, 25);
            lblTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // txtTimKiem
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Location = new Point(260, 58);
            txtTimKiem.Size = new Size(280, 27);
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;

            // btnTimKiem
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.Location = new Point(550, 57);
            btnTimKiem.BackColor = Color.RoyalBlue;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnTimKiem.Click += btnTimKiem_Click;

            // btnLamMoi
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(100, 30);
            btnLamMoi.Location = new Point(660, 57);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnLamMoi.Click += btnLamMoi_Click;

            // dgvDangO
            dgvDangO.Name = "dgvDangO";
            dgvDangO.Location = new Point(20, 100);
            dgvDangO.Size = new Size(940, 440);
            dgvDangO.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDangO.ReadOnly = true;
            dgvDangO.AllowUserToAddRows = false;
            dgvDangO.AllowUserToDeleteRows = false;
            dgvDangO.MultiSelect = false;
            dgvDangO.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDangO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDangO.BackgroundColor = Color.White;

            // lblSoLuong
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 sinh viên đang ở";
            lblSoLuong.Location = new Point(20, 550);
            lblSoLuong.Size = new Size(320, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // btnChuyenPhong
            btnChuyenPhong.Name = "btnChuyenPhong";
            btnChuyenPhong.Text = "Chuyển phòng";
            btnChuyenPhong.Size = new Size(140, 38);
            btnChuyenPhong.Location = new Point(630, 570);
            btnChuyenPhong.BackColor = Color.SeaGreen;
            btnChuyenPhong.ForeColor = Color.White;
            btnChuyenPhong.FlatStyle = FlatStyle.Flat;
            btnChuyenPhong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnChuyenPhong.Click += btnChuyenPhong_Click;

            // btnDong
            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(850, 570);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(btnLamMoi);
            Controls.Add(dgvDangO);
            Controls.Add(lblSoLuong);
            Controls.Add(btnChuyenPhong);
            Controls.Add(btnDong);

            Load += FrmPhanPhong_Load;

            ((System.ComponentModel.ISupportInitialize)dgvDangO).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private Button btnLamMoi;
        private DataGridView dgvDangO;
        private Label lblSoLuong;
        private Button btnChuyenPhong;
        private Button btnDong;

        #endregion
    }
}
