using Daily.ViewModels;

namespace Daily.Pages;

public partial class TaskEditPage : ContentPage
{
	public TaskEditPage(TaskEditPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}