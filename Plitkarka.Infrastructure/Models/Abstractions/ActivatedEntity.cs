using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Plitkarka.Infrastructure.Models.Abstractions;

public abstract record ActivatedEntity : Entity
{
    public bool IsActive { get; set; }
}
