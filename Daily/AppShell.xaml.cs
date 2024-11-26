using Daily.Pages;

namespace Daily
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(TaskPage), typeof(TaskPage));
            Routing.RegisterRoute(nameof(TaskEditPage), typeof(TaskEditPage));
            Routing.RegisterRoute(nameof(ThoughtPage), typeof(ThoughtPage));
        }
    }
}