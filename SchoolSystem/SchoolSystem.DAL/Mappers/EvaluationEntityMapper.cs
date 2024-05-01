
using DAL.Entities;

namespace DAL.Mappers;

public class EvaluationEntityMapper : IEntityMapper<EvaluationEntity>
{
    public void MapToExistingEntity(EvaluationEntity existingEntity, EvaluationEntity newEntity)
    {
        existingEntity.Score = newEntity.Score;
        existingEntity.Description = newEntity.Description;
        existingEntity.ActivityId = newEntity.ActivityId;
        existingEntity.Activity = newEntity.Activity;
        existingEntity.StudentId = newEntity.StudentId;
        existingEntity.Student = newEntity.Student;
    }
}
