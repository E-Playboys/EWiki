using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EWiki.Api.DataAccess;
using Microsoft.EntityFrameworkCore;
using EWiki.Api.Models;
using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;
using Hangfire;

namespace EWiki.Api
{
    public partial class Startup
    {
        private readonly string _connectionString;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _connectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<EWikiContext>(options =>
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<User, ApplicationRole>()
                .AddEntityFrameworkStores<EWikiContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowEwikiBDOrigin",
                    builder => builder
                                .WithOrigins("http://localhost:8080")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .WithExposedHeaders());
            });

            // Hangfire
            services.AddHangfire(config => config.UseSqlServerStorage(_connectionString));

            // Add application services.
            services.AddSingleton<IDbFactory, DbFactory>();

            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<ILocationRepository, LocationRepository>();
            services.AddSingleton<IMoveRepository, MoveRepository>();
            services.AddSingleton<IPageRepository, PageRepository>();
            services.AddSingleton<IPageContentRepository, PageContentRepository>();
            services.AddSingleton<IPageLangRepository, PageLangRepository>();
            services.AddSingleton<IPageMetaRepository, PageMetaRepository>();
            services.AddSingleton<IPokedexRepository, PokedexRepository>();
            services.AddSingleton<IWikiImageRepository, WikiImageRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IFeedInfoRepository, FeedInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ConfigureAuth(app);

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<EWikiContext>()
                             .Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // For more details: https://docs.asp.net/en/latest/security/cors.html
            app.UseCors("AllowEwikiBDOrigin");
        }
    }
}
