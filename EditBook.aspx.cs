
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using BookStoreWebForms.Data;
using Microsoft.AspNet.Identity;

namespace BookStoreWebForms
{
    public partial class EditBook : System.Web.UI.Page
    {
        private int BookId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["BookId"], out id)
                    ? id
                    : 0;
            }
        }

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
                ShowMessage("Access denied. Seller access is required.", false);
                UpdateBookButton.Enabled = false;
                return;
            }

            if (BookId <= 0)
            {
                ShowMessage("Invalid book ID.", false);
                UpdateBookButton.Enabled = false;
                return;
            }

            if (!IsPostBack)
            {
                LoadCategories();
                LoadBook();
            }
        }

        private void LoadCategories()
        {
            const string sql = @"
                SELECT CategoryId, CategoryName
                FROM dbo.Categories
                WHERE IsActive = 1
                ORDER BY CategoryName";

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    CategoryDropDown.DataSource = reader;
                    CategoryDropDown.DataTextField = "CategoryName";
                    CategoryDropDown.DataValueField = "CategoryId";
                    CategoryDropDown.DataBind();
                }
            }
        }

        private void LoadBook()
        {
            const string sql = @"
                SELECT CategoryId, Title, Author, Description,
                       Price, Stock, IsActive
                FROM dbo.Books
                WHERE BookId = @BookId
                  AND SellerUserId = @SellerUserId";

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@BookId", SqlDbType.Int).Value = BookId;
                command.Parameters.Add("@SellerUserId", SqlDbType.NVarChar, 128)
                    .Value = User.Identity.GetUserId();

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        ShowMessage(
                            "Book not found or it does not belong to your account.",
                            false);

                        UpdateBookButton.Enabled = false;
                        return;
                    }

                    TitleTextBox.Text = Convert.ToString(reader["Title"]);
                    AuthorTextBox.Text = Convert.ToString(reader["Author"]);

                    DescriptionTextBox.Text =
                        reader["Description"] == DBNull.Value
                            ? ""
                            : Convert.ToString(reader["Description"]);

                    PriceTextBox.Text = Convert.ToDecimal(reader["Price"])
                        .ToString(CultureInfo.InvariantCulture);

                    StockTextBox.Text = Convert.ToString(reader["Stock"]);

                    string categoryId = Convert.ToString(reader["CategoryId"]);

                    if (CategoryDropDown.Items.FindByValue(categoryId) != null)
                    {
                        CategoryDropDown.SelectedValue = categoryId;
                    }

                    IsActiveCheckBox.Checked =
                        Convert.ToBoolean(reader["IsActive"]);
                }
            }
        }

        protected void UpdateBookButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                return;
            }

            if (!Page.IsValid)
                return;

            int categoryId;
            int stock;
            decimal price;

            if (BookId <= 0 ||
                !int.TryParse(CategoryDropDown.SelectedValue, out categoryId))
            {
                ShowMessage("Please select a valid category.", false);
                return;
            }

            if (!decimal.TryParse(
                PriceTextBox.Text,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out price) || price <= 0)
            {
                ShowMessage("Enter a valid price greater than zero.", false);
                return;
            }

            if (!int.TryParse(StockTextBox.Text, out stock) || stock < 0)
            {
                ShowMessage(
                    "Stock must be zero or a positive whole number.", false);
                return;
            }

            const string sql = @"
                UPDATE dbo.Books
                SET CategoryId = @CategoryId,
                    Title = @Title,
                    Author = @Author,
                    Description = @Description,
                    Price = @Price,
                    Stock = @Stock,
                    IsActive = @IsActive
                WHERE BookId = @BookId
                  AND SellerUserId = @SellerUserId";

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@CategoryId", SqlDbType.Int)
                    .Value = categoryId;

                command.Parameters.Add("@Title", SqlDbType.NVarChar, 200)
                    .Value = TitleTextBox.Text.Trim();

                command.Parameters.Add("@Author", SqlDbType.NVarChar, 150)
                    .Value = AuthorTextBox.Text.Trim();

                command.Parameters.Add("@Description", SqlDbType.NVarChar, -1)
                    .Value = string.IsNullOrWhiteSpace(DescriptionTextBox.Text)
                        ? (object)DBNull.Value
                        : DescriptionTextBox.Text.Trim();

                SqlParameter priceParameter =
                    command.Parameters.Add("@Price", SqlDbType.Decimal);

                priceParameter.Precision = 18;
                priceParameter.Scale = 2;
                priceParameter.Value = price;

                command.Parameters.Add("@Stock", SqlDbType.Int).Value = stock;

                command.Parameters.Add("@IsActive", SqlDbType.Bit)
                    .Value = IsActiveCheckBox.Checked;

                command.Parameters.Add("@BookId", SqlDbType.Int).Value = BookId;

                command.Parameters.Add("@SellerUserId", SqlDbType.NVarChar, 128)
                    .Value = User.Identity.GetUserId();

                connection.Open();

                int rowsUpdated = command.ExecuteNonQuery();

                if (rowsUpdated == 1)
                {
                    ShowMessage("Book updated successfully.", true);
                }
                else
                {
                    ShowMessage(
                        "Book not found or you do not own it.", false);
                }
            }
        }

        private void ShowMessage(string message, bool success)
        {
            MessageLabel.CssClass = success
                ? "alert alert-success d-block"
                : "alert alert-danger d-block";

            MessageLabel.Text = Server.HtmlEncode(message);
        }
    }
}
