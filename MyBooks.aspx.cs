
using System;
using System.Data;
using System.Data.SqlClient;
using BookStoreWebForms.Data;
using Microsoft.AspNet.Identity;

namespace BookStoreWebForms
{
    public partial class MyBooks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;
                MessageLabel.Text = "Access denied. Seller access is required.";
                return;
            }

            if (!IsPostBack)
            {
                LoadMyBooks();
            }
        }

        private void LoadMyBooks()
        {
            string sellerUserId = User.Identity.GetUserId();


            const string sql = @"
    SELECT
        b.BookId,
        b.Title,
        b.Author,
        c.CategoryName,
        b.Price,
        b.Stock,
        CASE
            WHEN b.IsActive = 1 THEN 'Active'
            ELSE 'Inactive'
        END AS Status
    FROM dbo.Books AS b
    LEFT JOIN dbo.Categories AS c
        ON b.CategoryId = c.CategoryId
    WHERE b.SellerUserId = @SellerUserId
    ORDER BY b.CreatedAt DESC";


            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@SellerUserId", SqlDbType.NVarChar, 128).Value =
                    sellerUserId;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    BooksGridView.DataSource = table;
                    BooksGridView.DataBind();
                }
            }
        }
    }
}
