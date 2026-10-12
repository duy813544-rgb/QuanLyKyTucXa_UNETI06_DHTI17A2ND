using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Hộp thoại "Xem chi tiết" dùng chung cho màn hình Tra cứu thông tin (UC10):
    /// hiển thị đầy đủ các trường của một bản ghi (Sinh viên / Phòng / Hợp đồng)
    /// dưới dạng nhãn : giá trị, kèm theo (nếu có) một bảng dữ liệu liên quan,
    /// ví dụ danh sách hợp đồng của sinh viên, danh sách sinh viên đang ở trong
    /// phòng, hoặc danh sách hóa đơn của hợp đồng.
    /// </summary>
    public partial class FrmChiTietTraCuu : BaseAppForm
    {
        /// <summary>Tên cột nút "Xem chi tiết" được thêm vào bảng dữ liệu liên quan khi
        /// đó là danh sách hóa đơn (xem <see cref="_coNutXemHoaDon"/>).</summary>
        private const string ColXemHoaDon = "colXemHoaDon";

        /// <summary>True khi bảng dữ liệu liên quan (dgvLienQuan) đang hiển thị danh sách
        /// HÓA ĐƠN (có cột MaHoaDon) - lúc đó cần thêm nút "Xem chi tiết" để mở
        /// <see cref="FrmChiTietHoaDon"/> xem đầy đủ các dòng phí của từng hóa đơn.</summary>
        private readonly bool _coNutXemHoaDon;

        /// <param name="tieuDe">Tiêu đề hiển thị trên đầu Form, ví dụ "Chi tiết sinh viên: SV001".</param>
        /// <param name="thongTin">Danh sách (nhãn, giá trị) hiển thị trong bảng chi tiết.</param>
        /// <param name="tieuDeLienQuan">Tiêu đề khu vực dữ liệu liên quan (để trống nếu không có).</param>
        /// <param name="dsLienQuan">Danh sách dữ liệu liên quan để hiển thị dạng bảng (để trống nếu không có).</param>
        /// <param name="coNutXemHoaDon">True nếu <paramref name="dsLienQuan"/> là danh sách hóa đơn
        /// (có cột MaHoaDon) và cần thêm nút "Xem chi tiết" để mở chi tiết từng hóa đơn.</param>
        public FrmChiTietTraCuu(string tieuDe, List<(string Nhan, string GiaTri)> thongTin,
            string? tieuDeLienQuan = null, IList<object>? dsLienQuan = null, bool coNutXemHoaDon = false)
        {
            InitializeComponent();

            Text = "Chi tiết";
            lblTitle.Text = tieuDe.ToUpperInvariant();
            _coNutXemHoaDon = coNutXemHoaDon;

            NapThongTin(thongTin);

            if (!string.IsNullOrEmpty(tieuDeLienQuan) && dsLienQuan is { Count: > 0 })
            {
                lblLienQuan.Text = tieuDeLienQuan;
                lblLienQuan.Visible = true;
                dgvLienQuan.Visible = true;

                // Cột nút "Xem chi tiết" chỉ được thêm SAU KHI DataGridView đã sinh xong các
                // cột dữ liệu tự động (việc này xảy ra bất đồng bộ) - nếu thêm ngay sau khi
                // gán DataSource, cột nút có thể bị chèn sai vị trí hoặc chưa có hiệu lực.
                if (_coNutXemHoaDon)
                    dgvLienQuan.DataBindingComplete += (_, _) => ThemCotXemHoaDon();

                dgvLienQuan.DataSource = dsLienQuan;
            }
            else
            {
                lblLienQuan.Visible = false;
                dgvLienQuan.Visible = false;
            }

            SapXepLaiBoCuc();
        }

        /// <summary>Thêm cột nút "Xem chi tiết" vào cuối bảng dữ liệu liên quan (chỉ khi
        /// đang hiển thị danh sách hóa đơn - xem <see cref="_coNutXemHoaDon"/>).</summary>
        private void ThemCotXemHoaDon()
        {
            if (dgvLienQuan.Columns.Count == 0) return; // chưa có cột dữ liệu nào thì chưa thêm nút

            if (dgvLienQuan.Columns.Contains(ColXemHoaDon))
            {
                dgvLienQuan.Columns[ColXemHoaDon].DisplayIndex = dgvLienQuan.Columns.Count - 1;
                return;
            }

            var cot = new DataGridViewButtonColumn
            {
                Name = ColXemHoaDon,
                HeaderText = "",
                Text = "Xem chi tiết",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 110,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgvLienQuan.Columns.Add(cot);
            cot.DisplayIndex = dgvLienQuan.Columns.Count - 1;
        }

        /// <summary>Bấm nút "Xem chi tiết" trên một dòng hóa đơn -> mở hộp thoại chi tiết hóa đơn
        /// (đầy đủ các dòng phí điện, nước, internet...) của đúng hóa đơn đó.</summary>
        private void dgvLienQuan_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (!_coNutXemHoaDon) return;
            if (e.RowIndex < 0) return;
            if (dgvLienQuan.Columns[e.ColumnIndex].Name != ColXemHoaDon) return;
            if (!dgvLienQuan.Columns.Contains("MaHoaDon")) return;

            var maHoaDon = dgvLienQuan.Rows[e.RowIndex].Cells["MaHoaDon"].Value?.ToString();
            if (string.IsNullOrEmpty(maHoaDon)) return;

            using var frmChiTiet = new FrmChiTietHoaDon(maHoaDon);
            frmChiTiet.ShowDialog(this);
        }

        private void btnDong_Click(object? sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Đổ danh sách (nhãn, giá trị) vào bảng 2 cột: nhãn in đậm bên trái, giá trị bên phải.</summary>
        private void NapThongTin(List<(string Nhan, string GiaTri)> thongTin)
        {
            tblThongTin.Controls.Clear();
            tblThongTin.RowStyles.Clear();
            tblThongTin.RowCount = thongTin.Count;

            for (int i = 0; i < thongTin.Count; i++)
            {
                tblThongTin.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var lblNhan = new Label
                {
                    Text = thongTin[i].Nhan,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(95, 99, 104),
                    AutoSize = false,
                    Size = new Size(150, 26),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 4, 0, 4)
                };

                var lblGiaTri = new Label
                {
                    Text = string.IsNullOrWhiteSpace(thongTin[i].GiaTri) ? "(không có)" : thongTin[i].GiaTri,
                    Font = new Font("Segoe UI", 9.5F),
                    ForeColor = Color.FromArgb(32, 33, 36),
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 4, 0, 4)
                };

                tblThongTin.Controls.Add(lblNhan, 0, i);
                tblThongTin.Controls.Add(lblGiaTri, 1, i);
            }
        }

        /// <summary>
        /// tblThongTin có chiều cao tự co giãn theo số dòng; sau khi đổ dữ liệu cần dồn lại
        /// vị trí của khu vực "dữ liệu liên quan" và nút Đóng để không bị chồng lên nhau.
        /// </summary>
        private void SapXepLaiBoCuc()
        {
            int yTiepTheo = tblThongTin.Bottom + 16;

            if (lblLienQuan.Visible)
            {
                lblLienQuan.Location = new Point(lblLienQuan.Left, yTiepTheo);
                dgvLienQuan.Location = new Point(dgvLienQuan.Left, lblLienQuan.Bottom + 6);

                int chieuCaoConLai = ClientSize.Height - dgvLienQuan.Top - 60;
                dgvLienQuan.Height = System.Math.Max(90, chieuCaoConLai);
            }
            else
            {
                // Không có dữ liệu liên quan: thu gọn Form lại cho vừa vặn.
                ClientSize = new Size(ClientSize.Width, yTiepTheo + 60);
            }
        }
    }
}
