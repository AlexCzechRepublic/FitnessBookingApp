using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessBookingApp.Infrastructure
{
    public static class ServiceLocator
    {
        public static IServiceProvider Services { get; private set; } = default!;

        public static void Init(IServiceProvider services) => Services = services;

        public static IHttpContextAccessor Http => Services.GetRequiredService<IHttpContextAccessor>();

        public static T Get<T>() where T : notnull => Services.GetRequiredService<T>();
    }
}