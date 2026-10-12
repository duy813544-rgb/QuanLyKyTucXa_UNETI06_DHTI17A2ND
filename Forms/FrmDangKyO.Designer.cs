namespace DoAnNhom06.Forms
{
    partial class FrmDangKyO
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblMaSv;
        private TextBox txtMaSv;
        private Label lblPhong;
        private ComboBox cboPhong;
        private Label lblGhiChuPhong;
        private Label lblNgayBatDau;
        private DateTimePicker dtpNgayBatDau;

        private Button btnDangKy;
        private Button btnHuy;
        private Label lblTrangThaiHienTai;

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
            lblMaSv = new Label();
            txtMaSv = new TextBox();
            lblPhong = new Label();
            cboPhong = new ComboBox();
            lblGhiChuPhong = new Label();
            lblNgayBatDau = new Label();
            dtpNgayBatDau = new DateTimePicker();
            btnDangKy = new Button();
            btnHuy = new Button();
            lblTrangThaiHienTai = new Label();

            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(540, 360);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đăng ký ở ký túc xá";
            BackColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblTitle.Text = "ĐĂNG KÝ Ở KÝ TÚC XÁ";
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(540, 35);
            lblTitle.Location = new Point(0, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblMaSv.Text = "Mã số sinh viên:";
            lblMaSv.Location = new Point(30, 70);
            lblMaSv.Size = new Size(150, 25);

            txtMaSv.Name = "txtMaSv";
            txtMaSv.Location = new Point(200, 66);
            txtMaSv.Size = new Size(290, 27);

            lblPhong.Text = "Chọn phòng còn trống:";
            lblPhong.Location = new Point(30, 115);
            lblPhong.Size = new Size(160, 25);

            cboPhong.Name = "cboPhong";
            cboPhong.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhong.Location = new Point(200, 111);
            cboPhong.Size = new Size(290, 27);

            lblGhiChuPhong.Name = "lblGhiChuPhong";
            lblGhiChuPhong.Location = new Point(200, 141);
            lblGhiChuPhong.Size = new Size(310, 34);
            lblGhiChuPhong.ForeColor = Color.DimGray;
            lblGhiChuPhong.Font = new Font("Segoe UI", 8F);

            lblNgayBatDau.Text = "Ngày bắt đầu mong muốn:";
            lblNgayBatDau.Location = new Point(30, 195);
            lblNgayBatDau.Size = new Size(170, 25);

            dtpNgayBatDau.Name = "dtpNgayBatDau";
            dtpNgayBatDau.Format = DateTimePickerFormat.Short;
            dtpNgayBatDau.Location = new Point(200, 191);
            dtpNgayBatDau.Size = new Size(290, 27);

            btnDangKy.Name = "btnDangKy";
            btnDangKy.Text = "Đăng ký";
            btnDangKy.Size = new Size(140, 40);
            btnDangKy.Location = new Point(120, 265);
            btnDangKy.BackColor = Color.RoyalBlue;
            btnDangKy.ForeColor = Color.White;
            btnDangKy.FlatStyle = FlatStyle.Flat;
            btnDangKy.Click += btnDangKy_Click;

            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(140, 40);
            btnHuy.Location = new Point(280, 265);
            btnHuy.BackColor = Color.Gainsboro;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            // Khung hiển thị tình trạng đăng ký (thay thế toàn bộ Form khi sinh viên đã có
            // yêu cầu đăng ký đang chờ duyệt hoặc đã được duyệt) - ẩn theo mặc định, chỉ hiện
            // khi FrmDangKyO_Load phát hiện đã có yêu cầu trước đó.
            lblTrangThaiHienTai.Name = "lblTrangThaiHienTai";
            lblTrangThaiHienTai.Location = new Point(30, 65);
            lblTrangThaiHienTai.Size = new Size(480, 190);
            lblTrangThaiHienTai.Font = new Font("Segoe UI", 10F);
            lblTrangThaiHienTai.ForeColor = Color.FromArgb(32, 33, 36);
            lblTrangThaiHienTai.Visible = false;

            Controls.Add(lblTitle);
            Controls.Add(lblMaSv);
            Controls.Add(txtMaSv);
            Controls.Add(lblPhong);
            Controls.Add(cboPhong);
            Controls.Add(lblGhiChuPhong);
            Controls.Add(lblNgayBatDau);
            Controls.Add(dtpNgayBatDau);
            Controls.Add(btnDangKy);
            Controls.Add(btnHuy);
            Controls.Add(lblTrangThaiHienTai);

            Load += FrmDangKyO_Load;

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
