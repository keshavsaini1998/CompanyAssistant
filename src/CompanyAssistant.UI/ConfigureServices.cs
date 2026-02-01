using MudBlazor;
using MudBlazor.Services;

namespace CompanyAssistant.UI
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBlazorUIServices(this IServiceCollection services)
        {
            services.AddMudBlazorDialog();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = true;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 4000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            return services;
        }
    }
}
