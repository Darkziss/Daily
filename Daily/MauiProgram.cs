using Microsoft.Extensions.Logging;
using Daily.Tasks;
using Daily.Thoughts;
using Daily.Diary;
using Daily.Data;
using Daily.ViewModels;
using Daily.Pages;
using CommunityToolkit.Maui;
using Plugin.SegmentedControl.Maui;
using Sharpnado.TaskLoaderView;

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
                    fonts.AddFont("Nunito-ExtraBold.ttf", "NunitoBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            Initializer.Initialize(true);
#else
            Initializer.Initialize(false);
#endif

            IServiceCollection collection = builder.Services;

            collection.AddSingleton<UnitOfWork>();

            RegisterStorages(collection);
            RegisterViewModels(collection);
            RegisterViews(collection);

            return builder.Build();
        }

        private static void RegisterStorages(IServiceCollection collection)
        {
            collection
                .AddSingleton<GoalStorage>()
                .AddSingleton<GeneralTaskStorage>()
                .AddSingleton<ConditionalTaskStorage>()
                .AddSingleton<ThoughtStorage>()
                .AddSingleton<DiaryRecordStorage>();
        }

        private static void RegisterViewModels(IServiceCollection collection)
        {
            collection
                .AddSingleton<MainPageViewModel>()
                .AddSingleton<TaskPageViewModel>()
                .AddSingleton<TaskEditPageViewModel>()
                .AddSingleton<GoalEditPageViewModel>()
                .AddSingleton<ThoughtPageViewModel>()
                .AddSingleton<ThoughtEditPageViewModel>()
                .AddSingleton<DiaryRecordPageViewModel>()
                .AddSingleton<DiaryRecordEditPageViewModel>();
        }

        private static void RegisterViews(IServiceCollection collection)
        {
            collection
                .AddSingleton<MainPage>()
                .AddSingleton<TaskPage>()
                .AddSingleton<TaskEditPage>()
                .AddSingleton<GoalEditPage>()
                .AddSingleton<ThoughtPage>()
                .AddSingleton<ThoughtEditPage>()
                .AddSingleton<DiaryRecordPage>()
                .AddSingleton<DiaryRecordEditPage>();
        }
    }
}