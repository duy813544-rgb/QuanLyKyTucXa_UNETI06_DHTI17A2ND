namespace DoAnNhom06.Forms
{
    partial class FrmThemSuaPhong
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

            lblMaPhong = new Label();
            txtMaPhong = new TextBox();

            lblKhuNha = new Label();
            cboKhuNha = new ComboBox();

            lblSoLuongToiDa = new Label();
            numSoLuongToiDa = new NumericUpDown();

            lblSoLuongHienTai = new Label();
            numSoLuongHienTai = new NumericUpDown();

            btnLuu = new Button();
            btnHuy = new Button();

            ((System.ComponentModel.ISupportInitialize)numSoLuongToiDa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSoLuongHienTai).BeginInit();
            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(480, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Thêm phòng";
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // lblTitle
            lblTitle.Text = "THÔNG TIN PHÒNG";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.RoyalBlue;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(480, 40);
            lblTitle.Location = new Point(0, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblMaPhong
            lblMaPhong.Text = "Mã phòng";
            lblMaPhong.Location = new Point(40, 70);
            lblMaPhong.Size = new Size(140, 25);

            // txtMaPhong
            txtMaPhong.Name = "txtMaPhong";
            txtMaPhong.Location = new Point(190, 70);
            txtMaPhong.Size = new Size(250, 27);

            // lblKhuNha
            lblKhuNha.Text = "Khu nhà";
            lblKhuNha.Location = new Point(40, 115);
            lblKhuNha.Size = new Size(140, 25);

            // cboKhuNha
            cboKhuNha.Name = "cboKhuNha";
            cboKhuNha.Location = new Point(190, 115);
            cboKhuNha.Size = new Size(250, 27);
            cboKhuNha.DropDownStyle = ComboBoxStyle.DropDownList;

            // lblSoLuongToiDa
            lblSoLuongToiDa.Text = "Số lượng tối đa";
            lblSoLuongToiDa.Location = new Point(40, 160);
            lblSoLuongToiDa.Size = new Size(140, 25);

            // numSoLuongToiDa
            numSoLuongToiDa.Name = "numSoLuongToiDa";
            numSoLuongToiDa.Location = new Point(190, 160);
            numSoLuongToiDa.Size = new Size(250, 27);
            numSoLuongToiDa.Minimum = 0;
            numSoLuongToiDa.Maximum = 100;

            // lblSoLuongHienTai
            lblSoLuongHienTai.Text = "Số lượng hiện tại";
            lblSoLuongHienTai.Location = new Point(40, 205);
            lblSoLuongHienTai.Size = new Size(140, 25);

            // numSoLuongHienTai
            numSoLuongHienTai.Name = "numSoLuongHienTai";
            numSoLuongHienTai.Location = new Point(190, 205);
            numSoLuongHienTai.Size = new Size(250, 27);
            numSoLuongHienTai.Minimum = 0;
            numSoLuongHienTai.Maximum = 100;

            // btnLuu
            btnLuu.Name = "btnLuu";
            btnLuu.Text = "Lưu";
            btnLuu.Size = new Size(120, 40);
            btnLuu.Location = new Point(100, 320);
            btnLuu.BackColor = Color.RoyalBlue;
            btnLuu.ForeColor = Color.White;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Click += btnLuu_Click;

            // btnHuy
            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(120, 40);
            btnHuy.Location = new Point(250, 320);
            btnHuy.BackColor = Color.IndianRed;
            btnHuy.ForeColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblMaPhong);
            Controls.Add(txtMaPhong);
            Controls.Add(lblKhuNha);
            Controls.Add(cboKhuNha);
            Controls.Add(lblSoLuongToiDa);
            Controls.Add(numSoLuongToiDa);
            Controls.Add(lblSoLuongHienTai);
            Controls.Add(numSoLuongHienTai);
            Controls.Add(btnLuu);
            Controls.Add(btnHuy);

            ((System.ComponentModel.ISupportInitialize)numSoLuongToiDa).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSoLuongHienTai).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblMaPhong;
        private TextBox txtMaPhong;
        private Label lblKhuNha;
        private ComboBox cboKhuNha;
        private Label lblSoLuongToiDa;
        private NumericUpDown numSoLuongToiDa;
        private Label lblSoLuongHienTai;
        private NumericUpDown numSoLuongHienTai;
        private Button btnLuu;
        private Button btnHuy;

        #endregion
    }
}
