
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Checkout.aspx.cs"
    Inherits="BookStoreWebForms.Checkout" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>BookStore | Checkout</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">

<form id="form1" runat="server">

    <nav class="navbar bg-white border-bottom">
        <div class="container">
            <a class="navbar-brand fw-bold" href="Books.aspx">BookStore</a>
            <a class="btn btn-outline-secondary" href="Cart.aspx">
                Back to Cart
            </a>
        </div>
    </nav>

    <main class="container py-5">

        <h1 class="mb-2">Checkout</h1>
        <p class="text-muted mb-4">
            Confirm your delivery address and place your order.
        </p>

        <div id="CheckoutAlert" runat="server"
            class="alert alert-danger alert-dismissible fade show d-none"
            role="alert">

            <asp:Label ID="MessageLabel" runat="server" />

            <button type="button"
                class="btn-close"
                aria-label="Close"
                onclick="this.parentElement.classList.add('d-none');">
            </button>
        </div>

        <asp:Panel ID="CheckoutPanel" runat="server">

            <div class="row g-4">

                <div class="col-12 col-lg-7">

                    <div class="card shadow-sm">
                        <div class="card-body p-4">

                            <h4 class="mb-3">Delivery Details</h4>

                            <label for="ShippingAddressTextBox"
                                class="form-label">
                                Complete Shipping Address
                            </label>

                            <asp:TextBox ID="ShippingAddressTextBox"
                                runat="server"
                                TextMode="MultiLine"
                                Rows="4"
                                MaxLength="500"
                                CssClass="form-control"
                                placeholder="House number, street, area, city"
                                required="required" />

                            <asp:RequiredFieldValidator
                                ID="ShippingAddressValidator"
                                runat="server"
                                ControlToValidate="ShippingAddressTextBox"
                                ErrorMessage="Please enter your shipping address."
                                CssClass="text-danger small"
                                Display="Dynamic" />

                            <hr class="my-4" />

                            <h4 class="mb-3">Payment Method</h4>

                            <div class="border rounded p-3 bg-light">
                                <strong>Cash on Delivery (COD)</strong>
                                <p class="text-muted mb-0 mt-1">
                                    Pay when your order is delivered.
                                </p>
                            </div>

                            <asp:Button ID="PlaceOrderButton"
                                runat="server"
                                Text="Place COD Order"
                                CssClass="btn btn-primary btn-lg w-100 mt-4"
                                OnClick="PlaceOrderButton_Click" />

                        </div>
                    </div>

                </div>

                <div class="col-12 col-lg-5">

                    <div class="card shadow-sm">
                        <div class="card-body p-4">

                            <h4 class="mb-3">Order Summary</h4>

                            <asp:Repeater ID="CheckoutRepeater"
                                runat="server">

                                <ItemTemplate>
                                    <div class="d-flex justify-content-between gap-3 mb-3">
                                        <div>
                                            <div class="fw-semibold">
                                                <%#: Eval("Title") %>
                                            </div>
                                            <small class="text-muted">
                                                Quantity:
                                                <%#: Eval("Quantity") %>
                                            </small>
                                        </div>

                                        <div class="text-nowrap">
                                            PKR <%#: Eval("Subtotal", "{0:N2}") %>
                                        </div>
                                    </div>
                                </ItemTemplate>

                            </asp:Repeater>

                            <hr />

                            <div class="d-flex justify-content-between">
                                <strong>Total</strong>
                                <strong class="text-primary">
                                    PKR <asp:Label ID="TotalLabel" runat="server" />
                                </strong>
                            </div>

                            <small class="text-muted d-block mt-3">
                                The final amount is calculated again on
                                the server when you place your order.
                            </small>

                        </div>
                    </div>

                </div>

            </div>

        </asp:Panel>

        <asp:Panel ID="ConfirmationPanel" runat="server"
            Visible="false">

            <div class="card shadow-sm p-4 p-md-5 text-center">

                <h2 class="text-success">Order placed successfully!</h2>

                <p class="mt-3">
                    Your order number is
                    <strong>
                        #<asp:Label ID="OrderNumberLabel" runat="server" />
                    </strong>
                </p>

                <p>
                    Payment method: <strong>Cash on Delivery</strong>
                </p>

                <p>
                    Payment status: <span class="badge text-bg-warning">
                        Pending
                    </span>
                </p>

                <p class="text-muted">
                    Please pay when your order is delivered.
                </p>

                <div>
                    <a href="Books.aspx" class="btn btn-primary">
                        Continue Shopping
                    </a>
                </div>

            </div>

        </asp:Panel>

    </main>

</form>
</body>
</html>
