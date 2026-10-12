namespace DoAnNhom06.Forms
{
    partial class FrmDanhSachSinhVien
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboGioiTinh;

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
            dgvSinhVien = new DataGridView();
            lblSoLuong = new Label();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnDong = new Button();
            cboGioiTinh = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.Location = new Point(0, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(950, 40);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "QUẢN LÝ HỒ SƠ SINH VIÊN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTimKiem
            // 
            lblTimKiem.Location = new Point(0, 62);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(224, 25);
            lblTimKiem.TabIndex = 2;
            lblTimKiem.Text = "Tìm kiếm (mã SV / họ tên / lớp):";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(230, 58);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(300, 27);
            txtTimKiem.TabIndex = 3;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.RoyalBlue;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(540, 57);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.TabIndex = 4;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Location = new Point(650, 57);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(100, 30);
            btnLamMoi.TabIndex = 5;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // dgvSinhVien
            // 
            dgvSinhVien.AllowUserToAddRows = false;
            dgvSinhVien.AllowUserToDeleteRows = false;
            dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSinhVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSinhVien.BackgroundColor = Color.White;
            dgvSinhVien.ColumnHeadersHeight = 29;
            dgvSinhVien.Location = new Point(20, 100);
            dgvSinhVien.MultiSelect = false;
            dgvSinhVien.Name = "dgvSinhVien";
            dgvSinhVien.ReadOnly = true;
            dgvSinhVien.RowHeadersWidth = 51;
            dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSinhVien.Size = new Size(910, 440);
            dgvSinhVien.TabIndex = 6;
            dgvSinhVien.CellDoubleClick += dgvSinhVien_CellDoubleClick;
            // 
            // lblSoLuong
            // 
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Location = new Point(20, 550);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.TabIndex = 7;
            lblSoLuong.Text = "Tổng số: 0 sinh viên";
            // 
            // btnThem
            // 
            btnThem.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnThem.BackColor = Color.SeaGreen;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(430, 570);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(110, 38);
            btnThem.TabIndex = 8;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSua.BackColor = Color.RoyalBlue;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(550, 570);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(110, 38);
            btnSua.TabIndex = 9;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = false;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnXoa.BackColor = Color.IndianRed;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(670, 570);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(110, 38);
            btnXoa.TabIndex = 10;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Location = new Point(820, 570);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 38);
            btnDong.TabIndex = 11;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = false;
            btnDong.Click += btnDong_Click;
            // 
            // cboGioiTinh
            // 
            cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioiTinh.Location = new Point(777, 59);
            cboGioiTinh.Name = "cboGioiTinh";
            cboGioiTinh.Size = new Size(120, 28);
            cboGioiTinh.TabIndex = 0;
            cboGioiTinh.SelectedIndexChanged += cboGioiTinh_SelectedIndexChanged;
            // 
            // FrmDanhSachSinhVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(950, 620);
            Controls.Add(cboGioiTinh);
            Controls.Add(lblTitle);
            Controls.Add(lblTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(btnLamMoi);
            Controls.Add(dgvSinhVien);
            Controls.Add(lblSoLuong);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnDong);
            MinimumSize = new Size(800, 500);
            Name = "FrmDanhSachSinhVien";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý hồ sơ sinh viên";
            Load += FrmDanhSachSinhVien_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSinhVien).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private Button btnLamMoi;
        private DataGridView dgvSinhVien;
        private Label lblSoLuong;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnDong;

        #endregion
    }
}
