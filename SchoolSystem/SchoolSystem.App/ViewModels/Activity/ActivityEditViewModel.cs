﻿using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using SchoolSystem.App.Messages;
using SchoolSystem.App.Services;
using SchoolSystem.App.Services.Interfaces;
using SchoolSystem.App.ViewModels;
using SchoolSystem.BL.Facades.Interfaces;
using SchoolSystem.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class ActivityEditViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;
    private readonly IStudentFacade _studentFacade;

    public Guid UserId { get; set; }
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public DateTime EndDate { get; set; }
    public TimeSpan EndTime { get; set; }

    public DateTime StartDate { get; set; }
    public TimeSpan StartTime { get; set; }

    public UserDetailModel User { get; set; }
    public string[] Projects { get; set; }
    public string SelectedProject { get; set; }

    public ActivityEditViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService,
        IStudentFacade userFacade)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _alertService = alertService;
        var viewModel = (AppShellViewModel)Shell.Current.BindingContext;
        UserId = viewModel.UserId;
        _studentFacade = userFacade;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        User = await _studentFacade.GetAsync(UserId);

        Projects = User.UserProjects.Select(up => up.ProjectName).ToArray();
        SelectedProject = User.UserProjects.Where(up => up.ProjectId == Activity.SubjectId).Select(up => up.ProjectName).FirstOrDefault();

        EndDate = Activity?.End.Date ?? DateTime.Now;
        EndTime = Activity?.End.TimeOfDay ?? DateTime.Now.TimeOfDay;
        StartDate = Activity?.Start.Date ?? DateTime.Now;
        StartTime = Activity?.Start.TimeOfDay ?? DateTime.Now.TimeOfDay;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Activity.Start = StartDate.Date.Add(StartTime);
        Activity.End = EndDate.Date.Add(EndTime);
        if (Activity.Start >= Activity.End)
        {
            await _alertService.DisplayAsync("Invalid Time", "Activity cannot be add because start time is same or greater then end time.");
            return;
        }

        var user_Activities = await _activityFacade.GetAsyncListStudent(UserId);

        if (CheckActivitiesTime(user_Activities, Activity))
        {
            await _alertService.DisplayAsync("Activities Overlap", "Activity cannot be add because different activity is planned for this time.");
            return;
        }

        if (SelectedProject is null)
        {
            await _alertService.DisplayAsync("Project is not selected", "You must select a project when adding activity.");
            return;
        }

        Activity.SubjectId = User.UserProjects.Where(up => up.ProjectName == SelectedProject).Select(p => p.ProjectId).FirstOrDefault();
        Activity.StudentId = User.Id;

        await _activityFacade.SaveAsync(Activity);
        MessengerService.Send(new EditMessage { Id = Activity.Id });
        _navigationService.SendBackButtonPressed();
    }

    public static bool CheckActivitiesTime(IEnumerable<ActivityListModel> userActivities, ActivityDetailModel newActivity)
    {
        foreach (var userActivity in userActivities)
        {
            if (userActivity.Id == newActivity.Id) continue;

            if (newActivity.Start < userActivity.End && userActivity.Start < newActivity.End)
            {
                return true;
            }
        }
        return false;
    }


}