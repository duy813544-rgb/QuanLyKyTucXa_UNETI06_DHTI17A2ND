using System.Drawing;
using System.Windows.Forms;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Form nền dùng chung cho các màn hình nghiệp vụ (Thanh toán phí, Xác nhận
    /// thanh toán, Tra cứu thông tin...).
    ///
    /// Trước đây mỗi Form tự khai báo lại y hệt: màu nền WhiteSmoke, font tiêu
    /// đề "Segoe UI 14pt Bold" màu RoyalBlue, vị trí CenterParent, kích thước
    /// tối thiểu... và tự phối màu nút bấm theo cảm tính (mỗi Form một kiểu).
    /// Kết quả là 3 màn hình cùng một nhóm chức năng lại "trông" như 3 ý tưởng
    /// khác nhau, và khi cần đổi màu/giao diện chung phải sửa ở từng nơi.
    ///
    /// Lớp này gom phần giao diện dùng chung đó lại một chỗ, để các Form kế
    /// thừa chỉ còn khai báo phần khác biệt thật sự (bố cục, control riêng).
    /// </summary>
    public class BaseAppForm : Form
    {
        public BaseAppForm()
        {
            BackColor = Color.WhiteSmoke;
            AutoScaleMode = AutoScaleMode.Font;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(750, 450);
        }

        /// <summary>Nhãn tiêu đề in hoa, in đậm, căn giữa, đặt ở đầu Form.</summary>
        protected Label TaoNhanTieuDe(string noiDung, int rongForm)
        {
            return new Label
            {
                Text = noiDung,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.RoyalBlue,
                AutoSize = false,
                Size = new Size(rongForm, 40),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
        }

        /// <summary>3 kiểu màu nút chuẩn, dùng thống nhất trên toàn ứng dụng.</summary>
        protected enum KieuNut
        {
            ChinhDien,  // RoyalBlue  - tìm kiếm, làm mới, hành động chính trung tính
            ThanhCong,  // SeaGreen   - xác nhận, thanh toán, đồng ý
            NguyHiem,   // IndianRed  - từ chối, xóa
            Phu         // Gainsboro  - đóng, hủy
        }

        /// <summary>Nút bấm theo 1 trong 4 kiểu màu chuẩn của ứng dụng.</summary>
        protected Button TaoNut(string text, KieuNut kieu)
        {
            var (nen, chu) = kieu switch
            {
                KieuNut.ChinhDien => (Color.RoyalBlue, Color.White),
                KieuNut.ThanhCong => (Color.SeaGreen, Color.White),
                KieuNut.NguyHiem => (Color.IndianRed, Color.White),
                KieuNut.Phu => (Color.Gainsboro, Color.Black),
                _ => (Color.RoyalBlue, Color.White)
            };

            return new Button
            {
                Text = text,
                BackColor = nen,
                ForeColor = chu,
                FlatStyle = FlatStyle.Flat
            };
        }
    }
}
