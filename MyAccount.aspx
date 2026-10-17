
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MyAccount.aspx.cs"
    Inherits="BookStoreWebForms.MyAccount" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>My Account - BookStore</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand bg-white border-bottom">
            <div class="container">
                <a class="navbar-brand" href="Default.aspx">BookStore</a>
                <a class="btn btn-outline-danger" href="SignOut.aspx">
                    Sign Out
                </a>
            </div>
        </nav>

        <main class="container py-5">
            <div class="card shadow-sm">
                <div class="card-body p-4">
                    <h2>My Account</h2>
                    <p>Welcome, <asp:Label ID="FullNameLabel"
                        runat="server" />!</p>
                    <p>Email: <asp:Label ID="EmailLabel"
                        runat="server" /></p>

                    <hr />
                    <p>Your order history and account features will
                   <div class="d-flex flex-wrap gap-2">
    <a href="Books.aspx" class="btn btn-primary">
        Browse Books
    </a>

    <a href="Cart.aspx" class="btn btn-outline-primary">
        Shopping Cart
    </a>

    <a href="MyOrders.aspx" class="btn btn-outline-primary">
        My Orders
    </a>

    <a href="BecomeSeller.aspx" class="btn btn-outline-success">
        Register as a Seller
    </a>
</div>
                </div>
            </div>
        </main>
    </form>
</body>
</html>
