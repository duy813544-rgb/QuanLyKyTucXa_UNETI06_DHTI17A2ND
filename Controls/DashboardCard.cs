using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DoAnNhom06.Controls
{
    /// <summary>
    /// Icon vector riêng cho từng chức năng trên Trang chủ. Vẽ bằng GDI+ thuần
    /// (không phụ thuộc font icon nào) để chắc chắn hiển thị đúng trên mọi máy,
    /// và để mỗi chức năng có một "hình ảnh đại diện" khác nhau thay vì chỉ
    /// khác nhau ở màu nền như trước đây.
    /// </summary>
    public enum DashboardIconKind
    {
        NguoiDung,
        Phong,
        Duyet,
        HopDong,
        Chia,
        LamMoi,
        ViTien,
        HoaDon,
        KinhLup,

        // Các icon dành cho Quản trị viên
        TaiKhoan,
        BieuDo,
        ChuyenPhong,
        MayIn,

        /// <summary>Icon cảnh báo (tam giác + dấu chấm than) - dùng cho Báo hỏng cơ sở vật chất.</summary>
        CanhBao
    }

    /// <summary>
    /// Thẻ chức năng cho Trang chủ (FrmMain). Mỗi thẻ tự vẽ toàn bộ (owner-draw):
    /// icon riêng, màu nhấn riêng, tiêu đề và một dòng mô tả ngắn cho biết chức
    /// năng dùng để làm gì — điều mà các Button phẳng trước đây không có.
    /// Toàn bộ thẻ là một vùng bấm được (giống Button), có hiệu ứng hover/press
    /// và hỗ trợ bấm bằng bàn phím (Enter/Space) khi đang được focus.
    /// </summary>
    public class DashboardCard : Control
    {
        private bool _hover;
        private bool _pressed;

        public DashboardIconKind IconKind { get; set; } = DashboardIconKind.NguoiDung;
        public string Subtitle { get; set; } = string.Empty;
        public Color AccentColor { get; set; } = Color.RoyalBlue;

        /// <summary>
        /// True để vẽ thẻ theo dạng thanh tìm kiếm dài kiểu Google (bo tròn hết cỡ,
        /// icon + tiêu đề + mô tả nằm trên một hàng ngang) thay vì thẻ vuông thông thường.
        /// Dùng cho thẻ "Tra cứu thông tin" ở đầu trang chủ.
        /// </summary>
        public bool KieuThanhTimKiem { get; set; } = false;

        public DashboardCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.UserPaint
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.Selectable
                     | ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            TabStop = true;
            Size = new Size(268, 152);
            Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            ForeColor = Color.FromArgb(35, 35, 40);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            _pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _pressed = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Space) return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                _pressed = true;
                Invalidate();
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                _pressed = false;
                Invalidate();
                OnClick(EventArgs.Empty);
            }
            base.OnKeyUp(e);
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

        private static Color Lighten(Color c, float amount)
        {
            return Color.FromArgb(
                c.A,
                (int)(c.R + (255 - c.R) * amount),
                (int)(c.G + (255 - c.G) * amount),
                (int)(c.B + (255 - c.B) * amount));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (KieuThanhTimKiem)
            {
                VeKieuThanhTimKiem(e.Graphics);
                return;
            }

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var outer = new RectangleF(1, 1, Width - 3, Height - 3);
            using var path = BoRound(outer, 14f);

            Color viền = _hover ? AccentColor : Color.FromArgb(226, 228, 233);
            Color nen = _pressed ? Lighten(AccentColor, 0.90f)
                       : _hover ? Lighten(AccentColor, 0.94f)
                       : Color.White;

            using (var brNen = new SolidBrush(nen))
                g.FillPath(brNen, path);
            using (var butViền = new Pen(viền, _hover ? 1.6f : 1f))
                g.DrawPath(butViền, path);

            if (Focused)
            {
                using var penFocus = new Pen(Color.FromArgb(140, AccentColor), 1.4f) { DashStyle = DashStyle.Dot };
                var focusRect = new RectangleF(outer.X + 2, outer.Y + 2, outer.Width - 4, outer.Height - 4);
                using var focusPath = BoRound(focusRect, 12f);
                g.DrawPath(penFocus, focusPath);
            }

            // Vòng tròn icon riêng cho từng chức năng.
            float circleD = 46f;
            var circleRect = new RectangleF(16, 16, circleD, circleD);
            using (var brIcon = new SolidBrush(AccentColor))
                g.FillEllipse(brIcon, circleRect);
            VeIcon(g, circleRect);

            // Tiêu đề.
            var titleRect = new RectangleF(16, 70, Width - 32, 24);
            using (var brChu = new SolidBrush(ForeColor))
            using (var fmtTitle = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center })
                g.DrawString(Text, Font, brChu, titleRect, fmtTitle);

            // Mô tả ngắn.
            var subRect = new RectangleF(16, 94, Width - 32, Height - 94 - 12);
            using var fontSub = new Font("Segoe UI", 8.7F, FontStyle.Regular);
            using (var brSub = new SolidBrush(Color.FromArgb(120, 124, 132)))
                g.DrawString(Subtitle, fontSub, brSub, subRect);

            // Mũi tên chỉ dẫn, chỉ hiện khi hover, tạo cảm giác "đi tới".
            if (_hover)
            {
                float ax = Width - 26, ay = 24;
                using var penArrow = new Pen(AccentColor, 2.2f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                g.DrawLines(penArrow, new[]
                {
                    new PointF(ax - 5, ay - 6),
                    new PointF(ax + 2, ay),
                    new PointF(ax - 5, ay + 6)
                });
            }
        }

        /// <summary>Vẽ thẻ dạng thanh tìm kiếm dài, bo tròn hết cỡ (kiểu ô tìm kiếm của Google),
        /// icon kính lúp + tiêu đề + mô tả nằm ngang trên một hàng, chiếm trọn chiều rộng được cấp.</summary>
        private void VeKieuThanhTimKiem(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var outer = new RectangleF(1, 1, Width - 3, Height - 3);
            float banKinh = outer.Height / 2f; // Bo tròn hết cỡ -> dạng viên thuốc (pill).
            using var path = BoRound(outer, banKinh);

            Color viền = _hover ? AccentColor : Color.FromArgb(220, 223, 229);
            Color nen = _pressed ? Lighten(AccentColor, 0.90f)
                       : _hover ? Lighten(AccentColor, 0.95f)
                       : Color.White;

            using (var brNen = new SolidBrush(nen))
                g.FillPath(brNen, path);
            using (var butViền = new Pen(viền, _hover ? 1.8f : 1.2f))
                g.DrawPath(butViền, path);

            if (Focused)
            {
                using var penFocus = new Pen(Color.FromArgb(140, AccentColor), 1.4f) { DashStyle = DashStyle.Dot };
                var focusRect = new RectangleF(outer.X + 2, outer.Y + 2, outer.Width - 4, outer.Height - 4);
                using var focusPath = BoRound(focusRect, banKinh - 2);
                g.DrawPath(penFocus, focusPath);
            }

            // Icon kính lúp bên trái, căn giữa theo chiều dọc.
            float circleD = Math.Min(40f, Height - 16f);
            var circleRect = new RectangleF(14, (Height - circleD) / 2f, circleD, circleD);
            using (var brIcon = new SolidBrush(AccentColor))
                g.FillEllipse(brIcon, circleRect);
            VeIcon(g, circleRect);

            // Tiêu đề (đậm) + mô tả (nhạt hơn) nằm trên cùng một hàng, giống placeholder ô tìm kiếm.
            float chuX = circleRect.Right + 14f;
            var titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            SizeF titleSize = g.MeasureString(Text, titleFont, Width);
            var titleRect = new RectangleF(chuX, 0, titleSize.Width + 4, Height);
            using (var brChu = new SolidBrush(ForeColor))
            using (var fmtTitle = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter })
                g.DrawString(Text, titleFont, brChu, titleRect, fmtTitle);
            titleFont.Dispose();

            float subX = titleRect.Right + 10f;
            if (subX < Width - 40)
            {
                using var fontSub = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                var subRect = new RectangleF(subX, 0, Math.Max(0, Width - subX - 40), Height);
                using var brSub = new SolidBrush(Color.FromArgb(140, 144, 150));
                using var fmtSub = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
                g.DrawString(Subtitle, fontSub, brSub, subRect, fmtSub);
            }

            // Icon kính lúp mờ bên phải, luôn hiện (giống ô tìm kiếm luôn có icon tìm kiếm),
            // sáng rõ hơn khi hover để gợi ý có thể bấm vào.
            float rD = 20f;
            var rRect = new RectangleF(Width - rD - 20, (Height - rD) / 2f, rD, rD);
            using var penR = new Pen(_hover ? AccentColor : Color.FromArgb(190, 193, 199), 2.2f)
            { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawEllipse(penR, rRect.X + 2, rRect.Y + 2, rRect.Width - 8, rRect.Height - 8);
            g.DrawLine(penR, rRect.Right - 6, rRect.Bottom - 6, rRect.Right - 1, rRect.Bottom - 1);
        }

        /// <summary>Vẽ icon riêng theo từng loại — mỗi chức năng một hình khác nhau.</summary>
        private void VeIcon(Graphics g, RectangleF circle)
        {
            using var pen = new Pen(Color.White, 2.1f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
            using var brush = new SolidBrush(Color.White);
            float cx = circle.X + circle.Width / 2f;
            float cy = circle.Y + circle.Height / 2f;

            switch (IconKind)
            {
                case DashboardIconKind.NguoiDung:
                    g.FillEllipse(brush, cx - 6, cy - 12, 12, 12);
                    g.FillPie(brush, cx - 11, cy - 2, 22, 20, 180, 180);
                    break;

                case DashboardIconKind.Phong:
                    g.DrawRectangle(pen, cx - 10, cy - 12, 20, 24);
                    g.FillEllipse(brush, cx + 4, cy - 2, 3, 3);
                    break;

                case DashboardIconKind.Duyet:
                    g.DrawRectangle(pen, cx - 9, cy - 12, 18, 22);
                    g.DrawLine(pen, cx - 5, cy - 14, cx + 5, cy - 14);
                    g.DrawLines(pen, new[]
                    {
                        new PointF(cx - 5, cy),
                        new PointF(cx - 1, cy + 5),
                        new PointF(cx + 6, cy - 6)
                    });
                    break;

                case DashboardIconKind.HopDong:
                    g.DrawRectangle(pen, cx - 9, cy - 13, 18, 24);
                    g.DrawLine(pen, cx - 5, cy - 6, cx + 5, cy - 6);
                    g.DrawLine(pen, cx - 5, cy, cx + 5, cy);
                    g.DrawLine(pen, cx - 5, cy + 6, cx + 2, cy + 6);
                    break;

                case DashboardIconKind.Chia:
                    g.DrawEllipse(pen, cx - 11, cy - 9, 12, 12);
                    g.DrawLine(pen, cx, cy + 2, cx + 13, cy + 15);
                    g.DrawLine(pen, cx + 9, cy + 11, cx + 13, cy + 7);
                    g.DrawLine(pen, cx + 12, cy + 14, cx + 16, cy + 10);
                    break;

                case DashboardIconKind.LamMoi:
                    g.DrawArc(pen, cx - 11, cy - 11, 22, 22, -20, 260);
                    g.FillPolygon(brush, new[]
                    {
                        new PointF(cx + 9, cy - 13),
                        new PointF(cx + 15, cy - 9),
                        new PointF(cx + 7, cy - 6)
                    });
                    break;

                case DashboardIconKind.ViTien:
                    g.DrawRectangle(pen, cx - 12, cy - 8, 24, 17);
                    g.DrawLine(pen, cx - 12, cy - 2, cx + 12, cy - 2);
                    g.FillEllipse(brush, cx + 4, cy - 5, 6, 6);
                    break;

                case DashboardIconKind.HoaDon:
                    var zigzag = new System.Collections.Generic.List<PointF>();
                    for (int i = 0; i <= 6; i++)
                        zigzag.Add(new PointF(cx - 9 + i * 3, cy + 12 + (i % 2 == 0 ? 0 : -3)));
                    var outline = new System.Collections.Generic.List<PointF>
                    {
                        new(cx - 9, cy - 13),
                        new(cx + 9, cy - 13),
                        new(cx + 9, cy + 12)
                    };
                    outline.AddRange(zigzag);
                    outline.Add(new PointF(cx - 9, cy + 12));
                    g.DrawLines(pen, outline.ToArray());
                    g.DrawLines(pen, new[] { new PointF(cx - 12, cy - 3), new PointF(cx - 3, cy + 3), new PointF(cx + 5, cy - 6) });
                    break;
                case DashboardIconKind.TaiKhoan:
                    // Icon tài khoản
                    g.FillEllipse(brush, cx - 6, cy - 11, 12, 12);
                    g.FillPie(brush, cx - 11, cy - 1, 22, 18, 180, 180);
                    break;

                case DashboardIconKind.BieuDo:
                    // Icon biểu đồ
                    g.DrawLine(pen, cx - 11, cy + 11, cx - 11, cy - 10);
                    g.DrawLine(pen, cx - 11, cy + 11, cx + 12, cy + 11);
                    g.DrawRectangle(pen, cx - 7, cy + 1, 4, 10);
                    g.DrawRectangle(pen, cx, cy - 5, 4, 16);
                    g.DrawRectangle(pen, cx + 7, cy - 10, 4, 21);
                    break;

                case DashboardIconKind.ChuyenPhong:
                    // Icon chuyển phòng
                    g.DrawRectangle(pen, cx - 10, cy - 9, 16, 18);
                    g.DrawLine(pen, cx - 5, cy, cx + 12, cy);
                    g.DrawLine(pen, cx + 7, cy - 5, cx + 12, cy);
                    g.DrawLine(pen, cx + 7, cy + 5, cx + 12, cy);
                    break;

                case DashboardIconKind.MayIn:
                    // Icon máy in
                    g.DrawRectangle(pen, cx - 9, cy - 3, 18, 12);
                    g.DrawRectangle(pen, cx - 7, cy - 12, 14, 9);
                    g.DrawRectangle(pen, cx - 6, cy + 5, 12, 9);
                    g.FillEllipse(brush, cx + 5, cy - 1, 3, 3);
                    break;
                case DashboardIconKind.KinhLup:
                    g.DrawEllipse(pen, cx - 11, cy - 12, 15, 15);
                    g.DrawLine(pen, cx + 3, cy + 2, cx + 12, cy + 11);
                    break;

                case DashboardIconKind.CanhBao:
                    g.DrawPolygon(pen, new[]
                    {
                        new PointF(cx, cy - 13),
                        new PointF(cx + 12, cy + 10),
                        new PointF(cx - 12, cy + 10)
                    });
                    g.DrawLine(pen, cx, cy - 4, cx, cy + 3);
                    g.FillEllipse(brush, cx - 1.2f, cy + 6, 2.4f, 2.4f);
                    break;
            }
        }
    }
}
