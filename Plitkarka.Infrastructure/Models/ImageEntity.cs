using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record ImageEntity : ActivatedEntity
{
    [Required]
    public string ImageId { get; set; }
}
