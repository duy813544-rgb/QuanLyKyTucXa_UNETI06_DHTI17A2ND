namespace DoAnNhom06.Forms
{
    partial class FrmDangKy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblUser = new Label();
            txtTenDangNhap = new TextBox();
            lblPass = new Label();
            txtMatKhau = new TextBox();
            lblConfirmPass = new Label();
            txtXacNhanMatKhau = new TextBox();
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            lblNgaySinh = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblGioiTinh = new Label();
            cboGioiTinh = new ComboBox();
            lblSoDienThoai = new Label();
            txtSoDienThoai = new TextBox();
            lblQueQuan = new Label();
            txtQueQuan = new TextBox();
            lblGhiChuDinhDanh = new Label();
            chkHienMatKhau = new CheckBox();
            btnDangKy = new Button();
            btnHuy = new Button();

            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 680);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Đăng ký tài khoản";
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // lblTitle
            lblTitle.Text = "ĐĂNG KÝ TÀI KHOẢN";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(600, 50);
            lblTitle.Location = new Point(0, 20);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblUser
            lblUser.Text = "Tên đăng nhập";
            lblUser.Location = new Point(80, 85);
            lblUser.Size = new Size(120, 25);

            // txtTenDangNhap
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Location = new Point(220, 85);
            txtTenDangNhap.Size = new Size(260, 27);

            // lblPass
            lblPass.Text = "Mật khẩu";
            lblPass.Location = new Point(80, 128);
            lblPass.Size = new Size(120, 25);

            // txtMatKhau
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Location = new Point(220, 128);
            txtMatKhau.Size = new Size(260, 27);
            txtMatKhau.UseSystemPasswordChar = true;

            // lblConfirmPass
            lblConfirmPass.Text = "Xác nhận mật khẩu";
            lblConfirmPass.Location = new Point(80, 171);
            lblConfirmPass.Size = new Size(130, 25);

            // txtXacNhanMatKhau
            txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
            txtXacNhanMatKhau.Location = new Point(220, 171);
            txtXacNhanMatKhau.Size = new Size(260, 27);
            txtXacNhanMatKhau.UseSystemPasswordChar = true;

            // lblHoTen
            lblHoTen.Text = "Họ tên";
            lblHoTen.Location = new Point(80, 224);
            lblHoTen.Size = new Size(130, 25);

            // txtHoTen
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Location = new Point(220, 224);
            txtHoTen.Size = new Size(260, 27);

            // lblNgaySinh
            lblNgaySinh.Text = "Ngày sinh";
            lblNgaySinh.Location = new Point(80, 267);
            lblNgaySinh.Size = new Size(130, 25);

            // dtpNgaySinh
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Location = new Point(220, 267);
            dtpNgaySinh.Size = new Size(260, 27);
            dtpNgaySinh.Format = DateTimePickerFormat.Short;

            // lblGioiTinh
            lblGioiTinh.Text = "Giới tính";
            lblGioiTinh.Location = new Point(80, 310);
            lblGioiTinh.Size = new Size(130, 25);

            // cboGioiTinh
            cboGioiTinh.Name = "cboGioiTinh";
            cboGioiTinh.Location = new Point(220, 310);
            cboGioiTinh.Size = new Size(260, 27);
            cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });

            // lblSoDienThoai
            lblSoDienThoai.Text = "Số điện thoại";
            lblSoDienThoai.Location = new Point(80, 353);
            lblSoDienThoai.Size = new Size(130, 25);

            // txtSoDienThoai
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Location = new Point(220, 353);
            txtSoDienThoai.Size = new Size(260, 27);

            // lblQueQuan
            lblQueQuan.Text = "Quê quán";
            lblQueQuan.Location = new Point(80, 396);
            lblQueQuan.Size = new Size(130, 25);

            // txtQueQuan
            txtQueQuan.Name = "txtQueQuan";
            txtQueQuan.Location = new Point(220, 396);
            txtQueQuan.Size = new Size(260, 27);

            // lblGhiChuDinhDanh
            lblGhiChuDinhDanh.Text = "Hệ thống sẽ tự tạo hồ sơ và sinh Mã số sinh viên cho bạn,\nkhông cần nhập mã có sẵn.";
            lblGhiChuDinhDanh.Location = new Point(220, 425);
            lblGhiChuDinhDanh.Size = new Size(320, 36);
            lblGhiChuDinhDanh.ForeColor = Color.DimGray;
            lblGhiChuDinhDanh.Font = new Font("Segoe UI", 8F);

            // chkHienMatKhau
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.Location = new Point(220, 471);
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

            // btnDangKy
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Text = "Đăng ký";
            btnDangKy.Size = new Size(120, 40);
            btnDangKy.Location = new Point(150, 560);
            btnDangKy.BackColor = Color.RoyalBlue;
            btnDangKy.ForeColor = Color.White;
            btnDangKy.FlatStyle = FlatStyle.Flat;
            btnDangKy.Click += btnDangKy_Click;

            // btnHuy
            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(120, 40);
            btnHuy.Location = new Point(320, 560);
            btnHuy.BackColor = Color.IndianRed;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblUser);
            Controls.Add(txtTenDangNhap);
            Controls.Add(lblPass);
            Controls.Add(txtMatKhau);
            Controls.Add(lblConfirmPass);
            Controls.Add(txtXacNhanMatKhau);
            Controls.Add(lblHoTen);
            Controls.Add(txtHoTen);
            Controls.Add(lblNgaySinh);
            Controls.Add(dtpNgaySinh);
            Controls.Add(lblGioiTinh);
            Controls.Add(cboGioiTinh);
            Controls.Add(lblSoDienThoai);
            Controls.Add(txtSoDienThoai);
            Controls.Add(lblQueQuan);
            Controls.Add(txtQueQuan);
            Controls.Add(lblGhiChuDinhDanh);
            Controls.Add(chkHienMatKhau);
            Controls.Add(btnDangKy);
            Controls.Add(btnHuy);

            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblUser;
        private TextBox txtTenDangNhap;
        private Label lblPass;
        private TextBox txtMatKhau;
        private Label lblConfirmPass;
        private TextBox txtXacNhanMatKhau;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblGioiTinh;
        private ComboBox cboGioiTinh;
        private Label lblSoDienThoai;
        private TextBox txtSoDienThoai;
        private Label lblQueQuan;
        private TextBox txtQueQuan;
        private Label lblGhiChuDinhDanh;
        private CheckBox chkHienMatKhau;
        private Button btnDangKy;
        private Button btnHuy;

        #endregion
    }
}
