namespace DoAnNhom06.Forms
{
    partial class FrmBaoHong
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblPhongHienTai;
        private Label lblLoaiSuCo;
        private ComboBox cboLoaiSuCo;
        private Label lblMoTa;
        private TextBox txtMoTa;
        private Button btnGuiBaoHong;

        private Label lblDsTitle;
        private DataGridView dgvBaoHong;
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
            lblLoaiSuCo = new Label();
            cboLoaiSuCo = new ComboBox();
            lblMoTa = new Label();
            txtMoTa = new TextBox();
            btnGuiBaoHong = new Button();
            lblDsTitle = new Label();
            dgvBaoHong = new DataGridView();
            lblSoLuong = new Label();
            btnLamMoi = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvBaoHong).BeginInit();
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 640);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Báo hỏng cơ sở vật chất";
            BackColor = Color.WhiteSmoke;
            MinimumSize = new Size(620, 520);

            lblTitle.Text = "BÁO HỎNG CƠ SỞ VẬT CHẤT";
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
            lblPhongHienTai.Size = new Size(680, 24);
            lblPhongHienTai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            lblLoaiSuCo.Name = "lblLoaiSuCo";
            lblLoaiSuCo.Text = "Loại sự cố:";
            lblLoaiSuCo.Location = new Point(20, 90);
            lblLoaiSuCo.Size = new Size(100, 25);
            lblLoaiSuCo.Font = new Font("Segoe UI", 10F);
            lblLoaiSuCo.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            cboLoaiSuCo.Name = "cboLoaiSuCo";
            cboLoaiSuCo.Location = new Point(130, 88);
            cboLoaiSuCo.Size = new Size(280, 30);
            cboLoaiSuCo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiSuCo.Font = new Font("Segoe UI", 10F);
            cboLoaiSuCo.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblMoTa.Name = "lblMoTa";
            lblMoTa.Text = "Mô tả chi tiết (tuỳ chọn):";
            lblMoTa.Location = new Point(20, 128);
            lblMoTa.Size = new Size(250, 22);
            lblMoTa.Font = new Font("Segoe UI", 10F);
            lblMoTa.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            txtMoTa.Name = "txtMoTa";
            txtMoTa.Location = new Point(20, 152);
            txtMoTa.Size = new Size(680, 60);
            txtMoTa.Multiline = true;
            txtMoTa.Font = new Font("Segoe UI", 10F);
            txtMoTa.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnGuiBaoHong.Name = "btnGuiBaoHong";
            btnGuiBaoHong.Text = "Gửi báo hỏng";
            btnGuiBaoHong.Size = new Size(160, 38);
            btnGuiBaoHong.Location = new Point(20, 222);
            btnGuiBaoHong.BackColor = Color.SeaGreen;
            btnGuiBaoHong.ForeColor = Color.White;
            btnGuiBaoHong.FlatStyle = FlatStyle.Flat;
            btnGuiBaoHong.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnGuiBaoHong.Click += btnGuiBaoHong_Click;

            lblDsTitle.Name = "lblDsTitle";
            lblDsTitle.Text = "Các yêu cầu đã gửi:";
            lblDsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDsTitle.Location = new Point(20, 275);
            lblDsTitle.Size = new Size(300, 24);
            lblDsTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            dgvBaoHong.Name = "dgvBaoHong";
            dgvBaoHong.Location = new Point(20, 302);
            dgvBaoHong.Size = new Size(680, 260);
            dgvBaoHong.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBaoHong.ReadOnly = true;
            dgvBaoHong.AllowUserToAddRows = false;
            dgvBaoHong.AllowUserToDeleteRows = false;
            dgvBaoHong.MultiSelect = false;
            dgvBaoHong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBaoHong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoHong.BackgroundColor = Color.White;

            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Text = "Tổng số: 0 yêu cầu đã gửi";
            lblSoLuong.Location = new Point(20, 570);
            lblSoLuong.Size = new Size(300, 25);
            lblSoLuong.ForeColor = Color.DimGray;
            lblSoLuong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(110, 38);
            btnLamMoi.Location = new Point(470, 592);
            btnLamMoi.BackColor = Color.Gainsboro;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLamMoi.Click += btnLamMoi_Click;

            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(590, 592);
            btnDong.BackColor = Color.Gainsboro;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblPhongHienTai);
            Controls.Add(lblLoaiSuCo);
            Controls.Add(cboLoaiSuCo);
            Controls.Add(lblMoTa);
            Controls.Add(txtMoTa);
            Controls.Add(btnGuiBaoHong);
            Controls.Add(lblDsTitle);
            Controls.Add(dgvBaoHong);
            Controls.Add(lblSoLuong);
            Controls.Add(btnLamMoi);
            Controls.Add(btnDong);

            Load += FrmBaoHong_Load;

            ((System.ComponentModel.ISupportInitialize)dgvBaoHong).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
