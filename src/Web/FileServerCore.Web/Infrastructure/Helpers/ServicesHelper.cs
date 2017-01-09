namespace FileServerCore.Web.Infrastructure.Helpers
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServicesHelper
    {
        public static IServiceProvider ServiceProvider;

        public static void Initialize(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
