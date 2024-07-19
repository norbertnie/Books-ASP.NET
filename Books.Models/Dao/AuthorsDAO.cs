using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Books.Models.Dao
{
    public class AuthorsDAO
    {
        private readonly DbConnectionHolder _dbConnectionHolder;

        public AuthorsDAO(DbConnectionHolder dbConnectionHolder)
        {
            _dbConnectionHolder = dbConnectionHolder;
        }

        public List<Author> GetAuthors()
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var authorDict = new Dictionary<int, Author>();

                var authors = connection.Query<Author, Book, Author>(
                    @"SELECT a.AuthorID, a.DateofBirth, a.FirstName, a.LastName, b.BookID, b.ReleaseDate
                      FROM dbo.Authors a 
                      LEFT JOIN dbo.Books b ON a.AuthorId = b.AuthorId",
                    (author, book) =>
                    {
                        if (!authorDict.TryGetValue(author.AuthorId, out var currentAuthor))
                        {
                            currentAuthor = author;
                            currentAuthor.Books = new List<Book>();
                            authorDict.Add(currentAuthor.AuthorId, currentAuthor);
                        }

                        if (book != null)
                        {
                            currentAuthor.Books.Add(book);
                        }

                        return currentAuthor;
                    },
                    splitOn: "BookID").Distinct().ToList();

                return authors;
            }
        }

        public List<Author> GetAuthors(string firstName, string lastName)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var authorDict = new Dictionary<int, Author>();

                var authors = connection.Query<Author, Book, Author>(
                    @"SELECT a.AuthorID, a.DateofBirth, a.FirstName, a.LastName, b.BookID, b.ReleaseDate
                      FROM dbo.Authors a 
                      LEFT JOIN dbo.Books b ON a.AuthorId = b.AuthorId 
                      WHERE a.LastName = @lname OR a.FirstName = @fname",
                    (author, book) =>
                    {
                        if (!authorDict.TryGetValue(author.AuthorId, out var currentAuthor))
                        {
                            currentAuthor = author;
                            currentAuthor.Books = new List<Book>();
                            authorDict.Add(currentAuthor.AuthorId, currentAuthor);
                        }

                        if (book != null)
                        {
                            currentAuthor.Books.Add(book);
                        }

                        return currentAuthor;
                    },
                    new { lname = lastName, fname = firstName },
                    splitOn: "BookID").Distinct().ToList();

                return authors;
            }
        }

        public Author GetAuthor(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var authorDict = new Dictionary<int, Author>();

                var authors = connection.Query<Author, Book, Author>(
                    @"SELECT a.AuthorID, a.DateofBirth, a.FirstName, a.LastName, b.BookID, b.ReleaseDate
                      FROM dbo.Authors a 
                      LEFT JOIN dbo.Books b ON a.AuthorId = b.AuthorId 
                      WHERE a.AuthorID = @authorId",
                    (author, book) =>
                    {
                        if (!authorDict.TryGetValue(author.AuthorId, out var currentAuthor))
                        {
                            currentAuthor = author;
                            currentAuthor.Books = new List<Book>();
                            authorDict.Add(currentAuthor.AuthorId, currentAuthor);
                        }

                        if (book != null)
                        {
                            currentAuthor.Books.Add(book);
                        }

                        return currentAuthor;
                    },
                    new { authorId = id },
                    splitOn: "BookID").FirstOrDefault();

                return authors;
            }
        }

        public Author DeleteAuthor(int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var author = connection.Query<Author>("DELETE FROM dbo.Authors WHERE AuthorID = @authorId",
                    new { authorId = id }).FirstOrDefault();
                return author;
            }
        }

        public Author UpdateAuthor(string firstName, string lastName, int id)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var author = connection.Query<Author>("UPDATE dbo.Authors SET FirstName = @fname, LastName = @lname WHERE AuthorID = @authorID",
                    new { lname = lastName, fname = firstName, authorID = id }).FirstOrDefault();
                return author;
            }
        }

        public Author UpdateAuthorByBookId(string firstName, string lastName, int bookid)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var author = connection.Query<Author>("UPDATE dbo.Authors SET FirstName = @fname, LastName = @lname WHERE AuthorID = (SELECT AuthorID FROM dbo.Books WHERE BookID = @bookId)",
                    new { lname = lastName, fname = firstName, bookID = bookid }).FirstOrDefault();
                return author;
            }
        }

        public Author InsertAuthor(string firstName, string lastName)
        {
            using (var connection = _dbConnectionHolder.GetConnection())
            {
                var author = connection.Query<Author>("INSERT INTO dbo.Authors (FirstName, LastName) VALUES (@fname, @lname)",
                    new { lname = lastName, fname = firstName }).FirstOrDefault();
                return author;
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
