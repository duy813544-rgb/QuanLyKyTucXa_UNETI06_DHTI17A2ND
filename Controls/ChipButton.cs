using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DoAnNhom06.Controls
{
    /// <summary>
    /// Nút bo tròn hết cỡ (dạng "chip") dùng để chọn 1 lựa chọn trong một nhóm,
    /// ví dụ chọn loại tra cứu (Sinh viên / Phòng / Hợp đồng). Thay cho ComboBox
    /// truyền thống để giao diện gần với các thanh công cụ tìm kiếm hiện đại
    /// (Google, Gmail...), nơi bộ lọc thường hiển thị thành các "chip" bấm được
    /// thay vì phải mở dropdown.
    /// </summary>
    public class ChipButton : Control
    {
        private bool _hover;
        private bool _daChon;

        public Color AccentColor { get; set; } = Color.RoyalBlue;

        /// <summary>True nếu chip đang được chọn (tô đậm màu nhấn).</summary>
        public bool DaChon
        {
            get => _daChon;
            set { _daChon = value; Invalidate(); }
        }

        public ChipButton()
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
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            Size = new Size(120, 34);
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected override bool IsInputKey(Keys keyData) => keyData is Keys.Enter or Keys.Space || base.IsInputKey(keyData);

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Enter or Keys.Space) OnClick(EventArgs.Empty);
            base.OnKeyUp(e);
        }

        private static GraphicsPath BoTron(RectangleF r)
        {
            float d = r.Height;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 90, 180);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 180);
            p.CloseFigure();
            return p;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var outer = new RectangleF(0.5f, 0.5f, Width - 1, Height - 1);
            using var path = BoTron(outer);

            Color nen = _daChon ? AccentColor
                       : _hover ? Color.FromArgb(240, 242, 245)
                       : Color.White;
            Color vien = _daChon ? AccentColor : Color.FromArgb(218, 220, 224);
            Color chu = _daChon ? Color.White : Color.FromArgb(60, 64, 67);

            using (var br = new SolidBrush(nen)) g.FillPath(br, path);
            using (var p = new Pen(vien, 1f)) g.DrawPath(p, path);

            using var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using var brChu = new SolidBrush(chu);
            g.DrawString(Text, Font, brChu, outer, fmt);

            if (Focused)
            {
                using var penFocus = new Pen(Color.FromArgb(140, AccentColor), 1.2f) { DashStyle = DashStyle.Dot };
                var focusRect = new RectangleF(outer.X + 2, outer.Y + 2, outer.Width - 4, outer.Height - 4);
                using var focusPath = BoTron(focusRect);
                g.DrawPath(penFocus, focusPath);
            }
        }
    }
}
