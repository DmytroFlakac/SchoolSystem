
using System.Collections.ObjectModel;
using DAL.Entities;
using DAL.Enums;

namespace SchoolSystem.BL.Models;

public record ActivityDetailModel() : baseModel
{
    public required string Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; } 
    public Room Room { get; set; }  
    public int Tag { get; set; } 
    public string? Description { get; set; } 
    public ObservableCollection<EvaluationListModel>? Evaluations { get; set; } = new();
    
    public Guid? SubjectId { get; set; }

    public SubjectListModel? Subject { get; set; }
    
    public string? SubjectAbr { get; set; }
    
    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Start = DateTime.MinValue,
        End = DateTime.MinValue,
        Room = Room.D105,
        Tag = 0,
        Description = string.Empty,
        SubjectId = default,
        Subject = SubjectListModel.Empty,
        SubjectAbr = string.Empty
    };

    
}