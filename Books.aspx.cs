
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BookStoreWebForms.Data;

namespace BookStoreWebForms
{
    public partial class Books : System.Web.UI.Page
    {
        private Dictionary<int, int> GetCart()
        {
            var cart = Session["BookCart"] as Dictionary<int, int>;

            if (cart == null)
            {
                cart = new Dictionary<int, int>();
                Session["BookCart"] = cart;
            }

            return cart;
        }

        private void BindBooks()
        {
            int parsedCategoryId;

            int? categoryId = int.TryParse(
                CategoryFilter.SelectedValue,
                out parsedCategoryId)
                ? parsedCategoryId
                : (int?)null;

            BookRepeater.DataSource =
                new BookService().GetActiveBooks(
                    SearchBox.Text.Trim(),
                    categoryId);

            BookRepeater.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryFilter.DataSource =
                    new CategoryService().GetActiveCategories();

                CategoryFilter.DataTextField = "CategoryName";
                CategoryFilter.DataValueField = "CategoryId";
                CategoryFilter.DataBind();

                CategoryFilter.Items.Insert(
                    0,
                    new System.Web.UI.WebControls.ListItem(
                        "All categories", ""));

                BindBooks();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            BindBooks();
        }

        protected void BookRepeater_ItemCommand(
            object source,
            System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "AddToCart")
                return;

            int bookId;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out bookId) || bookId <= 0)
            {
                ShowCartMessage("Invalid book.", false);
                return;
            }

            const string sql = @"
                SELECT Stock
                FROM dbo.Books
                WHERE BookId = @BookId
                  AND IsActive = 1";

            using (SqlConnection connection = Db.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@BookId", SqlDbType.Int).Value =
                    bookId;

                connection.Open();

                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    ShowCartMessage(
                        "This book is no longer available.", false);
                    BindBooks();
                    return;
                }

                int stock = Convert.ToInt32(result);
                Dictionary<int, int> cart = GetCart();

                int currentQuantity = cart.ContainsKey(bookId)
                    ? cart[bookId]
                    : 0;

                if (stock <= 0)
                {
                    ShowCartMessage(
                        "This book is out of stock.", false);
                    BindBooks();
                    return;
                }

                if (currentQuantity >= stock)
                {
                    ShowCartMessage(
                        "Your cart already contains all available stock.",
                        false);
                    BindBooks();
                    return;
                }

                cart[bookId] = currentQuantity + 1;
                Session["BookCart"] = cart;
            }

            ShowCartMessage("Book added to your cart.", true);
            BindBooks();
        }


        private void ShowCartMessage(string message, bool success)
        {
            CartAlert.Attributes["class"] = success
                ? "alert alert-success alert-dismissible fade show"
                : "alert alert-danger alert-dismissible fade show";

            CartMessageLabel.Text = Server.HtmlEncode(message);
        }

    }
}
