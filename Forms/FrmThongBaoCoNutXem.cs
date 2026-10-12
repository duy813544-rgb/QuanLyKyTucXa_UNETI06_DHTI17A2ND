using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Hộp thoại thông báo dạng "info" giống MessageBox mặc định của Windows (icon chữ i
    /// màu xanh + nội dung), nhưng có thêm 1 nút phụ (mặc định là "Xem") đứng cạnh nút
    /// "OK" - dùng cho các trường hợp thông báo xong người dùng có thể muốn xem ngay dữ
    /// liệu liên quan (ví dụ: "Bạn đã nộp xong tất cả các khoản phí" kèm nút "Xem" để mở
    /// luôn danh sách hóa đơn đã thanh toán), mà MessageBox chuẩn không hỗ trợ đổi tên nút.
    /// </summary>
    public class FrmThongBaoCoNutXem : Form
    {
        private readonly Button _btnXem;
        private readonly Button _btnOk;

        private FrmThongBaoCoNutXem(string noiDung, string tieuDe, string textNutXem)
        {
            Text = tieuDe;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9.5F);

            var icon = new PictureBox
            {
                Image = SystemIcons.Information.ToBitmap(),
                SizeMode = PictureBoxSizeMode.CenterImage,
                Location = new Point(20, 22),
                Size = new Size(32, 32)
            };

            var lblNoiDung = new Label
            {
                Text = noiDung,
                AutoSize = true,
                MaximumSize = new Size(320, 0),
                Location = new Point(66, 20),
                ForeColor = Color.FromArgb(32, 33, 36)
            };

            // Kích thước Form co giãn theo nội dung, giữ tối thiểu đủ chỗ cho 2 nút.
            int rongForm = Math.Max(360, lblNoiDung.Right + 30);
            int caoNoiDung = Math.Max(icon.Bottom, lblNoiDung.Bottom);

            _btnXem = new Button
            {
                Text = textNutXem,
                DialogResult = DialogResult.Yes,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                Size = new Size(90, 32)
            };
            _btnXem.FlatAppearance.BorderSize = 0;

            _btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.RoyalBlue,
                Size = new Size(90, 32)
            };
            _btnOk.FlatAppearance.BorderColor = Color.RoyalBlue;
            _btnOk.FlatAppearance.BorderSize = 1;

            int yNut = caoNoiDung + 28;
            _btnOk.Location = new Point(rongForm - 30 - _btnOk.Width, yNut);
            _btnXem.Location = new Point(_btnOk.Left - 10 - _btnXem.Width, yNut);

            ClientSize = new Size(rongForm, yNut + _btnOk.Height + 20);

            Controls.Add(icon);
            Controls.Add(lblNoiDung);
            Controls.Add(_btnXem);
            Controls.Add(_btnOk);

            AcceptButton = _btnOk;
            CancelButton = _btnOk;
        }

        /// <summary>
        /// Hiện hộp thoại thông báo kèm nút phụ.
        /// Trả về true nếu người dùng bấm nút phụ (ví dụ "Xem"), false nếu bấm OK
        /// (hoặc đóng Form bằng nút X / phím Esc).
        /// </summary>
        public static bool Hien(IWin32Window? chuSoHuu, string noiDung, string tieuDe = "Thông báo",
            string textNutXem = "Xem")
        {
            using var frm = new FrmThongBaoCoNutXem(noiDung, tieuDe, textNutXem);
            var ketQua = chuSoHuu != null ? frm.ShowDialog(chuSoHuu) : frm.ShowDialog();
            return ketQua == DialogResult.Yes;
        }
    }
}
