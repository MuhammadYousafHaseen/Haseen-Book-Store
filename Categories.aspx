
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Categories.aspx.cs"
    Inherits="BookStoreWebForms.Categories" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Book Categories</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">
            <h1 class="mb-4">Browse Book Categories</h1>

            <asp:Repeater ID="CategoryRepeater" runat="server">
                <HeaderTemplate>
                    <div class="row g-3">
                </HeaderTemplate>

                <ItemTemplate>
                    <div class="col-12 col-sm-6 col-lg-4">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">
                                    <%#: Eval("CategoryName") %>
                                </h5>
                                <p class="card-text text-muted">
                                    <%#: Eval("Description") %>
                                </p>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>

                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
