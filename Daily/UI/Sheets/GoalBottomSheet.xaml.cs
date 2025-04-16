using Daily.ViewModels;
using The49.Maui.BottomSheet;

namespace Daily.Sheets
{
    public partial class GoalBottomSheet : BottomSheet
    {
        private readonly GoalBottomSheetViewModel _viewModel;
        
        public GoalBottomSheet(GoalBottomSheetViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = viewModel;

            Showing += (_, _) =>
            {
                _viewModel.PrepareView();
            };
        }
    }
}