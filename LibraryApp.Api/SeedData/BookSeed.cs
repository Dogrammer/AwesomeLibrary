using LibraryApp.Infrastructure.Context;
using LibraryApp.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibraryApp.Api.SeedData
{
    public class BookSeed
    {
        public static void SeedTestDataViaDbContext(ApplicationDbContext myDbContext)
        {
            if (!myDbContext.Books.Any(lt => lt.Title == "Harry Potter 1"))
            {

                using (var transaction = myDbContext.Database.BeginTransaction())
                {
                    var book1 = new Book
                    {
                        Id = 1,
                        Title = "Harry Potter 1",
                        LibraryId = 1,
                        YearPublished = 2000
                    };

                    //var author1 = new Author
                    //{
                    //    Id = 1,
                    //    DateOfBirth = DateTime.Now,
                    //    FirstName = "J.K.",
                    //    LastName = "Rowling",
                    //    DateOfDeath = DateTime.Now
                    //};
                    myDbContext.Books.Add(book1);
                    //myDbContext.Authors.Add(author1);
                    //myDbContext.BookAuthors.Add(new BookAuthor
                    //{
                    //    AuthorId = 1,
                    //    BookId = 1
                    //});
                   
                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.Books ON;");
                    myDbContext.SaveChanges();
                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.Books OFF;");
                    transaction.Commit();
                }
               
            }

            if (!myDbContext.Authors.Any(lt => lt.FirstName == "J.K." && lt.LastName == "Rowling"))
            {

                using (var transaction = myDbContext.Database.BeginTransaction())
                {

                    var author1 = new Author
                    {
                        Id = 1,
                        DateOfBirth = DateTime.Now,
                        FirstName = "J.K.",
                        LastName = "Rowling",
                        DateOfDeath = DateTime.Now
                    };
                    myDbContext.Authors.Add(author1);
                    

                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.Authors ON;");
                    myDbContext.SaveChanges();
                    myDbContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LibraryApp.dbo.Authors OFF;");
                    transaction.Commit();
                }

            }
            if (!myDbContext.BookAuthors.Any(lt => lt.AuthorId == 1 && lt.BookId == 1))
            {
                myDbContext.BookAuthors.Add(new BookAuthor
                {
                    AuthorId = 1,
                    BookId = 1
                });

                myDbContext.SaveChanges();
            }

        }

    }
}
