
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="SignIn.aspx.cs"
    Inherits="BookStoreWebForms.SignIn" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Sign In - BookStore</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-md-6 col-lg-5">
                    <div class="card shadow-sm">
                        <div class="card-body p-4">
                            <h2 class="text-center mb-4">Sign In</h2>

                            <asp:Label ID="MessageLabel" runat="server"
                                EnableViewState="false" />

                            <div class="mb-3">
                                <asp:Label runat="server"
                                    AssociatedControlID="EmailTextBox"
                                    CssClass="form-label"
                                    Text="Email address" />
                                <asp:TextBox ID="EmailTextBox" runat="server"
                                    CssClass="form-control"
                                    TextMode="Email" MaxLength="256" />
                                <asp:RequiredFieldValidator runat="server"
                                    ControlToValidate="EmailTextBox"
                                    ErrorMessage="Enter your email."
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>

                            <div class="mb-3">
                                <asp:Label runat="server"
                                    AssociatedControlID="PasswordTextBox"
                                    CssClass="form-label"
                                    Text="Password" />
                                <asp:TextBox ID="PasswordTextBox" runat="server"
                                    CssClass="form-control"
                                    TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server"
                                    ControlToValidate="PasswordTextBox"
                                    ErrorMessage="Enter your password."
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>

                            <asp:Button ID="SignInButton" runat="server"
                                Text="Sign In"
                                CssClass="btn btn-primary w-100"
                                OnClick="SignInButton_Click" />

                            <p class="text-center mt-3 mb-0">
                                Don't have an account?
                                <a href="SignUp.aspx">Create account</a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
