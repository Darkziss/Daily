namespace Daily;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void RouteToTaskPage(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TaskPage));
    }
}