using Daily.Thoughts;
using Daily.ViewModels;

namespace Daily.Pages
{
    public partial class ThoughtEditPage : ContentPage, IQueryAttributable
    {
        private readonly ThoughtEditPageViewModel _viewModel;

        public ThoughtEditPage(ThoughtEditPageViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = viewModel;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            bool haveThought = query.ContainsKey(nameof(Thought));

            if (haveThought)
            {
                Thought thought = (Thought)query[nameof(Thought)];

                _viewModel.PrepareViewForView(thought);
            }
            else _viewModel.ResetView();
        }
    }
}