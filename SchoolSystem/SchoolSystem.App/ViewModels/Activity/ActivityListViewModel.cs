﻿using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolSystem.App.Messages;
using SchoolSystem.App.Services.Interfaces;
using SchoolSystem.BL.Facades;
using SchoolSystem.BL.Facades.Interfaces;
using SchoolSystem.BL.Models;
using System.ComponentModel;
using static SchoolSystem.BL.Facades.Interfaces.IActivityFacade;

namespace SchoolSystem.App.ViewModels.Activity;

public partial class ActivityListViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<EditMessage>, IRecipient<DeleteMessage<ActivityListModel>>
{

    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;


    private int _tag = -1;
    public int Tag
    {
        get => _tag;
        set
        {
            if (_tag != value)
            {
                _tag = value;
                OnPropertyChanged(nameof(Tag));
                LoadDataAsync();  // Trigger data reload when the tag changes
            }
        }
    }

    public string[] Filters { get; set; } = Enum.GetNames(typeof(Interval));

    private string _selectedFilter = "NoFilter";
    // public string SelectedFilter
    // {
    //     get => _selectedFilter;
    //     set
    //     {
    //         if (_selectedFilter != value)
    //         {
    //             _selectedFilter = value;
    //             OnPropertyChanged(nameof(SelectedFilter));
    //             ParseInterval(_selectedFilter);
    //             LoadDataAsync();  // Ensure this is called to refresh the list
    //         }
    //     }
    // }
    
