
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="SignUp.aspx.cs"
    Inherits="BookStoreWebForms.SignUp" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Create Account - BookStore</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-md-6 col-lg-5">
                    <div class="card shadow-sm">
                        <div class="card-body p-4">
                            <h2 class="text-center mb-4">Create Account</h2>

                            <asp:Label ID="MessageLabel" runat="server"
                                EnableViewState="false" />

                            <div class="mb-3">
                                <asp:Label runat="server" AssociatedControlID="NameTextBox"
                                    CssClass="form-label" Text="Full name" />
                                <asp:TextBox ID="NameTextBox" runat="server"
                                    CssClass="form-control" MaxLength="100" />
                                <asp:RequiredFieldValidator runat="server"
                                    ControlToValidate="NameTextBox"
                                    ErrorMessage="Enter your name."
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="mb-3">
                                <asp:Label runat="server" AssociatedControlID="EmailTextBox"
                                    CssClass="form-label" Text="Email address" />
                                <asp:TextBox ID="EmailTextBox" runat="server"
                                    CssClass="form-control" TextMode="Email"
                                    MaxLength="256" />
                                <asp:RequiredFieldValidator runat="server"
                                    ControlToValidate="EmailTextBox"
                                    ErrorMessage="Enter your email."
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="mb-3">
                                <asp:Label runat="server" AssociatedControlID="PasswordTextBox"
                                    CssClass="form-label" Text="Password" />
                                <asp:TextBox ID="PasswordTextBox" runat="server"
                                    CssClass="form-control" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server"
                                    ControlToValidate="PasswordTextBox"
                                    ErrorMessage="Enter a password."
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="mb-3">
                                <asp:Label runat="server" AssociatedControlID="ConfirmPasswordTextBox"
                                    CssClass="form-label" Text="Confirm password" />
                                <asp:TextBox ID="ConfirmPasswordTextBox" runat="server"
                                    CssClass="form-control" TextMode="Password" />
                                <asp:CompareValidator runat="server"
                                    ControlToValidate="ConfirmPasswordTextBox"
                                    ControlToCompare="PasswordTextBox"
                                    ErrorMessage="Passwords do not match."
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <asp:Button ID="SignUpButton" runat="server"
                                Text="Create Account"
                                CssClass="btn btn-primary w-100"
                                OnClick="SignUpButton_Click" />

                            <p class="text-center mt-3 mb-0">
                                Already have an account?
                                <a href="SignIn.aspx">Sign in</a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
