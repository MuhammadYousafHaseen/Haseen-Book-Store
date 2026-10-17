<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyOrders.aspx.cs" Inherits="BookStoreWebForms.MyOrders" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>My Orders - BookStore</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
    
<style>
    body {
        color: #263449;
    }

    .order-card {
        border-radius: 14px;
        overflow: hidden;
    }

    .status-badge {
        display: inline-block;
        padding: 0.4rem 0.75rem;
        border-radius: 50rem;
        font-size: 0.82rem;
        font-weight: 600;
        white-space: nowrap;
    }

    .status-success {
        background: #d1e7dd;
        color: #0f5132;
    }

    .status-warning {
        background: #fff3cd;
        color: #664d03;
    }

    .status-danger {
        background: #f8d7da;
        color: #842029;
    }

    .status-info {
        background: #cfe2ff;
        color: #084298;
    }

    .status-neutral {
        background: #e9ecef;
        color: #495057;
    }

    .table thead th {
        white-space: nowrap;
    }

    .order-card .card-header {
        border-bottom: 1px solid #e9eef5;
    }
</style>

</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">

            <div class="d-flex justify-content-between align-items-center flex-wrap gap-3 mb-4">
                <div>
                    <h2 class="fw-bold">My Orders</h2>
                    <p class="text-muted mb-0">
                        View your purchases, fulfillment progress, and payment details.
                    </p>
                </div>

                <a href="Books.aspx" class="btn btn-outline-primary">
                    Continue Shopping
                </a>
            </div>

            <asp:Panel ID="MessagePanel" runat="server"
                Visible="false" CssClass="alert alert-danger">
                <asp:Label ID="MessageLabel" runat="server" />
            </asp:Panel>

            <asp:Panel ID="EmptyOrdersPanel" runat="server"
                Visible="false" CssClass="card border-0 shadow-sm">
                <div class="card-body text-center py-5">
                    <h4>No orders yet</h4>
                    <p class="text-muted">
                        Your purchases will appear here after you place an order.
                    </p>
                    <a href="Books.aspx" class="btn btn-primary">
                        Browse Books
                    </a>
                </div>
            </asp:Panel>

            <asp:Repeater ID="OrdersRepeater" runat="server">
                <ItemTemplate>
                  <div class="card order-card border-0 shadow-sm mb-4">

                        <div class="card-header bg-white p-3">
                            <div class="row g-3">

                                <div class="col-md-4">
                                    <small class="text-muted">Order Number</small>
                                    <div class="fw-bold">
                                        #<%#: Eval("OrderId") %>
                                    </div>
                                    <a class="btn btn-sm btn-outline-primary mt-2"
   href='<%# "OrderDetails.aspx?id=" +
       HttpUtility.UrlEncode(
           Convert.ToString(Eval("OrderId"))) %>'>
    View Order Details
</a>
                                </div>

                                <div class="col-md-4">
                                    <small class="text-muted">Order Date</small>
                                    <div>
                                        <%#: Eval("OrderDate", "{0:dd MMM yyyy, hh:mm tt}") %>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <small class="text-muted">Overall Order Status</small>
                                    <div>
                                       <span class='<%# GetOrderStatusCss(Eval("OrderStatus")) %>'>
    <%#: Eval("OrderStatus") %>
</span>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="card-body p-3">

                            <h6 class="fw-bold">
                                Books and Fulfillment Status
                            </h6>

                            <p class="mb-3 text-break">
                                <%#: Eval("Books") %>
                            </p>

                            <div class="row g-3">

                                <div class="col-md-4">
                                    <small class="text-muted">Total Order Amount</small>
                                    <div class="fw-bold">
                                        Rs. <%#: Eval("TotalAmount", "{0:N2}") %>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <small class="text-muted">Overall Payment Method</small>
                                    <div>
                                        <%#: Eval("PaymentMethod") %>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <small class="text-muted">Overall Payment Status</small>
                                    <div>
                                       <span class='<%# GetPaymentStatusCss(Eval("PaymentStatus")) %>'>
    <%#: Eval("PaymentStatus") %>
</span>
                                    </div>
                                </div>

                            </div>

                            <hr />

                            <h6 class="fw-bold">
                                Payment Breakdown by Seller
                            </h6>

                            <p class="small text-muted">
                                Each seller's payment is tracked separately.
                                Your overall payment status may differ from an
                                individual seller's payment status.
                            </p>

                            <div class="table-responsive">
                                <table class="table table-sm table-bordered bg-white align-middle">
                                    <thead class="table-light">
                                        <tr>
                                            <th>Fulfillment</th>
                                            <th>Amount</th>
                                            <th>Method</th>
                                            <th>Payment Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%# Eval("SellerPaymentsHtml") %>
                                    </tbody>
                                </table>
                            </div>

                            <hr />

                            <small class="text-muted">Shipping Address</small>
                            <div class="text-break">
                                <%#: Eval("ShippingAddress") %>
                            </div>

                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </form>
</body>
</html>
