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
            bool isGeneralTask = query.ContainsKey(nameof(GeneralTask));
            bool isConditionalTask = query.ContainsKey(nameof(ConditionalTask));

            if (isGeneralTask)
            {
                GeneralTask task = (GeneralTask)query[nameof(GeneralTask)];

                _viewModel.PrepareViewForEdit(task);
            }
            else if (isConditionalTask)
            {
                ConditionalTask task = (ConditionalTask)query[nameof(ConditionalTask)];
                _viewModel.PrepareViewForEdit(task);
            }
            else _viewModel.ResetView();
        }
    }
}