using Daily.Tasks;
using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class TaskEditPage : ContentPage, IQueryAttributable
    {
        private readonly TaskEditPageViewModel _viewModel;

        public TaskEditPage(TaskEditPageViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = viewModel;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            bool isOneTimeTask = query.ContainsKey(nameof(OneTimeTask));
            bool isRecurringTask = query.ContainsKey(nameof(RecurringTask));

            if (isOneTimeTask)
            {
                OneTimeTask task = (OneTimeTask)query[nameof(OneTimeTask)];

                _viewModel.PrepareViewForEdit(task);
            }
            else if (isRecurringTask)
            {
                RecurringTask task = (RecurringTask)query[nameof(RecurringTask)];
                _viewModel.PrepareViewForEdit(task);
            }
            else _viewModel.ResetView();
        }
    }
}