
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BookStoreWebForms.Models;

namespace BookStoreWebForms.Data
{
    public class BookService
    {
        public List<Book> GetActiveBooks(string search, int? categoryId)
        {
            var books = new List<Book>();

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(@"
                SELECT b.BookId, b.CategoryId, c.CategoryName,
                       b.Title, b.Author, b.Description, b.Price,
                       b.Stock, b.ImagePath, b.SellerUserId, b.IsActive
                FROM dbo.Books b
                INNER JOIN dbo.Categories c
                    ON c.CategoryId = b.CategoryId
                WHERE b.IsActive = 1
                  AND c.IsActive = 1
                  AND (@Search = ''
                       OR b.Title LIKE '%' + @Search + '%'
                       OR b.Author LIKE '%' + @Search + '%')
                  AND (@CategoryId IS NULL
                       OR b.CategoryId = @CategoryId)
                ORDER BY b.Title;", connection))
            {
                command.Parameters.Add("@Search", SqlDbType.NVarChar, 200)
                    .Value = (search ?? "").Trim();

                command.Parameters.Add("@CategoryId", SqlDbType.Int)
                    .Value = categoryId.HasValue
                        ? (object)categoryId.Value
                        : DBNull.Value;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            BookId = reader.GetInt32(0),
                            CategoryId = reader.GetInt32(1),
                            CategoryName = reader.GetString(2),
                            Title = reader.GetString(3),
                            Author = reader.GetString(4),
                            Description = reader.IsDBNull(5)
                                ? null : reader.GetString(5),
                            Price = reader.GetDecimal(6),
                            Stock = reader.GetInt32(7),
                            ImagePath = reader.IsDBNull(8)
                                ? null : reader.GetString(8),
                            SellerUserId = reader.IsDBNull(9)
                                ? null : reader.GetString(9),
                            IsActive = reader.GetBoolean(10)
                        });
                    }
                }
            }

            return books;
        }
    }
}
