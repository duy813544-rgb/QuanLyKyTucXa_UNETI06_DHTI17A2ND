namespace DoAnNhom06.Forms
{
    partial class FrmThemSuaSinhVien
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

            lblMaSv = new Label();
            txtMaSv = new TextBox();

            lblHoTen = new Label();
            txtHoTen = new TextBox();

            lblNgaySinh = new Label();
            dtpNgaySinh = new DateTimePicker();

            lblGioiTinh = new Label();
            cboGioiTinh = new ComboBox();

            lblLop = new Label();
            txtLop = new TextBox();

            lblSoDienThoai = new Label();
            txtSoDienThoai = new TextBox();

            lblQueQuan = new Label();
            txtQueQuan = new TextBox();

            btnLuu = new Button();
            btnHuy = new Button();

            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(520, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Thêm hồ sơ sinh viên";
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // lblTitle
            lblTitle.Text = "HỒ SƠ SINH VIÊN";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(520, 40);
            lblTitle.Location = new Point(0, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblMaSv
            lblMaSv.Text = "Mã sinh viên";
            lblMaSv.Location = new Point(40, 70);
            lblMaSv.Size = new Size(140, 25);

            // txtMaSv
            txtMaSv.Name = "txtMaSv";
            txtMaSv.Location = new Point(200, 70);
            txtMaSv.Size = new Size(280, 27);

            // lblHoTen
            lblHoTen.Text = "Họ tên";
            lblHoTen.Location = new Point(40, 110);
            lblHoTen.Size = new Size(140, 25);

            // txtHoTen
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Location = new Point(200, 110);
            txtHoTen.Size = new Size(280, 27);

            // lblNgaySinh
            lblNgaySinh.Text = "Ngày sinh";
            lblNgaySinh.Location = new Point(40, 150);
            lblNgaySinh.Size = new Size(140, 25);

            // dtpNgaySinh
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Location = new Point(200, 150);
            dtpNgaySinh.Size = new Size(280, 27);
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.ShowCheckBox = true;
            dtpNgaySinh.Checked = false;

            // lblGioiTinh
            lblGioiTinh.Text = "Giới tính";
            lblGioiTinh.Location = new Point(40, 190);
            lblGioiTinh.Size = new Size(140, 25);

            // cboGioiTinh
            cboGioiTinh.Name = "cboGioiTinh";
            cboGioiTinh.Location = new Point(200, 190);
            cboGioiTinh.Size = new Size(280, 27);
            cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });

            // lblLop
            lblLop.Text = "Lớp";
            lblLop.Location = new Point(40, 230);
            lblLop.Size = new Size(140, 25);

            // txtLop
            txtLop.Name = "txtLop";
            txtLop.Location = new Point(200, 230);
            txtLop.Size = new Size(280, 27);

            // lblSoDienThoai
            lblSoDienThoai.Text = "Số điện thoại";
            lblSoDienThoai.Location = new Point(40, 270);
            lblSoDienThoai.Size = new Size(140, 25);

            // txtSoDienThoai
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Location = new Point(200, 270);
            txtSoDienThoai.Size = new Size(280, 27);

            // lblQueQuan
            lblQueQuan.Text = "Quê quán";
            lblQueQuan.Location = new Point(40, 310);
            lblQueQuan.Size = new Size(140, 25);

            // txtQueQuan
            txtQueQuan.Name = "txtQueQuan";
            txtQueQuan.Location = new Point(200, 310);
            txtQueQuan.Size = new Size(280, 27);

            // btnLuu
            btnLuu.Name = "btnLuu";
            btnLuu.Text = "Lưu";
            btnLuu.Size = new Size(120, 40);
            btnLuu.Location = new Point(130, 400);
            btnLuu.BackColor = Color.RoyalBlue;
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Click += btnLuu_Click;

            // btnHuy
            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(120, 40);
            btnHuy.Location = new Point(270, 400);
            btnHuy.BackColor = Color.IndianRed;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblMaSv);
            Controls.Add(txtMaSv);
            Controls.Add(lblHoTen);
            Controls.Add(txtHoTen);
            Controls.Add(lblNgaySinh);
            Controls.Add(dtpNgaySinh);
            Controls.Add(lblGioiTinh);
            Controls.Add(cboGioiTinh);
            Controls.Add(lblLop);
            Controls.Add(txtLop);
            Controls.Add(lblSoDienThoai);
            Controls.Add(txtSoDienThoai);
            Controls.Add(lblQueQuan);
            Controls.Add(txtQueQuan);
            Controls.Add(btnLuu);
            Controls.Add(btnHuy);

            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblMaSv;
        private TextBox txtMaSv;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblGioiTinh;
        private ComboBox cboGioiTinh;
        private Label lblLop;
        private TextBox txtLop;
        private Label lblSoDienThoai;
        private TextBox txtSoDienThoai;
        private Label lblQueQuan;
        private TextBox txtQueQuan;
        private Button btnLuu;
        private Button btnHuy;

        #endregion
    }
}
