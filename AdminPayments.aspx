<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="AdminPayments.aspx.cs"
    Inherits="BookStoreWebForms.AdminPayments" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Admin Payment Overview - BookStore</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background: #f4f7fb;
            color: #253247;
            margin: 0;
        }

        .container {
            max-width: 1400px;
            margin: 30px auto;
            padding: 0 20px;
        }

        .page-header {
            margin-bottom: 25px;
        }

        .page-header h1 {
            margin-bottom: 8px;
            color: #203b63;
        }

        .muted {
            color: #64748b;
        }

        .summary-grid {
            display: grid;
            grid-template-columns:
                repeat(auto-fit, minmax(180px, 1fr));
            gap: 16px;
            margin-bottom: 25px;
        }

        .summary-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 10px;
            padding: 20px;
        }

        .summary-card .label {
            color: #64748b;
            font-size: 14px;
            margin-bottom: 12px;
        }

        .summary-card .value {
            font-size: 26px;
            font-weight: bold;
            overflow-wrap: anywhere;
        }

        .panel {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 25px;
        }

        .filters {
            display: flex;
            gap: 12px;
            align-items: center;
            flex-wrap: wrap;
            margin: 18px 0;
        }

        .filters select,
        .filters input,
        .filters button {
            padding: 10px;
            border: 1px solid #cbd5e1;
            border-radius: 6px;
            font-size: 14px;
        }

        .filters button {
            background: #2563eb;
            color: white;
            border: none;
            cursor: pointer;
        }

        .table-wrapper {
            overflow-x: auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            min-width: 1000px;
        }

        th, td {
            text-align: left;
            padding: 12px;
            border-bottom: 1px solid #e2e8f0;
            vertical-align: top;
        }

        th {
            background: #f8fafc;
            color: #334155;
            white-space: nowrap;
        }

        .badge {
            display: inline-block;
            padding: 5px 9px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
            white-space: nowrap;
        }

        .status-paid {
            background: #dcfce7;
            color: #166534;
        }

        .status-pending {
            background: #fef3c7;
            color: #92400e;
        }

        .status-failed {
            background: #fee2e2;
            color: #991b1b;
        }

        .status-refunded {
            background: #dbeafe;
            color: #1e40af;
        }

        .message {
            display: block;
            padding: 14px;
            border-radius: 6px;
            margin-bottom: 20px;
        }

        .message-error {
            background: #fee2e2;
            color: #991b1b;
        }

        .message-info {
            background: #e0f2fe;
            color: #075985;
        }

        .empty {
            padding: 20px;
            text-align: center;
            color: #64748b;
        }

        @media (max-width: 600px) {
            .container {
                margin: 18px auto;
                padding: 0 12px;
            }

            .panel {
                padding: 14px;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <main class="container">

            <asp:Panel ID="pnlAccessDenied"
                runat="server" Visible="false">
                <div class="message message-error">
                    <h2>Access denied</h2>
                    <p>
                        You must sign in with an account that has
                        the Admin role to view this page.
                    </p>
                    <p>
                        If you are already an administrator,
                        verify that your account has the Admin
                        role in the database.
                    </p>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlAdminContent"
                runat="server" Visible="false">

                <div class="page-header">
                    <h1>Admin Payment Overview</h1>
                    <p class="muted">
                        Review seller payments, payment statuses,
                        and recorded payment history.
                    </p>
                </div>

                <asp:Label ID="lblMessage"
                    runat="server"
                    Visible="false"
                    CssClass="message message-error" />

                <div class="summary-grid">
                    <div class="summary-card">
                        <div class="label">Total Seller Payments</div>
                        <div class="value">
                            <asp:Label ID="lblTotalPayments"
                                runat="server" Text="0" />
                        </div>
                    </div>

                    <div class="summary-card">
                        <div class="label">Total Payment Amount</div>
                        <div class="value">
                            <asp:Label ID="lblTotalAmount"
                                runat="server" Text="Rs. 0.00" />
                        </div>
                    </div>

                    <div class="summary-card">
                        <div class="label">Pending</div>
                        <div class="value">
                            <asp:Label ID="lblPending"
                                runat="server" Text="0" />
                        </div>
                    </div>

                    <div class="summary-card">
                        <div class="label">Paid</div>
                        <div class="value">
                            <asp:Label ID="lblPaid"
                                runat="server" Text="0" />
                        </div>
                    </div>

                    <div class="summary-card">
                        <div class="label">Failed</div>
                        <div class="value">
                            <asp:Label ID="lblFailed"
                                runat="server" Text="0" />
                        </div>
                    </div>

                    <div class="summary-card">
                        <div class="label">Refunded</div>
                        <div class="value">
                            <asp:Label ID="lblRefunded"
                                runat="server" Text="0" />
                        </div>
                    </div>
                </div>

                <section class="panel">
                    <h2>Seller Payment Records</h2>

                    <div class="filters">
                        <asp:Label ID="lblFilter"
                            runat="server"
                            AssociatedControlID="ddlStatus"
                            Text="Payment status:" />

                        <asp:DropDownList ID="ddlStatus"
                            runat="server">
                            <asp:ListItem Text="All statuses"
                                Value="" />
                            <asp:ListItem Text="Pending"
                                Value="Pending" />
                            <asp:ListItem Text="Paid"
                                Value="Paid" />
                            <asp:ListItem Text="Failed"
                                Value="Failed" />
                            <asp:ListItem Text="Refunded"
                                Value="Refunded" />
                        </asp:DropDownList>

                        <asp:Button ID="btnFilter"
                            runat="server"
                            Text="Apply Filter"
                            OnClick="btnFilter_Click" />

                        <asp:Button ID="btnReset"
                            runat="server"
                            Text="Clear Filter"
                            CausesValidation="false"
                            OnClick="btnReset_Click" />
                    </div>

                    <div class="table-wrapper">
                        <asp:GridView ID="gvPayments"
                            runat="server"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            Width="100%"
                            EmptyDataText="No seller payment records found."
                            EmptyDataRowStyle-CssClass="empty"
                            OnRowDataBound="gvPayments_RowDataBound">

                            <Columns>
                                <asp:BoundField
                                    DataField="OrderId"
                                    HeaderText="Order #" />

                                <asp:BoundField
                                    DataField="CustomerName"
                                    HeaderText="Customer" />

                                <asp:BoundField
                                    DataField="SellerName"
                                    HeaderText="Seller" />

                                <asp:BoundField
                                    DataField="Amount"
                                    HeaderText="Amount (Rs.)"
                                    DataFormatString="{0:N2}"
                                    HtmlEncode="false" />

                                <asp:BoundField
                                    DataField="PaymentMethod"
                                    HeaderText="Method" />

                                <asp:TemplateField
                                    HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblRowStatus"
                                            runat="server"
                                            Text='<%# Eval("PaymentStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField
                                    DataField="TransactionReference"
                                    HeaderText="Transaction Reference"
                                    NullDisplayText="—" />

                                <asp:BoundField
                                    DataField="UpdatedAt"
                                    HeaderText="Last Updated"
                                    DataFormatString="{0:dd MMM yyyy, hh:mm tt}"
                                    NullDisplayText="—" />

                                <asp:BoundField
                                    DataField="LastChangeNote"
                                    HeaderText="Latest Note"
                                    NullDisplayText="—" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </section>

                <section class="panel">
                    <h2>Recent Payment Status History</h2>
                    <p class="muted">
                        The latest recorded status changes across
                        seller payments. This section is read-only.
                    </p>

                    <div class="table-wrapper">
                        <asp:GridView ID="gvHistory"
                            runat="server"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            Width="100%"
                            EmptyDataText="No payment history records found."
                            EmptyDataRowStyle-CssClass="empty">

                            <Columns>
                                <asp:BoundField
                                    DataField="OrderId"
                                    HeaderText="Order #" />

                                <asp:BoundField
                                    DataField="SellerName"
                                    HeaderText="Seller" />

                                <asp:BoundField
                                    DataField="PreviousStatus"
                                    HeaderText="Previous Status"
                                    NullDisplayText="—" />

                                <asp:BoundField
                                    DataField="NewStatus"
                                    HeaderText="New Status" />

                                <asp:BoundField
                                    DataField="ChangedBy"
                                    HeaderText="Changed By" />

                                <asp:BoundField
                                    DataField="ChangeNote"
                                    HeaderText="Change Note"
                                    NullDisplayText="—" />

                                <asp:BoundField
                                    DataField="ChangedAt"
                                    HeaderText="Changed At"
                                    DataFormatString="{0:dd MMM yyyy, hh:mm tt}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </section>

            </asp:Panel>
        </main>
    </form>
</body>
</html>