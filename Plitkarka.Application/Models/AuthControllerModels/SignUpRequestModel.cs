using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.AuthControllerModels;

public record class SignUpRequestModel
{
    [MaxLength(15, ErrorMessage = "Login length should be less or equal 15 symbols")]
    [Required(ErrorMessage = "Login is required")]
    public string Login { get; set; }

    [MaxLength(30, ErrorMessage = "User name length should be less or equal 30 symbols")]
    [Required(ErrorMessage = "User name is required")]
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email length should be less or equal 100 symbols")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MaxLength(30, ErrorMessage = "Password length should be less or equal 30 symbols")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Repeat password is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords are different")]
    public string RepeatPassword { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Birth Date is required")]
    public DateTime BirthDate { get; set; }
}
