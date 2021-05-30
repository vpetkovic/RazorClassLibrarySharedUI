using Microsoft.Extensions.DependencyInjection;

namespace Demo.SharedUI
{
    public static class StartupHelpers
    {
        public static void AddSharedUI(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(SharedUIConfigureOptions));
        }
    }
}
