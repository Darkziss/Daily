using Daily.Tasks;
using Daily.Navigation;
using Daily.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace Daily.ViewModels
{
    public partial class GoalBottomSheetViewModel : ObservableObject
    {
        [ObservableProperty] private string _goal;
        [ObservableProperty] private DateTime _deadline;

        [ObservableProperty] private bool _canSave = false;

        private readonly GoalStorage _goalStorage;

        public Command SaveGoalCommand { get; }

        public GoalBottomSheetViewModel(GoalStorage goalStorage)
        {
            _goalStorage = goalStorage;

            _goal = goalStorage.Goal;
            _deadline = DateTime.Now;

            SaveGoalCommand = new Command(async () =>
            {
                CanSave = false;

                //if (!_goalStorage.IsSameGoal(Goal))
                //{
                    
                //    Debug.WriteLine("SetGoalAsync");
                //}

                await _goalStorage.SetGoalAsync(Goal);

                WeakReferenceMessenger.Default.Send<GoalSaveMessage>();

                await SheetNavigator.HideCurrentSheetAsync();

                CanSave = true;
            });
        }

        public void PrepareView()
        {
            CanSave = true;
            
            Goal = _goalStorage.Goal;
        }
    }
}