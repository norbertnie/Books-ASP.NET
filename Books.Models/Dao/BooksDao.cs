using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dapper;

namespace Books.Models.Dao
{
    public class BooksDao
    {
        private readonly DbConnectionHolder _dbConnectionHolder;

        public BooksDao(DbConnectionHolder dbConnectionHolder)
        {
            _dbConnectionHolder = dbConnectionHolder;
        }

        public List<Book> GetBooks()
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var books = connection.Query<Book>("SELECT * FROM dbo.Books INNER JOIN dbo.Authors ON dbo.Authors.AuthorId = dbo.Books.AuthorId").ToList();
                return books;
            }
        }

        public List<Book> GetBooks(string title = "", string lastName = "", string firstName = "", string rlastNamerSec = "")
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var sql = new StringBuilder("SELECT * FROM dbo.Books INNER JOIN dbo.Authors ON dbo.Authors.AuthorId = dbo.Books.AuthorId WHERE 1=1");

                if (!string.IsNullOrEmpty(firstName))
                {
                    sql.Append(" AND dbo.Authors.FirstName LIKE @rfirstName");
                }

                if (!string.IsNullOrEmpty(lastName))
                {
                    sql.Append(" AND dbo.Authors.LastName LIKE @rlastNamer");
                }

                if (!string.IsNullOrEmpty(rlastNamerSec))
                {
                    sql.Append(" OR dbo.Authors.LastName LIKE @rlastNamerSec");
                }

                if (!string.IsNullOrEmpty(title))
                {
                    sql.Append(" AND dbo.Books.title LIKE @rtitle");
                }

                var books = connection.Query<Book>(sql.ToString(),
                    new
                    {
                        rlastNamer = "%" + lastName + "%",
                        rfirstName = "%" + firstName + "%",
                        rlastNamerSec = "%" + rlastNamerSec + "%",
                        rtitle = "%" + title + "%"
                    }).ToList();

                return books;
            }
        }

        public Book GetBook(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var book = connection.Query<Book>("SELECT * FROM dbo.Books INNER JOIN dbo.Authors ON dbo.Authors.AuthorId = dbo.Books.AuthorId where dbo.Books.bookId = @bookId",
                    new
                    {
                        bookId = id
                    }).FirstOrDefault();

                return book;
            }
        }

        public Book DeleteBook(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
            
                    var book = connection.Query<Book>("DELETE FROM dbo.Books  Where dbo.Books.bookId  = @bookId",
                        new
                        {
                            bookId = id
                        }).FirstOrDefault();
                    return book;
            }
        }

        public Book UpdateBook(string firstName, string lastName, string title, string releaseDate, int id)
        {

            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var book = connection.Query<Book>("UPDATE dbo.Books SET dbo.Books.Title = @title, dbo.Books.ReleaseDate = @relDate  Where dbo.Books.BookID = @bookId",
                    new
                    {
                        fname=  firstName,
                        lname =  lastName,
                        title =  title,
                        relDate = releaseDate,
                        bookId = id
                    }).FirstOrDefault();
                return book;
            }
        }

        public Book InsertBook(string firstName, string lastName, string title, string releaseDate)
        {

            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var book = connection.Query<Book>("INSERT INTO dbo.Books (dbo.Books.Title, dbo.Books.ReleaseDate, dbo.Books.AuthorId) VALUES (@title, @relDate, (SELECT dbo.Authors.AuthorId FROM dbo.Authors WHERE dbo.Authors.FirstName = @fname AND dbo.Authors.LastName = @lname))",
                    new
                    {
                        fname = firstName,
                        lname =  lastName,
                        Title = title,
                        relDate =  releaseDate
                    }).FirstOrDefault();
                    return book;
            }
        }

        public void UpdateFirstBookReleaseDate(int authorId, DateTime releaseDate)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var book = connection.QueryFirstOrDefault<Book>(
                    "SELECT TOP 1 * FROM dbo.Books WHERE AuthorId = @AuthorId ORDER BY ReleaseDate ASC",
                    new { AuthorId = authorId });

                if (book != null)
                {
                    connection.Execute(
                        "UPDATE dbo.Books SET ReleaseDate = @ReleaseDate WHERE BookID = @BookId",
                        new { ReleaseDate = releaseDate, BookId = book.BookId });
                }
            }
        }
    }
}
