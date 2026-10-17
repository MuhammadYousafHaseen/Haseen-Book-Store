
using System.Collections.Generic;
using System.Data.SqlClient;
using BookStoreWebForms.Models;

namespace BookStoreWebForms.Data
{
    public class CategoryService
    {
        public List<Category> GetActiveCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(
                @"SELECT CategoryId, CategoryName, Description, IsActive
                  FROM dbo.Categories
                  WHERE IsActive = 1
                  ORDER BY CategoryName;", connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = reader.GetInt32(0),
                            CategoryName = reader.GetString(1),
                            Description = reader.IsDBNull(2)
                                ? null : reader.GetString(2),
                            IsActive = reader.GetBoolean(3)
                        });
                    }
                }
            }

            return categories;
        }
    }
}
