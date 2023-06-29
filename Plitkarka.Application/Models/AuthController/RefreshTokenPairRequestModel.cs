using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Application.Models.AuthController;

public record RefreshTokenPairRequestModel
{
    public string RefreshToken { get; set; }   

    [MaxLength(128, ErrorMessage = "Unique identifier be less or equal 128 symbols")]
    [Required(ErrorMessage = "Unique identifier is required")]
    public string UniqueIdentifier { get; set; }
}
