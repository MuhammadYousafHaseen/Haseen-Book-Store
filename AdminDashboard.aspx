
<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs"
    Inherits="BookStoreWebForms.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .admin-dashboard {
            padding: 32px 0 48px;
            color: #263449;
        }

        .admin-heading {
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 16px;
            margin-bottom: 28px;
        }

        .admin-heading h1 {
            font-size: 30px;
            font-weight: 750;
            margin-bottom: 6px;
        }

        .admin-heading p {
            color: #64748b;
            margin: 0;
        }

        .admin-badge {
            background: #e0f2fe;
            color: #075985;
            padding: 9px 15px;
            border-radius: 24px;
            font-weight: 600;
            font-size: 13px;
        }

        .admin-stats {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }

        .admin-stat {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 16px;
            padding: 24px;
            box-shadow: 0 5px 18px rgba(15, 23, 42, .04);
        }

        .admin-stat-label {
            color: #64748b;
            font-size: 14px;
            font-weight: 600;
        }

        .admin-stat-number {
            display: block;
            font-size: 32px;
            font-weight: 750;
            margin-top: 12px;
            color: #0f172a;
        }

        .admin-stat-icon {
            font-size: 25px;
            margin-bottom: 14px;
        }

        .admin-section {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 16px;
            padding: 24px;
            margin-bottom: 24px;
        }

        .admin-section h2 {
            font-size: 20px;
            font-weight: 700;
            margin: 0 0 8px;
        }

        .admin-section p {
            color: #64748b;
            margin-bottom: 20px;
        }

        .admin-actions {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 14px;
        }

        .admin-action {
            display: block;
            text-decoration: none !important;
            color: #263449 !important;
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 12px;
            padding: 18px;
            transition: background .2s, border-color .2s;
        }

        .admin-action:hover {
            background: #eff6ff;
            border-color: #93c5fd;
        }

        .admin-action strong {
            display: block;
            margin-bottom: 7px;
            color: #1d4ed8;
        }

        .admin-action span {
            font-size: 13px;
            color: #64748b;
        }

        .admin-message {
            display: block;
            padding: 12px 16px;
            background: #fff7ed;
            color: #9a3412;
            border-radius: 10px;
            margin-bottom: 20px;
        }

        @media (max-width: 767px) {
            .admin-stats,
            .admin-actions {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }

            .admin-stat {
                padding: 18px;
            }

            .admin-stat-number {
                font-size: 27px;
            }
        }

        @media (max-width: 480px) {
            .admin-stats,
            .admin-actions {
                grid-template-columns: 1fr;
            }

            .admin-heading h1 {
                font-size: 25px;
            }
        }
    </style>

    <div class="container admin-dashboard">

        <div class="admin-heading">
            <div>
                <h1>Admin Dashboard</h1>
                <p>Manage your bookstore from one place.</p>
            </div>
            <span class="admin-badge">Administrator</span>
        </div>

        <asp:Label ID="lblError" runat="server"
            CssClass="admin-message" Visible="false" />

        <div class="admin-stats">

            <div class="admin-stat">
                <div class="admin-stat-icon">📚</div>
                <div class="admin-stat-label">Total Books</div>
                <asp:Label ID="lblTotalBooks" runat="server"
                    CssClass="admin-stat-number" Text="—" />
            </div>

            <div class="admin-stat">
                <div class="admin-stat-icon">🗂️</div>
                <div class="admin-stat-label">Categories</div>
                <asp:Label ID="lblTotalCategories" runat="server"
                    CssClass="admin-stat-number" Text="—" />
            </div>

            <div class="admin-stat">
                <div class="admin-stat-icon">📦</div>
                <div class="admin-stat-label">Total Orders</div>
                <asp:Label ID="lblTotalOrders" runat="server"
                    CssClass="admin-stat-number" Text="—" />
            </div>

            <div class="admin-stat">
                <div class="admin-stat-icon">👥</div>
                <div class="admin-stat-label">Registered Users</div>
                <asp:Label ID="lblTotalUsers" runat="server"
                    CssClass="admin-stat-number" Text="—" />
            </div>

            <div class="admin-stat">
                <div class="admin-stat-icon">💳</div>
                <div class="admin-stat-label">Payment Records</div>
                <asp:Label ID="lblTotalPayments" runat="server"
                    CssClass="admin-stat-number" Text="—" />
            </div>

        </div>

        <div class="admin-section">
            <h2>Quick Management</h2>
            <p>Open a section to manage your bookstore.</p>

            <div class="admin-actions">

                <a class="admin-action" href="<%= ResolveUrl("~/Books.aspx") %>">
                    <strong>📚 Browse Books</strong>
                    <span>View the bookstore catalogue.</span>
                </a>

                <a class="admin-action" href="<%= ResolveUrl("~/Categories.aspx") %>">
                    <strong>🗂️ Categories</strong>
                    <span>View book categories.</span>
                </a>

                <a class="admin-action" href="<%= ResolveUrl("~/AdminPayments.aspx") %>">
                    <strong>💳 Payment Management</strong>
                    <span>Review payment records and statuses.</span>
                </a>

                <a class="admin-action" href="<%= ResolveUrl("~/MyOrders.aspx") %>">
                    <strong>📦 My Orders</strong>
                    <span>Open the existing orders page.</span>
                </a>

                <a class="admin-action" href="<%= ResolveUrl("~/MyAccount.aspx") %>">
                    <strong>👤 My Account</strong>
                    <span>View your account page.</span>
                </a>

                <a class="admin-action" href="<%= ResolveUrl("~/Default.aspx") %>">
                    <strong>🏠 View Store</strong>
                    <span>Return to the bookstore homepage.</span>
                </a>

            </div>
        </div>

        <div class="admin-section">
            <h2>Administration Overview</h2>
            <p>
                This dashboard displays record totals from the connected database.
                Additional management features can be added as separate modules.
            </p>
        </div>

    </div>

</asp:Content>
