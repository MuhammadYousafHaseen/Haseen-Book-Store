
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MyBooks.aspx.cs"
    Inherits="BookStoreWebForms.MyBooks" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>My Books - BookStore</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">
            <h1 class="h3 mb-4">My Books</h1>

            <asp:Label ID="MessageLabel" runat="server"
                CssClass="d-block mb-3" />

            <div class="mb-3">
                <a href="AddBook.aspx" class="btn btn-primary">Add New Book</a>
                <a href="SellerDashboard.aspx"
                   class="btn btn-outline-secondary ms-2">Dashboard</a>
            </div>

            <div class="table-responsive bg-white p-3 rounded shadow-sm">
                <asp:GridView ID="BooksGridView" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-hover"
                    GridLines="None"
                    EmptyDataText="You have not added any books yet.">
                   
<Columns>
    <asp:BoundField DataField="Title" HeaderText="Title" />
    <asp:BoundField DataField="Author" HeaderText="Author" />
    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
    <asp:BoundField DataField="Price"
        HeaderText="Price" DataFormatString="{0:N2}" />
    <asp:BoundField DataField="Stock" HeaderText="Stock" />
    <asp:BoundField DataField="Status" HeaderText="Status" />

    <asp:HyperLinkField
        Text="Edit / Update Stock"
        HeaderText="Actions"
        DataNavigateUrlFields="BookId"
        DataNavigateUrlFormatString="EditBook.aspx?BookId={0}"
        ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
</Columns>

                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
