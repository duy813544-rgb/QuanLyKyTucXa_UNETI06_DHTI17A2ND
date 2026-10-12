namespace DoAnNhom06.Forms
{
    partial class FrmThemSuaTaiKhoan
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

            lblTenDangNhap = new Label();
            txtTenDangNhap = new TextBox();

            lblMatKhau = new Label();
            txtMatKhau = new TextBox();
            lblGhiChuMatKhau = new Label();

            chkHienMatKhau = new CheckBox();

            lblQuyen = new Label();
            cboQuyen = new ComboBox();

            lblSinhVien = new Label();
            cboSinhVien = new ComboBox();

            btnLuu = new Button();
            btnHuy = new Button();

            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(480, 430);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Thêm tài khoản";
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // lblTitle
            lblTitle.Text = "THÔNG TIN TÀI KHOẢN";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(480, 40);
            lblTitle.Location = new Point(0, 12);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblTenDangNhap
            lblTenDangNhap.Text = "Tên đăng nhập";
            lblTenDangNhap.Location = new Point(35, 65);
            lblTenDangNhap.Size = new Size(140, 25);

            // txtTenDangNhap
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Location = new Point(185, 65);
            txtTenDangNhap.Size = new Size(250, 27);

            // lblMatKhau
            lblMatKhau.Text = "Mật khẩu";
            lblMatKhau.Location = new Point(35, 108);
            lblMatKhau.Size = new Size(140, 25);

            // txtMatKhau
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Location = new Point(185, 108);
            txtMatKhau.Size = new Size(250, 27);
            txtMatKhau.UseSystemPasswordChar = true;

            // lblGhiChuMatKhau
            lblGhiChuMatKhau.Text = "(để trống nếu không đổi mật khẩu)";
            lblGhiChuMatKhau.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblGhiChuMatKhau.ForeColor = Color.Gray;
            lblGhiChuMatKhau.Location = new Point(185, 136);
            lblGhiChuMatKhau.Size = new Size(260, 20);

            // chkHienMatKhau
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.Location = new Point(185, 158);
            chkHienMatKhau.Size = new Size(150, 24);
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

            // lblQuyen
            lblQuyen.Text = "Quyền";
            lblQuyen.Location = new Point(35, 195);
            lblQuyen.Size = new Size(140, 25);

            // cboQuyen
            cboQuyen.Name = "cboQuyen";
            cboQuyen.Location = new Point(185, 195);
            cboQuyen.Size = new Size(250, 27);
            cboQuyen.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuyen.SelectedIndexChanged += cboQuyen_SelectedIndexChanged;

            // lblSinhVien
            lblSinhVien.Text = "Sinh viên liên kết";
            lblSinhVien.Location = new Point(35, 238);
            lblSinhVien.Size = new Size(140, 25);

            // cboSinhVien
            cboSinhVien.Name = "cboSinhVien";
            cboSinhVien.Location = new Point(185, 238);
            cboSinhVien.Size = new Size(250, 27);
            cboSinhVien.DropDownStyle = ComboBoxStyle.DropDownList;

            // btnLuu
            btnLuu.Name = "btnLuu";
            btnLuu.Text = "Lưu";
            btnLuu.Size = new Size(120, 40);
            btnLuu.Location = new Point(100, 350);
            btnLuu.BackColor = Color.RoyalBlue;
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Click += btnLuu_Click;

            // btnHuy
            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(120, 40);
            btnHuy.Location = new Point(250, 350);
            btnHuy.BackColor = Color.IndianRed;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblTenDangNhap);
            Controls.Add(txtTenDangNhap);
            Controls.Add(lblMatKhau);
            Controls.Add(txtMatKhau);
            Controls.Add(lblGhiChuMatKhau);
            Controls.Add(chkHienMatKhau);
            Controls.Add(lblQuyen);
            Controls.Add(cboQuyen);
            Controls.Add(lblSinhVien);
            Controls.Add(cboSinhVien);
            Controls.Add(btnLuu);
            Controls.Add(btnHuy);

            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblTenDangNhap;
        private TextBox txtTenDangNhap;
        private Label lblMatKhau;
        private TextBox txtMatKhau;
        private Label lblGhiChuMatKhau;
        private CheckBox chkHienMatKhau;
        private Label lblQuyen;
        private ComboBox cboQuyen;
        private Label lblSinhVien;
        private ComboBox cboSinhVien;
        private Button btnLuu;
        private Button btnHuy;

        #endregion
    }
}
