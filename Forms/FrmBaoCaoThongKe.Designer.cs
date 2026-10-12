using DoAnNhom06.Controls;
using DoAnNhom06.Helpers;

namespace DoAnNhom06.Forms
{
    partial class FrmBaoCaoThongKe
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

        /// <summary>Tạo 1 trang báo cáo con: nhãn tóm tắt phía trên + bảng dữ liệu chiếm hết phần còn lại.</summary>
        private static (Label lbl, DataGridView dgv) TaoTrangBaoCao(TabPage trang, Color mauNhan)
        {
            var lbl = new Label
            {
                Dock = DockStyle.Top,
                Height = 70,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(60, 64, 70),
                Padding = new Padding(4, 6, 4, 6),
                BackColor = Color.White
            };

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill
            };
            GridStyle.ApDungHienDai(dgv, mauNhan);

            trang.Padding = new Padding(6);
            trang.BackColor = Color.White;
            trang.Controls.Add(dgv);
            trang.Controls.Add(lbl);

            return (lbl, dgv);
        }

        /// <summary>Tạo 1 thẻ KPI (chỉ đọc) cho hàng số liệu tổng quan phía trên các tab báo cáo.</summary>
        private static StatCard TaoTheKpiBaoCao(string name, Color mauNhan)
        {
            return new StatCard
            {
                Name = name,
                AccentColor = mauNhan,
                Margin = new Padding(10)
            };
        }

        /// <summary>
        /// UC19 - Thống kê doanh thu: dựng khối "bảng điều khiển" trực quan ở đầu tab Doanh thu
        /// (3 thẻ KPI riêng của doanh thu, biểu đồ cột doanh thu 5 tháng gần nhất, và tình trạng
        /// lấp đầy theo từng khu nhà) - nằm phía TRÊN bảng chi tiết theo tháng (dgvDoanhThu) đã
        /// có sẵn, để vừa xem nhanh bằng biểu đồ, vừa có số liệu đầy đủ khi cần xuất Excel.
        /// </summary>
        private void TaoBangDieuKhienDoanhThu(TabPage trang)
        {
            pnlDoanhThuDashboard = new Panel
            {
                Dock = DockStyle.Top,
                // 128 (3 thẻ KPI) + 12 (margin trên biểu đồ) + 300 (biểu đồ) + 12 (margin trên
                // thẻ lấp đầy) + chiều cao cố định của cardLapDayKhu (60 + 56*5, xem bên dưới).
                Height = 128 + 12 + 300 + 12 + (60 + 56 * 5),
                BackColor = Color.FromArgb(244, 246, 250),
                Padding = new Padding(0, 8, 0, 0)
            };

            // Hàng 3 thẻ KPI riêng của Doanh thu (khác 4 thẻ tổng quan ở đầu trang).
            flpTheDoanhThu = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 120,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(4, 0, 0, 0)
            };

            cardDoanhThuThangNay = TaoTheKpiBaoCao("cardDoanhThuThangNay", Color.FromArgb(66, 99, 235));
            cardHoaDonChuaTT = TaoTheKpiBaoCao("cardHoaDonChuaTT", Color.FromArgb(160, 35, 35));
            cardDangKyChoDuyetTab = TaoTheKpiBaoCao("cardDangKyChoDuyetTab", Color.FromArgb(133, 79, 11));

            flpTheDoanhThu.Controls.Add(cardDoanhThuThangNay);
            flpTheDoanhThu.Controls.Add(cardHoaDonChuaTT);
            flpTheDoanhThu.Controls.Add(cardDangKyChoDuyetTab);

            // Thẻ "Doanh thu theo tháng" - biểu đồ cột.
            cardBieuDoDoanhThu = new CardPanel
            {
                Dock = DockStyle.Top,
                Height = 300,
                TieuDe = "Doanh thu theo tháng",
                Margin = new Padding(0, 12, 0, 0)
            };

            bieuDoDoanhThu = new BieuDoCotDoanhThu
            {
                Dock = DockStyle.Fill
            };
            cardBieuDoDoanhThu.Controls.Add(bieuDoDoanhThu);

            // Thẻ "Tình trạng lấp đầy theo khu nhà" - danh sách thanh tiến trình.
            // Chiều cao thẻ này CỐ ĐỊNH (đủ hiển thị ~5 khu) thay vì co giãn theo đúng
            // số khu nhà thực tế trong CSDL: dữ liệu mẫu hiện có tới 20 khu (KHU_A..KHU_T),
            // nếu để thẻ tự phình to theo số khu sẽ đẩy bảng dgvDoanhThu ở dưới ra ngoài
            // vùng nhìn thấy mà không có thanh cuộn nào để xem tiếp (lỗi "không lướt xuống
            // xem được"). Danh sách khu nhà vẫn hiển thị ĐẦY ĐỦ, chỉ là cuộn được bên trong
            // thẻ (pnlLapDayKhuScroll.AutoScroll) thay vì phình cả khối dashboard.
            const int SO_HANG_HIEN_THI_TOI_DA = 5;
            cardLapDayKhu = new CardPanel
            {
                Dock = DockStyle.Top,
                Height = 60 + 56 * SO_HANG_HIEN_THI_TOI_DA,
                TieuDe = "Tình trạng lấp đầy theo khu nhà",
                Margin = new Padding(0, 12, 0, 0)
            };

            pnlLapDayKhuScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };

            dsLapDayKhu = new DanhSachLapDayKhu
            {
                Dock = DockStyle.Top
            };
            pnlLapDayKhuScroll.Controls.Add(dsLapDayKhu);
            cardLapDayKhu.Controls.Add(pnlLapDayKhuScroll);

            // Thêm theo thứ tự trong-ra-ngoài: Fill/trong cùng thêm trước, mép Top thêm sau
            // (giống quy ước đã dùng ở InitializeComponent chính) để thứ tự hiển thị từ trên
            // xuống đúng là: 3 thẻ KPI -> biểu đồ doanh thu -> lấp đầy theo khu.
            pnlDoanhThuDashboard.Controls.Add(cardLapDayKhu);
            pnlDoanhThuDashboard.Controls.Add(cardBieuDoDoanhThu);
            pnlDoanhThuDashboard.Controls.Add(flpTheDoanhThu);

            // trang đã có sẵn (lblDoanhThu Dock=Top, dgvDoanhThu Dock=Top chiều cao cố định -
            // xem chỗ gọi TaoTrangBaoCao(tabDoanhThu,...) ở InitializeComponent) từ TaoTrangBaoCao;
            // thêm dashboard SAU CÙNG để nó nằm NGOÀI CÙNG phía trên (trên cả lblDoanhThu).
            trang.Controls.Add(pnlDoanhThuDashboard);
        }

        /// <summary>Màu nhấn riêng cho từng tab báo cáo - dùng chung cho cả tiêu đề cột bảng
        /// (GridStyle.ApDungHienDai) VÀ cho hiệu ứng tab được chọn (Tab_DrawItem), để mỗi tab
        /// có một "màu nhận diện" nhất quán xuyên suốt màn hình.</summary>
        internal static Color MauTab(int chiSoTab) => chiSoTab switch
        {
            0 => Color.FromArgb(24, 95, 165),   // Tổng quan hệ thống - xanh dương
            1 => Color.FromArgb(59, 109, 17),   // Sinh viên - xanh lá
            2 => Color.FromArgb(153, 60, 29),   // Phòng - cam đất
            3 => Color.FromArgb(83, 74, 183),   // Hợp đồng & Hóa đơn - tím
            4 => Color.FromArgb(133, 79, 11),   // Doanh thu - vàng đồng
            5 => Color.FromArgb(153, 53, 86),   // Nhân viên - hồng đậm
            _ => Color.RoyalBlue
        };

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTieuDeHeader = new Label();
            lblMoTaHeader = new Label();
            lblCapNhat = new Label();

            flpKpi = new FlowLayoutPanel();

            tabBaoCao = new TabControl();
            tabTongQuan = new TabPage();
            tabSinhVien = new TabPage();
            tabPhong = new TabPage();
            tabHopDong = new TabPage();
            tabDoanhThu = new TabPage();
            tabNhanVien = new TabPage();

            pnlActions = new Panel();
            pnlActionsVien = new Panel();
            flpActions = new FlowLayoutPanel();
            btnXuatExcel = new Button();
            btnLamMoi = new Button();
            btnDong = new Button();

            SuspendLayout();

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1080, 780);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Báo cáo thống kê";
            this.BackColor = Color.FromArgb(244, 246, 250);
            this.MinimumSize = new Size(900, 620);

            // ---------------------------------------------------------------
            // pnlHeader: thanh tiêu đề đầu trang (đồng bộ phong cách với Trang chủ
            // Quản trị viên - nền xanh navy đậm, tiêu đề + mô tả ngắn bên trái,
            // mốc thời gian cập nhật gần nhất ở góc phải).
            // ---------------------------------------------------------------
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 84;
            pnlHeader.BackColor = Color.FromArgb(24, 49, 95);

            lblTieuDeHeader.Text = "BÁO CÁO THỐNG KÊ TOÀN BỘ HỆ THỐNG";
            lblTieuDeHeader.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTieuDeHeader.ForeColor = Color.White;
            lblTieuDeHeader.AutoSize = false;
            lblTieuDeHeader.Location = new Point(24, 14);
            lblTieuDeHeader.Size = new Size(650, 30);
            lblTieuDeHeader.TextAlign = ContentAlignment.MiddleLeft;

            lblMoTaHeader.Text = "Sinh viên, phòng, hợp đồng, hóa đơn, doanh thu và nhân viên - xuất Excel toàn bộ";
            lblMoTaHeader.Font = new Font("Segoe UI", 9.5F);
            lblMoTaHeader.ForeColor = Color.FromArgb(190, 205, 230);
            lblMoTaHeader.AutoSize = false;
            lblMoTaHeader.Location = new Point(26, 48);
            lblMoTaHeader.Size = new Size(650, 22);
            lblMoTaHeader.TextAlign = ContentAlignment.MiddleLeft;

            lblCapNhat.Name = "lblCapNhat";
            lblCapNhat.Text = "Cập nhật lúc: --:--";
            lblCapNhat.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblCapNhat.ForeColor = Color.FromArgb(190, 205, 230);
            lblCapNhat.AutoSize = false;
            lblCapNhat.Location = new Point(660, 30);
            lblCapNhat.Size = new Size(280, 24);
            lblCapNhat.TextAlign = ContentAlignment.MiddleRight;
            lblCapNhat.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            pnlHeader.Controls.Add(lblTieuDeHeader);
            pnlHeader.Controls.Add(lblMoTaHeader);
            pnlHeader.Controls.Add(lblCapNhat);

            // ---------------------------------------------------------------
            // flpKpi: hàng 4 thẻ KPI tổng hợp nhanh (Tổng sinh viên, Tổng phòng,
            // Doanh thu đã thu, Hóa đơn chờ xác nhận) - dùng chung StatCard với
            // Trang chủ Quản trị viên để đồng bộ phong cách toàn ứng dụng, nhưng
            // là 4 chỉ số KHÁC (bổ sung góc nhìn) so với 4 thẻ ở Trang chủ.
            // ---------------------------------------------------------------
            flpKpi.Dock = DockStyle.Top;
            flpKpi.Height = 128;
            flpKpi.FlowDirection = FlowDirection.LeftToRight;
            flpKpi.WrapContents = false;
            flpKpi.AutoScroll = true;
            flpKpi.BackColor = Color.FromArgb(244, 246, 250);
            flpKpi.Padding = new Padding(14, 8, 0, 0);

            cardTongSinhVien = TaoTheKpiBaoCao("cardBcTongSinhVien", Color.FromArgb(24, 95, 165));   // Xanh dương
            cardTongPhongBc = TaoTheKpiBaoCao("cardBcTongPhong", Color.FromArgb(59, 109, 17));       // Xanh lá
            cardDoanhThuBc = TaoTheKpiBaoCao("cardBcDoanhThu", Color.FromArgb(133, 79, 11));         // Vàng đồng
            cardChoXacNhanBc = TaoTheKpiBaoCao("cardBcChoXacNhan", Color.FromArgb(153, 53, 86));     // Hồng đậm

            flpKpi.Controls.Add(cardTongSinhVien);
            flpKpi.Controls.Add(cardTongPhongBc);
            flpKpi.Controls.Add(cardDoanhThuBc);
            flpKpi.Controls.Add(cardChoXacNhanBc);

            // ---------------------------------------------------------------
            // tabBaoCao: 6 tab dữ liệu chi tiết, tự vẽ tiêu đề tab (owner-draw) để
            // tab đang chọn có gạch chân màu nhấn + chữ đậm thay vì kiểu tab mặc
            // định phẳng của Windows, đồng bộ với bộ màu nhấn của từng tab.
            // ---------------------------------------------------------------
            tabBaoCao.Dock = DockStyle.Fill;
            tabBaoCao.Font = new Font("Segoe UI", 9.5F);
            tabBaoCao.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabBaoCao.SizeMode = TabSizeMode.Fixed;
            tabBaoCao.ItemSize = new Size(158, 40);
            tabBaoCao.Padding = new Point(16, 6);
            tabBaoCao.DrawItem += tabBaoCao_DrawItem;
            tabBaoCao.SelectedIndexChanged += (s, e) => tabBaoCao.Invalidate();

            tabTongQuan.Text = "Tổng quan hệ thống";
            tabSinhVien.Text = "Sinh viên";
            tabPhong.Text = "Phòng";
            tabHopDong.Text = "Hợp đồng & Hóa đơn";
            tabDoanhThu.Text = "Doanh thu";
            tabNhanVien.Text = "Nhân viên";

            (lblTongQuan, dgvTongQuan) = TaoTrangBaoCao(tabTongQuan, MauTab(0));
            (lblSinhVien, dgvSinhVien) = TaoTrangBaoCao(tabSinhVien, MauTab(1));
            (lblPhong, dgvPhong) = TaoTrangBaoCao(tabPhong, MauTab(2));
            (lblHopDong, dgvHopDong) = TaoTrangBaoCao(tabHopDong, MauTab(3));
            (lblDoanhThu, dgvDoanhThu) = TaoTrangBaoCao(tabDoanhThu, MauTab(4));
            (lblNhanVien, dgvNhanVien) = TaoTrangBaoCao(tabNhanVien, MauTab(5));

            // Riêng tab Doanh thu: nội dung phía trên (dashboard + nhãn tóm tắt) khá cao,
            // nên cho phép TỰ CUỘN (AutoScroll) cả trang thay vì Dock=Fill như các tab khác
            // - nếu không, khi dashboard cao hơn vùng hiển thị thì dgvDoanhThu sẽ bị đẩy ra
            // ngoài mà không có cách nào cuộn xuống để xem (đúng lỗi đang gặp). dgvDoanhThu
            // vì vậy đổi từ Dock=Fill sang chiều cao cố định để nằm ngay dưới nhãn tóm tắt
            // và luôn xem được đầy đủ bằng cách cuộn trang xuống.
            tabDoanhThu.AutoScroll = true;
            dgvDoanhThu.Dock = DockStyle.Top;
            dgvDoanhThu.Height = 320;

            TaoBangDieuKhienDoanhThu(tabDoanhThu);

            tabBaoCao.TabPages.Add(tabTongQuan);
            tabBaoCao.TabPages.Add(tabSinhVien);
            tabBaoCao.TabPages.Add(tabPhong);
            tabBaoCao.TabPages.Add(tabHopDong);
            tabBaoCao.TabPages.Add(tabDoanhThu);
            tabBaoCao.TabPages.Add(tabNhanVien);

            // ---------------------------------------------------------------
            // pnlActions: thanh hành động cuối trang (đường viền mảnh phía trên +
            // 3 nút được nhóm lại, tự bám mép phải bằng FlowLayoutPanel để không
            // vỡ bố cục khi thay đổi kích thước cửa sổ - thay vì tọa độ tuyệt đối
            // như bản trước).
            // ---------------------------------------------------------------
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Height = 70;
            pnlActions.BackColor = Color.White;

            pnlActionsVien.Dock = DockStyle.Top;
            pnlActionsVien.Height = 1;
            pnlActionsVien.BackColor = Color.FromArgb(225, 228, 233);

            flpActions.Dock = DockStyle.Right;
            flpActions.FlowDirection = FlowDirection.LeftToRight;
            flpActions.WrapContents = false;
            flpActions.AutoSize = true;
            flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpActions.Padding = new Padding(0, 14, 20, 14);

            // btnXuatExcel
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.Size = new Size(150, 42);
            btnXuatExcel.Margin = new Padding(0, 0, 10, 0);
            btnXuatExcel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXuatExcel.BackColor = Color.SeaGreen;
            btnXuatExcel.ForeColor = Color.White;
            btnXuatExcel.FlatStyle = FlatStyle.Flat;
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.Cursor = Cursors.Hand;
            btnXuatExcel.Click += btnXuatExcel_Click;

            // btnLamMoi
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.Size = new Size(120, 42);
            btnLamMoi.Margin = new Padding(0, 0, 10, 0);
            btnLamMoi.Font = new Font("Segoe UI", 9.5F);
            btnLamMoi.BackColor = Color.White;
            btnLamMoi.ForeColor = Color.FromArgb(60, 64, 70);
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.FlatAppearance.BorderSize = 1;
            btnLamMoi.FlatAppearance.BorderColor = Color.FromArgb(210, 213, 218);
            btnLamMoi.Cursor = Cursors.Hand;
            btnLamMoi.Click += btnLamMoi_Click;

            // btnDong
            btnDong.Name = "btnDong";
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(110, 42);
            btnDong.Margin = new Padding(0);
            btnDong.Font = new Font("Segoe UI", 9.5F);
            btnDong.BackColor = Color.FromArgb(238, 240, 244);
            btnDong.ForeColor = Color.FromArgb(60, 64, 70);
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.Cursor = Cursors.Hand;
            btnDong.Click += btnDong_Click;

            flpActions.Controls.Add(btnXuatExcel);
            flpActions.Controls.Add(btnLamMoi);
            flpActions.Controls.Add(btnDong);

            pnlActions.Controls.Add(flpActions);
            pnlActions.Controls.Add(pnlActionsVien);

            // Fill trước, rồi các mép Bottom/Top theo đúng thứ tự trong-ra-ngoài
            // (control thêm SAU cùng nằm ngoài cùng) để pnlHeader luôn ở mép trên
            // cùng, flpKpi ngay bên dưới, pnlActions ở mép dưới, tabBaoCao lấp đầy
            // phần còn lại - giống cách bố cục đã dùng ở Trang chủ (FrmMain).
            Controls.Add(tabBaoCao);
            Controls.Add(pnlActions);
            Controls.Add(flpKpi);
            Controls.Add(pnlHeader);

            Load += FrmBaoCaoThongKe_Load;

            ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Label lblTieuDeHeader;
        private Label lblMoTaHeader;
        private Label lblCapNhat;

        private FlowLayoutPanel flpKpi;
        private StatCard cardTongSinhVien;
        private StatCard cardTongPhongBc;
        private StatCard cardDoanhThuBc;
        private StatCard cardChoXacNhanBc;

        private TabControl tabBaoCao;
        private TabPage tabTongQuan;
        private TabPage tabSinhVien;
        private TabPage tabPhong;
        private TabPage tabHopDong;
        private TabPage tabDoanhThu;
        private TabPage tabNhanVien;

        private Label lblTongQuan;
        private DataGridView dgvTongQuan;
        private Label lblSinhVien;
        private DataGridView dgvSinhVien;
        private Label lblPhong;
        private DataGridView dgvPhong;
        private Label lblHopDong;
        private DataGridView dgvHopDong;
        private Label lblDoanhThu;
        private DataGridView dgvDoanhThu;
        private Label lblNhanVien;
        private DataGridView dgvNhanVien;

        // Bảng điều khiển trực quan riêng của tab Doanh thu (UC19).
        private Panel pnlDoanhThuDashboard;
        private FlowLayoutPanel flpTheDoanhThu;
        private StatCard cardDoanhThuThangNay;
        private StatCard cardHoaDonChuaTT;
        private StatCard cardDangKyChoDuyetTab;
        private CardPanel cardBieuDoDoanhThu;
        private BieuDoCotDoanhThu bieuDoDoanhThu;
        private CardPanel cardLapDayKhu;
        private Panel pnlLapDayKhuScroll;
        private DanhSachLapDayKhu dsLapDayKhu;

        private Panel pnlActions;
        private Panel pnlActionsVien;
        private FlowLayoutPanel flpActions;
        private Button btnXuatExcel;
        private Button btnLamMoi;
        private Button btnDong;

        #endregion
    }
}
