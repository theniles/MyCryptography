using MyCryptographyApp.Services;

namespace MyCryptographyApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterDependencyInjection();

            return builder.Build();
        }

        public static MauiAppBuilder RegisterDependencyInjection(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<OperationService>();
            return builder;
        }
    }
}