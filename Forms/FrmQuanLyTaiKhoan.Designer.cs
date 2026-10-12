namespace DoAnNhom06.Forms
{
    partial class FrmQuanLyTaiKhoan
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
            cboLocQuyen = new ComboBox();

            dgvTaiKhoan = new DataGridView();

            lblSoLuong = new Label();

            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvTaiKhoan).BeginInit();
            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(980, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý tài khoản";
            this.BackColor = Color.WhiteSmoke;
            this.MinimumSize = new Size(820, 500);

            // lblTitle
            lblTitle.Text = "QUẢN LÝ TÀI KHOẢN";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(980, 40);
            lblTitle.Location = new Point(0, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // lblTimKiem
            lblTimKiem.Text = "Tìm kiếm (tên đăng nhập / mã SV):";
            lblTimKiem.Location = new Point(20, 62);
            lblTimKiem.Size = new Size(220, 25);
            lblTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // txtTimKiem
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Location = new Point(240, 58);
            txtTimKiem.Size = new Size(260, 27);
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;

            // cboLocQuyen
            cboLocQuyen.Name = "cboLocQuyen";
            cboLocQuyen.Location = new Point(510, 58);
            cboLocQuyen.Size = new Size(160, 27);
            cboLocQuyen.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocQuyen.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cboLocQuyen.SelectedIndexChanged += cboLocQuyen_SelectedIndexChanged;

            // btnTimKiem
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.Location = new Point(680, 57);
            btnTimKiem.BackColor = Color.RoyalBlue;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnTimKiem.Click += btnTimKiem_Click;

            // btnLamMoi
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(100, 30);
            btnLamMoi.Location = new Point(790, 57);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnLamMoi.Click += btnLamMoi_Click;

            // dgvTaiKhoan
            dgvTaiKhoan.Name = "dgvTaiKhoan";
            dgvTaiKhoan.Location = new Point(20, 100);
            dgvTaiKhoan.Size = new Size(940, 440);
            dgvTaiKhoan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTaiKhoan.ReadOnly = true;
            dgvTaiKhoan.AllowUserToAddRows = false;
            dgvTaiKhoan.AllowUserToDeleteRows = false;
            dgvTaiKhoan.MultiSelect = false;
            dgvTaiKhoan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTaiKhoan.BackgroundColor = Color.White;
            dgvTaiKhoan.CellDoubleClick += dgvTaiKhoan_CellDoubleClick;

            // lblSoLuong
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 tài khoản";
            lblSoLuong.Location = new Point(20, 550);
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // btnThem
            btnThem.Name = "btnThem";
            btnThem.Text = "Thêm";
            btnThem.Size = new Size(110, 38);
            btnThem.Location = new Point(460, 570);
            btnThem.BackColor = Color.SeaGreen;
            btnThem.ForeColor = Color.White;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnThem.Click += btnThem_Click;

            // btnSua
            btnSua.Name = "btnSua";
            btnSua.Text = "Sửa";
            btnSua.Size = new Size(110, 38);
            btnSua.Location = new Point(580, 570);
            btnSua.BackColor = Color.RoyalBlue;
            btnSua.ForeColor = Color.White;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSua.Click += btnSua_Click;

            // btnXoa
            btnXoa.Name = "btnXoa";
            btnXoa.Text = "Xóa";
            btnXoa.Size = new Size(110, 38);
            btnXoa.Location = new Point(700, 570);
            btnXoa.BackColor = Color.IndianRed;
            btnXoa.ForeColor = Color.White;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnXoa.Click += btnXoa_Click;

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
            Controls.Add(cboLocQuyen);
            Controls.Add(btnTimKiem);
            Controls.Add(btnLamMoi);
            Controls.Add(dgvTaiKhoan);
            Controls.Add(lblSoLuong);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnDong);

            Load += FrmQuanLyTaiKhoan_Load;

            ((System.ComponentModel.ISupportInitialize)dgvTaiKhoan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private ComboBox cboLocQuyen;
        private Button btnTimKiem;
        private Button btnLamMoi;
        private DataGridView dgvTaiKhoan;
        private Label lblSoLuong;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnDong;

        #endregion
    }
}
