namespace DoAnNhom06.Forms
{
    partial class FrmChonNgayGiaHan
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblThongTin;
        private Label lblChonNgay;
        private DateTimePicker dtpNgayKetThucMoi;
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
            lblChonNgay = new Label();
            dtpNgayKetThucMoi = new DateTimePicker();
            btnDongY = new Button();
            btnHuy = new Button();

            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 190);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Gia hạn hợp đồng";
            BackColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblThongTin.Name = "lblThongTin";
            lblThongTin.Text = "";
            lblThongTin.Location = new Point(20, 15);
            lblThongTin.Size = new Size(360, 50);

            lblChonNgay.Text = "Ngày kết thúc mới:";
            lblChonNgay.Location = new Point(20, 80);
            lblChonNgay.Size = new Size(150, 25);

            dtpNgayKetThucMoi.Name = "dtpNgayKetThucMoi";
            dtpNgayKetThucMoi.Format = DateTimePickerFormat.Short;
            dtpNgayKetThucMoi.Location = new Point(180, 76);
            dtpNgayKetThucMoi.Size = new Size(180, 27);

            btnDongY.Name = "btnDongY";
            btnDongY.Text = "Đồng ý";
            btnDongY.Size = new Size(110, 38);
            btnDongY.Location = new Point(90, 130);
            btnDongY.BackColor = Color.RoyalBlue;
            btnDongY.ForeColor = Color.White;
            btnDongY.FlatStyle = FlatStyle.Flat;
            btnDongY.Click += btnDongY_Click;

            btnHuy.Name = "btnHuy";
            btnHuy.Text = "Hủy";
            btnHuy.Size = new Size(110, 38);
            btnHuy.Location = new Point(210, 130);
            btnHuy.BackColor = Color.Gainsboro;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Click += btnHuy_Click;

            Controls.Add(lblThongTin);
            Controls.Add(lblChonNgay);
            Controls.Add(dtpNgayKetThucMoi);
            Controls.Add(btnDongY);
            Controls.Add(btnHuy);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
