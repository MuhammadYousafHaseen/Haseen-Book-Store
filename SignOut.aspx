
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="SignOut.aspx.cs"
    Inherits="BookStoreWebForms.SignOut" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Sign Out - BookStore</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5 text-center">
            <div class="card shadow-sm mx-auto" style="max-width: 450px;">
                <div class="card-body p-4">
                    <h2>Sign Out</h2>
                    <p>Are you sure you want to sign out?</p>
                    <asp:Button ID="SignOutButton" runat="server"
                        Text="Yes, Sign Out"
                        CssClass="btn btn-danger"
                        OnClick="SignOutButton_Click" />
                    <a href="Default.aspx" class="btn btn-secondary">
                        Cancel
                    </a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
