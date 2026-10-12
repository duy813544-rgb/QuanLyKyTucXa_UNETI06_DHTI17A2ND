using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DoAnNhom06.Controls
{
    /// <summary>
    /// Ô nhập từ khóa bo tròn hết cỡ (dạng "viên thuốc"), có icon kính lúp bên
    /// trái và nút xóa (x) bên phải khi có nội dung - đúng kiểu thanh tìm kiếm
    /// của Google. Bọc quanh một TextBox thật (không viền) để vẫn gõ/dán/chọn
    /// văn bản bình thường, chỉ khác phần vẽ nền.
    /// </summary>
    public class SearchBox : Panel
    {
        public TextBox Input { get; }
        public Color AccentColor { get; set; } = Color.RoyalBlue;

        private bool _focus;
        private readonly Label _lblXoa;

        public string PlaceholderText
        {
            get => Input.PlaceholderText;
            set => Input.PlaceholderText = value;
        }

        public override string Text
        {
            get => Input.Text;
            set => Input.Text = value;
        }

        public event EventHandler? XoaClick;

        public SearchBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.UserPaint, true);

            BackColor = Color.White;
            Height = 48;

            Input = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 11.5F),
                BackColor = Color.White,
            };
            Input.GotFocus += (_, _) => { _focus = true; Invalidate(); };
            Input.LostFocus += (_, _) => { _focus = false; Invalidate(); };
            Input.TextChanged += (_, _) => { _lblXoa.Visible = Input.Text.Length > 0; };

            _lblXoa = new Label
            {
                Text = "✕",
                AutoSize = false,
                Size = new Size(22, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(154, 160, 166),
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Visible = false,
                BackColor = Color.Transparent
            };
            _lblXoa.Click += (_, _) => { Input.Clear(); XoaClick?.Invoke(this, EventArgs.Empty); Input.Focus(); };

            Controls.Add(Input);
            Controls.Add(_lblXoa);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // OnResize có thể được gọi bởi base constructor (Control/Panel) trước khi
            // Input và _lblXoa được khởi tạo trong constructor của SearchBox.
            // Nếu không kiểm tra null ở đây sẽ ném NullReferenceException.
            if (Input == null || _lblXoa == null)
                return;

            int iconZone = Height; // vùng icon kính lúp bên trái, vuông bằng chiều cao
            Input.Location = new Point(iconZone, (Height - Input.Height) / 2);
            Input.Width = Width - iconZone - 40;

            _lblXoa.Location = new Point(Width - 34, (Height - _lblXoa.Height) / 2);
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

            var outer = new RectangleF(1f, 1f, Width - 2, Height - 2);
            using var path = BoTron(outer);

            using (var brNen = new SolidBrush(Color.White))
                g.FillPath(brNen, path);

            Color vien = _focus ? AccentColor : Color.FromArgb(223, 225, 229);
            using (var pen = new Pen(vien, _focus ? 1.8f : 1.2f))
                g.DrawPath(pen, path);

            if (_focus)
            {
                using var glow = new Pen(Color.FromArgb(60, AccentColor), 3.5f);
                g.DrawPath(glow, path);
                using (var brNen2 = new SolidBrush(Color.White)) g.FillPath(brNen2, path);
                using (var pen2 = new Pen(AccentColor, 1.8f)) g.DrawPath(pen2, path);
            }

            // Icon kính lúp, căn giữa vùng vuông bên trái.
            float d = Math.Min(18f, Height - 20f);
            float cx = Height / 2f, cy = Height / 2f;
            using var penIcon = new Pen(_focus ? AccentColor : Color.FromArgb(95, 99, 104), 2f)
            { StartCap = LineCap.Round, EndCap = LineCap.Round };
            var circle = new RectangleF(cx - d / 2 - 2, cy - d / 2 - 2, d, d);
            g.DrawEllipse(penIcon, circle);
            g.DrawLine(penIcon, circle.Right - 1, circle.Bottom - 1, circle.Right + 5, circle.Bottom + 5);
        }
    }
}
