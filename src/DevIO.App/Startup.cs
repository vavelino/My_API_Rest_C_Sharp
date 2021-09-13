using DevIO.App.Configurations;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DevIO.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        /*
                public Startup(IConfiguration configuration)
                {
                    Configuration = configuration;
                }
        */
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
                //Colocar a informação do banco No computador
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var conection = "Server=localhost;User Id=root;Password=;Database=MyWebAPI";

            //services.AddIdentityConfiguration();
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseMySql(
                    conection,
                    ServerVersion.AutoDetect(conection),
                    //Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("DevIO.App")
                    );
                //options.EnableSensitiveDataLogging();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            });

            //services.AddDatabaseDeveloperPageExceptionFilter();


           // services.AddMvcConfiguration();

           services.AddAutoMapper(typeof(Startup)); // Procure qualquer class que tem o profile

            // Suprimindo A validação automatica do model State
            //Você quer validar para validar 
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });

          services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
           

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseExceptionHandler("/error/500");
                app.UseStatusCodePagesWithRedirects("/error/{0}");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseGlobalizationConfig();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            */
        }
    }
}
