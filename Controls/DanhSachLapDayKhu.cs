using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DoAnNhom06.Controls
{
    /// <summary>
    /// Danh sách thanh tiến trình (tự vẽ) cho khối "Tình trạng lấp đầy theo khu nhà"
    /// trong Báo cáo thống kê: mỗi khu nhà 1 hàng - tên khu + tỷ lệ % lấp đầy ở trên,
    /// thanh tiến trình bo tròn ở dưới. Khu có tỷ lệ lấp đầy thấp (dưới ngưỡng cảnh
    /// báo) được tô màu cam để dễ nhận ra cần chú ý hơn các khu còn lại.
    /// </summary>
    public class DanhSachLapDayKhu : Control
    {
        private List<(string TenKhu, double TyLeLapDay)> _duLieu = new();

        public int ChieuCaoMoiHang { get; set; } = 56;

        /// <summary>Tỷ lệ (%) trở xuống thì coi là cần chú ý, tô màu cam thay vì xanh.</summary>
        public double NguongCanhBao { get; set; } = 75;

        public Color MauBinhThuong { get; set; } = Color.FromArgb(66, 99, 235);
        public Color MauCanhBao { get; set; } = Color.FromArgb(214, 92, 30);

        public DanhSachLapDayKhu()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.White;
        }

        /// <summary>Nạp danh sách (Tên khu, Tỷ lệ lấp đầy %) và tự điều chỉnh chiều cao theo số hàng.</summary>
        public void SetDuLieu(List<(string TenKhu, double TyLeLapDay)> duLieu)
        {
            _duLieu = duLieu ?? new List<(string, double)>();
            Height = Math.Max(ChieuCaoMoiHang, _duLieu.Count * ChieuCaoMoiHang);
            Invalidate();
        }

        private static GraphicsPath ThanhTron(RectangleF r)
        {
            float d = r.Height;
            var p = new GraphicsPath();
            if (r.Width <= d)
            {
                p.AddEllipse(r);
                return p;
            }

            p.AddArc(r.X, r.Y, d, d, 90, 180);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 180);
            p.CloseFigure();
            return p;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (_duLieu.Count == 0 || Width <= 0) return;

            using var fontTen = new Font("Segoe UI", 10F);
            using var fontPhanTram = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var brushTen = new SolidBrush(Color.FromArgb(50, 54, 62));

            for (int i = 0; i < _duLieu.Count; i++)
            {
                var (tenKhu, tyLe) = _duLieu[i];
                float yHang = i * ChieuCaoMoiHang;

                var rectTen = new RectangleF(0, yHang, Width - 80, 24);
                g.DrawString(tenKhu, fontTen, brushTen, rectTen);

                Color mau = tyLe < NguongCanhBao ? MauCanhBao : MauBinhThuong;

                var rectPhanTram = new RectangleF(Width - 80, yHang, 80, 24);
                using var fmtPhai = new StringFormat { Alignment = StringAlignment.Far };
                using var brushPhanTram = new SolidBrush(mau);
                g.DrawString($"{tyLe:N0}%", fontPhanTram, brushPhanTram, rectPhanTram, fmtPhai);

                float yThanh = yHang + 28;
                float caoThanh = 10f;
                var rectNen = new RectangleF(0, yThanh, Width, caoThanh);
                using (var pathNen = ThanhTron(rectNen))
                using (var brushNen = new SolidBrush(Color.FromArgb(232, 234, 238)))
                {
                    g.FillPath(brushNen, pathNen);
                }

                float tyLeVe = (float)Math.Max(0, Math.Min(100, tyLe)) / 100f;
                float rongDay = Width * tyLeVe;
                if (rongDay > 1)
                {
                    var rectDay = new RectangleF(0, yThanh, rongDay, caoThanh);
                    using var pathDay = ThanhTron(rectDay);
                    using var brushDay = new SolidBrush(mau);
                    g.FillPath(brushDay, pathDay);
                }
            }
        }
    }
}
