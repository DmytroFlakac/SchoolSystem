using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using DAL.Enums;
using SchoolSystem.App.Messages;
using SchoolSystem.App.Services;
using SchoolSystem.App.Services.Interfaces;
using SchoolSystem.App.ViewModels;
using SchoolSystem.BL.Facades.Interfaces;
using SchoolSystem.BL.Models;

namespace SchoolSystem.App.ViewModels.Activity;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class ActivityEditViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;
    private readonly ISubjectFacade _subjectFacade;

    public Guid SubjectId { get; set; }
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    
    public List<Room> Rooms => Enum.GetValues(typeof(Room)).Cast<Room>().ToList();
    public DateTime EndDate { get; set; }
    public TimeSpan EndTime { get; set; }
    
    public int Tag { get; set; }
    
    public Room Room { get; set; }

    public DateTime StartDate { get; set; }
    public TimeSpan StartTime { get; set; }
    
    public SubjectListModel? Subject { get; set; }

    public ActivityEditViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService,
        ISubjectFacade subjectFacade)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _alertService = alertService;
        // var viewModel = (AppShellViewModel)Shell.Current.BindingContext;
        // SubjectId = viewModel.SubjectId;
        _subjectFacade = subjectFacade;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        
        // EndDate = Activity.End.Date;
        // EndTime = Activity?.End.TimeOfDay ?? DateTime.Now.TimeOfDay;
        // StartDate = Activity.Start.Date;
        // StartTime = Activity?.Start.TimeOfDay ?? DateTime.Now.TimeOfDay;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Activity.Start = StartDate.Date.Add(StartTime);
        Activity.End = EndDate.Date.Add(EndTime);
        Subject = await _subjectFacade.GetSubjectByAbbrAsync(Activity!.SubjectAbr);
        SubjectId = Subject.Id;
        Activity.SubjectId = SubjectId;
        Activity.Subject = Subject;
        if (Activity.Start >= Activity.End)
        {
            await _alertService.DisplayAsync("Invalid Time", "Activity cannot be add because start time is same or greater then end time.");
            return;
        }
        
        var subject_Activities = await _activityFacade.GetAsyncListBySubject(SubjectId);
        
        if (CheckActivitiesTime(subject_Activities, Activity))
        {
            await _alertService.DisplayAsync("Activities Overlap", "Activity cannot be add because different activity is planned for this time.");
            return;
        }
        
        await _activityFacade.SaveAsync(Activity);
        // await _subjectFacade.AddActivityToSubject(SubjectId, Activity.Id);
        MessengerService.Send(new EditMessage { Id = Activity.Id });
        _navigationService.SendBackButtonPressed();
    }

    public static bool CheckActivitiesTime(IEnumerable<ActivityListModel> studentActivities, ActivityDetailModel newActivity)
    {
        foreach (var studentActivity in studentActivities)
        {
            if (studentActivity.Id == newActivity.Id) continue;

            if (newActivity.Start < studentActivity.End && studentActivity.Start < newActivity.End)
            {
                return true;
            }
        }
        return false;
    }
}