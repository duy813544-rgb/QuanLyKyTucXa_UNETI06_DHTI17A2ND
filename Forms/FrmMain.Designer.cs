using DoAnNhom06.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace DoAnNhom06.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuStrip1;
        private ToolStripMenuItem heThongToolStripMenuItem;
        private ToolStripMenuItem dangXuatToolStripMenuItem;
        private ToolStripMenuItem thoatToolStripMenuItem;

        private ToolStripMenuItem danhMucToolStripMenuItem;
        private ToolStripMenuItem sinhVienToolStripMenuItem;
        private ToolStripMenuItem phongToolStripMenuItem;
        private ToolStripMenuItem khuNhaToolStripMenuItem;

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblPhu;
        private Label lblTaiKhoan;
        private ContextMenuStrip cmsTaiKhoan;
        private ToolStripMenuItem mnuThongTinBanThan;
        private ToolStripMenuItem mnuDoiMatKhau;
        private ToolStripMenuItem mnuDangXuat;

        private Panel pnlBody;
        private TableLayoutPanel tlpBody;
        private Panel pnlTraCuu;
        private FlowLayoutPanel flpThongKe;
        private FlowLayoutPanel flpCards;

        private DashboardCard btnQuanLySinhVien;
        private DashboardCard btnQuanLyPhong;
        private DashboardCard btnDuyetDangKy;
        private DashboardCard btnQuanLyHopDong;
        private DashboardCard btnDangKyO;
        private DashboardCard btnGiaHanHopDong;
        private DashboardCard btnThanhToanPhi;
        private DashboardCard btnXacNhanThanhToan;
        private DashboardCard btnTraCuuThongTin;

        // --- Control RIÊNG của Sinh viên: xem phòng của mình (UC15 - chỉ xem, không chuyển) ---
        private DashboardCard btnPhongCuaToi;

        // --- Báo hỏng cơ sở vật chất: Sinh viên gửi yêu cầu, Nhân viên xử lý ---
        private DashboardCard btnBaoHong;
        private DashboardCard btnXuLyBaoHong;

        // --- Xin chuyển phòng: Sinh viên gửi yêu cầu, Nhân viên duyệt ---
        private DashboardCard btnXinChuyenPhong;
        private DashboardCard btnDuyetChuyenPhong;

        // --- Quản lý tài khoản, Báo cáo thống kê: riêng Quản trị viên (UC11, UC12).
        // Phân phòng, In hóa đơn: dùng chung Quản trị viên + Nhân viên (UC15, UC16) ---
        private StatCard cardTongPhong;
        private StatCard cardPhongTrong;
        private StatCard cardSinhVienDangO;
        private StatCard cardTyLeLapDay;

        private DashboardCard btnQuanLyTaiKhoan;
        private DashboardCard btnBaoCaoThongKe;
        private DashboardCard btnPhanPhongAdmin;
        private DashboardCard btnInHoaDon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>Tạo một thẻ chức năng với icon/màu/mô tả riêng cho mỗi chức năng.</summary>
        private static DashboardCard TaoThe(string name, string tieuDe, string moTa, DashboardIconKind icon, Color mauNhan)
        {
            return new DashboardCard
            {
                Name = name,
                Text = tieuDe,
                Subtitle = moTa,
                IconKind = icon,
                AccentColor = mauNhan,
                Margin = new Padding(10)
            };
        }

        /// <summary>Tạo một thẻ KPI (chỉ đọc) cho hàng số liệu tổng quan của Quản trị viên.</summary>
        private static StatCard TaoTheKpi(string name, Color mauNhan)
        {
            return new StatCard
            {
                Name = name,
                AccentColor = mauNhan,
                Margin = new Padding(10)
            };
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();

            heThongToolStripMenuItem = new ToolStripMenuItem();
            dangXuatToolStripMenuItem = new ToolStripMenuItem();
            thoatToolStripMenuItem = new ToolStripMenuItem();

            danhMucToolStripMenuItem = new ToolStripMenuItem();
            sinhVienToolStripMenuItem = new ToolStripMenuItem();
            phongToolStripMenuItem = new ToolStripMenuItem();
            khuNhaToolStripMenuItem = new ToolStripMenuItem();

            pnlHeader = new Panel();
            lblTitle = new Label();
            lblPhu = new Label();
            lblTaiKhoan = new Label();
            cmsTaiKhoan = new ContextMenuStrip();
            mnuThongTinBanThan = new ToolStripMenuItem();
            mnuDoiMatKhau = new ToolStripMenuItem();
            mnuDangXuat = new ToolStripMenuItem();

            pnlBody = new Panel();
            tlpBody = new TableLayoutPanel();
            pnlTraCuu = new Panel();
            flpThongKe = new FlowLayoutPanel();
            flpCards = new FlowLayoutPanel();

            SuspendLayout();

            // Menu
            menuStrip1.Items.AddRange(new ToolStripItem[]
            {
                heThongToolStripMenuItem,
                danhMucToolStripMenuItem
            });

            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Size = new Size(960, 28);

            // Hệ thống
            heThongToolStripMenuItem.Text = "Hệ thống";

            dangXuatToolStripMenuItem.Text = "Đăng xuất";
            dangXuatToolStripMenuItem.Click += dangXuatToolStripMenuItem_Click;
            thoatToolStripMenuItem.Text = "Thoát";
            thoatToolStripMenuItem.Click += thoatToolStripMenuItem_Click;

            heThongToolStripMenuItem.DropDownItems.Add(dangXuatToolStripMenuItem);
            heThongToolStripMenuItem.DropDownItems.Add(thoatToolStripMenuItem);

            // Danh mục
            danhMucToolStripMenuItem.Text = "Danh mục";

            sinhVienToolStripMenuItem.Text = "Sinh viên";
            sinhVienToolStripMenuItem.Click += sinhVienToolStripMenuItem_Click;
            phongToolStripMenuItem.Text = "Phòng";
            phongToolStripMenuItem.Click += phongToolStripMenuItem_Click;
            khuNhaToolStripMenuItem.Text = "Khu nhà";

            danhMucToolStripMenuItem.DropDownItems.Add(sinhVienToolStripMenuItem);
            danhMucToolStripMenuItem.DropDownItems.Add(phongToolStripMenuItem);
            danhMucToolStripMenuItem.DropDownItems.Add(khuNhaToolStripMenuItem);

            // pnlHeader (thanh tiêu đề) - DÙNG CHUNG cho cả 3 vai trò (Sinh viên, Nhân
            // viên, Quản trị viên) để giao diện tiêu đề luôn đồng nhất trên toàn hệ thống.
            pnlHeader.Location = new Point(0, 28);
            pnlHeader.Size = new Size(960, 84);
            pnlHeader.BackColor = Color.FromArgb(24, 49, 95);
            pnlHeader.Dock = DockStyle.Top;

            lblTitle.Text = "HỆ THỐNG QUẢN LÝ KÝ TÚC XÁ";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = false;
            lblTitle.Location = new Point(24, 14);
            lblTitle.Size = new Size(600, 32);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblPhu.Text = "Chọn một chức năng bên dưới để bắt đầu";
            lblPhu.Font = new Font("Segoe UI", 9.5F);
            lblPhu.ForeColor = Color.FromArgb(190, 205, 230);
            lblPhu.AutoSize = false;
            lblPhu.Location = new Point(26, 48);
            lblPhu.Size = new Size(500, 22);
            lblPhu.TextAlign = ContentAlignment.MiddleLeft;

            lblTaiKhoan.Text = "Bạn đăng nhập: ";
            lblTaiKhoan.Font = new Font("Segoe UI", 9.5F);
            lblTaiKhoan.ForeColor = Color.White;
            lblTaiKhoan.AutoSize = false;
            lblTaiKhoan.Location = new Point(660, 14);
            lblTaiKhoan.Size = new Size(280, 50);
            lblTaiKhoan.TextAlign = ContentAlignment.MiddleRight;
            lblTaiKhoan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTaiKhoan.Cursor = Cursors.Hand;
            lblTaiKhoan.Click += lblTaiKhoan_Click;

            // Menu bấm vào tên tài khoản: Thông tin bản thân / Đổi mật khẩu / Đăng xuất
            // (UC13, UC14) - dùng chung cho Sinh viên, Nhân viên VÀ Quản trị viên.
            mnuThongTinBanThan.Text = "Thông tin cá nhân";
            mnuThongTinBanThan.Click += mnuThongTinBanThan_Click;

            mnuDoiMatKhau.Text = "Đổi mật khẩu";
            mnuDoiMatKhau.Click += mnuDoiMatKhau_Click;

            mnuDangXuat.Text = "Đăng xuất";
            mnuDangXuat.Click += mnuDangXuat_Click;

            cmsTaiKhoan.Items.Add(mnuThongTinBanThan);
            cmsTaiKhoan.Items.Add(mnuDoiMatKhau);
            cmsTaiKhoan.Items.Add(new ToolStripSeparator());
            cmsTaiKhoan.Items.Add(mnuDangXuat);

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblPhu);
            pnlHeader.Controls.Add(lblTaiKhoan);

            // pnlBody: khung ngoài cùng, chứa 1 layout 3 hàng -
            //   hàng 0 (Sinh viên/Nhân viên): thanh tìm kiếm "Tra cứu thông tin";
            //   hàng 1 (CHỈ Quản trị viên): 4 thẻ số liệu tổng quan (KPI) kiểu ảnh mẫu;
            //   hàng 2 (chiếm hết phần còn lại): lưới các thẻ chức năng, tự xuống dòng/cuộn.
            // Với mỗi vai trò, FrmMain.cs sẽ ẩn/hiện panel tương ứng VÀ đưa chiều cao
            // hàng không dùng tới về 0 để không để lại khoảng trống thừa.
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.BackColor = Color.FromArgb(244, 246, 250);
            pnlBody.Padding = new Padding(20);

            tlpBody.Dock = DockStyle.Fill;
            tlpBody.ColumnCount = 1;
            tlpBody.RowCount = 3;
            tlpBody.BackColor = Color.FromArgb(244, 246, 250);
            tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBody.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));   // 0: Tra cứu (SV/NV)
            tlpBody.RowStyles.Add(new RowStyle(SizeType.Absolute, 116F));  // 1: KPI (Admin)
            tlpBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // 2: Lưới thẻ chức năng

            pnlTraCuu.Dock = DockStyle.Fill;
            pnlTraCuu.BackColor = Color.FromArgb(244, 246, 250);
            pnlTraCuu.Padding = new Padding(0, 0, 0, 14);

            // flpThongKe: hàng 4 thẻ KPI "Tổng số phòng / Phòng còn trống / Sinh viên
            // đang ở / Tỷ lệ lấp đầy" - chỉ hiển thị với Quản trị viên (xem FrmMain.cs).
            flpThongKe.Dock = DockStyle.Fill;
            flpThongKe.FlowDirection = FlowDirection.LeftToRight;
            flpThongKe.WrapContents = false;
            flpThongKe.AutoScroll = false;
            flpThongKe.BackColor = Color.FromArgb(244, 246, 250);
            flpThongKe.Padding = new Padding(0, 0, 0, 6);

            flpCards.Dock = DockStyle.Fill;
            flpCards.FlowDirection = FlowDirection.LeftToRight;
            flpCards.WrapContents = true;
            flpCards.AutoScroll = true;
            flpCards.BackColor = Color.FromArgb(244, 246, 250);

            // Mỗi thẻ dưới đây có icon, màu nhấn và mô tả riêng — không còn dùng
            // chung một kiểu Button phẳng chỉ khác màu nền như bản trước.
            // Mỗi thẻ một màu nhấn RIÊNG (không thẻ nào trùng màu với thẻ khác),
            // để dù tài khoản có được thấy bao nhiêu thẻ cùng lúc thì cũng
            // không có 2 thẻ nào trông "giống ý tưởng" nhau như bản trước.
            btnQuanLySinhVien = TaoThe("btnQuanLySinhVien", "Hồ sơ sinh viên",
                "Thêm, sửa, tra thông tin sinh viên đang ở trọ",
                DashboardIconKind.NguoiDung, Color.FromArgb(24, 95, 165));   // Xanh dương
            btnQuanLySinhVien.Click += sinhVienToolStripMenuItem_Click;

            btnQuanLyPhong = TaoThe("btnQuanLyPhong", "Quản lý phòng",
                "Xem sơ đồ phòng, số chỗ trống, tình trạng phòng",
                DashboardIconKind.Phong, Color.FromArgb(59, 109, 17));      // Xanh lá
            btnQuanLyPhong.Click += phongToolStripMenuItem_Click;

            btnDuyetDangKy = TaoThe("btnDuyetDangKy", "Duyệt đăng ký",
                "Xem xét, chấp thuận hoặc từ chối đơn xin ở",
                DashboardIconKind.Duyet, Color.FromArgb(153, 60, 29));      // Cam đất (coral)
            btnDuyetDangKy.Click += btnDuyetDangKy_Click;

            btnQuanLyHopDong = TaoThe("btnQuanLyHopDong", "Quản lý hợp đồng",
                "Theo dõi hợp đồng thuê phòng đang hiệu lực",
                DashboardIconKind.HopDong, Color.FromArgb(83, 74, 183));    // Tím
            btnQuanLyHopDong.Click += btnQuanLyHopDong_Click;

            btnDangKyO = TaoThe("btnDangKyO", "Đăng ký ở KTX",
                "Gửi yêu cầu đăng ký một chỗ ở ký túc xá",
                DashboardIconKind.Chia, Color.FromArgb(95, 94, 90));        // Xám trung tính
            btnDangKyO.Click += btnDangKyO_Click;

            btnGiaHanHopDong = TaoThe("btnGiaHanHopDong", "Gia hạn hợp đồng",
                "Kéo dài thời gian hợp đồng đang thuê phòng",
                DashboardIconKind.LamMoi, Color.FromArgb(163, 45, 45));     // Đỏ (nhắc hạn sắp hết)
            btnGiaHanHopDong.Click += btnGiaHanHopDong_Click;

            btnThanhToanPhi = TaoThe("btnThanhToanPhi", "Thanh toán phí",
                "Nộp tiền phòng, điện nước và các khoản phí",
                DashboardIconKind.ViTien, Color.FromArgb(133, 79, 11));     // Vàng đồng (tiền)
            btnThanhToanPhi.Click += btnThanhToanPhi_Click;

            btnXacNhanThanhToan = TaoThe("btnXacNhanThanhToan", "Xác nhận thanh toán",
                "Kiểm tra và xác nhận các khoản đã nộp",
                DashboardIconKind.HoaDon, Color.FromArgb(153, 53, 86));     // Hồng đậm
            btnXacNhanThanhToan.Click += btnXacNhanThanhToan_Click;

            // ---- Thẻ chức năng RIÊNG của Sinh viên: xem phòng của mình ----
            btnPhongCuaToi = TaoThe("btnPhongCuaToi", "Phòng của tôi",
                "Xem phòng đang ở và lịch sử phân phòng của bạn",
                DashboardIconKind.ChuyenPhong, Color.FromArgb(59, 109, 17));  // Xanh lá
            btnPhongCuaToi.Click += btnPhongCuaToi_Click;

            // ---- Báo hỏng cơ sở vật chất: RIÊNG của Sinh viên (gửi yêu cầu) ----
            btnBaoHong = TaoThe("btnBaoHong", "Báo hỏng cơ sở vật chất",
                "Báo bóng đèn, quạt, điều hòa, vòi nước... bị hỏng",
                DashboardIconKind.CanhBao, Color.FromArgb(163, 45, 45));    // Đỏ (cảnh báo)
            btnBaoHong.Click += btnBaoHong_Click;

            // ---- Xử lý báo hỏng cơ sở vật chất: RIÊNG của Nhân viên ----
            btnXuLyBaoHong = TaoThe("btnXuLyBaoHong", "Xử lý báo hỏng",
                "Tiếp nhận và cập nhật trạng thái sửa chữa",
                DashboardIconKind.CanhBao, Color.FromArgb(133, 79, 11));    // Vàng đồng
            btnXuLyBaoHong.Click += btnXuLyBaoHong_Click;

            // ---- Xin chuyển phòng: RIÊNG của Sinh viên (gửi yêu cầu) ----
            btnXinChuyenPhong = TaoThe("btnXinChuyenPhong", "Xin chuyển phòng",
                "Gửi yêu cầu chuyển sang phòng khác kèm lý do",
                DashboardIconKind.ChuyenPhong, Color.FromArgb(15, 110, 86));   // Xanh ngọc
            btnXinChuyenPhong.Click += btnXinChuyenPhong_Click;

            // ---- Duyệt chuyển phòng: RIÊNG của Nhân viên ----
            btnDuyetChuyenPhong = TaoThe("btnDuyetChuyenPhong", "Duyệt chuyển phòng",
                "Xem xét, chấp thuận hoặc từ chối yêu cầu chuyển phòng",
                DashboardIconKind.Duyet, Color.FromArgb(153, 60, 29));        // Cam đất
            btnDuyetChuyenPhong.Click += btnDuyetChuyenPhong_Click;

            // Thẻ "Tra cứu thông tin" tách riêng, vẽ theo kiểu thanh tìm kiếm dài (giống ô tìm
            // kiếm của Google), đặt ở đầu trang và kéo dài hết chiều rộng thay vì chỉ là một
            // thẻ vuông nằm lẫn trong lưới như các chức năng khác.
            btnTraCuuThongTin = TaoThe("btnTraCuuThongTin", "Tra cứu thông tin",
                "Tìm nhanh thông tin phòng, sinh viên, hóa đơn...",
                DashboardIconKind.KinhLup, Color.FromArgb(15, 110, 86));    // Xanh ngọc (teal)
            btnTraCuuThongTin.KieuThanhTimKiem = true;
            btnTraCuuThongTin.Dock = DockStyle.Fill;
            btnTraCuuThongTin.Margin = new Padding(0);
            btnTraCuuThongTin.Click += btnTraCuuThongTin_Click;

            // ---- 4 thẻ: Quản lý tài khoản/Báo cáo thống kê RIÊNG Quản trị viên (UC11, UC12);
            //      Phân phòng/In hóa đơn dùng chung Quản trị viên VÀ Nhân viên (UC15, UC16) ----
            btnQuanLyTaiKhoan = TaoThe("btnQuanLyTaiKhoan", "Quản lý tài khoản",
                "Thêm, sửa, khóa/xóa tài khoản đăng nhập hệ thống",
                DashboardIconKind.TaiKhoan, Color.FromArgb(24, 95, 165));   // Xanh dương
            btnQuanLyTaiKhoan.Click += btnQuanLyTaiKhoan_Click;

            btnBaoCaoThongKe = TaoThe("btnBaoCaoThongKe", "Báo cáo thống kê",
                "Thống kê sinh viên, phòng, doanh thu và xuất báo cáo",
                DashboardIconKind.BieuDo, Color.FromArgb(83, 74, 183));     // Tím
            btnBaoCaoThongKe.Click += btnBaoCaoThongKe_Click;

            btnPhanPhongAdmin = TaoThe("btnPhanPhongAdmin", "Phân phòng",
                "Chuyển sinh viên đang ở sang phòng khác khi cần",
                DashboardIconKind.ChuyenPhong, Color.FromArgb(59, 109, 17)); // Xanh lá
            btnPhanPhongAdmin.Click += btnPhanPhongAdmin_Click;

            btnInHoaDon = TaoThe("btnInHoaDon", "In hóa đơn",
                "Xem và in hóa đơn phí ký túc xá của sinh viên",
                DashboardIconKind.MayIn, Color.FromArgb(153, 53, 86));      // Hồng đậm
            btnInHoaDon.Click += btnInHoaDon_Click;

            flpCards.Controls.Add(btnQuanLySinhVien);
            flpCards.Controls.Add(btnQuanLyPhong);
            flpCards.Controls.Add(btnDuyetDangKy);
            flpCards.Controls.Add(btnQuanLyHopDong);
            flpCards.Controls.Add(btnDangKyO);
            flpCards.Controls.Add(btnGiaHanHopDong);
            flpCards.Controls.Add(btnThanhToanPhi);
            flpCards.Controls.Add(btnXacNhanThanhToan);
            flpCards.Controls.Add(btnPhongCuaToi);
            flpCards.Controls.Add(btnBaoHong);
            flpCards.Controls.Add(btnXuLyBaoHong);
            flpCards.Controls.Add(btnXinChuyenPhong);
            flpCards.Controls.Add(btnDuyetChuyenPhong);
            flpCards.Controls.Add(btnQuanLyTaiKhoan);
            flpCards.Controls.Add(btnBaoCaoThongKe);
            flpCards.Controls.Add(btnPhanPhongAdmin);
            flpCards.Controls.Add(btnInHoaDon);

            pnlTraCuu.Controls.Add(btnTraCuuThongTin);

            // ---- Hàng số liệu tổng quan (KPI) - chỉ Quản trị viên nhìn thấy ----
            cardTongPhong = TaoTheKpi("cardTongPhong", Color.FromArgb(64, 156, 255));      // Xanh dương
            cardPhongTrong = TaoTheKpi("cardPhongTrong", Color.FromArgb(60, 190, 120));    // Xanh lá
            cardSinhVienDangO = TaoTheKpi("cardSinhVienDangO", Color.FromArgb(255, 170, 60)); // Cam
            cardTyLeLapDay = TaoTheKpi("cardTyLeLapDay", Color.FromArgb(200, 100, 240));   // Tím hồng

            flpThongKe.Controls.Add(cardTongPhong);
            flpThongKe.Controls.Add(cardPhongTrong);
            flpThongKe.Controls.Add(cardSinhVienDangO);
            flpThongKe.Controls.Add(cardTyLeLapDay);

            tlpBody.Controls.Add(pnlTraCuu, 0, 0);
            tlpBody.Controls.Add(flpThongKe, 0, 1);
            tlpBody.Controls.Add(flpCards, 0, 2);

            pnlBody.Controls.Add(tlpBody);

            // Form
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 640);
            MinimumSize = new Size(760, 480);
            Controls.Add(pnlBody);
            Controls.Add(pnlHeader);
            Controls.Add(menuStrip1);

            MainMenuStrip = menuStrip1;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(244, 246, 250);
            Text = "Trang chủ";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
