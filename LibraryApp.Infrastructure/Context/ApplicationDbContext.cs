﻿using System;
using System.Collections.Generic;
using System.Text;
using LibraryApp.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<BookInventory> BookInventories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanStatus> LoanStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //book-author
            modelBuilder.Entity<BookAuthor>()
                .HasKey(bc => new { bc.BookId, bc.AuthorId });
            modelBuilder.Entity<BookAuthor>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(bc => bc.Author)
                .WithMany(c => c.BookAuthors)
                .HasForeignKey(bc => bc.AuthorId);

            //book-loan
            modelBuilder.Entity<BookLoan>()
               .HasKey(bc => new { bc.BookId, bc.LoanId });
            modelBuilder.Entity<BookLoan>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookLoans)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookLoan>()
                .HasOne(bc => bc.Loan)
                .WithMany(c => c.BookLoans)
                .HasForeignKey(bc => bc.LoanId);
        }


    }
}
