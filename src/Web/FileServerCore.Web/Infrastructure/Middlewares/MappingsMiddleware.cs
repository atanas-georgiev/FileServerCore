namespace FileServerCore.Web.Infrastructure.Middlewares
{
    using System.Collections.Generic;
    using System.Reflection;

    using Avg.Services.Mappings;

    using Microsoft.AspNetCore.Builder;

    public static class MappingsMiddleware
    {
        public static void AddAutomaticMigration(this IApplicationBuilder app)
        {
            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(new List<Assembly> { Assembly.GetEntryAssembly() });
        }
    }
}