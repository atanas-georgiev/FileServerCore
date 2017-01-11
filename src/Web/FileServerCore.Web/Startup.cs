namespace FileServerCore.Web
{
    using System;
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using Avg.Data;
    using Avg.Data.Common;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;

    using Newtonsoft.Json.Serialization;
    using FileServerCore.Web.Infrastructure.Helpers;
    using Avg.Data.Models;
    using AvgIdentity.Extensions;
    using AvgIdentity.Managers;    

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("aspnet-WebApplication1-d6b4432f-02eb-4d60-be7f-05ad30607873");
            }

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IUserRoleManager<AvgIdentityUser, AvgDbContext> userService,
            IServiceProvider provider)
        {
            var supportedCultures =
                this.Configuration.GetSection("SupportedCultures")
                    .GetChildren()
                    .Select(c => new CultureInfo(c.Value))
                    .ToList();

            app.UseRequestLocalization(
                new RequestLocalizationOptions
                    {
                        DefaultRequestCulture =
                            new RequestCulture(supportedCultures.First().ToString()),
                        SupportedCultures = supportedCultures,
                        SupportedUICultures = supportedCultures
                    });

            CultureInfo.DefaultThreadCurrentCulture = supportedCultures.Last();
            CultureInfo.DefaultThreadCurrentUICulture = supportedCultures.Last();

            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));

            // loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(
                new StaticFileOptions()
                    {
                        OnPrepareResponse = (context) =>
                            {
                                var headers = context.Context.Response.GetTypedHeaders();
                                headers.CacheControl = new CacheControlHeaderValue() { MaxAge = TimeSpan.FromDays(100) };
                            }
                    });

            app.UseIdentity();

            //app.UseFacebookAuthentication(
            //    new FacebookOptions()
            //        {
            //            AppId = this.Configuration["Security:ExternalProviders:Facebook:Id"],
            //            AppSecret = this.Configuration["Security:ExternalProviders:Facebook:Secret"]
            //        });

            //app.UseGoogleAuthentication(
            //    new GoogleOptions()
            //        {
            //            ClientId = this.Configuration["Security:ExternalProviders:Google:Id"],
            //            ClientSecret = this.Configuration["Security:ExternalProviders:Google:Secret"]
            //        });

            app.UseSession();

            app.AddAvgIdentityMigration<AvgDbContext, AvgIdentityUser>(scopeFactory, Configuration);

            app.UseMvc(
                routes =>
                    {
                        routes.MapRoute(
                            name: "areaRoute",
                            template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                    });

      
            app.UseKendo(env);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AvgDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("FileServerCoreDb")));            
            services.AddScoped<DbContext, AvgDbContext>();
            services.Add(ServiceDescriptor.Scoped(typeof(IRepository<,>), typeof(Repository<,>)));
            services.Add(ServiceDescriptor.Scoped(typeof(IRepository<>), typeof(Repository<>)));
            services.Add(ServiceDescriptor.Scoped(typeof(IUserRoleManager<,>), typeof(UserRoleManager<,>)));

            //services.Add(ServiceDescriptor.Scoped(typeof(IdentityDbContext<>), typeof(AvgDbContext<>)));
            
            services.AddAvgIdentityServices<AvgDbContext, AvgIdentityUser>(Configuration);

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMemoryCache();

            services.AddLocalization();

            services.AddMvc()
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddKendo();
            
            ServicesHelper.Initialize(services);

            services.AddAutoMapper();
        }

        private int IdentityDbContext<T>()
        {
            throw new NotImplementedException();
        }
    }
}