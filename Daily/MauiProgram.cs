using Microsoft.Extensions.Logging;
using Daily.Tasks;
using Daily.Thoughts;
using Daily.Diary;
using Daily.Data;
using Daily.ViewModels;
using Daily.Pages;
using Daily.Sheets;
using Daily.Navigation;
using CommunityToolkit.Maui;
using Plugin.SegmentedControl.Maui;
using The49.Maui.BottomSheet;

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
                .UseBottomSheet()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Nunito-Bold.ttf", "Nunito");
                    fonts.AddFont("Nunito-ExtraBold.ttf", "NunitoBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            IServiceCollection collection = builder.Services;

            collection.AddSingleton<DataProvider>();

            RegisterStorages(collection);
            RegisterViewModels(collection);
            RegisterViews(collection);
            RegisterSheets(collection);

            ServiceProvider serviceProvider = collection.BuildServiceProvider();
            SheetShell.Init(serviceProvider);

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
                .AddSingleton<GoalBottomSheetViewModel>()
                .AddSingleton<TaskEditPageViewModel>()
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
                .AddSingleton<ThoughtPage>()
                .AddSingleton<ThoughtEditPage>()
                .AddSingleton<DiaryRecordPage>()
                .AddSingleton<DiaryRecordEditPage>();
        }

        private static void RegisterSheets(IServiceCollection collection)
        {
            collection
                .AddSingleton<GoalBottomSheet>();
        }
    }
}