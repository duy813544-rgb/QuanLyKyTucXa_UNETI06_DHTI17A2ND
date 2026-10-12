using DoAnNhom06.Helpers;

namespace DoAnNhom06.Forms
{
    partial class FrmThanhToanPhi
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblMaSv;
        private TextBox txtMaSv;
        private Button btnTimKiem;

        private DataGridView dgvHoaDon;
        private Label lblGhiChu;

        private Label lblPhuongThuc;
        private ComboBox cboPhuongThuc;
        private Button btnThanhToan;
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
            // Màu nền, font, vị trí hiển thị... đã được thiết lập chung ở BaseAppForm.
            lblTitle = TaoNhanTieuDe("THANH TOÁN PHÍ KÝ TÚC XÁ", 900);

            lblMaSv = new Label();
            txtMaSv = new TextBox();
            btnTimKiem = TaoNut("Xem hóa đơn", KieuNut.ChinhDien);

            dgvHoaDon = new DataGridView();
            lblGhiChu = new Label();

            lblPhuongThuc = new Label();
            cboPhuongThuc = new ComboBox();
            btnThanhToan = TaoNut("Thanh toán", KieuNut.ThanhCong);
            btnDong = TaoNut("Đóng", KieuNut.Phu);

            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
            SuspendLayout();

            ClientSize = new Size(900, 560);
            Text = "Thanh toán phí ký túc xá";

            lblMaSv.Text = "Nhập Mã số sinh viên của bạn:";
            lblMaSv.Location = new Point(20, 62);
            lblMaSv.Size = new Size(210, 25);
            lblMaSv.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            txtMaSv.Name = "txtMaSv";
            txtMaSv.Location = new Point(230, 58);
            txtMaSv.Size = new Size(220, 27);
            txtMaSv.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            txtMaSv.KeyDown += txtMaSv_KeyDown;

            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(140, 30);
            btnTimKiem.Location = new Point(460, 57);
            btnTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnTimKiem.Click += btnTimKiem_Click;

            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.Location = new Point(20, 100);
            dgvHoaDon.Size = new Size(860, 340);
            dgvHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GridStyle.ApDungChiDoc(dgvHoaDon);
            dgvHoaDon.DataBindingComplete += dgvHoaDon_DataBindingComplete;
            dgvHoaDon.CellContentClick += dgvHoaDon_CellContentClick;

            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Text = "Nhập mã số sinh viên rồi bấm \"Xem hóa đơn\" để tải danh sách.";
            lblGhiChu.Location = new Point(20, 450);
            lblGhiChu.Size = new Size(600, 25);
            lblGhiChu.ForeColor = Color.DimGray;
            lblGhiChu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            lblPhuongThuc.Text = "Phương thức:";
            lblPhuongThuc.Location = new Point(20, 485);
            lblPhuongThuc.Size = new Size(100, 25);
            lblPhuongThuc.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            cboPhuongThuc.Name = "cboPhuongThuc";
            cboPhuongThuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhuongThuc.Location = new Point(125, 481);
            cboPhuongThuc.Size = new Size(180, 27);
            cboPhuongThuc.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cboPhuongThuc.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Ví điện tử" });
            cboPhuongThuc.SelectedIndex = 0;

            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(130, 38);
            btnThanhToan.Location = new Point(640, 505);
            btnThanhToan.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnThanhToan.Click += btnThanhToan_Click;

            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(770, 505);
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblMaSv);
            Controls.Add(txtMaSv);
            Controls.Add(btnTimKiem);
            Controls.Add(dgvHoaDon);
            Controls.Add(lblGhiChu);
            Controls.Add(lblPhuongThuc);
            Controls.Add(cboPhuongThuc);
            Controls.Add(btnThanhToan);
            Controls.Add(btnDong);

            Load += FrmThanhToanPhi_Load;

            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
