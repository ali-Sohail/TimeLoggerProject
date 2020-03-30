using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TimeLogger.Web
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
            services.AddControllers();

            //services.AddSingleton<IItemRepository, ItemRepository>();

            string connectionstring = Configuration.GetConnectionString("LoggerDatabase");

            if (string.IsNullOrEmpty(connectionstring))
            {
                services.AddDbContext<Models.LoggerDBContext>(opt =>
                    opt.UseSqlServer("Server=.;Database=LoggerDB;Trusted_Connection=True;MultipleActiveResultSets=true"));
            }
            else
            {
                services.AddDbContext<Models.LoggerDBContext>(opt =>
                    opt.UseSqlServer(connectionstring));
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //
            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Creating Context Inline
            //endpoints.MapGet("/", async context =>
            //{
            //    await context.Response.WriteAsync($"Hello From {System.Diagnostics.Process.GetCurrentProcess().ProcessName}!");
            //});
        }
    }
}