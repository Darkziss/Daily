using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace Daily
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Nunito-Bold.ttf", "Nunito");
                    fonts.AddFont("Nunito-BoldItalic.ttf", "NunitoItalic");
                    fonts.AddFont("Nunito-ExtraBold.ttf", "NunitoBold");
                });

            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}