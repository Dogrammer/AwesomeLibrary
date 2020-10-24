using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Api.SeedData
{
    public class LibrarySeed
    {
        public static void SeedTestDataViaDbContext(ApplicationDbContext myDbContext)
        {
            if (!myDbContext.Libraries.Any(lt => lt.Name == "AwesomeLibrary"))
            {
                myDbContext.Libraries.Add(new Library
                {
                    Name = "AwesomeLibrary",
                    Address = "Zagrebačka 10"
                });

                myDbContext.SaveChanges();
            }
            
        }
    }
}
