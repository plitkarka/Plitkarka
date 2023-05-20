namespace Plitkarka.Domain.Models.Abstractions;

public abstract record LogicModel
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }
}
