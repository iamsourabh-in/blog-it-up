using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Configuration.Core;
using Blog.Web.ConfigModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Blog.Persistence;
using Blog.Entities;
using Blog.Services;
using AutoMapper;
using Blog.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;
using System.Buffers;
using Blog.Web.Models.ApiModel;
using Newtonsoft.Json;
using Blog.Web.Middleware;
using Microsoft.Extensions.Logging;

namespace Blog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Used for register Cyrillic encoding type when in views.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddRouting();
            //Loading App Settings section from the file.
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();

            // Registering MongoDB connection string as service for repository to use
            services.Configure<BlogAppMongoDbSetting>(Configuration.GetSection(nameof(BlogAppMongoDbSetting)));
            services.AddSingleton<IBlogAppMongoDbSetting>(sp => sp.GetRequiredService<IOptions<BlogAppMongoDbSetting>>().Value);

            // Adding HttpContext for the views to access
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // Adding repositories to IOC container
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Adding services to IOC container
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUserService, UserService>();

            // Adding AutoMapper as a service
            services.AddAutoMapper(typeof(Startup));

            //Register Controller with views
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // to avoid error : No service for type 'Blog.Web.Middleware.APIRequestMorpher' has been registered.
            services.AddScoped<APIRequestMorpher>();

            //Add API Controllers 
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
            }
            else
            {
                // The default HSTS value is 30 days. 
                app.UseHsts();
            }

            loggerFactory.AddFile("Logs/myapp-{Date}.txt");

            app.UseStatusCodePagesWithReExecute("/anonymmus/error", "?statusCode ={0}");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseAPIRequestMorpher(true);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }


}
