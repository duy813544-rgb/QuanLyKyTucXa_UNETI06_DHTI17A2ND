namespace DoAnNhom06.Forms
{
    partial class FrmXinChuyenPhong
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblPhongHienTai;
        private Label lblPhongMoi;
        private ComboBox cboPhongMoi;
        private Label lblLyDo;
        private TextBox txtLyDo;
        private Button btnGuiYeuCau;

        private Label lblDsTitle;
        private DataGridView dgvYeuCau;
        private Label lblSoLuong;

        private Button btnLamMoi;
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
            lblPhongHienTai = new Label();
            lblPhongMoi = new Label();
            cboPhongMoi = new ComboBox();
            lblLyDo = new Label();
            txtLyDo = new TextBox();
            btnGuiYeuCau = new Button();
            lblDsTitle = new Label();
            dgvYeuCau = new DataGridView();
            lblSoLuong = new Label();
            btnLamMoi = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvYeuCau).BeginInit();
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 660);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xin chuyển phòng";
            BackColor = Color.WhiteSmoke;
            MinimumSize = new Size(620, 540);

            lblTitle.Text = "XIN CHUYỂN PHÒNG";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(720, 36);
            lblTitle.Location = new Point(0, 10);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblPhongHienTai.Name = "lblPhongHienTai";
            lblPhongHienTai.Text = "Phòng hiện tại: ";
            lblPhongHienTai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPhongHienTai.ForeColor = Color.DimGray;
            lblPhongHienTai.Location = new Point(20, 55);
            lblPhongHienTai.Size = new Size(680, 44);
            lblPhongHienTai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblPhongMoi.Name = "lblPhongMoi";
            lblPhongMoi.Text = "Chuyển đến phòng:";
            lblPhongMoi.Location = new Point(20, 105);
            lblPhongMoi.Size = new Size(150, 25);
            lblPhongMoi.Font = new Font("Segoe UI", 10F);
            lblPhongMoi.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            cboPhongMoi.Name = "cboPhongMoi";
            cboPhongMoi.Location = new Point(180, 103);
            cboPhongMoi.Size = new Size(400, 30);
            cboPhongMoi.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhongMoi.Font = new Font("Segoe UI", 10F);
            cboPhongMoi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblLyDo.Name = "lblLyDo";
            lblLyDo.Text = "Lý do (tuỳ chọn):";
            lblLyDo.Location = new Point(20, 145);
            lblLyDo.Size = new Size(250, 22);
            lblLyDo.Font = new Font("Segoe UI", 10F);
            lblLyDo.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            txtLyDo.Name = "txtLyDo";
            txtLyDo.Location = new Point(20, 169);
            txtLyDo.Size = new Size(680, 60);
            txtLyDo.Multiline = true;
            txtLyDo.Font = new Font("Segoe UI", 10F);
            txtLyDo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnGuiYeuCau.Name = "btnGuiYeuCau";
            btnGuiYeuCau.Text = "Gửi yêu cầu";
            btnGuiYeuCau.Size = new Size(150, 38);
            btnGuiYeuCau.Location = new Point(20, 240);
            btnGuiYeuCau.BackColor = Color.SeaGreen;
            btnGuiYeuCau.ForeColor = Color.White;
            btnGuiYeuCau.FlatStyle = FlatStyle.Flat;
            btnGuiYeuCau.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnGuiYeuCau.Click += btnGuiYeuCau_Click;

            lblDsTitle.Name = "lblDsTitle";
            lblDsTitle.Text = "Các yêu cầu đã gửi:";
            lblDsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDsTitle.Location = new Point(20, 293);
            lblDsTitle.Size = new Size(300, 24);
            lblDsTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            dgvYeuCau.Name = "dgvYeuCau";
            dgvYeuCau.Location = new Point(20, 320);
            dgvYeuCau.Size = new Size(680, 260);
            dgvYeuCau.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvYeuCau.ReadOnly = true;
            dgvYeuCau.AllowUserToAddRows = false;
            dgvYeuCau.AllowUserToDeleteRows = false;
            dgvYeuCau.MultiSelect = false;
            dgvYeuCau.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvYeuCau.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvYeuCau.BackgroundColor = Color.White;

            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 yêu cầu đã gửi";
            lblSoLuong.Location = new Point(20, 590);
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(110, 38);
            btnLamMoi.Location = new Point(470, 612);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLamMoi.Click += btnLamMoi_Click;

            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(590, 612);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblPhongHienTai);
            Controls.Add(lblPhongMoi);
            Controls.Add(cboPhongMoi);
            Controls.Add(lblLyDo);
            Controls.Add(txtLyDo);
            Controls.Add(btnGuiYeuCau);
            Controls.Add(lblDsTitle);
            Controls.Add(dgvYeuCau);
            Controls.Add(lblSoLuong);
            Controls.Add(btnLamMoi);
            Controls.Add(btnDong);

            Load += FrmXinChuyenPhong_Load;

            ((System.ComponentModel.ISupportInitialize)dgvYeuCau).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
