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
            Activity = entity.Activity is null ? ActivityListModel.Empty : new ActivityModelMapper().MapToListModel(entity.Activity)
        };
    
    public override EvaluationDetailModel MapToDetailModel(EvaluationEntity? entity) =>
        entity is null
            ? EvaluationDetailModel.Empty
            : new EvaluationDetailModel()
            {
                Id = entity.Id, Score = entity.Score, Description = entity.Description,
                ActivityId = entity.ActivityId, StudentId = entity.StudentId
                , Activity = entity.Activity is null ? ActivityListModel.Empty : new ActivityModelMapper().MapToListModel(entity.Activity),
                Student = entity.Student is null ? StudentListModel.Empty : new StudentModelMapper(new SubjectModelMapper()).MapToListModel(entity.Student)
            };

    public override EvaluationEntity MapToEntity(EvaluationDetailModel model) =>
        new() { Id = model.Id,  Activity = null!, ActivityId = model.ActivityId, StudentId = model.StudentId, Student = null!, Score = model.Score, Description = model.Description};
}