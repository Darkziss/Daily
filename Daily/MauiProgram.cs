using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Daily.Tasks;
using Daily.Thoughts;
using Daily.Diary;
using Daily.Data;
using Daily.ViewModels;
using Daily.Pages;
using CommunityToolkit.Maui;
using Plugin.SegmentedControl.Maui;
using Sharpnado.TaskLoaderView;
using Android.Content.Res;
using AndroidColor = Android.Graphics.Color;

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

#if ANDROID
            PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(AndroidColor.Transparent);
            });
            
            DatePickerHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(AndroidColor.Transparent);
            });
#endif
            IServiceCollection collection = builder.Services;

            collection.AddSingleton<IUnitOfWork, UnitOfWork>();

            RegisterStorages(collection);
            RegisterViewModels(collection);
            RegisterViews(collection);

            return builder.Build();
        }

        private static void RegisterStorages(IServiceCollection collection)
        {
            collection
                .AddSingleton<GoalStorage>()
                .AddSingleton<OneTimeTaskStorage>()
                .AddSingleton<RecurringTaskStorage>()
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
                .AddSingleton<ThoughtEditPage>()
                .AddSingleton<DiaryRecordPage>()
                .AddSingleton<DiaryRecordEditPage>();
        }
    }
}