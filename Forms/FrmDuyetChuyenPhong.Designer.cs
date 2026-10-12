namespace DoAnNhom06.Forms
{
    partial class FrmDuyetChuyenPhong
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private ComboBox cboTrangThai;
        private DataGridView dgvYeuCau;
        private Label lblSoLuong;

        private Label lblGhiChu;
        private TextBox txtGhiChu;

        private Button btnDuyet;
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
            lblTitle = new Label();
            cboTrangThai = new ComboBox();
            dgvYeuCau = new DataGridView();
            lblSoLuong = new Label();
            lblGhiChu = new Label();
            txtGhiChu = new TextBox();
            btnDuyet = new Button();
            btnTuChoi = new Button();
            btnLamMoi = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvYeuCau).BeginInit();
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1020, 600);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Duyệt yêu cầu chuyển phòng";
            BackColor = Color.WhiteSmoke;
            MinimumSize = new Size(870, 480);

            lblTitle.Text = "DUYỆT YÊU CẦU CHUYỂN PHÒNG";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(1020, 40);
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

            dgvYeuCau.Name = "dgvYeuCau";
            dgvYeuCau.Location = new Point(20, 100);
            dgvYeuCau.Size = new Size(980, 340);
            dgvYeuCau.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvYeuCau.ReadOnly = true;
            dgvYeuCau.AllowUserToAddRows = false;
            dgvYeuCau.AllowUserToDeleteRows = false;
            dgvYeuCau.MultiSelect = false;
            dgvYeuCau.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvYeuCau.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvYeuCau.BackgroundColor = Color.White;

            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 yêu cầu";
            lblSoLuong.Location = new Point(20, 450);
            lblSoLuong.Size = new Size(260, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Text = "Ghi chú (tuỳ chọn):";
            lblGhiChu.Location = new Point(300, 450);
            lblGhiChu.Size = new Size(150, 25);
            lblGhiChu.ForeColor = Color.DimGray;
            lblGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Location = new Point(450, 447);
            txtGhiChu.Size = new Size(550, 28);
            txtGhiChu.Font = new Font("Segoe UI", 10F);
            txtGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            btnDuyet.Name = "btnDuyet";
            btnDuyet.Text = "Duyệt";
            btnDuyet.Size = new Size(120, 38);
            btnDuyet.Location = new Point(20, 530);
            btnDuyet.BackColor = Color.SeaGreen;
            btnDuyet.ForeColor = Color.White;
            btnDuyet.FlatStyle = FlatStyle.Flat;
            btnDuyet.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDuyet.Click += btnDuyet_Click;

            btnTuChoi.Name = "btnTuChoi";
            btnTuChoi.Text = "Từ chối";
            btnTuChoi.Size = new Size(120, 38);
            btnTuChoi.Location = new Point(150, 530);
            btnTuChoi.BackColor = Color.IndianRed;
            btnTuChoi.ForeColor = Color.White;
            btnTuChoi.FlatStyle = FlatStyle.Flat;
            btnTuChoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnTuChoi.Click += btnTuChoi_Click;

            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(110, 38);
            btnLamMoi.Location = new Point(780, 530);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLamMoi.Click += btnLamMoi_Click;

            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(890, 530);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(cboTrangThai);
            Controls.Add(dgvYeuCau);
            Controls.Add(lblSoLuong);
            Controls.Add(lblGhiChu);
            Controls.Add(txtGhiChu);
            Controls.Add(btnDuyet);
            Controls.Add(btnTuChoi);
            Controls.Add(btnLamMoi);
            Controls.Add(btnDong);

            Load += FrmDuyetChuyenPhong_Load;

            ((System.ComponentModel.ISupportInitialize)dgvYeuCau).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
