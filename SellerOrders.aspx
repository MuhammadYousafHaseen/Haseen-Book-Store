
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellerOrders.aspx.cs" Inherits="BookStoreWebForms.SellerOrders" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Seller Order Management</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">

            <div class="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h2 class="fw-bold">Seller Order Management</h2>
                    <p class="text-muted mb-0">
                        Manage orders for your books.
                    </p>
                </div>

                <a href="SellerDashboard.aspx"
                   class="btn btn-outline-primary">
                    Seller Dashboard
                </a>
                <a href="SellerOrders.aspx" class="btn btn-outline-primary">
    Manage Orders
</a>
            </div>

            <asp:Panel ID="MessagePanel" runat="server"
                Visible="false" CssClass="alert alert-info">
                <asp:Label ID="MessageLabel" runat="server" />
            </asp:Panel>

            <asp:Panel ID="EmptyOrdersPanel" runat="server"
                Visible="false" CssClass="card border-0 shadow-sm">
                <div class="card-body text-center py-5">
                    <h4>No orders found</h4>
                    <p class="text-muted mb-0">
                        Orders containing your books will appear here.
                    </p>
                </div>
            </asp:Panel>

       
<asp:GridView ID="OrdersGridView" runat="server"
    AutoGenerateColumns="False"
    CssClass="table table-striped table-hover bg-white"
    GridLines="None"
    DataKeyNames="FulfillmentId"
    OnRowDataBound="OrdersGridView_RowDataBound"
    OnRowCommand="OrdersGridView_RowCommand"
    EmptyDataText="No orders found.">

    <Columns>
        <asp:BoundField DataField="OrderId"
            HeaderText="Order #" />

        <asp:BoundField DataField="OrderDate"
            HeaderText="Order Date"
            DataFormatString="{0:dd MMM yyyy}" />

        <asp:BoundField DataField="Books"
            HeaderText="Your Books" />

        <asp:BoundField DataField="ShippingAddress"
            HeaderText="Shipping Address" />

        <asp:BoundField DataField="FulfillmentStatus"
            HeaderText="Your Fulfillment Status" />

        <asp:TemplateField HeaderText="Update Status">
            <ItemTemplate>
                <asp:DropDownList ID="StatusDropDown"
                    runat="server"
                    CssClass="form-select form-select-sm mb-2">

                    <asp:ListItem Text="Pending" Value="Pending" />
                    <asp:ListItem Text="Confirmed" Value="Confirmed" />
                    <asp:ListItem Text="Processing" Value="Processing" />
                    <asp:ListItem Text="Shipped" Value="Shipped" />
                    <asp:ListItem Text="Delivered" Value="Delivered" />
                    <asp:ListItem Text="Cancelled" Value="Cancelled" />

                </asp:DropDownList>

                <asp:Button ID="UpdateStatusButton"
                    runat="server"
                    Text="Update"
                    CssClass="btn btn-primary btn-sm"
                    CommandName="UpdateStatus"
                    CommandArgument='<%# Eval("FulfillmentId") %>'
                    CausesValidation="false" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


                <Columns>
                    <asp:BoundField DataField="OrderId"
                        HeaderText="Order #" />

                    <asp:BoundField DataField="OrderDate"
                        HeaderText="Order Date"
                        DataFormatString="{0:dd MMM yyyy}" />

                    <asp:BoundField DataField="Books"
                        HeaderText="Your Books" />

                    <asp:BoundField DataField="ShippingAddress"
                        HeaderText="Shipping Address" />

                    <asp:BoundField DataField="OrderStatus"
                        HeaderText="Current Status" />

                    <asp:TemplateField HeaderText="Update Status">
                        <ItemTemplate>
                            <asp:DropDownList ID="StatusDropDown"
                                runat="server"
                                CssClass="form-select form-select-sm mb-2">
                                <asp:ListItem Text="Pending" Value="Pending" />
                                <asp:ListItem Text="Processing" Value="Processing" />
                                <asp:ListItem Text="Shipped" Value="Shipped" />
                                <asp:ListItem Text="Delivered" Value="Delivered" />
                                <asp:ListItem Text="Cancelled" Value="Cancelled" />
                            </asp:DropDownList>

                            <asp:Button ID="UpdateStatusButton"
                                runat="server"
                                Text="Update"
                                CssClass="btn btn-primary btn-sm"
                                CommandName="UpdateStatus"
                                CommandArgument='<%# Eval("OrderId") %>'
                                CausesValidation="false" />

                            <asp:Label ID="RestrictionLabel"
                                runat="server"
                                CssClass="small text-muted"
                                Visible="false"
                                Text="Mixed-seller order: status updates are disabled." />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </div>
    </form>
</body>
</html>
