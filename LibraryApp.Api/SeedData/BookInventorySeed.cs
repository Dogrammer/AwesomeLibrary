using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Api.SeedData
{
    public class BookInventorySeed
    {
        public static void SeedTestDataViaDbContext(ApplicationDbContext myDbContext)
        {
            if (!myDbContext.BookInventories.Any(lt => lt.BookId == 1))
            {
                myDbContext.BookInventories.Add(new BookInventory
                {
                    BookId = 1,
                    Quantity = 10,
                    CurrentQuantity = 10
                });

                myDbContext.SaveChanges();
            }

            if (!myDbContext.BookInventories.Any(lt => lt.BookId == 2))
            {
                myDbContext.BookInventories.Add(new BookInventory
                {
                    BookId = 2,
                    Quantity = 10,
                    CurrentQuantity = 10
                });

                myDbContext.SaveChanges();
            }

        }
    }
}
