using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using Microsoft.EntityFrameworkCore;
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

                using (var transaction = myDbContext.Database.BeginTransaction())
                {
                    var loanStatus1 = new LoanStatus
                    {
                        Id = 1,
                        Name = "Loaned",
                        IsActive = true,
                        IsDeleted = false
                    };


                    myDbContext.LoanStatuses.Add(loanStatus1);
                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.LoanStatuses ON;");
                    myDbContext.SaveChanges();
                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.LoanStatuses OFF;");
                    transaction.Commit();
                }

            }

            if (!myDbContext.LoanStatuses.Any(lt => lt.Name == "Returned"))
            {

                using (var transaction = myDbContext.Database.BeginTransaction())
                {

                    var loanStatus2 = new LoanStatus
                    {
                        Id = 2,
                        Name = "Returned",
                        IsActive = true,
                        IsDeleted = false
                    };

                    myDbContext.LoanStatuses.Add(loanStatus2);

                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.LoanStatuses ON;");
                    myDbContext.SaveChanges();
                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.LoanStatuses OFF;");
                    transaction.Commit();
                }

            }
           

        }
    }
}
