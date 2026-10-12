using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DoAnNhom06.Controls
{
    /// <summary>
    /// Khung thẻ trắng bo góc, có viền mảnh và tiêu đề in đậm ở góc trên - dùng để
    /// nhóm các khối trong Báo cáo thống kê (biểu đồ, tình trạng lấp đầy...) lại
    /// thành từng "thẻ" riêng biệt, giống bố cục các khối trên Trang chủ.
    /// Nội dung bên trong (biểu đồ, danh sách...) được thêm vào như Controls con
    /// bình thường; <see cref="Padding"/> đã chừa sẵn chỗ cho tiêu đề ở trên.
    /// </summary>
    public class CardPanel : Panel
    {
        public string TieuDe { get; set; } = "";

        public CardPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.White;
            Padding = new Padding(20, 46, 20, 16);
            Margin = new Padding(0, 0, 0, 16);
        }

        private static GraphicsPath BoRound(RectangleF r, float bk)
        {
            float d = bk * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var outer = new RectangleF(0.5f, 0.5f, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            using var path = BoRound(outer, 12f);

            using (var brNen = new SolidBrush(Color.White))
                g.FillPath(brNen, path);
            using (var penVien = new Pen(Color.FromArgb(228, 231, 236), 1f))
                g.DrawPath(penVien, path);

            using var fontTitle = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            using var brushTitle = new SolidBrush(Color.FromArgb(35, 39, 46));
            g.DrawString(TieuDe, fontTitle, brushTitle, new PointF(20, 16));
        }
    }
}
