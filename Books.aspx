
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Books.aspx.cs"
    Inherits="BookStoreWebForms.Books" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Book Store | Browse Books</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">
<form id="form1" runat="server">
   
<nav class="navbar navbar-expand-lg bg-white border-bottom shadow-sm">
    <div class="container">
        <a class="navbar-brand fw-bold text-primary" href="Books.aspx">
            BookStore
        </a>

        <button class="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#bookStoreNavbar"
            aria-controls="bookStoreNavbar"
            aria-expanded="false"
            aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="bookStoreNavbar">
            <div class="navbar-nav ms-auto align-items-lg-center gap-lg-2">
                <a class="nav-link" href="Books.aspx">
                    Browse Books
                </a>

                <a class="nav-link" href="Categories.aspx">
                    Categories
                </a>

                <a class="nav-link" href="Cart.aspx">
                    Shopping Cart
                </a>

                <a class="btn btn-primary btn-sm px-3"
                    href="MyOrders.aspx">
                    My Orders
                </a>
            </div>
        </div>
    </div>
</nav>


    <main class="container py-5">
        <h1 class="mb-2">Discover Your Next Book</h1>
        <p class="text-muted mb-4">Browse our book collection.</p>

        <div class="row g-2 mb-4">
            <div class="col-12 col-md-6">
                <asp:TextBox ID="SearchBox" runat="server"
                    CssClass="form-control"
                    placeholder="Search by title or author" />
            </div>
            <div class="col-12 col-md-4">
                <asp:DropDownList ID="CategoryFilter" runat="server"
                    CssClass="form-select" />
            </div>
            <div class="col-12 col-md-2">
                <asp:Button ID="SearchButton" runat="server"
                    Text="Search" CssClass="btn btn-primary w-100"
                    OnClick="SearchButton_Click" />
            </div>
        </div>
       
<div id="CartAlert" runat="server"
    class="alert alert-success alert-dismissible fade show d-none"
    role="alert">

    <asp:Label ID="CartMessageLabel" runat="server" />

    <button type="button"
        class="btn-close"
        aria-label="Close"
        onclick="this.parentElement.classList.add('d-none');">
    </button>
</div>

      <asp:Repeater ID="BookRepeater" runat="server"
    OnItemCommand="BookRepeater_ItemCommand">
            <HeaderTemplate>
                <div class="row g-4">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="col-12 col-sm-6 col-lg-4 col-xl-3">
                    <div class="card h-100 shadow-sm">
                        <asp:PlaceHolder runat="server"
                            Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("ImagePath"))) %>'>
                            <img src='<%#: ResolveUrl(Convert.ToString(Eval("ImagePath"))) %>'
                                 class="card-img-top"
                                 alt="Book cover"
                                 style="height:220px;object-fit:contain;padding:12px;" />
                        </asp:PlaceHolder>
                        <div class="card-body d-flex flex-column">
                            <span class="badge text-bg-light align-self-start mb-2">
                                <%#: Eval("CategoryName") %>
                            </span>
                            <h5 class="card-title"><%#: Eval("Title") %></h5>
                            <p class="text-muted mb-2">
                                By <%#: Eval("Author") %>
                            </p>
                            <p class="card-text small">
                                <%#: Eval("Description") %>
                            </p>
                            <div class="mt-auto">
                                <p class="fw-bold text-primary mb-1">
                                    PKR <%#: Eval("Price", "{0:N2}") %>
                                </p>
                              <small class="text-muted d-block mb-3">
    <%#: Convert.ToInt32(Eval("Stock")) > 0
        ? "In stock"
        : "Out of stock" %>
</small>

<asp:Button ID="AddToCartButton" runat="server"
    Text="Add to Cart"
    CssClass="btn btn-primary w-100"
    CommandName="AddToCart"
    CommandArgument='<%# Eval("BookId") %>'
    Enabled='<%# Convert.ToInt32(Eval("Stock")) > 0 %>' />
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </main>
</form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
