using Daily.ViewModels;

namespace Daily;

public partial class TaskPage : ContentPage
{
	public TaskPage()
	{
		InitializeComponent();

		BindingContext = new TaskPageViewModel();
	}
}