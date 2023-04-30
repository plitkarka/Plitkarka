using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plitkarka.Infrastructure.ModelAbstractions;

namespace Plitkarka.Infrastructure.Models;

public record ImageEntity : ActivatedEntity
{
    [Required]
    public string ImageId { get; set; }
}
