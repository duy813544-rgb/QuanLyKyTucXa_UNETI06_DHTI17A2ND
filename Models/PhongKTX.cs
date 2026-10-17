//Trần văn cường
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DoAnNhom06.Models;


public partial class PhongKTX
{
    [Key]
    [Display(Name = "Mã phòng")]
    public int MaPhong { get; set; }

    [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
    [StringLength(50)]
    [Display(Name = "Tên phòng")]
    public string TenPhong { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng chọn loại phòng.")]
    [Display(Name = "Loại phòng")]
    public int MaLoaiPhong { get; set; }

    [Required(ErrorMessage = "Tòa nhà là bắt buộc.")]
    [StringLength(50)]
    [Display(Name = "Tòa nhà")]
    public string ToaNha { get; set; } = null!;

    [Range(1, 50, ErrorMessage = "Tầng phải là số nguyên từ 1 đến 50.")]
    public int Tang { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn giới tính của phòng.")]
    [StringLength(10)]
    [Display(Name = "Giới tính phòng")]
    public string GioiTinhPhong { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string TrangThai { get; set; } = "DangSuDung";

    [StringLength(250)]
    [DataType(DataType.MultilineText)]
    public string? MoTa { get; set; }


    [ValidateNever]
    public virtual LoaiPhong LoaiPhong { get; set; } = null!;

    public virtual ICollection<PhanPhong> PhanPhongs { get; set; } = new List<PhanPhong>();

}
