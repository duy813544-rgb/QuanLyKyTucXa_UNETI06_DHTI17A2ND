namespace DoAnNhom06.Forms
{
    partial class FrmDuyetDangKy
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private DataGridView dgvChoDuyet;
        private Label lblSoLuong;

        private Button btnDuyet;
        private Button btnTuChoi;
        private Button btnLamMoi;
        private Button btnDong;
        private ComboBox cboTrangThai;

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
            dgvChoDuyet = new DataGridView();
            lblSoLuong = new Label();
            btnDuyet = new Button();
            btnTuChoi = new Button();
            btnLamMoi = new Button();
            btnDong = new Button();
            cboTrangThai = new ComboBox();

            ((System.ComponentModel.ISupportInitialize)dgvChoDuyet).BeginInit();
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 560);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Duyệt đăng ký ở ký túc xá";
            BackColor = Color.WhiteSmoke;
            MinimumSize = new Size(800, 450);

            lblTitle.Text = "DUYỆT ĐĂNG KÝ Ở KÝ TÚC XÁ";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(950, 40);
            lblTitle.Location = new Point(0, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.Location = new Point(20, 58);
            cboTrangThai.Size = new Size(160, 30);
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Font = new Font("Segoe UI", 10F);
            cboTrangThai.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cboTrangThai.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;

            dgvChoDuyet.Name = "dgvChoDuyet";
            //dgvChoDuyet.Location = new Point(20, 60);
            dgvChoDuyet.Location = new Point(20, 100);
            //gvChoDuyet.Size = new Size(910, 400);
               dgvChoDuyet.Size = new Size(910, 360);
            dgvChoDuyet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChoDuyet.ReadOnly = true;
            dgvChoDuyet.AllowUserToAddRows = false;
            dgvChoDuyet.AllowUserToDeleteRows = false;
            dgvChoDuyet.MultiSelect = false;
            dgvChoDuyet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChoDuyet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChoDuyet.BackgroundColor = Color.White;

            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 yêu cầu chờ duyệt";
            lblSoLuong.Location = new Point(20, 470);
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btnDuyet.Name = "btnDuyet";
            btnDuyet.Text = "Duyệt";
            btnDuyet.Size = new Size(120, 38);
            btnDuyet.Location = new Point(420, 490);
            btnDuyet.BackColor = Color.SeaGreen;
            btnDuyet.ForeColor = Color.White;
            btnDuyet.FlatStyle = FlatStyle.Flat;
            btnDuyet.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDuyet.Click += btnDuyet_Click;

            btnTuChoi.Name = "btnTuChoi";
            btnTuChoi.Text = "Từ chối";
            btnTuChoi.Size = new Size(120, 38);
            btnTuChoi.Location = new Point(550, 490);
            btnTuChoi.BackColor = Color.IndianRed;
            btnTuChoi.ForeColor = Color.White;
            btnTuChoi.FlatStyle = FlatStyle.Flat;
            btnTuChoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnTuChoi.Click += btnTuChoi_Click;

            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(110, 38);
            btnLamMoi.Location = new Point(680, 490);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLamMoi.Click += btnLamMoi_Click;

            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(820, 490);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(cboTrangThai);
            Controls.Add(dgvChoDuyet);
            Controls.Add(lblSoLuong);
            Controls.Add(btnDuyet);
            Controls.Add(btnTuChoi);
            Controls.Add(btnLamMoi);
            Controls.Add(btnDong);

            Load += FrmDuyetDangKy_Load;

            ((System.ComponentModel.ISupportInitialize)dgvChoDuyet).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
