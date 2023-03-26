namespace BookShop
{
    using Data;
    using EFCore.BulkExtensions;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //int input = int.Parse(Console.ReadLine());
            int result = RemoveBooks(db);
            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            string[] booksTitles = context.Books
                .AsNoTracking()
                .AsEnumerable()
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return String.Join(Environment.NewLine, booksTitles);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] booksTitles = context.Books
                .AsNoTracking()
                .AsEnumerable()
                .Where(b => b.EditionType.ToString() == "Gold"
                && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return String.Join(Environment.NewLine, booksTitles);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .AsNoTracking()
                .AsEnumerable()
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    Price = b.Price.ToString()
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksTitles = context.Books
                .AsNoTracking()
                .AsEnumerable()
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return String.Join(Environment.NewLine, booksTitles);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            string[] bookTitles = context.Books
                .Where(b => b.BookCategories
                    .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Date < DateTime.Parse(DateTime.Parse(date).ToString("dd-MM-yyyy")).Date)
                .OrderByDescending(b => b.ReleaseDate);

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .ToArray()
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToArray();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] booksTitles = context.Books
                .Select(b => b.Title)
                .OrderBy(b => b)
                .Where(b => b.ToLower().Contains(input.ToLower()))
                .ToArray();

            return String.Join(Environment.NewLine, booksTitles);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .AsNoTracking()
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorName = b.Author.FirstName + ' ' + b.Author.LastName
                }); ;

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .AsEnumerable()
                .Select(a => new
                {
                    AuthorName = a.FirstName + ' ' + a.LastName,
                    BooksCount = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.BooksCount);

            foreach (var a in authors)
            {
                sb.AppendLine($"{a.AuthorName} - {a.BooksCount}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Select(a => new
                {
                    a.Name,
                    TotalPrice = a.CategoryBooks.Sum(c => c.Book.Copies * c.Book.Price)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalPrice);

            foreach (var c in categories)
            {
                sb.AppendLine($"{c.Name} ${c.TotalPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .AsEnumerable()
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(b => b.Book.ReleaseDate)
                    .Select(b => new
                    {
                        b.Book.Title,
                        ReleaseYear = b.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                    .ToArray()
                })
                .ToArray();

            foreach (var c in categories)
            {
                sb.AppendLine($"--{c.Name}");
                foreach (var b in c.Books)
                {
                    sb.AppendLine($"{b.Title} ({b.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var b in books)
            {
                b.Price += 5;
            }

            context.BulkUpdate(books);
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            int count = booksToRemove.Length;

            context.Books
                .Where(b => b.Copies < 4200)
                .BatchDelete();

            context.SaveChanges();
            return count;
        }
    }
}


