using Daily.Navigation;
using Daily.Pages;

namespace Daily
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TaskPage), typeof(TaskPage));
            Routing.RegisterRoute(nameof(GoalEditPage), typeof(GoalEditPage));
            Routing.RegisterRoute(nameof(TaskEditPage), typeof(TaskEditPage));
        }
    }
}