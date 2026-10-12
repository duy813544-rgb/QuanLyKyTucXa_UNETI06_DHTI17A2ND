using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace DoAnNhom06.Controls
{
    /// <summary>
    /// Biểu đồ cột đơn giản (tự vẽ bằng GDI+, không phụ thuộc thư viện ngoài) cho
    /// khối "Doanh thu theo tháng" trong Báo cáo thống kê. Cột của tháng được đánh
    /// dấu nổi bật (thường là tháng hiện tại) hiển thị màu khác các cột còn lại;
    /// di chuột vào một cột sẽ hiện số tiền chính xác của tháng đó.
    /// </summary>
    public class BieuDoCotDoanhThu : Control
    {
        private List<(string Nhan, decimal GiaTri)> _duLieu = new();
        private int _chiSoNoiBat = -1;
        private int _chiSoHover = -1;
        private readonly ToolTip _toolTip = new();
        private readonly List<RectangleF> _vungCot = new();

        public Color MauCotThuong { get; set; } = Color.FromArgb(66, 99, 235);
        public Color MauCotNoiBat { get; set; } = Color.FromArgb(16, 150, 105);

        public BieuDoCotDoanhThu()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.White;
            Height = 230;

            MouseMove += BieuDoCotDoanhThu_MouseMove;
            MouseLeave += (s, e) =>
            {
                if (_chiSoHover != -1)
                {
                    _chiSoHover = -1;
                    Invalidate();
                }
            };
        }

        /// <summary>Nạp dữ liệu để vẽ lại biểu đồ.</summary>
        /// <param name="duLieu">Danh sách (Nhãn cột, Giá trị) theo đúng thứ tự hiển thị trái sang phải.</param>
        /// <param name="chiSoNoiBat">Chỉ số cột cần tô màu nổi bật (VD: tháng hiện tại); -1 nếu không có.</param>
        public void SetDuLieu(List<(string Nhan, decimal GiaTri)> duLieu, int chiSoNoiBat = -1)
        {
            _duLieu = duLieu ?? new List<(string, decimal)>();
            _chiSoNoiBat = chiSoNoiBat;
            Invalidate();
        }

        private void BieuDoCotDoanhThu_MouseMove(object? sender, MouseEventArgs e)
        {
            int chiSoMoi = -1;
            for (int i = 0; i < _vungCot.Count; i++)
            {
                if (_vungCot[i].Contains(e.Location))
                {
                    chiSoMoi = i;
                    break;
                }
            }

            if (chiSoMoi != _chiSoHover)
            {
                _chiSoHover = chiSoMoi;
                if (chiSoMoi >= 0 && chiSoMoi < _duLieu.Count)
                {
                    var (nhan, giaTri) = _duLieu[chiSoMoi];
                    _toolTip.SetToolTip(this, $"{nhan}: {giaTri:N0} đ");
                }
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            _vungCot.Clear();

            if (_duLieu.Count == 0 || Width <= 0 || Height <= 0)
                return;

            const float chieuCaoNhan = 24f;
            const float leTren = 10f;
            const float leNgang = 8f;

            float vungVeCao = Height - chieuCaoNhan - leTren;
            if (vungVeCao <= 0) return;

            decimal maxGiaTri = _duLieu.Max(x => x.GiaTri);
            if (maxGiaTri <= 0) maxGiaTri = 1;

            int soCot = _duLieu.Count;
            float khoangCachTong = Width - leNgang * 2;
            float doRongCot = khoangCachTong / soCot * 0.55f;
            float khoangHo = khoangCachTong / soCot * 0.45f;

            using var fontNhan = new Font("Segoe UI", 9F);
            using var brushNhan = new SolidBrush(Color.FromArgb(120, 124, 132));
            using var fmt = new StringFormat { Alignment = StringAlignment.Center };

            for (int i = 0; i < soCot; i++)
            {
                float xTam = leNgang + (khoangHo + doRongCot) * i + khoangHo / 2f + doRongCot / 2f;
                float tyLeCao = (float)(_duLieu[i].GiaTri / maxGiaTri);
                float caoCot = Math.Max(4f, vungVeCao * tyLeCao);

                var rectCot = new RectangleF(xTam - doRongCot / 2f, leTren + (vungVeCao - caoCot), doRongCot, caoCot);
                _vungCot.Add(new RectangleF(xTam - (doRongCot + khoangHo) / 2f, 0, doRongCot + khoangHo, Height));

                bool noiBat = i == _chiSoNoiBat;
                bool dangHover = i == _chiSoHover;
                Color mauCot = noiBat ? MauCotNoiBat : MauCotThuong;
                if (dangHover) mauCot = ControlPaint.Light(mauCot, 0.15f);

                using var path = BoTronDinhTren(rectCot, 6f);
                using var brushCot = new SolidBrush(mauCot);
                g.FillPath(brushCot, path);

                var rectNhan = new RectangleF(xTam - doRongCot, Height - chieuCaoNhan, doRongCot * 2, chieuCaoNhan);
                g.DrawString(_duLieu[i].Nhan, fontNhan, brushNhan, rectNhan, fmt);
            }
        }

        private static GraphicsPath BoTronDinhTren(RectangleF r, float bk)
        {
            float d = Math.Min(bk * 2, Math.Min(r.Width, r.Height));
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddLine(r.Right, r.Y + d / 2, r.Right, r.Bottom);
            p.AddLine(r.Right, r.Bottom, r.X, r.Bottom);
            p.CloseFigure();
            return p;
        }
    }
}
