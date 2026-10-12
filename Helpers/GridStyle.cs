using System.Drawing;
using System.Windows.Forms;

namespace DoAnNhom06.Helpers
{
    /// <summary>
    /// Áp dụng cấu hình hiển thị chuẩn cho các DataGridView chỉ đọc (danh sách,
    /// kết quả tra cứu, danh sách chờ duyệt...).
    ///
    /// 7 dòng cấu hình này trước đây được copy-paste giống hệt nhau trong hơn
    /// 8 Form khác nhau; nay gom về một chỗ để mọi bảng dữ liệu trong ứng dụng
    /// có cùng hành vi (không cho thêm/xóa dòng, chọn cả dòng, tự co giãn cột...).
    /// </summary>
    public static class GridStyle
    {
        public static void ApDungChiDoc(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
        }

        /// <summary>
        /// Giống <see cref="ApDungChiDoc"/> nhưng thêm giao diện "phẳng" hiện đại:
        /// tiêu đề cột tô màu nhấn, không kẻ lưới dọc, các hàng so le màu nhạt.
        /// Dùng cho màn hình Tra cứu thông tin - nơi kết quả cần dễ quét mắt hơn
        /// là một bảng dữ liệu nghiệp vụ thông thường.
        /// </summary>
        public static void ApDungHienDai(DataGridView dgv, Color mauNhan)
        {
            ApDungChiDoc(dgv);

            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(235, 237, 240);
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 38;
            dgv.RowTemplate.Height = 32;
            dgv.Font = new Font("Segoe UI", 9.5F);

            dgv.ColumnHeadersDefaultCellStyle.BackColor = mauNhan;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(mauNhan, 0.85f);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 248, 250);
        }
    }
}
