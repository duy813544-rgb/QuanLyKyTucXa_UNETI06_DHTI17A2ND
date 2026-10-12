namespace DoAnNhom06.Forms
{
    partial class FrmInHoaDon
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
            dgvHoaDon = new DataGridView();
            lblSoLuong = new Label();
            btnInHoaDon = new Button();
            btnDong = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.Location = new Point(0, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(980, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "IN HÓA ĐƠN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTimKiem
            // 
            lblTimKiem.Location = new Point(20, 62);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(250, 25);
            lblTimKiem.TabIndex = 1;
            lblTimKiem.Text = "Tìm kiếm (mã hóa đơn / mã SV / họ tên):";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(270, 58);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(280, 27);
            txtTimKiem.TabIndex = 2;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.RoyalBlue;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(560, 57);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(100, 30);
            btnTimKiem.TabIndex = 3;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Location = new Point(670, 57);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(100, 30);
            btnLamMoi.TabIndex = 4;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // dgvHoaDon
            // 
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.AllowUserToDeleteRows = false;
            dgvHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.BackgroundColor = Color.White;
            dgvHoaDon.ColumnHeadersHeight = 29;
            dgvHoaDon.Location = new Point(20, 100);
            dgvHoaDon.MultiSelect = false;
            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.RowHeadersWidth = 51;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.Size = new Size(940, 440);
            dgvHoaDon.TabIndex = 5;
            dgvHoaDon.CellDoubleClick += dgvHoaDon_CellDoubleClick;
            // 
            // lblSoLuong
            // 
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Location = new Point(20, 550);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.TabIndex = 6;
            lblSoLuong.Text = "Tổng số: 0 hóa đơn";
            // 
            // btnInHoaDon
            // 
            btnInHoaDon.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnInHoaDon.BackColor = Color.RoyalBlue;
            btnInHoaDon.FlatStyle = FlatStyle.Flat;
            btnInHoaDon.ForeColor = Color.White;
            btnInHoaDon.Location = new Point(690, 570);
            btnInHoaDon.Name = "btnInHoaDon";
            btnInHoaDon.Size = new Size(120, 38);
            btnInHoaDon.TabIndex = 8;
            btnInHoaDon.Text = "Xuất Excel";
            btnInHoaDon.UseVisualStyleBackColor = false;
            btnInHoaDon.Click += btnInHoaDon_Click;
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Location = new Point(850, 570);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 38);
            btnDong.TabIndex = 9;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = false;
            btnDong.Click += btnDong_Click;
            // 
            // FrmInHoaDon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(980, 620);
            Controls.Add(lblTitle);
            Controls.Add(lblTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(btnTimKiem);
            Controls.Add(btnLamMoi);
            Controls.Add(dgvHoaDon);
            Controls.Add(lblSoLuong);
            Controls.Add(btnInHoaDon);
            Controls.Add(btnDong);
            MinimumSize = new Size(820, 500);
            Name = "FrmInHoaDon";
            StartPosition = FormStartPosition.CenterParent;
            Text = "In hóa đơn";
            Load += FrmInHoaDon_Load;
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private Button btnLamMoi;
        private DataGridView dgvHoaDon;
        private Label lblSoLuong;
        private Button btnInHoaDon;
        private Button btnDong;

        #endregion
    }
}
