using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LibraryApp.Core;
using LibraryApp.Core.Contracts;
using LibraryApp.Core.Implementations;
using LibraryApp.Core.Uow;
using LibraryApp.Infrastructure.Context;
using LibraryApp.Infrastructure.Localization;
using LibraryApp.Infrastructure.MIddlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Recognizer.Adapter.RecognizerAdapterService;
using Recognizer.Manager;

namespace LibraryApp.Api
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

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            //    .AddDataAnnotationsLocalization(options =>
            //    {
            //        options.DataAnnotationLocalizerProvider = (type, factory) =>
            //        {
            //            var assemblyName = new AssemblyName(typeof(LocalizationResources).GetTypeInfo().Assembly.FullName);
            //            return factory.Create(nameof(LocalizationResources), assemblyName.Name);
            //        };
            //    });
            services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    ).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                     .AddDataAnnotationsLocalization(options =>
                     {
                         options.DataAnnotationLocalizerProvider = (type, factory) =>
                         {
                             var assemblyName = new AssemblyName(typeof(LocalizationResources).GetTypeInfo().Assembly.FullName);
                             return factory.Create(nameof(LocalizationResources), assemblyName.Name);
                         };
                     });

            services.AddCors();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILoanRepository, LoanRepository>();
            services.AddTransient<IBookInventoryRepository, BookInventoryRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
            
            //intergration services
            services.AddTransient<IRecognizerManager, RecognizerManager>();
            services.AddTransient<IRecognizerAdapterService, RecognizerAdapterService>();
            services.Configure<RecognizerSettings>(Configuration.GetSection(nameof(RecognizerSettings)));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddTransient<ICustomerRepository, CustomerRepository>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    opt => opt.MigrationsAssembly("LibraryApp.Infrastructure"))
               );

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("hr-HR"),

                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "hr-HR", uiCulture: "hr-HR");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionMiddleware();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
