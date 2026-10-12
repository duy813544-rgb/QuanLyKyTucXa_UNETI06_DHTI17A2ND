using DoAnNhom06.Controls;
using DoAnNhom06.Helpers;

namespace DoAnNhom06.Forms
{
    partial class FrmTraCuuThongTin
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblSub;

        private Panel pnlChips;
        private Panel pnlSearchRow;
        private SearchBox searchBox;
        private Button btnTimKiem;

        private DataGridView dgvKetQua;
        private Label lblKetQua;
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
            lblTitle = TaoNhanTieuDe("TRA CỨU THÔNG TIN", 900);

            lblSub = new Label();
            pnlChips = new Panel();
            pnlSearchRow = new Panel();
            searchBox = new SearchBox();
            btnTimKiem = TaoNut("Tìm kiếm", KieuNut.ChinhDien);

            dgvKetQua = new DataGridView();
            lblKetQua = new Label();
            btnDong = TaoNut("Đóng", KieuNut.Phu);

            ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
            SuspendLayout();

            ClientSize = new Size(900, 580);
            Text = "Tra cứu thông tin";
            BackColor = Color.White;

            // ----- Tiêu đề + mô tả ngắn (kiểu tagline của trang tìm kiếm) -----
            lblTitle.ForeColor = Color.FromArgb(60, 64, 67);
            lblTitle.Location = new Point(0, 14);

            lblSub.Name = "lblSub";
            lblSub.Text = "Tìm nhanh sinh viên, phòng, hợp đồng hoặc thanh toán trong hệ thống";
            lblSub.Font = new Font("Segoe UI", 9.5F);
            lblSub.ForeColor = Color.FromArgb(120, 124, 132);
            lblSub.TextAlign = ContentAlignment.MiddleCenter;
            lblSub.Location = new Point(0, 54);
            lblSub.Size = new Size(900, 22);
            lblSub.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ----- Hàng "chip" chọn loại tra cứu (thay ComboBox) -----
            pnlChips.Name = "pnlChips";
            pnlChips.Location = new Point(0, 88);
            pnlChips.Size = new Size(900, 42);
            pnlChips.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlChips.BackColor = Color.White;

            // ----- Ô tìm kiếm dạng viên thuốc (Google-style) + nút Tìm kiếm -----
            pnlSearchRow.Name = "pnlSearchRow";
            pnlSearchRow.Location = new Point(0, 140);
            pnlSearchRow.Size = new Size(900, 52);
            pnlSearchRow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSearchRow.BackColor = Color.White;

            searchBox.Name = "searchBox";
            searchBox.PlaceholderText = "Nhập mã sinh viên, họ tên, số phòng, mã hợp đồng...";
            searchBox.Size = new Size(560, 48);
            searchBox.Location = new Point(110, 2);
            searchBox.Input.KeyDown += txtTuKhoa_KeyDown;

            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(110, 48);
            btnTimKiem.Location = new Point(682, 2);
            btnTimKiem.Click += btnTimKiem_Click;

            pnlSearchRow.Controls.Add(searchBox);
            pnlSearchRow.Controls.Add(btnTimKiem);

            // ----- Trạng thái + kết quả -----
            lblKetQua.Name = "lblKetQua";
            lblKetQua.Text = "Chọn loại, nhập từ khóa rồi bấm \"Tìm kiếm\".";
            lblKetQua.Location = new Point(24, 202);
            lblKetQua.Size = new Size(850, 24);
            lblKetQua.ForeColor = Color.FromArgb(120, 124, 132);
            lblKetQua.Font = new Font("Segoe UI", 9F);
            lblKetQua.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            dgvKetQua.Name = "dgvKetQua";
            dgvKetQua.Location = new Point(24, 232);
            dgvKetQua.Size = new Size(852, 288);
            dgvKetQua.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GridStyle.ApDungHienDai(dgvKetQua, Color.RoyalBlue);
            dgvKetQua.CellContentClick += dgvKetQua_CellContentClick;

            btnDong.Name = "btnDong";
            btnDong.Size = new Size(110, 38);
            btnDong.Location = new Point(766, 530);
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Click += btnDong_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblSub);
            Controls.Add(pnlChips);
            Controls.Add(pnlSearchRow);
            Controls.Add(lblKetQua);
            Controls.Add(dgvKetQua);
            Controls.Add(btnDong);

            Load += FrmTraCuuThongTin_Load;
            Resize += (_, _) => CanhGiuaChip();

            ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
