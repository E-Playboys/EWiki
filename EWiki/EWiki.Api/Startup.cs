using EWiki.Api.DataAccess;
using EWiki.Api.Identity;
using EWiki.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;

namespace EWiki.Api
{
    public class Startup
    {
        const string TokenAudience = "EWikiAudience";
        const string TokenIssuer = "EWikiIssuer";
        private RsaSecurityKey key;
        private TokenAuthOptions tokenOptions;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RSAParameters keyParams = RSAKeyUtils.GetRandomKey();
            key = new RsaSecurityKey(keyParams);
            tokenOptions = new TokenAuthOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
            };

            // Add framework services.
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<EWikiContext>(options =>
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<User, IdentityRole>()
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

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("TokenAuth", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
                //auth.AddPolicy("PremiumUser", new AuthorizationPolicyBuilder()
                //    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                //    .RequireAuthenticatedUser().Build());
                //auth.AddPolicy("User", new AuthorizationPolicyBuilder()
                //    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                //    .RequireAuthenticatedUser().Build());
                //auth.AddPolicy("Guest", new AuthorizationPolicyBuilder()
                //    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                //    .RequireAuthenticatedUser().Build());
            });

            // Add application services.
            services.AddSingleton(tokenOptions);
            services.AddSingleton<IDbFactory, DbFactory>();

            services.AddSingleton<IArchiveRepository, ArchiveRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<ILocationRepository, LocationRepository>();
            services.AddSingleton<IMoveRepository, MoveRepository>();
            services.AddSingleton<IPageRepository, PageRepository>();
            services.AddSingleton<IPageContentRepository, PageContentRepository>();
            services.AddSingleton<IPageLangRepository, PageLangRepository>();
            services.AddSingleton<IPageMetaRepository, PageMetaRepository>();
            services.AddSingleton<IPokedexRepository, PokedexRepository>();
            services.AddSingleton<IRevisionRepository, RevisionRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IWikiImageRepository, WikiImageRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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
                catch(Exception ex)
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

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(
                                new { authenticated = false, tokenExpired = true }));
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject
                            (new { success = false, error = error.Error.Message }));
                    }
                    else await next();
                });
            });

            // For more details: https://docs.asp.net/en/latest/security/cors.html
            app.UseCors("AllowEwikiBDOrigin");

            // Basic settings - signing key to validate with, audience and issuer.
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateAudience = true,
                    ValidAudience = tokenOptions.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });
        }
    }
}
