namespace DoAnNhom06.Forms
{
    partial class FrmGiaHanHopDong
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblMaSv;
        private TextBox txtMaSv;
        private Button btnTimKiem;
        private Button btnTatCa;

        private DataGridView dgvHopDong;
        private Label lblGhiChu;

        private Button btnGiaHan;
        private Button btnDong;

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
            btnTimKiem = new Button();
            dgvHopDong = new DataGridView();
            lblGhiChu = new Label();
            btnGiaHan = new Button();
            btnDong = new Button();
            btnTatCa = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvHopDong).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.Location = new Point(0, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(900, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "GIA HẠN HỢP ĐỒNG Ở KÝ TÚC XÁ";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMaSv
            // 
            lblMaSv.Location = new Point(20, 62);
            lblMaSv.Name = "lblMaSv";
            lblMaSv.Size = new Size(210, 25);
            lblMaSv.TabIndex = 1;
            lblMaSv.Text = "Nhập Mã số sinh viên :";
            lblMaSv.Click += lblMaSv_Click;
            // 
            // txtMaSv
            // 
            txtMaSv.Location = new Point(230, 58);
            txtMaSv.Name = "txtMaSv";
            txtMaSv.Size = new Size(220, 27);
            txtMaSv.TabIndex = 2;
            txtMaSv.KeyDown += txtMaSv_KeyDown;
            // 
            // btnTimKiem
            // 
            btnTimKiem.BackColor = Color.RoyalBlue;
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Location = new Point(460, 57);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(140, 30);
            btnTimKiem.TabIndex = 3;
            btnTimKiem.Text = "Xem hợp đồng";
            btnTimKiem.UseVisualStyleBackColor = false;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // dgvHopDong
            // 
            dgvHopDong.AllowUserToAddRows = false;
            dgvHopDong.AllowUserToDeleteRows = false;
            dgvHopDong.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHopDong.BackgroundColor = Color.White;
            dgvHopDong.ColumnHeadersHeight = 29;
            dgvHopDong.Location = new Point(20, 100);
            dgvHopDong.MultiSelect = false;
            dgvHopDong.Name = "dgvHopDong";
            dgvHopDong.ReadOnly = true;
            dgvHopDong.RowHeadersWidth = 51;
            dgvHopDong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHopDong.Size = new Size(860, 380);
            dgvHopDong.TabIndex = 4;
            // 
            // lblGhiChu
            // 
            lblGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblGhiChu.ForeColor = Color.DimGray;
            lblGhiChu.Location = new Point(20, 490);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(500, 25);
            lblGhiChu.TabIndex = 5;
            lblGhiChu.Text = "Nhập mã số sinh viên rồi bấm \"Xem hợp đồng\" để tải danh sách.";
            // 
            // btnGiaHan
            // 
            btnGiaHan.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGiaHan.BackColor = Color.SeaGreen;
            btnGiaHan.FlatStyle = FlatStyle.Flat;
            btnGiaHan.ForeColor = Color.White;
            btnGiaHan.Location = new Point(650, 510);
            btnGiaHan.Name = "btnGiaHan";
            btnGiaHan.Size = new Size(120, 38);
            btnGiaHan.TabIndex = 6;
            btnGiaHan.Text = "Gia hạn";
            btnGiaHan.UseVisualStyleBackColor = false;
            btnGiaHan.Click += btnGiaHan_Click;
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Location = new Point(770, 510);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 38);
            btnDong.TabIndex = 7;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = false;
            btnDong.Click += btnDong_Click;
            // 
            // btnTatCa
            // 
            btnTatCa.BackColor = Color.SeaGreen;
            btnTatCa.FlatStyle = FlatStyle.Flat;
            btnTatCa.ForeColor = Color.White;
            btnTatCa.Location = new Point(650, 58);
            btnTatCa.Name = "btnTatCa";
            btnTatCa.Size = new Size(100, 30);
            btnTatCa.TabIndex = 8;
            btnTatCa.Text = "Tất cả";
            btnTatCa.UseVisualStyleBackColor = false;
            btnTatCa.Click += btnTatCa_Click;
            // 
            // FrmGiaHanHopDong
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(900, 560);
            Controls.Add(lblTitle);
            Controls.Add(lblMaSv);
            Controls.Add(txtMaSv);
            Controls.Add(btnTimKiem);
            Controls.Add(dgvHopDong);
            Controls.Add(lblGhiChu);
            Controls.Add(btnGiaHan);
            Controls.Add(btnDong);
            Controls.Add(btnTatCa);
            MinimumSize = new Size(750, 450);
            Name = "FrmGiaHanHopDong";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gia hạn hợp đồng ở ký túc xá";
            Load += FrmGiaHanHopDong_Load;
            ((System.ComponentModel.ISupportInitialize)dgvHopDong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
