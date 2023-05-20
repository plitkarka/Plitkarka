namespace Plitkarka.Domain.Models.Abstractions;

public abstract record ActivatedLogicModel : LogicModel
{
    public bool IsActive { get; set; }
}
