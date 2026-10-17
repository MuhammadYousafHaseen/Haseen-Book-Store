<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellerPayments.aspx.cs" Inherits="BookStoreWebForms.SellerPayments" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Seller Payment Management</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">

            <div class="d-flex justify-content-between align-items-center flex-wrap gap-3 mb-4">
                <div>
                    <h2 class="fw-bold">Seller Payment Management</h2>
                    <p class="text-muted mb-0">
                        View your payments and record cash collection.
                    </p>
                </div>

                <div class="d-flex gap-2 flex-wrap">
                    <a href="SellerDashboard.aspx"
                       class="btn btn-outline-primary">
                        Seller Dashboard
                    </a>

                    <a href="SellerOrders.aspx"
                       class="btn btn-outline-primary">
                        Manage Orders
                    </a>
                </div>
            </div>

            <asp:Panel ID="MessagePanel" runat="server"
                Visible="false" CssClass="alert alert-info">
                <asp:Label ID="MessageLabel" runat="server" />
            </asp:Panel>

            <div class="alert alert-warning">
                <strong>Important:</strong>
                Mark a Cash on Delivery payment as Paid only after
                you have actually received the cash. Refunds should
                only be recorded after a refund has actually been made.
                Online gateway payments will require provider verification.
            </div>

            <asp:Panel ID="EmptyPaymentsPanel" runat="server"
                Visible="false" CssClass="card border-0 shadow-sm">
                <div class="card-body text-center py-5">
                    <h4>No payments found</h4>
                    <p class="text-muted mb-0">
                        Payments for your order fulfillments will appear here.
                    </p>
                </div>
            </asp:Panel>

            <div class="table-responsive bg-white rounded shadow-sm">
                <asp:GridView ID="PaymentsGridView" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-hover align-middle mb-0"
                    GridLines="None"
                    DataKeyNames="SellerFulfillmentPaymentId"
                    OnRowDataBound="PaymentsGridView_RowDataBound"
                    OnRowCommand="PaymentsGridView_RowCommand"
                    EmptyDataText="No payments found.">

                    <Columns>
                        <asp:BoundField DataField="OrderId"
                            HeaderText="Order #" />

                        <asp:BoundField DataField="OrderDate"
                            HeaderText="Order Date"
                            DataFormatString="{0:dd MMM yyyy}" />

                        <asp:BoundField DataField="Books"
                            HeaderText="Your Books" />

                        <asp:BoundField DataField="Amount"
                            HeaderText="Your Amount"
                            DataFormatString="Rs. {0:N2}" />

                        <asp:BoundField DataField="PaymentMethod"
                            HeaderText="Method" />

                        <asp:BoundField DataField="PaymentStatus"
                            HeaderText="Current Payment Status" />

                        <asp:BoundField DataField="TransactionReference"
                            HeaderText="Transaction Reference" />

                        <asp:TemplateField HeaderText="New Status">
                            <ItemTemplate>
                                <asp:DropDownList ID="PaymentStatusDropDown"
                                    runat="server"
                                    CssClass="form-select form-select-sm mb-2">
                                    <asp:ListItem Text="Pending" Value="Pending" />
                                    <asp:ListItem Text="Paid" Value="Paid" />
                                    <asp:ListItem Text="Failed" Value="Failed" />
                                    <asp:ListItem Text="Refunded" Value="Refunded" />
                                </asp:DropDownList>

                                <asp:TextBox ID="ChangeNoteTextBox"
                                    runat="server"
                                    MaxLength="500"
                                    TextMode="MultiLine"
                                    Rows="2"
                                    CssClass="form-control form-control-sm mb-2"
                                    placeholder="Reason or note (optional)" />

                                <asp:Button ID="UpdatePaymentButton"
                                    runat="server"
                                    Text="Save Status"
                                    CssClass="btn btn-primary btn-sm"
                                    CommandName="UpdatePayment"
                                    CommandArgument='<%# Eval("SellerFulfillmentPaymentId") %>'
                                    CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </form>
</body>
</html>