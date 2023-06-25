using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.UserController;

public record UpdateProfileRequestModel
{
    [MaxLength(15, ErrorMessage = "Login length should be less or equal 15 symbols")]
    public string? Login { get; set; } = "";

    [MaxLength(30, ErrorMessage = "User name length should be less or equal 30 symbols")]
    public string? Name { get; set; } = "";

    [MaxLength(255, ErrorMessage = "Description length should be less or equal 255 symbols")]
    public string? Description { get; set; } = "";

    [MaxLength(255, ErrorMessage = "Link length should be less or equal 255 symbols")]
    public string? Link { get; set; } = "";
}
