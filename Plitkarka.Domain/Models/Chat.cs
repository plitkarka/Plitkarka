using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record Chat : ActivatedLogicModel
{
    public DateTime LastUpdateTime { get; set; }
}
