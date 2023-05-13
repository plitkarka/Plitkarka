using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.AuthController;

public record SignInBodyRequestModel
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email length should be less or equal 100 symbols")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MaxLength(30, ErrorMessage = "Password length should be less or equal 30 symbols")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
