namespace DoAnNhom06.Forms
{
    partial class FrmDangNhap
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
            lblPass = new Label();
            txtTenDangNhap = new TextBox();
            txtMatKhau = new TextBox();
            btnDangNhap = new Button();
            btnThoat = new Button();
            chkHienMatKhau = new CheckBox();
            lnkDangKy = new LinkLabel();

            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // lblTitle
            lblTitle.Text = "HỆ THỐNG QUẢN LÝ KÝ TÚC XÁ";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(600, 50);
            lblTitle.Location = new Point(0, 20);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblUser
            lblUser.Text = "Tên đăng nhập";
            lblUser.Location = new Point(80, 100);
            lblUser.Size = new Size(120, 25);

            // txtTenDangNhap
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Location = new Point(220, 100);
            txtTenDangNhap.Size = new Size(260, 27);

            // lblPass
            lblPass.Text = "Mật khẩu";
            lblPass.Location = new Point(80, 160);
            lblPass.Size = new Size(120, 25);

            // txtMatKhau
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Location = new Point(220, 160);
            txtMatKhau.Size = new Size(260, 27);
            txtMatKhau.UseSystemPasswordChar = true;

            // CheckBox
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.Location = new Point(220, 195);
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

            // btnDangNhap
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.Size = new Size(120, 40);
            btnDangNhap.Location = new Point(150, 260);
            btnDangNhap.BackColor = Color.RoyalBlue;
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Click += btnDangNhap_Click;

            // btnThoat
            btnThoat.Name = "btnThoat";
            btnThoat.Text = "Thoát";
            btnThoat.Size = new Size(120, 40);
            btnThoat.Location = new Point(320, 260);
            btnThoat.BackColor = Color.IndianRed;
            btnThoat.ForeColor = Color.White;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.Click += btnThoat_Click;

            // lnkDangKy
            lnkDangKy.Name = "lnkDangKy";
            lnkDangKy.Text = "Chưa có tài khoản? Đăng ký ngay";
            lnkDangKy.AutoSize = false;
            lnkDangKy.Size = new Size(600, 25);
            lnkDangKy.Location = new Point(0, 330);
            lnkDangKy.TextAlign = ContentAlignment.MiddleCenter;
            lnkDangKy.LinkColor = Color.RoyalBlue;
            lnkDangKy.LinkClicked += lnkDangKy_LinkClicked;

            Controls.Add(lblTitle);
            Controls.Add(lblUser);
            Controls.Add(txtTenDangNhap);
            Controls.Add(lblPass);
            Controls.Add(txtMatKhau);
            Controls.Add(chkHienMatKhau);
            Controls.Add(btnDangNhap);
            Controls.Add(btnThoat);
            Controls.Add(lnkDangKy);

            Load += FrmDangNhap_Load;

            ResumeLayout(false);
            PerformLayout();
        }
        private Label lblTitle;
        private Label lblUser;
        private Label lblPass;

        private TextBox txtTenDangNhap;
        private TextBox txtMatKhau;

        private Button btnDangNhap;
        private Button btnThoat;
        private CheckBox chkHienMatKhau;
        private LinkLabel lnkDangKy;

        #endregion

    }
}