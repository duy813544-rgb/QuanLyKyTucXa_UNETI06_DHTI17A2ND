namespace DoAnNhom06.Forms
{
    partial class FrmThongTinCaNhan
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
            pnlHeader = new Panel();
            pnlAvatar = new Panel();
            lblHoTenHeader = new Label();
            lblMaSvHeader = new Label();
            lblMucHocVan = new Label();
            pnlKeHocVan = new Panel();
            lblLblMaSv = new Label();
            lblGtMaSv = new Label();
            lblLblLop = new Label();
            lblGtLop = new Label();
            lblMucCaNhan = new Label();
            pnlKeCaNhan = new Panel();
            lblLblNgaySinh = new Label();
            lblGtNgaySinh = new Label();
            lblLblGioiTinh = new Label();
            lblGtGioiTinh = new Label();
            lblLblSdt = new Label();
            lblGtSdt = new Label();
            lblLblQueQuan = new Label();
            lblGtQueQuan = new Label();
            btnDong = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(24, 49, 95);
            pnlHeader.Controls.Add(pnlAvatar);
            pnlHeader.Controls.Add(lblHoTenHeader);
            pnlHeader.Controls.Add(lblMaSvHeader);
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(620, 110);
            pnlHeader.TabIndex = 0;
            // 
            // pnlAvatar
            // 
            pnlAvatar.BackColor = Color.Transparent;
            pnlAvatar.Location = new Point(24, 20);
            pnlAvatar.Name = "pnlAvatar";
            pnlAvatar.Size = new Size(70, 70);
            pnlAvatar.TabIndex = 0;
            pnlAvatar.Paint += pnlAvatar_Paint;
            // 
            // lblHoTenHeader
            // 
            lblHoTenHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHoTenHeader.ForeColor = Color.White;
            lblHoTenHeader.Location = new Point(112, 26);
            lblHoTenHeader.Name = "lblHoTenHeader";
            lblHoTenHeader.Size = new Size(460, 28);
            lblHoTenHeader.TabIndex = 1;
            // 
            // lblMaSvHeader
            // 
            lblMaSvHeader.Font = new Font("Segoe UI", 9.5F);
            lblMaSvHeader.ForeColor = Color.FromArgb(190, 205, 230);
            lblMaSvHeader.Location = new Point(112, 58);
            lblMaSvHeader.Name = "lblMaSvHeader";
            lblMaSvHeader.Size = new Size(460, 22);
            lblMaSvHeader.TabIndex = 2;
            // 
            // lblMucHocVan
            // 
            lblMucHocVan.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblMucHocVan.ForeColor = Color.RoyalBlue;
            lblMucHocVan.Location = new Point(24, 128);
            lblMucHocVan.Name = "lblMucHocVan";
            lblMucHocVan.Size = new Size(320, 24);
            lblMucHocVan.TabIndex = 1;
            lblMucHocVan.Text = "THÔNG TIN HỌC VẤN";
            // 
            // pnlKeHocVan
            // 
            pnlKeHocVan.BackColor = Color.Gainsboro;
            pnlKeHocVan.Location = new Point(24, 155);
            pnlKeHocVan.Name = "pnlKeHocVan";
            pnlKeHocVan.Size = new Size(572, 1);
            pnlKeHocVan.TabIndex = 2;
            // 
            // lblLblMaSv
            // 
            lblLblMaSv.ForeColor = Color.DimGray;
            lblLblMaSv.Location = new Point(24, 168);
            lblLblMaSv.Name = "lblLblMaSv";
            lblLblMaSv.Size = new Size(120, 22);
            lblLblMaSv.TabIndex = 3;
            lblLblMaSv.Text = "Mã sinh viên";
            // 
            // lblGtMaSv
            // 
            lblGtMaSv.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblGtMaSv.Location = new Point(24, 188);
            lblGtMaSv.Name = "lblGtMaSv";
            lblGtMaSv.Size = new Size(260, 24);
            lblGtMaSv.TabIndex = 4;
            // 
            // lblLblLop
            // 
            lblLblLop.ForeColor = Color.DimGray;
            lblLblLop.Location = new Point(320, 168);
            lblLblLop.Name = "lblLblLop";
            lblLblLop.Size = new Size(120, 22);
            lblLblLop.TabIndex = 5;
            lblLblLop.Text = "Lớp";
            // 
            // lblGtLop
            // 
            lblGtLop.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblGtLop.Location = new Point(320, 188);
            lblGtLop.Name = "lblGtLop";
            lblGtLop.Size = new Size(260, 24);
            lblGtLop.TabIndex = 6;
            // 
            // lblMucCaNhan
            // 
            lblMucCaNhan.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblMucCaNhan.ForeColor = Color.RoyalBlue;
            lblMucCaNhan.Location = new Point(24, 228);
            lblMucCaNhan.Name = "lblMucCaNhan";
            lblMucCaNhan.Size = new Size(320, 24);
            lblMucCaNhan.TabIndex = 7;
            lblMucCaNhan.Text = "THÔNG TIN CÁ NHÂN";
            // 
            // pnlKeCaNhan
            // 
            pnlKeCaNhan.BackColor = Color.Gainsboro;
            pnlKeCaNhan.Location = new Point(24, 255);
            pnlKeCaNhan.Name = "pnlKeCaNhan";
            pnlKeCaNhan.Size = new Size(572, 1);
            pnlKeCaNhan.TabIndex = 8;
            // 
            // lblLblNgaySinh
            // 
            lblLblNgaySinh.ForeColor = Color.DimGray;
            lblLblNgaySinh.Location = new Point(24, 268);
            lblLblNgaySinh.Name = "lblLblNgaySinh";
            lblLblNgaySinh.Size = new Size(120, 22);
            lblLblNgaySinh.TabIndex = 9;
            lblLblNgaySinh.Text = "Ngày sinh";
            // 
            // lblGtNgaySinh
            // 
            lblGtNgaySinh.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblGtNgaySinh.Location = new Point(24, 288);
            lblGtNgaySinh.Name = "lblGtNgaySinh";
            lblGtNgaySinh.Size = new Size(260, 24);
            lblGtNgaySinh.TabIndex = 10;
            // 
            // lblLblGioiTinh
            // 
            lblLblGioiTinh.ForeColor = Color.DimGray;
            lblLblGioiTinh.Location = new Point(320, 268);
            lblLblGioiTinh.Name = "lblLblGioiTinh";
            lblLblGioiTinh.Size = new Size(120, 22);
            lblLblGioiTinh.TabIndex = 11;
            lblLblGioiTinh.Text = "Giới tính";
            // 
            // lblGtGioiTinh
            // 
            lblGtGioiTinh.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblGtGioiTinh.Location = new Point(320, 288);
            lblGtGioiTinh.Name = "lblGtGioiTinh";
            lblGtGioiTinh.Size = new Size(260, 24);
            lblGtGioiTinh.TabIndex = 12;
            // 
            // lblLblSdt
            // 
            lblLblSdt.ForeColor = Color.DimGray;
            lblLblSdt.Location = new Point(24, 318);
            lblLblSdt.Name = "lblLblSdt";
            lblLblSdt.Size = new Size(120, 22);
            lblLblSdt.TabIndex = 13;
            lblLblSdt.Text = "Số điện thoại";
            // 
            // lblGtSdt
            // 
            lblGtSdt.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblGtSdt.Location = new Point(24, 338);
            lblGtSdt.Name = "lblGtSdt";
            lblGtSdt.Size = new Size(260, 24);
            lblGtSdt.TabIndex = 14;
            // 
            // lblLblQueQuan
            // 
            lblLblQueQuan.ForeColor = Color.DimGray;
            lblLblQueQuan.Location = new Point(320, 318);
            lblLblQueQuan.Name = "lblLblQueQuan";
            lblLblQueQuan.Size = new Size(120, 22);
            lblLblQueQuan.TabIndex = 15;
            lblLblQueQuan.Text = "Quê quán";
            // 
            // lblGtQueQuan
            // 
            lblGtQueQuan.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblGtQueQuan.Location = new Point(320, 338);
            lblGtQueQuan.Name = "lblGtQueQuan";
            lblGtQueQuan.Size = new Size(260, 24);
            lblGtQueQuan.TabIndex = 16;
            // 
            // btnDong
            // 
            btnDong.BackColor = Color.RoyalBlue;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.ForeColor = Color.White;
            btnDong.Location = new Point(486, 391);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 36);
            btnDong.TabIndex = 17;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = false;
            btnDong.Click += btnDong_Click;
            // 
            // FrmThongTinCaNhan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(620, 499);
            Controls.Add(pnlHeader);
            Controls.Add(lblMucHocVan);
            Controls.Add(pnlKeHocVan);
            Controls.Add(lblLblMaSv);
            Controls.Add(lblGtMaSv);
            Controls.Add(lblLblLop);
            Controls.Add(lblGtLop);
            Controls.Add(lblMucCaNhan);
            Controls.Add(pnlKeCaNhan);
            Controls.Add(lblLblNgaySinh);
            Controls.Add(lblGtNgaySinh);
            Controls.Add(lblLblGioiTinh);
            Controls.Add(lblGtGioiTinh);
            Controls.Add(lblLblSdt);
            Controls.Add(lblGtSdt);
            Controls.Add(lblLblQueQuan);
            Controls.Add(lblGtQueQuan);
            Controls.Add(btnDong);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmThongTinCaNhan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thông tin cá nhân";
            Load += FrmThongTinCaNhan_Load;
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlAvatar;
        private Label lblHoTenHeader;
        private Label lblMaSvHeader;

        private Label lblMucHocVan;
        private Panel pnlKeHocVan;
        private Label lblLblMaSv;
        private Label lblGtMaSv;
        private Label lblLblLop;
        private Label lblGtLop;

        private Label lblMucCaNhan;
        private Panel pnlKeCaNhan;
        private Label lblLblNgaySinh;
        private Label lblGtNgaySinh;
        private Label lblLblGioiTinh;
        private Label lblGtGioiTinh;
        private Label lblLblSdt;
        private Label lblGtSdt;
        private Label lblLblQueQuan;
        private Label lblGtQueQuan;

        private Button btnDong;

        #endregion
    }
}
