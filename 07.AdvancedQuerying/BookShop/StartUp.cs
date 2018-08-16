using BookShop.Data;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BookShop
{
    public class StartUp
    {
        static void Main()
        {
            using (BookShopContext context = new BookShopContext())
            {
                string result = GetMostRecentBooks(context);
                Console.WriteLine(result);
            }
        }

        //P01_AgeRestriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);
            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .ToArray();

            var booktitles = books.Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            return string.Join(Environment.NewLine, booktitles);
        }

        //P02_GoldenBooks
        public static string GetGoldenBooks(BookShopContext context)
        {
            var editionTypeName = "Gold";
            var editionType = (EditionType)Enum.Parse(typeof(EditionType), editionTypeName);
            var books = context.Books
                .Where(x => x.EditionType == editionType && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //P03_BooksbyPrice
        public static string GetBooksByPrice(BookShopContext context)
        {
            var titlesAndPricesOfBooks = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => $"{x.Title} - ${x.Price:F2}")
                .ToArray();

            return string.Join(Environment.NewLine, titlesAndPricesOfBooks);
        }

        //P04_NotReleasedIn
        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var titlesBooksNotReleasedInYear = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, titlesBooksNotReleasedInYear);
        }

        //P05_BookTitlesbyCategory
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            List<Book> books = new List<Book>();
            for (int i = 0; i < categories.Length; i++)
            {
                books.AddRange(context.Books
                    .Where(x => x.BookCategories
                    .Any(y => string.Equals(y.Category.Name, categories[i], StringComparison.OrdinalIgnoreCase))));

            }

            string result = string.Join(Environment.NewLine, books.OrderBy(x => x.Title)
                     .Select(x => x.Title)
                     .ToList());

            return result;
        }

        //P06_ReleasedBeforeDate
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var infoForBooksBeforeDate = context.Books
                .Where(x => x.ReleaseDate < dateTime)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, infoForBooksBeforeDate);
        }

        //P07_AuthorSearch 
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorFullNames = context.Authors
                .Where(x => EF.Functions.Like(x.FirstName, "%" + input))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x => $"{x.FirstName} {x.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, authorFullNames);
        }

        //P08_BookSearch
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => EF.Functions.Like(x.Title, $"%{input}%"))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //P09_BookSearchbyAuthor
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => EF.Functions.Like(x.Author.LastName, $"{input}%"))
                .OrderBy(x => x.BookId)
                .Select(x => $"{x.Title} ({x.Author.FirstName} {x.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //P10_CountBook
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var bookTitlesCount = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .Count();

            return bookTitlesCount;
        }

        //P11_TotalBookCopies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new
                {
                    FullName = $"{x.FirstName} {x.LastName}",
                    TotalCopies = x.Books.Sum(y => y.Copies)
                })
                .OrderByDescending(x => x.TotalCopies);

            return string.Join(Environment.NewLine, authors.Select(x => $"{x.FullName} - {x.TotalCopies}"));
        }

        //P12_ProfitbyCategory
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    Profit = x.CategoryBooks.Sum(y => y.Book.Copies * y.Book.Price)
                })
                .OrderByDescending(x => x.Profit)
                .ThenBy(x => x.CategoryName)
                .ToArray();

            return string.Join(Environment.NewLine, categories.Select(x => $"{x.CategoryName} ${x.Profit:f2}"));
        }

        //P13_MostRecentBooks
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    Books = x.CategoryBooks.Select(c => new
                    {
                        c.Book.Title,
                        c.Book.ReleaseDate
                    })
                    .OrderByDescending(b => b.ReleaseDate)
                    .Take(3)
                })
                .OrderBy(x => x.CategoryName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var c in categories)
            {
                sb.Append("--");
                sb.AppendLine(c.CategoryName);
                foreach (var b in c.Books)
                {
                    sb.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //P14_IncreasePrices
        public static void IncreasePrices(BookShopContext context)
        {
            decimal increasement = 5;
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010);

            foreach (var b in books)
            {
                b.Price += increasement;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            int lessThanCopies = 4200;

            var books = context.Books
                .Where(x => x.Copies < lessThanCopies)
                .ToArray();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Length;
        }
    }
}

