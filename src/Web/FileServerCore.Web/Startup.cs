﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Avg.Data;
using Avg.Data.Common;
using Microsoft.EntityFrameworkCore;
using Avg.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Avg.Services.Users;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using Microsoft.Net.Http.Headers;
using System;
using FileServerCore.Web.Infrastructure.Middlewares;

namespace FileServerCore.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("aspnet-WebApplication1-d6b4432f-02eb-4d60-be7f-05ad30607873");
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AvgDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("FileServerCoreDb")));
            services.AddScoped<DbContext, AvgDbContext>();
            services.Add(ServiceDescriptor.Scoped(typeof(IRepository<,>), typeof(Repository<,>)));
            services.Add(ServiceDescriptor.Scoped(typeof(IRepository<>), typeof(Repository<>)));

            services.AddScoped<IUserService, UserService>();

            services.AddIdentity<AvgUser, IdentityRole>(
                o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                }).AddEntityFrameworkStores<AvgDbContext>().AddDefaultTokenProviders();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMemoryCache();

            services.AddLocalization();

            services.AddMvc()
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddKendo();
        }        

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory, IUserService userService)
        {            
            var supportedCultures = this.Configuration.GetSection("SupportedCultures").GetChildren().Select(c => new CultureInfo(c.Value)).ToList();
            
            app.UseRequestLocalization(
                 new RequestLocalizationOptions
                 {
                     DefaultRequestCulture = new RequestCulture(supportedCultures.First().ToString()),                                
                                SupportedCultures = supportedCultures,                                
                                SupportedUICultures = supportedCultures
                 });

            CultureInfo.DefaultThreadCurrentCulture = supportedCultures.Last();
            CultureInfo.DefaultThreadCurrentUICulture = supportedCultures.Last();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

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
            app.UseSession();

            app.AddAutomaticMigration(userService, scopeFactory, this.Configuration);

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
    }
}
