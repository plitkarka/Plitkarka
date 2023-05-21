using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Infrastructure.Models.Abstractions;

public abstract record ImageEntity : ActivatedEntity
{
    [Required]
    public string ImageKey { get; set; }
}
