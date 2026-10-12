using System.Windows.Forms;
using DoAnNhom06.Forms;

namespace DoAnNhom06.Helpers
{
    /// <summary>
    /// Các hộp thoại thông báo dùng chung cho toàn bộ ứng dụng.
    ///
    /// Trước khi có lớp này, mỗi Form tự viết lại
    /// "MessageBox.Show(noiDung, tieuDe, MessageBoxButtons.OK, MessageBoxIcon...)"
    /// (lặp lại 80+ lần trong cả dự án, mỗi nơi một kiểu tiêu đề/icon hơi khác
    /// nhau). Gom về đây để mọi màn hình thông báo/cảnh báo/lỗi/hỏi theo đúng
    /// MỘT kiểu thống nhất, và khi cần đổi (VD: đổi icon, thêm log...) chỉ sửa
    /// một chỗ duy nhất.
    /// </summary>
    public static class ThongBao
    {
        public static void Info(string noiDung, string tieuDe = "Thông báo")
            => MessageBox.Show(noiDung, tieuDe, MessageBoxButtons.OK, MessageBoxIcon.Information);

        /// <summary>
        /// Giống <see cref="Info"/> nhưng có thêm 1 nút phụ (mặc định "Xem") cạnh nút OK.
        /// Trả về true nếu người dùng bấm nút phụ, false nếu bấm OK (hoặc đóng hộp thoại).
        /// </summary>
        public static bool InfoCoNutXem(IWin32Window? chuSoHuu, string noiDung,
            string tieuDe = "Thông báo", string textNutXem = "Xem")
            => FrmThongBaoCoNutXem.Hien(chuSoHuu, noiDung, tieuDe, textNutXem);

        public static void CanhBao(string noiDung, string tieuDe = "Thông báo")
            => MessageBox.Show(noiDung, tieuDe, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static void Loi(string noiDung, string tieuDe = "Lỗi")
            => MessageBox.Show(noiDung, tieuDe, MessageBoxButtons.OK, MessageBoxIcon.Error);

        /// <summary>Hộp thoại Có/Không, trả về true nếu người dùng chọn "Yes".</summary>
        public static bool Hoi(string noiDung, string tieuDe = "Xác nhận")
            => MessageBox.Show(noiDung, tieuDe, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               == DialogResult.Yes;
    }
}
