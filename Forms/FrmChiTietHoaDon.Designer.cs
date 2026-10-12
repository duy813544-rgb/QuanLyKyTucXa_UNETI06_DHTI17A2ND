namespace DoAnNhom06.Forms
{
    partial class FrmChiTietHoaDon
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
            pnlThongTin = new Panel();
            lblMaHoaDon = new Label();
            lblMaSv = new Label();
            lblHoTen = new Label();
            lblPhong = new Label();
            lblThangNam = new Label();
            lblNgayLap = new Label();
            lblTrangThai = new Label();
            dgvChiTiet = new DataGridView();
            pnlTong = new Panel();
            lblTongTien = new Label();
            lblTongChiTiet = new Label();
            btnDong = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            pnlThongTin.SuspendLayout();
            pnlTong.SuspendLayout();
            SuspendLayout();

            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            lblTitle.Location = new Point(0, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(900, 35);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "CHI TIẾT HÓA ĐƠN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // pnlThongTin
            // 
            pnlThongTin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlThongTin.BackColor = System.Drawing.Color.AliceBlue;
            pnlThongTin.BorderStyle = BorderStyle.FixedSingle;
            pnlThongTin.Controls.Add(lblMaHoaDon);
            pnlThongTin.Controls.Add(lblMaSv);
            pnlThongTin.Controls.Add(lblHoTen);
            pnlThongTin.Controls.Add(lblPhong);
            pnlThongTin.Controls.Add(lblThangNam);
            pnlThongTin.Controls.Add(lblNgayLap);
            pnlThongTin.Controls.Add(lblTrangThai);
            pnlThongTin.Location = new Point(20, 55);
            pnlThongTin.Name = "pnlThongTin";
            pnlThongTin.Size = new Size(860, 150);
            pnlThongTin.TabIndex = 1;

            // 
            // lblMaHoaDon
            // 
            lblMaHoaDon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMaHoaDon.Location = new Point(15, 15);
            lblMaHoaDon.Name = "lblMaHoaDon";
            lblMaHoaDon.Size = new Size(400, 20);
            lblMaHoaDon.TabIndex = 0;
            lblMaHoaDon.Text = "Mã hóa đơn: ";

            // 
            // lblMaSv
            // 
            lblMaSv.Font = new Font("Segoe UI", 10F);
            lblMaSv.Location = new Point(15, 40);
            lblMaSv.Name = "lblMaSv";
            lblMaSv.Size = new Size(400, 20);
            lblMaSv.TabIndex = 1;
            lblMaSv.Text = "Mã SV: ";

            // 
            // lblHoTen
            // 
            lblHoTen.Font = new Font("Segoe UI", 10F);
            lblHoTen.Location = new Point(15, 65);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(400, 20);
            lblHoTen.TabIndex = 2;
            lblHoTen.Text = "Họ tên: ";

            // 
            // lblPhong
            // 
            lblPhong.Font = new Font("Segoe UI", 10F);
            lblPhong.Location = new Point(450, 15);
            lblPhong.Name = "lblPhong";
            lblPhong.Size = new Size(400, 20);
            lblPhong.TabIndex = 3;
            lblPhong.Text = "Phòng: ";

            // 
            // lblThangNam
            // 
            lblThangNam.Font = new Font("Segoe UI", 10F);
            lblThangNam.Location = new Point(450, 40);
            lblThangNam.Name = "lblThangNam";
            lblThangNam.Size = new Size(400, 20);
            lblThangNam.TabIndex = 4;
            lblThangNam.Text = "Tháng/Năm: ";

            // 
            // lblNgayLap
            // 
            lblNgayLap.Font = new Font("Segoe UI", 10F);
            lblNgayLap.Location = new Point(450, 65);
            lblNgayLap.Name = "lblNgayLap";
            lblNgayLap.Size = new Size(400, 20);
            lblNgayLap.TabIndex = 5;
            lblNgayLap.Text = "Ngày lập: ";

            // 
            // lblTrangThai
            // 
            lblTrangThai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTrangThai.ForeColor = System.Drawing.Color.DarkGreen;
            lblTrangThai.Location = new Point(15, 115);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(400, 20);
            lblTrangThai.TabIndex = 6;
            lblTrangThai.Text = "Trạng thái: ";

            // 
            // dgvChiTiet
            // 
            dgvChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            dgvChiTiet.ColumnHeadersHeight = 29;
            dgvChiTiet.Location = new Point(20, 220);
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(860, 250);
            dgvChiTiet.TabIndex = 2;

            // 
            // pnlTong
            // 
            pnlTong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlTong.BackColor = System.Drawing.Color.LightYellow;
            pnlTong.BorderStyle = BorderStyle.FixedSingle;
            pnlTong.Controls.Add(lblTongTien);
            pnlTong.Controls.Add(lblTongChiTiet);
            pnlTong.Location = new Point(20, 475);
            pnlTong.Name = "pnlTong";
            pnlTong.Size = new Size(860, 60);
            pnlTong.TabIndex = 3;

            // 
            // lblTongTien
            // 
            lblTongTien.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTongTien.ForeColor = System.Drawing.Color.DarkRed;
            lblTongTien.Location = new Point(15, 12);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(500, 28);
            lblTongTien.TabIndex = 0;
            lblTongTien.Text = "Tổng tiền: 0 đ";

            // 
            // lblTongChiTiet
            // 
            lblTongChiTiet.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblTongChiTiet.ForeColor = System.Drawing.Color.Gray;
            lblTongChiTiet.Location = new Point(15, 35);
            lblTongChiTiet.Name = "lblTongChiTiet";
            lblTongChiTiet.Size = new Size(500, 18);
            lblTongChiTiet.TabIndex = 1;
            lblTongChiTiet.Text = "(Tính từ chi tiết: 0 đ)";

            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.BackColor = System.Drawing.Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Font = new Font("Segoe UI", 10F);
            btnDong.Location = new Point(760, 545);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(120, 38);
            btnDong.TabIndex = 4;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = false;
            btnDong.Click += btnDong_Click;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel = new Button();
            btnXuatExcel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnXuatExcel.BackColor = Color.ForestGreen;
            btnXuatExcel.FlatStyle = FlatStyle.Flat;
            btnXuatExcel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXuatExcel.ForeColor = Color.White;
            btnXuatExcel.Location = new Point(620, 545);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(130, 38);
            btnXuatExcel.TabIndex = 5;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.UseVisualStyleBackColor = false;
            btnXuatExcel.Click += btnXuatExcel_Click;

            Controls.Add(btnXuatExcel);
            // 
            // FrmChiTietHoaDon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            ClientSize = new Size(900, 600);
            Controls.Add(lblTitle);
            Controls.Add(pnlThongTin);
            Controls.Add(dgvChiTiet);
            Controls.Add(pnlTong);
            Controls.Add(btnDong);
            MinimumSize = new Size(700, 450);
            Name = "FrmChiTietHoaDon";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết hóa đơn";
            Load += FrmChiTietHoaDon_Load;
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            pnlThongTin.ResumeLayout(false);
            pnlTong.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private Panel pnlThongTin;
        private Label lblMaHoaDon;
        private Label lblMaSv;
        private Label lblHoTen;
        private Label lblPhong;
        private Label lblThangNam;
        private Label lblNgayLap;
        private Label lblTrangThai;
        private DataGridView dgvChiTiet;
        private Panel pnlTong;
        private Label lblTongTien;
        private Label lblTongChiTiet;
        private Button btnDong;
        private Button btnXuatExcel;
    }
}