namespace DoAnNhom06.Forms
{
    partial class FrmDanhSachPhong
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

            dgvPhong = new DataGridView();

            lblSoLuong = new Label();

            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvPhong).BeginInit();
            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(950, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quản lý phòng";
            this.BackColor = Color.WhiteSmoke;
            this.MinimumSize = new Size(800, 500);

            // lblTitle
            lblTitle.Text = "QUẢN LÝ PHÒNG";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(950, 40);
            lblTitle.Location = new Point(0, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // lblTimKiem
            lblTimKiem.Text = "Tìm kiếm (mã phòng / khu nhà):";
            lblTimKiem.Location = new Point(20, 62);
            lblTimKiem.Size = new Size(200, 25);
            lblTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // txtTimKiem
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Location = new Point(220, 58);
            txtTimKiem.Size = new Size(300, 27);
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;

            // btnTimKiem
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.Location = new Point(530, 57);
            btnTimKiem.BackColor = Color.RoyalBlue;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnTimKiem.Click += btnTimKiem_Click;

            // btnLamMoi
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(100, 30);
            btnLamMoi.Location = new Point(640, 57);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnLamMoi.Click += btnLamMoi_Click;

            // dgvPhong
            dgvPhong.Name = "dgvPhong";
            dgvPhong.Location = new Point(20, 100);
            dgvPhong.Size = new Size(910, 440);
            dgvPhong.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPhong.ReadOnly = true;
            dgvPhong.AllowUserToAddRows = false;
            dgvPhong.AllowUserToDeleteRows = false;
            dgvPhong.MultiSelect = false;
            dgvPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhong.BackgroundColor = Color.White;
            dgvPhong.CellDoubleClick += dgvPhong_CellDoubleClick;

            // lblSoLuong
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 phòng";
            lblSoLuong.Location = new Point(20, 550);
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // btnThem
            btnThem.Name = "btnThem";
            btnThem.Text = "Thêm";
            btnThem.Size = new Size(110, 38);
            btnThem.Location = new Point(430, 570);
            btnThem.BackColor = Color.SeaGreen;
            btnThem.ForeColor = Color.White;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnThem.Click += btnThem_Click;

            // btnSua
            btnSua.Name = "btnSua";
            btnSua.Text = "Sửa";
            btnSua.Size = new Size(110, 38);
            btnSua.Location = new Point(550, 570);
            btnSua.BackColor = Color.RoyalBlue;
            btnSua.ForeColor = Color.White;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSua.Click += btnSua_Click;

            // btnXoa
            btnXoa.Name = "btnXoa";
            btnXoa.Text = "Xóa";
            btnXoa.Size = new Size(110, 38);
            btnXoa.Location = new Point(670, 570);
            btnXoa.BackColor = Color.IndianRed;
            btnXoa.ForeColor = Color.White;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnXoa.Click += btnXoa_Click;

            // btnDong
            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(820, 570);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(btnLamMoi);
            Controls.Add(dgvPhong);
            Controls.Add(lblSoLuong);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnDong);

            Load += FrmDanhSachPhong_Load;

            ((System.ComponentModel.ISupportInitialize)dgvPhong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private Button btnLamMoi;
        private DataGridView dgvPhong;
        private Label lblSoLuong;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnDong;

        #endregion
    }
}
