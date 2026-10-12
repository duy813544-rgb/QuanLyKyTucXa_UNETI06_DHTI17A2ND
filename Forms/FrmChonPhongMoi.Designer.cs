namespace DoAnNhom06.Forms
{
    partial class FrmChonPhongMoi
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblThongTin;
        private Label lblChonPhong;
        private ComboBox cboPhongMoi;
        private Button btnDongY;
        private Button btnHuy;

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
            lblThongTin = new Label();
            lblChonPhong = new Label();
            cboPhongMoi = new ComboBox();
            btnDongY = new Button();
            btnHuy = new Button();

            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 190);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Phân phòng";
            BackColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblThongTin.Name = "lblThongTin";
            lblThongTin.Text = "";
            lblThongTin.Location = new Point(20, 15);
            lblThongTin.Size = new Size(380, 50);

            lblChonPhong.Text = "Chuyển đến phòng:";
            lblChonPhong.Location = new Point(20, 80);
            lblChonPhong.Size = new Size(140, 25);

            cboPhongMoi.Name = "cboPhongMoi";
            cboPhongMoi.Location = new Point(160, 76);
            cboPhongMoi.Size = new Size(230, 27);
            cboPhongMoi.DropDownStyle = ComboBoxStyle.DropDownList;

            btnDongY.Name = "btnDongY";
            btnDongY.Text = "Đồng ý";
            btnDongY.Size = new Size(110, 38);
            btnDongY.Location = new Point(100, 130);
            btnDongY.BackColor = Color.RoyalBlue;
            btnDongY.ForeColor = Color.White;
            btnDongY.FlatStyle = FlatStyle.Flat;
            btnDongY.Click += btnDongY_Click;

            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(110, 38);
            btnHuy.Location = new Point(220, 130);
            btnHuy.BackColor = Color.Gainsboro;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            Controls.Add(lblThongTin);
            Controls.Add(lblChonPhong);
            Controls.Add(cboPhongMoi);
            Controls.Add(btnDongY);
            Controls.Add(btnHuy);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
