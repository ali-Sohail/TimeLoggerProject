using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TimeLogger.Web.Services;

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
            services.AddCors();

            services.AddControllers();

            //services.AddSingleton<IMyService>((container) =>
            //{
            //	var logger = container.GetRequiredService<ILogger<MyService>>();
            //	return new MyService() { Logger = logger };
            //});

            //services.AddSingleton<IItemRepository, ItemRepository>();

            services.AddDbContext<Models.LoggerDBContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("LoggerDatabase")));

            // configure strongly typed settings objects

            var appSettingsSection = Configuration.GetSection("AppSettings");

            services.Configure<Helpers.AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<Helpers.AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            try
            {
                using (var context = new Models.LoggerDBContext())
                {
                    // Create Database if not Created
                    context.Database.EnsureCreated();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, ILogger logger*/)
        {
            if (env.IsDevelopment())
            {
                //logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Redirection of HTTP to HTTPS
            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

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