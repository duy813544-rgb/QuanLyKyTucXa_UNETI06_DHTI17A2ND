namespace DoAnNhom06.Forms
{
    partial class FrmDoiMatKhau
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
            lblMatKhauCu = new Label();
            txtMatKhauCu = new TextBox();
            lblMatKhauMoi = new Label();
            txtMatKhauMoi = new TextBox();
            lblXacNhanMatKhauMoi = new Label();
            txtXacNhanMatKhauMoi = new TextBox();
            chkHienMatKhau = new CheckBox();
            btnLuu = new Button();
            btnHuy = new Button();

            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(480, 380);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Đổi mật khẩu";
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // lblTitle
            lblTitle.Text = "ĐỔI MẬT KHẨU";
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(480, 45);
            lblTitle.Location = new Point(0, 18);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblMatKhauCu
            lblMatKhauCu.Text = "Mật khẩu hiện tại";
            lblMatKhauCu.Location = new Point(40, 85);
            lblMatKhauCu.Size = new Size(140, 25);

            // txtMatKhauCu
            txtMatKhauCu.Name = "txtMatKhauCu";
            txtMatKhauCu.Location = new Point(190, 85);
            txtMatKhauCu.Size = new Size(250, 27);
            txtMatKhauCu.UseSystemPasswordChar = true;

            // lblMatKhauMoi
            lblMatKhauMoi.Text = "Mật khẩu mới";
            lblMatKhauMoi.Location = new Point(40, 130);
            lblMatKhauMoi.Size = new Size(140, 25);

            // txtMatKhauMoi
            txtMatKhauMoi.Name = "txtMatKhauMoi";
            txtMatKhauMoi.Location = new Point(190, 130);
            txtMatKhauMoi.Size = new Size(250, 27);
            txtMatKhauMoi.UseSystemPasswordChar = true;

            // lblXacNhanMatKhauMoi
            lblXacNhanMatKhauMoi.Text = "Xác nhận mật khẩu mới";
            lblXacNhanMatKhauMoi.Location = new Point(40, 175);
            lblXacNhanMatKhauMoi.Size = new Size(140, 25);

            // txtXacNhanMatKhauMoi
            txtXacNhanMatKhauMoi.Name = "txtXacNhanMatKhauMoi";
            txtXacNhanMatKhauMoi.Location = new Point(190, 175);
            txtXacNhanMatKhauMoi.Size = new Size(250, 27);
            txtXacNhanMatKhauMoi.UseSystemPasswordChar = true;

            // chkHienMatKhau
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.Location = new Point(190, 215);
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

            // btnLuu
            btnLuu.Name = "btnLuu";
            btnLuu.Text = "Lưu";
            btnLuu.Size = new Size(120, 40);
            btnLuu.Location = new Point(100, 290);
            btnLuu.BackColor = Color.RoyalBlue;
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Click += btnLuu_Click;

            // btnHuy
            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(120, 40);
            btnHuy.Location = new Point(260, 290);
            btnHuy.BackColor = Color.IndianRed;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblMatKhauCu);
            Controls.Add(txtMatKhauCu);
            Controls.Add(lblMatKhauMoi);
            Controls.Add(txtMatKhauMoi);
            Controls.Add(lblXacNhanMatKhauMoi);
            Controls.Add(txtXacNhanMatKhauMoi);
            Controls.Add(chkHienMatKhau);
            Controls.Add(btnLuu);
            Controls.Add(btnHuy);

            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblMatKhauCu;
        private TextBox txtMatKhauCu;
        private Label lblMatKhauMoi;
        private TextBox txtMatKhauMoi;
        private Label lblXacNhanMatKhauMoi;
        private TextBox txtXacNhanMatKhauMoi;
        private CheckBox chkHienMatKhau;
        private Button btnLuu;
        private Button btnHuy;

        #endregion
    }
}
