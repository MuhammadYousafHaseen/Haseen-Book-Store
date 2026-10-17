
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BookStoreWebForms.Data;
using Microsoft.AspNet.Identity;

namespace BookStoreWebForms
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/SignIn.aspx");
                return;
            }

            if (!User.IsInRole("Customer") &&
                !User.IsInRole("Seller"))
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;
                CheckoutPanel.Visible = false;

                ShowMessage(
                    "You must have a Customer or Seller account to place an order.");

                return;
            }

            if (!IsPostBack)
            {
                BindSummary();
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

        private void BindSummary()
        {
            Dictionary<int, int> cart = GetCart();
            DataTable table = new DataTable();

            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Subtotal", typeof(decimal));

            decimal total = 0m;
            List<int> bookIds = new List<int>(cart.Keys);

            foreach (int bookId in bookIds)
            {
                const string sql = @"
                    SELECT Title, Price, Stock
                    FROM dbo.Books
                    WHERE BookId = @BookId
                      AND IsActive = 1
                      AND SellerUserId IS NOT NULL";

                using (SqlConnection connection = Db.GetConnection())
                using (SqlCommand command =
                    new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(
                        "@BookId", SqlDbType.Int).Value = bookId;

                    connection.Open();

                    using (SqlDataReader reader =
                        command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            cart.Remove(bookId);
                            continue;
                        }

                        int stock =
                            Convert.ToInt32(reader["Stock"]);

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

                        decimal price =
                            Convert.ToDecimal(reader["Price"]);

                        decimal subtotal = price * quantity;

                        DataRow row = table.NewRow();
                        row["Title"] =
                            Convert.ToString(reader["Title"]);
                        row["Quantity"] = quantity;
                        row["Subtotal"] = subtotal;

                        table.Rows.Add(row);
                        total += subtotal;
                    }
                }
            }

            Session["BookCart"] = cart;

            CheckoutRepeater.DataSource = table;
            CheckoutRepeater.DataBind();

            TotalLabel.Text = total.ToString("N2");

            bool hasItems = table.Rows.Count > 0;

            CheckoutPanel.Visible = hasItems;
            PlaceOrderButton.Enabled = hasItems;

            if (!hasItems)
            {
                ShowMessage(
                    "Your cart is empty or contains no available books with an assigned seller. Please add available books first.");
            }
        }

        protected void PlaceOrderButton_Click(
            object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated ||
                (!User.IsInRole("Customer") &&
                 !User.IsInRole("Seller")))
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;

                CheckoutPanel.Visible = false;
                ShowMessage(
                    "You are not authorized to place orders.");

                return;
            }

            Page.Validate();

            if (!Page.IsValid)
                return;

            string userId = User.Identity.GetUserId();
            string address =
                ShippingAddressTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(address) ||
                address.Length > 500)
            {
                ShowMessage("Enter a valid shipping address.");
                return;
            }

            Dictionary<int, int> cart = GetCart();

            if (cart.Count == 0)
            {
                BindSummary();
                ShowMessage("Your cart is empty.");
                return;
            }

            int orderId = 0;

            try
            {
                using (SqlConnection connection =
                    Db.GetConnection())
                {
                    connection.Open();

                    using (SqlTransaction transaction =
                        connection.BeginTransaction(
                            IsolationLevel.Serializable))
                    {
                        try
                        {
                            decimal total = 0m;

                            // Each item includes its assigned seller.
                            var items =
                                new List<Tuple<int, int, decimal, string>>();

                            foreach (
                                KeyValuePair<int, int> cartItem in cart)
                            {
                                int bookId = cartItem.Key;
                                int quantity = cartItem.Value;

                                if (quantity <= 0)
                                {
                                    throw new InvalidOperationException(
                                        "The cart contains an invalid quantity.");
                                }

                                const string bookSql = @"
                                    SELECT
                                        BookId,
                                        Price,
                                        Stock,
                                        SellerUserId
                                    FROM dbo.Books
                                        WITH (UPDLOCK, HOLDLOCK)
                                    WHERE BookId = @BookId
                                      AND IsActive = 1";

                                using (SqlCommand command =
                                    new SqlCommand(
                                        bookSql,
                                        connection,
                                        transaction))
                                {
                                    command.Parameters.Add(
                                        "@BookId", SqlDbType.Int
                                    ).Value = bookId;

                                    using (SqlDataReader reader =
                                        command.ExecuteReader())
                                    {
                                        if (!reader.Read())
                                        {
                                            throw new InvalidOperationException(
                                                "A book in your cart is no longer available.");
                                        }

                                        if (reader["SellerUserId"] ==
                                            DBNull.Value)
                                        {
                                            throw new InvalidOperationException(
                                                "A book in your cart has no assigned seller. Please remove it from your cart.");
                                        }

                                        string sellerId =
                                            Convert.ToString(
                                                reader["SellerUserId"]);

                                        if (string.Equals(
                                            sellerId,
                                            userId,
                                            StringComparison.Ordinal))
                                        {
                                            throw new InvalidOperationException(
                                                "You cannot purchase your own listed book. Remove it from your cart to continue.");
                                        }

                                        int stock =
                                            Convert.ToInt32(
                                                reader["Stock"]);

                                        if (stock < quantity)
                                        {
                                            throw new InvalidOperationException(
                                                "There is not enough stock for a book in your cart. Please update your cart.");
                                        }

                                        decimal price =
                                            Convert.ToDecimal(
                                                reader["Price"]);

                                        items.Add(
                                            Tuple.Create(
                                                bookId,
                                                quantity,
                                                price,
                                                sellerId));

                                        total += price * quantity;
                                    }
                                }
                            }

                            if (items.Count == 0 || total <= 0m)
                            {
                                throw new InvalidOperationException(
                                    "Your cart does not contain any purchasable books.");
                            }

                            // Create the customer's main order.
                            const string orderSql = @"
                                INSERT INTO dbo.Orders
                                    (
                                        CustomerUserId,
                                        OrderDate,
                                        TotalAmount,
                                        ShippingAddress,
                                        OrderStatus
                                    )
                                VALUES
                                    (
                                        @UserId,
                                        GETDATE(),
                                        @Total,
                                        @Address,
                                        @Status
                                    );

                                SELECT CAST(SCOPE_IDENTITY() AS int);";

                            using (SqlCommand command =
                                new SqlCommand(
                                    orderSql,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.Add(
                                    "@UserId",
                                    SqlDbType.NVarChar,
                                    128).Value = userId;

                                SqlParameter totalParameter =
                                    command.Parameters.Add(
                                        "@Total",
                                        SqlDbType.Decimal);

                                totalParameter.Precision = 18;
                                totalParameter.Scale = 2;
                                totalParameter.Value = total;

                                command.Parameters.Add(
                                    "@Address",
                                    SqlDbType.NVarChar,
                                    500).Value = address;

                                command.Parameters.Add(
                                    "@Status",
                                    SqlDbType.NVarChar,
                                    30).Value = "Pending";

                                orderId = Convert.ToInt32(
                                    command.ExecuteScalar());
                            }

                            // Maps each seller to their fulfillment ID
                            // for this customer order.
                            var fulfillmentIds =
                                new Dictionary<string, int>(
                                    StringComparer.Ordinal);
                            var sellerPaymentAmounts =
    new Dictionary<string, decimal>(
        StringComparer.Ordinal);

                            foreach (
                                Tuple<int, int, decimal, string> item
                                in items)
                            {
                                int bookId = item.Item1;
                                int quantity = item.Item2;
                                decimal price = item.Item3;
                                string sellerId = item.Item4;

                                // Create the order item and obtain its ID.
                                const string itemSql = @"
                                    INSERT INTO dbo.OrderItems
                                        (
                                            OrderId,
                                            BookId,
                                            Quantity,
                                            UnitPrice
                                        )
                                    VALUES
                                        (
                                            @OrderId,
                                            @BookId,
                                            @Quantity,
                                            @UnitPrice
                                        );

                                    SELECT CAST(
                                        SCOPE_IDENTITY() AS int);";

                                int orderItemId;

                                using (SqlCommand command =
                                    new SqlCommand(
                                        itemSql,
                                        connection,
                                        transaction))
                                {
                                    command.Parameters.Add(
                                        "@OrderId",
                                        SqlDbType.Int).Value = orderId;

                                    command.Parameters.Add(
                                        "@BookId",
                                        SqlDbType.Int).Value = bookId;

                                    command.Parameters.Add(
                                        "@Quantity",
                                        SqlDbType.Int).Value = quantity;

                                    SqlParameter priceParameter =
                                        command.Parameters.Add(
                                            "@UnitPrice",
                                            SqlDbType.Decimal);

                                    priceParameter.Precision = 18;
                                    priceParameter.Scale = 2;
                                    priceParameter.Value = price;

                                    orderItemId = Convert.ToInt32(
                                        command.ExecuteScalar());
                                }

                                int fulfillmentId;
                                decimal currentSellerAmount;

                                if (!sellerPaymentAmounts.TryGetValue(
                                    sellerId, out currentSellerAmount))
                                {
                                    currentSellerAmount = 0m;
                                }

                                sellerPaymentAmounts[sellerId] =
                                    currentSellerAmount + (price * quantity);
                                // Reuse the seller's fulfillment if it
                                // already exists for this order.
                                if (!fulfillmentIds.TryGetValue(
                                    sellerId, out fulfillmentId))
                                {
                                    const string fulfillmentSql = @"
                                        INSERT INTO
                                            dbo.SellerOrderFulfillments
                                            (
                                                OrderId,
                                                SellerUserId,
                                                FulfillmentStatus,
                                                CreatedAt,
                                                UpdatedAt
                                            )
                                        VALUES
                                            (
                                                @OrderId,
                                                @SellerId,
                                                @Status,
                                                SYSDATETIME(),
                                                SYSDATETIME()
                                            );

                                        SELECT CAST(
                                            SCOPE_IDENTITY() AS int);";

                                    using (SqlCommand command =
                                        new SqlCommand(
                                            fulfillmentSql,
                                            connection,
                                            transaction))
                                    {
                                        command.Parameters.Add(
                                            "@OrderId",
                                            SqlDbType.Int).Value =
                                            orderId;

                                        command.Parameters.Add(
                                            "@SellerId",
                                            SqlDbType.NVarChar,
                                            128).Value = sellerId;

                                        command.Parameters.Add(
                                            "@Status",
                                            SqlDbType.NVarChar,
                                            30).Value = "Pending";

                                        fulfillmentId =
                                            Convert.ToInt32(
                                                command.ExecuteScalar());
                                    }

                                    fulfillmentIds.Add(
                                        sellerId, fulfillmentId);
                                }

                                // Link this order item to its seller.
                                const string linkSql = @"
                                    INSERT INTO
                                        dbo.SellerOrderFulfillmentItems
                                        (
                                            FulfillmentId,
                                            OrderItemId
                                        )
                                    VALUES
                                        (
                                            @FulfillmentId,
                                            @OrderItemId
                                        );";

                                using (SqlCommand command =
                                    new SqlCommand(
                                        linkSql,
                                        connection,
                                        transaction))
                                {
                                    command.Parameters.Add(
                                        "@FulfillmentId",
                                        SqlDbType.Int).Value =
                                        fulfillmentId;

                                    command.Parameters.Add(
                                        "@OrderItemId",
                                        SqlDbType.Int).Value =
                                        orderItemId;

                                    command.ExecuteNonQuery();
                                }

                                // Deduct stock within the same transaction.
                                const string stockSql = @"
                                    UPDATE dbo.Books
                                    SET Stock = Stock - @Quantity
                                    WHERE BookId = @BookId
                                      AND IsActive = 1
                                      AND SellerUserId = @SellerId
                                      AND Stock >= @Quantity";

                                using (SqlCommand command =
                                    new SqlCommand(
                                        stockSql,
                                        connection,
                                        transaction))
                                {
                                    command.Parameters.Add(
                                        "@Quantity",
                                        SqlDbType.Int).Value =
                                        quantity;

                                    command.Parameters.Add(
                                        "@BookId",
                                        SqlDbType.Int).Value =
                                        bookId;

                                    command.Parameters.Add(
                                        "@SellerId",
                                        SqlDbType.NVarChar,
                                        128).Value = sellerId;

                                    if (command.ExecuteNonQuery() != 1)
                                    {
                                        throw new InvalidOperationException(
                                            "Stock changed during checkout. Please try again.");
                                    }
                                }
                            }


                            // One payment record for the whole order.

                            // Create one payment record for each seller.
                            foreach (
                                KeyValuePair<string, int> fulfillmentEntry
                                in fulfillmentIds)
                            {
                                string sellerIdForPayment =
                                    fulfillmentEntry.Key;

                                int fulfillmentIdForPayment =
                                    fulfillmentEntry.Value;

                                decimal sellerAmount =
                                    sellerPaymentAmounts[
                                        sellerIdForPayment];

                                const string sellerPaymentSql = @"
                                    INSERT INTO dbo.SellerFulfillmentPayments
                                    (
                                        FulfillmentId,
                                        PaymentMethod,
                                        PaymentStatus,
                                        Amount,
                                        TransactionReference,
                                        UpdatedAt
                                    )
                                    VALUES
                                    (
                                        @FulfillmentId,
                                        @Method,
                                        @Status,
                                        @Amount,
                                        NULL,
                                        SYSDATETIME()
                                    );";

                                using (SqlCommand command =
                                    new SqlCommand(
                                        sellerPaymentSql,
                                        connection,
                                        transaction))
                                {
                                    command.Parameters.Add(
                                        "@FulfillmentId",
                                        SqlDbType.Int).Value =
                                        fulfillmentIdForPayment;

                                    command.Parameters.Add(
                                        "@Method",
                                        SqlDbType.NVarChar,
                                        50).Value = "CashOnDelivery";

                                    command.Parameters.Add(
                                        "@Status",
                                        SqlDbType.NVarChar,
                                        30).Value = "Pending";

                                    SqlParameter sellerAmountParameter =
                                        command.Parameters.Add(
                                            "@Amount",
                                            SqlDbType.Decimal);

                                    sellerAmountParameter.Precision = 18;
                                    sellerAmountParameter.Scale = 2;
                                    sellerAmountParameter.Value = sellerAmount;

                                    command.ExecuteNonQuery();
                                }
                            }

                            const string paymentSql = @"
                                INSERT INTO dbo.Payments
                                    (
                                        OrderId,
                                        PaymentMethod,
                                        PaymentStatus,
                                        Amount,
                                        TransactionReference,
                                        UpdatedAt
                                    )
                                VALUES
                                    (
                                        @OrderId,
                                        @Method,
                                        @Status,
                                        @Amount,
                                        NULL,
                                        GETDATE()
                                    );";

                            using (SqlCommand command =
                                new SqlCommand(
                                    paymentSql,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.Add(
                                    "@OrderId",
                                    SqlDbType.Int).Value = orderId;

                                command.Parameters.Add(
                                    "@Method",
                                    SqlDbType.NVarChar,
                                    50).Value = "CashOnDelivery";

                                command.Parameters.Add(
                                    "@Status",
                                    SqlDbType.NVarChar,
                                    30).Value = "Pending";

                                SqlParameter amountParameter =
                                    command.Parameters.Add(
                                        "@Amount",
                                        SqlDbType.Decimal);

                                amountParameter.Precision = 18;
                                amountParameter.Scale = 2;
                                amountParameter.Value = total;

                                command.ExecuteNonQuery();
                            }

                            // Everything succeeds together or rolls back.
                            transaction.Commit();
                        }
                        catch
                        {
                            try
                            {
                                transaction.Rollback();
                            }
                            catch
                            {
                                // Preserve the original exception.
                            }

                            throw;
                        }
                    }
                }

                // Clear the cart only after the transaction succeeds.
                Session["BookCart"] =
                    new Dictionary<int, int>();

                CheckoutPanel.Visible = false;
                ConfirmationPanel.Visible = true;
                OrderNumberLabel.Text = orderId.ToString();
            }
            catch (InvalidOperationException ex)
            {
                BindSummary();
                ShowMessage(ex.Message);
            }
            catch (Exception)
            {
                BindSummary();

                ShowMessage(
                    "We could not place your order. No order was completed. Please check your cart and try again.");
            }
        }

        private void ShowMessage(string message)
        {
            CheckoutAlert.Attributes["class"] =
                "alert alert-danger alert-dismissible fade show";

            MessageLabel.Text = Server.HtmlEncode(message);
        }
    }
}
