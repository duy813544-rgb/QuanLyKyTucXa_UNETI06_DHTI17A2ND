using System.Collections.Generic;
using System.Linq;

namespace DoAnNhom06.Helpers
{
    /// <summary>
    /// Sinh mã tự động dạng "TienToXXX" (VD: TT001, HD002...) dựa trên số lớn
    /// nhất hiện có trong danh sách mã truyền vào. Dùng chung cho các màn hình
    /// cần tự sinh mã, đơn giản và đủ dùng cho phạm vi đồ án.
    /// </summary>
    public static class MaSoHelper
    {
        public static string SinhMaMoi(IEnumerable<string> maHienCo, string tienTo, int soChuSo = 3)
        {
            int soLonNhat = maHienCo
                .Select(ma => int.TryParse(ma.Replace(tienTo, ""), out int so) ? so : 0)
                .DefaultIfEmpty(0)
                .Max();

            return tienTo + (soLonNhat + 1).ToString("D" + soChuSo);
        }
    }
}
