using System.ComponentModel.DataAnnotations;

namespace AspControler.Models;

public class CopyUser
{
    [Required]
    [MinLength(3)]
    [MaxLength(8)]
    [RegularExpression("Min length=3, max length=8")]
    public string? UserName { get; set; }
    [Required]
    //[RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage ="Minimum eight characters, at least one letter and one number")]
    public string ? Password { get; set; }
    [Required]
    //  [MinLength(3)]
    //  [MaxLength(20)]
    //  [RegularExpression("Min length=3 , max length = 20")]
    public string? Email { get; set; }
    [Required]
    [MaxLength(10)]
     [RegularExpression("max length = 10")]
    public string? Phone { get; set; }
    [Required(ErrorMessage ="Profil rasmini kiriting")]
    public IFormFile? UserPhotos { get; set; }
}