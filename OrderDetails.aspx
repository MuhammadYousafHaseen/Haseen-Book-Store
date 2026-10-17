
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="OrderDetails.aspx.cs"
    Inherits="BookStoreWebForms.OrderDetails" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Order Details - BookStore</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
    
<style>
    body {
        color: #263449;
    }

    .status-badge {
        display: inline-block;
        padding: 0.4rem 0.75rem;
        border-radius: 50rem;
        font-size: 0.85rem;
        font-weight: 600;
        background: #e9ecef;
        color: #495057;
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

    .card {
        border-radius: 14px;
    }

    .table thead th {
        white-space: nowrap;
        background: #f1f5f9;
    }

    .table td {
        vertical-align: middle;
    }
</style>

</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">

            <div class="d-flex justify-content-between align-items-center flex-wrap gap-3 mb-4">
                <div>
                    <h2 class="fw-bold">Order Details</h2>
                    <p class="text-muted mb-0">
                        View the books and progress of your purchase.
                    </p>
                </div>

                <a href="MyOrders.aspx" class="btn btn-outline-primary">
                    Back to My Orders
                </a>
            </div>
            
<div class="mb-4">
    <asp:Button ID="CancelOrderButton"
        runat="server"
        Text="Cancel Order"
        CssClass="btn btn-danger"
        CausesValidation="false"
        OnClientClick="return confirm('Are you sure you want to cancel this order? Stock will be restored if cancellation succeeds.');"
        OnClick="CancelOrderButton_Click" />

    <asp:Label ID="CancellationInfoLabel"
        runat="server"
        CssClass="text-muted ms-2"
        EnableViewState="true" />
</div>


            <asp:Panel ID="MessagePanel" runat="server"
                Visible="false" CssClass="alert alert-danger">
                <asp:Label ID="MessageLabel" runat="server" />
            </asp:Panel>

            <asp:Panel ID="OrderPanel" runat="server" Visible="false">

                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-body p-4">
                        <div class="row g-4">
                            <div class="col-md-4">
                                <small class="text-muted">Order Number</small>
                                <h5 class="fw-bold">
                                    #<asp:Label ID="lblOrderId" runat="server" />
                                </h5>
                            </div>

                            <div class="col-md-4">
                                <small class="text-muted">Order Date</small>
                                <h6>
                                    <asp:Label ID="lblOrderDate" runat="server" />
                                </h6>
                            </div>

                            <div class="col-md-4">
                                <small class="text-muted">Order Status</small>
                                <h6>
                                   <asp:Label ID="lblOrderStatus" runat="server"
    CssClass="status-badge" />
                                </h6>
                            </div>

                            <div class="col-md-4">
                                <small class="text-muted">Order Total</small>
                                <h5 class="fw-bold">
                                    Rs. <asp:Label ID="lblTotalAmount" runat="server" />
                                </h5>
                            </div>

                            <div class="col-md-4">
                                <small class="text-muted">Overall Payment Method</small>
                                <div>
                                    <asp:Label ID="lblPaymentMethod" runat="server" />
                                </div>
                            </div>

                            <div class="col-md-4">
                                <small class="text-muted">Overall Payment Status</small>
                                <div>
                                    <asp:Label ID="lblPaymentStatus" runat="server"
    CssClass="status-badge" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-body p-4">
                        <h5 class="fw-bold mb-3">Books in This Order</h5>

                        <div class="table-responsive">
                            <asp:GridView ID="gvItems" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table table-bordered table-hover align-middle"
                                GridLines="None"
                                EmptyDataText="No book items were found.">
                                <Columns>
                                    <asp:BoundField DataField="Title"
                                        HeaderText="Book" />

                                    <asp:BoundField DataField="Author"
                                        HeaderText="Author" />

                                    <asp:BoundField DataField="Quantity"
                                        HeaderText="Quantity" />

                                    <asp:BoundField DataField="UnitPrice"
                                        HeaderText="Unit Price (Rs.)"
                                        DataFormatString="{0:N2}" />

                                    <asp:BoundField DataField="LineTotal"
                                        HeaderText="Line Total (Rs.)"
                                        DataFormatString="{0:N2}" />

                                    <asp:BoundField DataField="FulfillmentStatus"
                                        HeaderText="Fulfillment Status" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-body p-4">
                        <h5 class="fw-bold mb-3">Seller Payment Breakdown</h5>

                        <div class="table-responsive">
                            <asp:GridView ID="gvSellerPayments" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table table-bordered align-middle"
                                GridLines="None"
                                EmptyDataText="No seller payment details are available.">
                                <Columns>
                                    <asp:BoundField DataField="FulfillmentId"
                                        HeaderText="Fulfillment #" />

                                    <asp:BoundField DataField="SellerName"
                                        HeaderText="Seller" />

                                    <asp:BoundField DataField="Amount"
                                        HeaderText="Amount (Rs.)"
                                        DataFormatString="{0:N2}" />

                                    <asp:BoundField DataField="PaymentMethod"
                                        HeaderText="Method" />

                                    <asp:BoundField DataField="PaymentStatus"
                                        HeaderText="Payment Status" />

                                    <asp:BoundField DataField="UpdatedAt"
                                        HeaderText="Last Updated"
                                        DataFormatString="{0:dd MMM yyyy, hh:mm tt}"
                                        NullDisplayText="—" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm">
                    <div class="card-body p-4">
                        <h5 class="fw-bold mb-2">Shipping Address</h5>
                        <asp:Label ID="lblShippingAddress" runat="server" />
                    </div>
                </div>

            </asp:Panel>
        </div>
    </form>
</body>
</html>
