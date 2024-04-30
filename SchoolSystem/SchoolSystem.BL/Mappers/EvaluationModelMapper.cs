using  SchoolSystem.BL.Models;
using DAL.Entities;

namespace SchoolSystem.BL.Mappers;

public class EvaluationModelMapper : ModelMapperBase<EvaluationEntity, EvaluationListModel, EvaluationDetailModel>
{
    public override EvaluationListModel MapToListModel(EvaluationEntity? entity) => entity is null
        ? EvaluationListModel.Empty
        : new EvaluationListModel()
        {
            Id = entity.Id, Score = entity.Score, Description = entity.Description,
            ActivityId = entity.ActivityId, StudentId = entity.StudentId,
            Activity = new ActivityListModel() { Id = entity!.Activity.Id, Name = entity.Activity.Name, Start = entity.Activity.Start, End = entity.Activity.End, Description = entity.Activity.Description, Tag = entity.Activity.Tag, Room = entity.Activity.Room, SubjectId = entity.Activity.SubjectId },
        };
    
    public override EvaluationDetailModel MapToDetailModel(EvaluationEntity? entity) =>
        entity is null
            ? EvaluationDetailModel.Empty
            : new EvaluationDetailModel()
            {
                Id = entity.Id, Score = entity.Score, Description = entity.Description,
                ActivityId = entity.ActivityId, StudentId = entity.StudentId,
                // Activity = new ActivityListModel() { Id = entity!.Activity.Id, Name = entity.Activity.Name, Start = entity.Activity.Start, End = entity.Activity.End, Description = entity.Activity.Description, Tag = entity.Activity.Tag, Room = entity.Activity.Room, SubjectId = entity.Activity.SubjectId },
                // Student = new StudentListModel() { Id = entity!.Student.Id, Name = entity.Student.Name, Surname = entity.Student.Surname}
            };

    public override EvaluationEntity MapToEntity(EvaluationDetailModel model) =>
        new() { Id = model.Id, ActivityId = model.ActivityId, StudentId = model.StudentId, Student = null!, Score = model.Score, Description = model.Description, Activity = new ActivityEntity() { Id = model.Activity.Id, Name = model.Activity.Name, Start = model.Activity.Start, End = model.Activity.End, Description = model.Activity.Description, Tag = model.Activity.Tag, Room = model.Activity.Room, SubjectId = model.Activity.SubjectId } }; 
}