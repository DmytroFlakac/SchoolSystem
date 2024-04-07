namespace DAL.Entities;

public record SubjectEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public ICollection<ActivityEntity>? Activities { get; set; } = new List<ActivityEntity>();

    public ICollection<StudentSubjectEntity>? StudentSubjects { get; set; } = new List<StudentSubjectEntity>();
}