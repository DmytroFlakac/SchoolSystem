namespace DAL.Entities;

public record EvaluationEntity : IEntity
{
    public Guid Id { get; set; }

    public int Score { get; set; }
    public string? Description { get; set; }
    public StudentEntity Student { get; set; }
    public ActivityEntity Activity { get; set; }
}