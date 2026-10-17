
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BookStoreWebForms.Data;

namespace BookStoreWebForms
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCart();
            }
        }

        private Dictionary<int, int> GetCart()
        {
            var cart =
                Session["BookCart"] as Dictionary<int, int>;

            if (cart == null)
            {
                cart = new Dictionary<int, int>();
                Session["BookCart"] = cart;
            }

            return cart;
        }

        private void BindCart()
        {
            Dictionary<int, int> cart = GetCart();

            DataTable table = new DataTable();

            table.Columns.Add("BookId", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Author", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Stock", typeof(int));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Subtotal", typeof(decimal));

            decimal total = 0m;

            List<int> bookIds = new List<int>(cart.Keys);

            foreach (int bookId in bookIds)
            {
                const string sql = @"
                    SELECT BookId, Title, Author, Price, Stock
                    FROM dbo.Books
                    WHERE BookId = @BookId
                      AND IsActive = 1";

                using (SqlConnection connection = Db.GetConnection())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@BookId", SqlDbType.Int)
                        .Value = bookId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            cart.Remove(bookId);
                            continue;
                        }

                        int stock = Convert.ToInt32(reader["Stock"]);

                        if (stock <= 0)
                        {
                            cart.Remove(bookId);
                            continue;
                        }

                        int quantity = cart[bookId];

                        if (quantity > stock)
                        {
                            quantity = stock;
                            cart[bookId] = quantity;
                        }

                        decimal price = Convert.ToDecimal(reader["Price"]);
                        decimal subtotal = price * quantity;

                        DataRow row = table.NewRow();

                        row["BookId"] = Convert.ToInt32(reader["BookId"]);
                        row["Title"] = Convert.ToString(reader["Title"]);
                        row["Author"] = Convert.ToString(reader["Author"]);
                        row["Price"] = price;
                        row["Stock"] = stock;
                        row["Quantity"] = quantity;
                        row["Subtotal"] = subtotal;

                        table.Rows.Add(row);

                        total += subtotal;
                    }
                }
            }

            Session["BookCart"] = cart;

            CartRepeater.DataSource = table;
            CartRepeater.DataBind();

            CartTotalLabel.Text = total.ToString("N2");

            bool hasItems = table.Rows.Count > 0;

            CartContentsPanel.Visible = hasItems;
            EmptyCartPanel.Visible = !hasItems;
            CheckoutButton.Enabled = hasItems;
        }

        protected void CartRepeater_ItemCommand(
            object source,
            System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            int bookId;

            if (!int.TryParse(Convert.ToString(e.CommandArgument), out bookId)
                || bookId <= 0)
            {
                ShowCartMessage("Invalid book.", false);
                return;
            }

            Dictionary<int, int> cart = GetCart();

            if (!cart.ContainsKey(bookId))
            {
                BindCart();
                ShowCartMessage("This book is not in your cart.", false);
                return;
            }

            if (e.CommandName == "RemoveBook")
            {
                cart.Remove(bookId);
                Session["BookCart"] = cart;

                BindCart();
                ShowCartMessage("Book removed from your cart.", true);
                return;
            }

            if (e.CommandName == "UpdateQuantity")
            {
                var quantityBox =
                    e.Item.FindControl("QuantityTextBox")
                    as System.Web.UI.WebControls.TextBox;

                int quantity;

                if (quantityBox == null ||
                    !int.TryParse(quantityBox.Text, out quantity) ||
                    quantity < 1)
                {
                    BindCart();
                    ShowCartMessage(
                        "Enter a quantity of at least 1.", false);
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
                    command.Parameters.Add("@BookId", SqlDbType.Int)
                        .Value = bookId;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        cart.Remove(bookId);
                        Session["BookCart"] = cart;

                        BindCart();
                        ShowCartMessage(
                            "This book is no longer available.", false);
                        return;
                    }

                    int stock = Convert.ToInt32(result);

                    if (stock <= 0 || quantity > stock)
                    {
                        BindCart();
                        ShowCartMessage(
                            "Requested quantity exceeds available stock.",
                            false);
                        return;
                    }
                }

                cart[bookId] = quantity;
                Session["BookCart"] = cart;

                BindCart();
                ShowCartMessage("Cart quantity updated.", true);
            }
        }


        protected void CheckoutButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Checkout.aspx");
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