    public string SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            if (_selectedFilter != value)
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ParseInterval(_selectedFilter);  // Ensures that interval changes apply immediately
                if (ManualFilter)
                {
                    LoadDataAsync();
                }
            }
        }
    }


    //private string _selectedFilterTag;
    //public string SelectedFilterTag
    //{
    //    get => _selectedFilterTag;
    //    set
    //    {
    //        if (_selectedFilterTag != value)
    //        {
    //            _selectedFilterTag = value;
    //            OnPropertyChanged(nameof(SelectedFilterTag));
    //            ParseInterval(_selectedFilterTag);
    //            LoadDataAsync();  // Ensure this is called to refresh the list
    //        }
    //    }
    //}

    public Interval Interval { get; set; } = Interval.NoFilter;

    //public DateTime? FilterStart { get; set; } = null;
    private DateTime? _filterStart = DateTime.Now;

    public DateTime? FilterStart
    {
        get => _filterStart;
        set
        {
            if (_filterStart != value)
            {
                _filterStart = value;
                if (!ManualFilter)
                {
                    OnPropertyChanged(nameof(FilterStart));
                    ParseInterval(_selectedFilter);

                }
                LoadDataAsync(); // Ensure this is called to refresh the list
            }
        }
    }

    private DateTime? _filterEnd = DateTime.Now;
    //public DateTime? FilterEnd { get; set; } = null;

   public DateTime? FilterEnd
   {
       get => _filterEnd;
       set
       {
           if (_filterEnd != value)
           {
               _filterEnd = value;
               if (!ManualFilter)
               {
                   OnPropertyChanged(nameof(FilterEnd));
                   ParseInterval(_selectedFilter);
                }

                    
               LoadDataAsync(); // Ensure this is called to refresh the list
           }
       }
   }
    public TimeSpan TimeFilterStart { get; set; }


    public TimeSpan TimeFilterEnd { get; set; }

    public bool ManualFilter = true;


    protected override async Task LoadDataAsync()
    {

        await base.LoadDataAsync();
        Activities = await activityFacade.GetAsync();
        if (FilterStart.HasValue)
        {
            // Combine the date part of FilterStart with the time part of TimeFilterStart
            // FilterStart = FilterStart.Value.Date + TimeFilterStart;
            _filterStart = FilterStart.Value.Date + TimeFilterStart;
        }
        if (FilterEnd.HasValue)
        {
            // Combine the date part of FilterStart with the time part of TimeFilterStart
            // FilterEnd = FilterEnd.Value.Date + TimeFilterEnd;
            _filterEnd = FilterEnd.Value.Date + TimeFilterEnd;
        }

        ParseInterval(SelectedFilter);

        if (FilterEnd == null)
        {
            FilterEnd = GetMaxTime(Activities, FilterEnd);
        }
        if (FilterStart == null)
        {
            FilterStart = GetMinTime(Activities, FilterStart);
        }

        // Activities = await activityFacade.GetAsyncFilter(FilterStart, FilterEnd, Tag);
        Activities = await activityFacade.GetAsyncFilter(_filterStart, _filterEnd, Tag);
    }

    public static DateTime? GetMinTime(IEnumerable<ActivityListModel> userActivities, DateTime? Start)
    {
        foreach (var userActivity in userActivities)
        {
            if (userActivity.Start < Start)
            {
                Start = userActivity.Start;
            }
        }
        return Start;
    }
    public static DateTime? GetMaxTime(IEnumerable<ActivityListModel> userActivities, DateTime? End)
    {
        foreach (var userActivity in userActivities)
        {
            if (userActivity.End > End)
            {
                End = userActivity.End;
            }
        }
        return End;
    }
    // private void ParseInterval(string selectedFilter)
    // {
    //     if (selectedFilter == "NoFilter")
    //         ManualFilter = true;
    //     else
    //         ManualFilter = false;
    //
    //     if (ManualFilter)
    //         return;
    //
    //     if (selectedFilter == null)
    //         return;
    //
    //     Interval = (Interval)Enum.Parse(typeof(Interval), SelectedFilter);
    //
    //     DateTime now = DateTime.Now;
    //     switch (Interval)
    //     {
    //         case Interval.Last24Hours:
    //             //FilterStart = now.AddDays(-1);
    //             _filterStart = now.AddDays(-1);
    //             //FilterEnd = now;  // Assuming you want to include up to the current moment.
    //             _filterEnd = now;
    //             break;
    //         case Interval.Last7Days:
    //             //FilterStart = now.AddDays(-7);
    //             _filterStart = now.AddDays(-7);
    //             //FilterEnd = now;
    //             _filterEnd = now;
    //             break;
    //         case Interval.CurrentMonth:
    //             // FilterStart = new DateTime(now.Year, now.Month, 1);
    //             // FilterEnd = FilterStart.Value.AddMonths(1).AddDays(-1);
    //             _filterStart = new DateTime(now.Year, now.Month, 1);
    //             _filterEnd = _filterStart.Value.AddMonths(1).AddDays(-1);
    //             break;
    //         case Interval.PreviousMonth:
    //             // FilterStart = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
    //             // FilterEnd = new DateTime(now.Year, now.Month, 1).AddDays(-1);
    //             _filterStart = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
    //             _filterEnd = new DateTime(now.Year, now.Month, 1).AddDays(-1);
    //             break;
    //         case Interval.LastYear:
    //             // FilterStart = now.AddYears(-1);
    //             // FilterEnd = now;
    //             _filterStart = now.AddYears(-1);
    //             _filterEnd = now;
    //             break;
    //         case Interval.NoFilter:
    //             // Assuming no need to set FilterStart or FilterEnd as we want all data
    //             // FilterStart = null;
    //             // FilterEnd = null;
    //             _filterStart = null;
    //             _filterEnd = null;
    //             break;
    //         default:
    //             throw new Exception("Undefined interval");
    //     }
    //
    //     //FilterEnd = now;
    //     _filterEnd = now;
    // }

    private void ParseInterval(string selectedFilter)
    {
        if (selectedFilter == null || !ManualFilter) return;

        Interval = (Interval)Enum.Parse(typeof(Interval), selectedFilter);
        DateTime now = DateTime.Now;

        switch (Interval)
        {
            case Interval.Last24Hours:
                _filterStart = now.AddDays(-1);
                _filterEnd = now;
                break;
            case Interval.Last7Days:
                _filterStart = now.AddDays(-7);
                _filterEnd = now;
                break;
            case Interval.CurrentMonth:
                _filterStart = new DateTime(now.Year, now.Month, 1);
                _filterEnd = _filterStart.Value.AddMonths(1).AddDays(-1);
                break;
            case Interval.PreviousMonth:
                _filterStart = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                _filterEnd = new DateTime(now.Year, now.Month, 1).AddDays(-1);
                break;
            case Interval.LastYear:
                _filterStart = now.AddYears(-1);
                _filterEnd = now;
                break;
            case Interval.NoFilter:
                break;
            default:
                throw new Exception("Undefined interval");
        }

        // Only trigger updates if in manual filter mode
        OnPropertyChanged(nameof(FilterStart));
        OnPropertyChanged(nameof(FilterEnd));
        //LoadDataAsync();
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



    [RelayCommand]
    private async Task GoToRefreshAsync()
    {
        await LoadDataAsync();
    }
}