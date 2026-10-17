
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="EditBook.aspx.cs"
    Inherits="BookStoreWebForms.EditBook" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Edit Book - BookStore</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">
            <div class="card shadow-sm mx-auto" style="max-width: 750px;">
                <div class="card-body p-4">
                    <h1 class="h3 mb-4">Edit Book / Update Stock</h1>

                    <asp:Label ID="MessageLabel" runat="server"
                        EnableViewState="false" CssClass="d-block mb-3" />

                    <div class="mb-3">
                        <label class="form-label">Category</label>
                        <asp:DropDownList ID="CategoryDropDown" runat="server"
                            CssClass="form-select" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Title</label>
                        <asp:TextBox ID="TitleTextBox" runat="server"
                            CssClass="form-control" MaxLength="200" />
                        <asp:RequiredFieldValidator runat="server"
                            ControlToValidate="TitleTextBox"
                            ErrorMessage="Title is required."
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Author</label>
                        <asp:TextBox ID="AuthorTextBox" runat="server"
                            CssClass="form-control" MaxLength="150" />
                        <asp:RequiredFieldValidator runat="server"
                            ControlToValidate="AuthorTextBox"
                            ErrorMessage="Author is required."
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Description</label>
                        <asp:TextBox ID="DescriptionTextBox" runat="server"
                            CssClass="form-control" TextMode="MultiLine"
                            Rows="4" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Price</label>
                        <asp:TextBox ID="PriceTextBox" runat="server"
                            CssClass="form-control" TextMode="Number" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Stock Quantity</label>
                        <asp:TextBox ID="StockTextBox" runat="server"
                            CssClass="form-control" TextMode="Number" />
                    </div>

                    <div class="form-check mb-4">
                        <asp:CheckBox ID="IsActiveCheckBox" runat="server"
                            CssClass="form-check-input" />
                        <label class="form-check-label"
                            for="<%= IsActiveCheckBox.ClientID %>">
                            Book is active and available to customers
                        </label>
                    </div>

                    <asp:Button ID="UpdateBookButton" runat="server"
                        Text="Save Changes" CssClass="btn btn-primary"
                        OnClick="UpdateBookButton_Click" />

                    <a href="MyBooks.aspx"
                       class="btn btn-outline-secondary ms-2">
                        Back to My Books
                    </a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
