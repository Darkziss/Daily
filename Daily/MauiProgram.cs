using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Plugin.SegmentedControl.Maui;
using Daily.Tasks;
using Daily.Thoughts;
using Daily.Diary;
using Daily.Data;
using Daily.ViewModels;
using Daily.Pages;

namespace Daily
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseSegmentedControl()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Nunito-Bold.ttf", "Nunito");
                    fonts.AddFont("Nunito-BoldItalic.ttf", "NunitoItalic");
                    fonts.AddFont("Nunito-ExtraBold.ttf", "NunitoBold");
                });

            RegisterModels(builder);
            RegisterViewModels(builder);
            RegisterViews(builder);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterModels(MauiAppBuilder builder)
        {
            builder
                .Services
                .AddSingleton<DataProvider>()
                .AddSingleton<GoalStorage>()
                .AddSingleton<GeneralTaskStorage>()
                .AddSingleton<ConditionalTaskStorage>()
                .AddSingleton<ThoughtStorage>()
                .AddSingleton<DiaryRecordStorage>();
        }

        private static void RegisterViewModels(MauiAppBuilder builder)
        {
            builder
                .Services
                .AddSingleton<MainPageViewModel>()
                .AddSingleton<TaskPageViewModel>()
                .AddSingleton<TaskEditPageViewModel>()
                .AddSingleton<ThoughtPageViewModel>()
                .AddSingleton<ThoughtEditPageViewModel>()
                .AddSingleton<DiaryRecordPageViewModel>()
                .AddSingleton<DiaryRecordEditPageViewModel>();
        }

        private static void RegisterViews(MauiAppBuilder builder)
        {
            builder
                .Services
                .AddSingleton<MainPage>()
                .AddSingleton<TaskPage>()
                .AddSingleton<TaskEditPage>()
                .AddSingleton<ThoughtPage>()
                .AddSingleton<ThoughtEditPage>()
                .AddSingleton<DiaryRecordPage>()
                .AddSingleton<DiaryRecordEditPage>();
        }
    }
}