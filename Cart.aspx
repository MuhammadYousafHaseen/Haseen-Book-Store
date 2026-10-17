
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Cart.aspx.cs"
    Inherits="BookStoreWebForms.Cart" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>BookStore | Shopping Cart</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">

<form id="form1" runat="server">

    <nav class="navbar bg-white border-bottom">
        <div class="container">
            <a class="navbar-brand fw-bold" href="Books.aspx">
                BookStore
            </a>
            <a class="btn btn-outline-primary" href="Books.aspx">
                Continue Shopping
            </a>
        </div>
    </nav>

    <main class="container py-5">

        <h1 class="mb-2">Your Shopping Cart</h1>
        <p class="text-muted mb-4">
            Review your selected books before checkout.
        </p>

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

        <asp:Panel ID="EmptyCartPanel" runat="server"
            CssClass="card shadow-sm text-center p-5"
            Visible="false">

            <h3>Your cart is empty</h3>
            <p class="text-muted">
                Browse our collection and add a book to get started.
            </p>
            <div>
                <a href="Books.aspx" class="btn btn-primary">
                    Browse Books
                </a>
            </div>
        </asp:Panel>

        <asp:Panel ID="CartContentsPanel" runat="server"
            Visible="false">

            <div class="row g-4">

                <div class="col-12 col-lg-8">

                    <asp:Repeater ID="CartRepeater" runat="server"
                        OnItemCommand="CartRepeater_ItemCommand">

                        <ItemTemplate>
                            <div class="card shadow-sm mb-3">
                                <div class="card-body">

                                    <div class="row g-3 align-items-center">

                                        <div class="col-12 col-md-5">
                                            <h5 class="mb-1">
                                                <%#: Eval("Title") %>
                                            </h5>

                                            <p class="text-muted mb-1">
                                                By <%#: Eval("Author") %>
                                            </p>

                                            <p class="mb-0">
                                                Price:
                                                <strong>
                                                    PKR <%#: Eval("Price", "{0:N2}") %>
                                                </strong>
                                            </p>

                                            <small class="text-muted">
                                                Available stock:
                                                <%#: Eval("Stock") %>
                                            </small>
                                        </div>

                                        <div class="col-6 col-md-3">
                                            <label class="form-label">
                                                Quantity
                                            </label>

                                            <asp:TextBox
                                                ID="QuantityTextBox"
                                                runat="server"
                                                Text='<%# Eval("Quantity") %>'
                                                TextMode="Number"
                                                min="1"
                                                CssClass="form-control" />
                                        </div>

                                        <div class="col-6 col-md-4 text-md-end">
                                            <p class="mb-2">
                                                Subtotal:
                                                <strong>
                                                    PKR <%#: Eval("Subtotal", "{0:N2}") %>
                                                </strong>
                                            </p>

                                            <asp:Button
                                                ID="UpdateButton"
                                                runat="server"
                                                Text="Update"
                                                CssClass="btn btn-outline-primary btn-sm me-1"
                                                CommandName="UpdateQuantity"
                                                CommandArgument='<%# Eval("BookId") %>' />

                                            <asp:Button
                                                ID="RemoveButton"
                                                runat="server"
                                                Text="Remove"
                                                CssClass="btn btn-outline-danger btn-sm"
                                                CommandName="RemoveBook"
                                                CommandArgument='<%# Eval("BookId") %>'
                                                CausesValidation="false"
                                                OnClientClick="return confirm('Remove this book from your cart?');" />
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </ItemTemplate>

                    </asp:Repeater>

                </div>

                <div class="col-12 col-lg-4">

                    <div class="card shadow-sm">
                        <div class="card-body">

                            <h4 class="mb-3">Order Summary</h4>

                            <div class="d-flex justify-content-between mb-3">
                                <span>Subtotal</span>
                                <strong>
                                    PKR
                                    <asp:Label ID="CartTotalLabel"
                                        runat="server" />
                                </strong>
                            </div>

                            <p class="text-muted small">
                                Delivery charges, if any, will be handled
                                during checkout.
                            </p>

                            <asp:Button ID="CheckoutButton"
                                runat="server"
                                Text="Proceed to Checkout"
                                CssClass="btn btn-primary w-100"
                                OnClick="CheckoutButton_Click" />

                        </div>
                    </div>

                </div>

            </div>

        </asp:Panel>

    </main>

</form>

</body>
</html>
