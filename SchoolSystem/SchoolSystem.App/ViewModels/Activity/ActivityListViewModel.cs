using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolSystem.App.Messages;
using SchoolSystem.App.Services.Interfaces;
using SchoolSystem.BL.Facades.Interfaces;
using SchoolSystem.BL.Models;

namespace SchoolSystem.App.ViewModels.Activity;

public partial class ActivityListViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<EditMessage>, IRecipient<DeleteMessage<ActivityListModel>>
{
    public IEnumerable<ActivityListModel> Activities { get; set; } =    activityFacade.GetAsync().Result;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activities = await activityFacade.GetAsync();
        // Activities = await activityFacade.GetAsyncListByStudent(StudentId);
        // ManualFilter = false;
        // ParseInterval(SelectedFilter);
        //
        // if (FilterEnd == null)
        // {
        //     FilterEnd = GetMaxTime(Activities, FilterEnd);
        // }
        // if (FilterStart == null)
        // {
        //     FilterStart = GetMinTime(Activities, FilterStart);
        // }
        //
        // Activities = await activityFacade.GetAsyncFilter(StudentId, FilterStart, FilterEnd, null, null);
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    public async void Receive(EditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(DeleteMessage<ActivityListModel> message)
    {
        await LoadDataAsync();
    }
}