using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plitkarka.Infrastructure.Models;

public record UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Login { get; set; }

    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(20)]
    public string SecondName { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    [MaxLength(6)]
    public string EmailCode { get; set; }

    [Required]
    [Column(TypeName="char(64)")]
    public string Password { get; set; }

    public int PasswordAttempts { get; set; }

    [Required]
    [Column(TypeName = "char(64)")]
    public string Salt { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    public bool? IsActive { get; set; }


    // ----- Relation properties -----

    public Guid? RefreshTokenId { get; set; }

    public RefreshTokenEntity? RefreshToken { get; set; }
}
