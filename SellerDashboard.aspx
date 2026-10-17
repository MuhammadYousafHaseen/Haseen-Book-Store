
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="SellerDashboard.aspx.cs"
    Inherits="BookStoreWebForms.SellerDashboard" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Seller Dashboard - BookStore</title>

    <link
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css"
        rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">
            <div class="card shadow-sm mx-auto" style="max-width: 750px;">
                <div class="card-body p-4">
                    <h1 class="h3 mb-3">Seller Dashboard</h1>

                    <asp:Label
                        ID="MessageLabel"
                        runat="server"
                        CssClass="d-block"
                        EnableViewState="false" />
                    <a href="AddBook.aspx" class="btn btn-primary mt-3 me-2">
    Add New Book
</a>

<a href="MyBooks.aspx" class="btn btn-outline-primary mt-3 me-2">
    My Books
</a>
                     <a href="SellerPayments.aspx" class="btn btn-outline-primary">
    Manage Payments
</a>

                    <a href="Books.aspx" class="btn btn-outline-primary mt-3">
                        Browse Books
                    </a>

                    <a href="MyAccount.aspx" class="btn btn-secondary mt-3">
                        My Account
                    </a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
