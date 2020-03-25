using Microsoft.Extensions.DependencyInjection;

namespace SalaryApp.Services
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddSystemTimeService(this IServiceCollection services) => services.AddSingleton<ITime, SystemTimeService>();
    }
}
