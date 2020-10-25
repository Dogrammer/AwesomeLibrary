using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Api.SeedData
{
    public class LoanStatusSeed
    {
        public static void SeedTestDataViaDbContext(ApplicationDbContext myDbContext)
        {
            if (!myDbContext.LoanStatuses.Any(lt => lt.Name == "Loaned"))
            {
                myDbContext.LoanStatuses.Add(new LoanStatus
                {
                    Name = "Loaned",
                });
            }
            if (!myDbContext.LoanStatuses.Any(lt => lt.Name == "Returned"))
            {
                myDbContext.LoanStatuses.Add(new LoanStatus
                {
                    Name = "Returned",
                });

            }

            myDbContext.SaveChanges();
        }
    }
}
