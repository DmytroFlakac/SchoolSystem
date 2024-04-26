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
    private async void GoToStudentListView()
    {
        await _navigationService.GoToAsync<StudentListViewModel>();
    }

    // Example method to navigate to the student detail view with a parameter
    private async void GoToStudentDetailView(string studentId)
    {
        var parameters = new Dictionary<string, object?> { { "Id", studentId } };
        await _navigationService.GoToAsync<StudentDetailViewModel>(parameters);
    }

    // Example method to navigate to the student edit view
    private async void GoToStudentEditView()
    {
        await _navigationService.GoToAsync<StudentEditViewModel>();
    }
}