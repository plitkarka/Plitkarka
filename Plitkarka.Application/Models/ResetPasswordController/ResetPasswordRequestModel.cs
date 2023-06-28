using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.ResetPasswordController;

public class ResetPasswordRequestModel
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email length should be less or equal 100 symbols")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [StringLength(6, MinimumLength = 6, ErrorMessage = "Reset password code should be exactly 6 digits")]
    [Required(ErrorMessage = "Email is required")]
    public string PasswordCode { get; set; }

    [DataType(DataType.Password)]
    [MaxLength(30, ErrorMessage = "Password length should be less or equal 30 symbols")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [MaxLength(32, ErrorMessage = "Unique identifier be less or equal 32 symbols")]
    [Required(ErrorMessage = "Unique identifier is required")]
    public string UniqueIdentifier { get; set; }
}
