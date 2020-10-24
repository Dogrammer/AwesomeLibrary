using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LibraryApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
        
            
                //Log.Information("Applying seed data ({ApplicationContext})...", AppName);

                using (var newScope = host.Services.CreateScope())
                {
                    var myDbContext = newScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    myDbContext.Database.Migrate();


                SeedData.LibrarySeed.SeedTestDataViaDbContext(myDbContext);
                SeedData.BookSeed.SeedTestDataViaDbContext(myDbContext);
                SeedData.BookInventorySeed.SeedTestDataViaDbContext(myDbContext);
                SeedData.LoanStatusSeed.SeedTestDataViaDbContext(myDbContext);


                try
                {
                        myDbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        myDbContext.Dispose();
                    }

                host.Run();

                //Log.Information("Finished seeding ({ApplicationContext})...", AppName);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
