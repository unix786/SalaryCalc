using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SalaryApp.Models;
using SalaryApp.Services;

namespace SalaryApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration) => this.configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<MSSqlLocalDBContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddSystemTimeService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app
                //.UseStatusCodePages()
                //.UseWhen(
                //    (context) => !context.Request.Path.StartsWithSegments("/src"),
                //    (app2) => app2.UseStaticFiles()
                //)
                .UseStaticFiles()
                .UseMvcWithDefaultRoute();
        }
    }
}
