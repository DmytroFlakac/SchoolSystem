using CommunityToolkit.Mvvm.Input;
using SchoolSystem.App.Services.Interfaces;
using SchoolSystem.App.ViewModels.Students;
using SchoolSystem.App;

namespace SchoolSystem.App.Shell;

public partial class AppShell
{
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        InitializeComponent();
    }

    // Example method to navigate to the student list view
    [RelayCommand]
    private async Task GoToStudentListView()
        =>    await _navigationService.GoToAsync<StudentListViewModel>();
    


    // Example method to navigate to the student detail view with a parameter
    [RelayCommand]
    private async Task GoToStudentDetailView(string studentId)
    =>  await _navigationService.GoToAsync<StudentDetailViewModel>();
  

    [RelayCommand]
    // Example method to navigate to the student edit view
    private async Task GoToStudentEditView()
    => await _navigationService.GoToAsync<StudentEditViewModel>();
    

}