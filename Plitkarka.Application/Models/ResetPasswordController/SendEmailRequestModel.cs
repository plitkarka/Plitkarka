using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.ResetPasswordController;

public record SendEmailRequestModel
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email length should be less or equal 100 symbols")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
}
