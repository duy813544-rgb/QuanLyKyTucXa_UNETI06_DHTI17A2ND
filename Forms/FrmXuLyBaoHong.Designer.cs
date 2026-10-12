namespace DoAnNhom06.Forms
{
    partial class FrmXuLyBaoHong
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private ComboBox cboTrangThai;
        private DataGridView dgvBaoHong;
        private Label lblSoLuong;

        private Label lblGhiChu;
        private TextBox txtGhiChu;

        private Button btnTiepNhan;
        private Button btnHoanThanh;
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
            lblTitle = new Label();
            cboTrangThai = new ComboBox();
            dgvBaoHong = new DataGridView();
            lblSoLuong = new Label();
            lblGhiChu = new Label();
            txtGhiChu = new TextBox();
            btnTiepNhan = new Button();
            btnHoanThanh = new Button();
            btnLamMoi = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvBaoHong).BeginInit();
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xử lý báo hỏng cơ sở vật chất";
            BackColor = Color.WhiteSmoke;
            MinimumSize = new Size(850, 480);

            lblTitle.Text = "XỬ LÝ BÁO HỎNG CƠ SỞ VẬT CHẤT";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(1000, 40);
            lblTitle.Location = new Point(0, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.Location = new Point(20, 58);
            cboTrangThai.Size = new Size(180, 30);
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Font = new Font("Segoe UI", 10F);
            cboTrangThai.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;

            dgvBaoHong.Name = "dgvBaoHong";
            dgvBaoHong.Location = new Point(20, 100);
            dgvBaoHong.Size = new Size(960, 340);
            dgvBaoHong.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBaoHong.ReadOnly = true;
            dgvBaoHong.AllowUserToAddRows = false;
            dgvBaoHong.AllowUserToDeleteRows = false;
            dgvBaoHong.MultiSelect = false;
            dgvBaoHong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaoHong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoHong.BackgroundColor = Color.White;

            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 yêu cầu";
            lblSoLuong.Location = new Point(20, 450);
            lblSoLuong.Size = new Size(260, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Text = "Ghi chú xử lý (tuỳ chọn):";
            lblGhiChu.Location = new Point(300, 450);
            lblGhiChu.Size = new Size(180, 25);
            lblGhiChu.ForeColor = Color.DimGray;
            lblGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Location = new Point(480, 447);
            txtGhiChu.Size = new Size(500, 28);
            txtGhiChu.Font = new Font("Segoe UI", 10F);
            txtGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnTiepNhan.Name = "btnTiepNhan";
            btnTiepNhan.Text = "Tiếp nhận (Đang sửa)";
            btnTiepNhan.Size = new Size(170, 38);
            btnTiepNhan.Location = new Point(20, 530);
            btnTiepNhan.BackColor = Color.DarkOrange;
            btnTiepNhan.ForeColor = Color.White;
            btnTiepNhan.FlatStyle = FlatStyle.Flat;
            btnTiepNhan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnTiepNhan.Click += btnTiepNhan_Click;

            btnHoanThanh.Name = "btnHoanThanh";
            btnHoanThanh.Text = "Hoàn thành";
            btnHoanThanh.Size = new Size(140, 38);
            btnHoanThanh.Location = new Point(200, 530);
            btnHoanThanh.BackColor = Color.SeaGreen;
            btnHoanThanh.ForeColor = Color.White;
            btnHoanThanh.FlatStyle = FlatStyle.Flat;
            btnHoanThanh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnHoanThanh.Click += btnHoanThanh_Click;

            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(110, 38);
            btnLamMoi.Location = new Point(760, 530);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLamMoi.Click += btnLamMoi_Click;

            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(870, 530);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(cboTrangThai);
            Controls.Add(dgvBaoHong);
            Controls.Add(lblSoLuong);
            Controls.Add(lblGhiChu);
            Controls.Add(txtGhiChu);
            Controls.Add(btnTiepNhan);
            Controls.Add(btnHoanThanh);
            Controls.Add(btnLamMoi);
            Controls.Add(btnDong);

            Load += FrmXuLyBaoHong_Load;

            ((System.ComponentModel.ISupportInitialize)dgvBaoHong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
