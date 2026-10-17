
using System;
using System.Data;
using System.Data.SqlClient;
using BookStoreWebForms.Data;
using Microsoft.AspNet.Identity;

namespace BookStoreWebForms
{
    public partial class AddBook : System.Web.UI.Page
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
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(
                @"SELECT CategoryId, CategoryName
                  FROM dbo.Categories
                  WHERE IsActive = 1
                  ORDER BY CategoryName", connection))
            {
                connection.Open();

                CategoryDropDown.DataSource = command.ExecuteReader();
                CategoryDropDown.DataTextField = "CategoryName";
                CategoryDropDown.DataValueField = "CategoryId";
                CategoryDropDown.DataBind();
            }

            CategoryDropDown.Items.Insert(
                0, new System.Web.UI.WebControls.ListItem(
                    "-- Select Category --", ""));
        }

        protected void SaveBookButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated ||
                !User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                return;
            }

            if (!Page.IsValid)
                return;

            int categoryId;
            decimal price;
            int stock;

            if (!int.TryParse(CategoryDropDown.SelectedValue, out categoryId))
            {
                ShowMessage("Please select a category.", false);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out price) || price <= 0)
            {
                ShowMessage("Enter a valid price greater than zero.", false);
                return;
            }

            if (!int.TryParse(StockTextBox.Text, out stock) || stock < 0)
            {
                ShowMessage("Stock must be zero or a positive whole number.", false);
                return;
            }

            string sellerUserId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(sellerUserId))
            {
                ShowMessage("Unable to identify your account. Please sign in again.", false);
                return;
            }

            const string sql = @"
                INSERT INTO dbo.Books
                    (CategoryId, Title, Author, Description, Price,
                     Stock, SellerUserId, IsActive, CreatedAt)
                VALUES
                    (@CategoryId, @Title, @Author, @Description, @Price,
                     @Stock, @SellerUserId, 1, GETDATE())";

            try
            {
                using (SqlConnection connection = Db.GetConnection())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(
                        "@CategoryId", SqlDbType.Int).Value = categoryId;

                    command.Parameters.Add(
                        "@Title", SqlDbType.NVarChar, 200).Value =
                        TitleTextBox.Text.Trim();

                    command.Parameters.Add(
                        "@Author", SqlDbType.NVarChar, 150).Value =
                        AuthorTextBox.Text.Trim();

                    command.Parameters.Add(
                        "@Description", SqlDbType.NVarChar, -1).Value =
                        string.IsNullOrWhiteSpace(DescriptionTextBox.Text)
                            ? (object)DBNull.Value
                            : DescriptionTextBox.Text.Trim();

                    command.Parameters.Add(
                        "@Price", SqlDbType.Decimal).Value = price;
                    command.Parameters["@Price"].Precision = 18;
                    command.Parameters["@Price"].Scale = 2;

                    command.Parameters.Add(
                        "@Stock", SqlDbType.Int).Value = stock;

                    command.Parameters.Add(
                        "@SellerUserId", SqlDbType.NVarChar, 128).Value =
                        sellerUserId;

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ShowMessage("Book added successfully!", true);

                TitleTextBox.Text = "";
                AuthorTextBox.Text = "";
                DescriptionTextBox.Text = "";
                PriceTextBox.Text = "";
                StockTextBox.Text = "";
                CategoryDropDown.SelectedIndex = 0;
            }
            catch (SqlException)
            {
                ShowMessage(
                    "The book could not be saved. Please check the entered data and try again.",
                    false);
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
