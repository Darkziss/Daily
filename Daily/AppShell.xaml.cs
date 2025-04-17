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
            Routing.RegisterRoute(nameof(TaskEditPage), typeof(TaskEditPage));

            Routing.RegisterRoute(nameof(ThoughtPage), typeof(ThoughtPage));
            Routing.RegisterRoute(nameof(ThoughtEditPage), typeof(ThoughtEditPage));

            Routing.RegisterRoute(nameof(DiaryRecordPage), typeof(DiaryRecordPage));
            Routing.RegisterRoute(nameof(DiaryRecordEditPage), typeof(DiaryRecordEditPage));
        }
    }
}