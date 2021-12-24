using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETToDoDemo.Contexts;

namespace NETToDoDemo
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //public Startup(IWebHostEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //           .SetBasePath(env.ContentRootPath)
        //           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //           .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //    //services.AddControllers();
        //}
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews();
            var builder = new ConfigurationBuilder()
                   .SetBasePath(_env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{_env.EnvironmentName}.json", optional: true)
                   .AddEnvironmentVariables()
                   .Build();
            var defaultConnectionString = string.Empty;
            if (_env.EnvironmentName == "Development")
            {
                defaultConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            }
            else
            {
                // Use connection string provided at runtime by Heroku.
                var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
                var userPassSide = connectionUrl.Split("@")[0];
                var hostSide = connectionUrl.Split("@")[1];

                var user = userPassSide.Split(":")[0];
                var password = userPassSide.Split(":")[1];
                var host = hostSide.Split("/")[0];
                var database = hostSide.Split("/")[1].Split("?")[0];

                defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
            }
            //var defaultConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<ToDoContext>(options =>
               options.UseNpgsql(defaultConnectionString));
            //var builder = WebApplication.CreateBuilder(args);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ToDoContext dataContext)
        {
            dataContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
