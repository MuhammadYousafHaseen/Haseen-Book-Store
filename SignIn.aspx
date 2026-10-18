
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="SignIn.aspx.cs"
    Inherits="BookStoreWebForms.SignIn" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Sign In | Haseen Books Store</title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <style>
        :root {
            --blue: #2563eb;
            --blue-dark: #1d4ed8;
            --text: #172b4d;
            --muted: #64748b;
            --border: #dbe5f1;
        }

        * { box-sizing: border-box; }

        body {
            margin: 0;
            min-height: 100vh;
            font-family: "Segoe UI", Arial, sans-serif;
            color: var(--text);
            background:
                radial-gradient(ellipse at 10% 10%,
                    #dbeafe 0, transparent 42%),
                radial-gradient(ellipse at 90% 90%,
                    #e0eaff 0, transparent 38%),
                #f5f8ff;
        }

        .signin-page {
            min-height: 100vh;
            display: flex;
            flex-direction: column;
        }

        .topbar {
            min-height: 76px;
            padding: 12px 5%;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 16px;
            background: rgba(255,255,255,.9);
            border-bottom: 1px solid var(--border);
        }

        .brand {
            display: flex;
            align-items: center;
            gap: 12px;
            text-decoration: none;
            color: var(--text);
        }

        .brand-icon {
            width: 46px;
            height: 46px;
            display: grid;
            place-items: center;
            border-radius: 13px;
            background: #dbeafe;
            font-size: 25px;
        }

        .brand-name {
            display: block;
            font-size: 18px;
            font-weight: 800;
        }

        .brand-tagline {
            display: block;
            margin-top: 3px;
            color: var(--muted);
            font-size: 12px;
        }

        .home-link {
            color: #334155;
            font-size: 14px;
            font-weight: 600;
            text-decoration: none;
            padding: 10px 14px;
            border-radius: 10px;
        }

        .home-link:hover {
            background: #eff6ff;
            color: var(--blue);
        }

        .main-layout {
            width: 100%;
            max-width: 1180px;
            margin: auto;
            padding: 48px 24px;
            display: grid;
            grid-template-columns: minmax(0, 1fr) minmax(390px, .9fr);
            align-items: center;
            gap: 56px;
        }

        .intro { padding: 12px 0; }

        .eyebrow {
            display: inline-block;
            padding: 9px 14px;
            margin-bottom: 22px;
            border-radius: 30px;
            background: #dbeafe;
            color: #1d4ed8;
            font-size: 12px;
            font-weight: 800;
            letter-spacing: .5px;
        }

        .intro h1 {
            max-width: 550px;
            margin: 0 0 20px;
            font-size: clamp(36px, 4vw, 53px);
            line-height: 1.15;
            font-weight: 800;
            letter-spacing: -1.5px;
        }

        .intro h1 span { color: var(--blue); }

        .intro-text {
            max-width: 490px;
            color: var(--muted);
            font-size: 16px;
            line-height: 1.8;
            margin-bottom: 26px;
        }

        .benefits {
            list-style: none;
            padding: 0;
            margin: 0;
            display: grid;
            gap: 14px;
        }

        .benefits li {
            display: flex;
            align-items: center;
            gap: 12px;
            color: #334155;
            font-size: 14px;
        }

        .check {
            flex: 0 0 28px;
            width: 28px;
            height: 28px;
            display: grid;
            place-items: center;
            border-radius: 50%;
            background: #dbeafe;
            color: #1d4ed8;
            font-weight: 800;
        }

        .signin-card {
            width: 100%;
            max-width: 500px;
            justify-self: end;
            overflow: hidden;
            background: #fff;
            border: 1px solid rgba(219,229,241,.95);
            border-radius: 22px;
            box-shadow: 0 24px 65px rgba(30,64,175,.13);
        }

        .card-accent {
            height: 7px;
            background: linear-gradient(90deg, #2563eb, #60a5fa, #bfdbfe);
        }

        .card-content { padding: 36px 38px 30px; }

        .form-heading { margin-bottom: 28px; }

        .form-heading h2 {
            margin: 0 0 9px;
            font-size: 30px;
            font-weight: 800;
            letter-spacing: -.6px;
        }

        .form-heading p {
            margin: 0;
            color: var(--muted);
            font-size: 14px;
            line-height: 1.7;
        }

        .field { margin-bottom: 22px; }

        .field-label {
            display: block;
            margin-bottom: 9px;
            color: #334155;
            font-size: 14px;
            font-weight: 700;
        }

        .input-wrap { position: relative; }

        .input-icon {
            position: absolute;
            top: 50%;
            left: 15px;
            transform: translateY(-50%);
            pointer-events: none;
            font-size: 17px;
        }

        .form-input {
            display: block;
            width: 100%;
            min-height: 54px;
            padding: 13px 15px 13px 46px;
            border: 1px solid var(--border);
            border-radius: 11px;
            background: #fbfdff;
            color: var(--text);
            font: inherit;
            font-size: 15px;
            outline: none;
            transition: border-color .2s, box-shadow .2s;
        }

        .form-input::placeholder { color: #94a3b8; }

        .form-input:focus {
            border-color: #60a5fa;
            background: #fff;
            box-shadow: 0 0 0 4px rgba(59,130,246,.12);
        }

        .validation-message {
            display: block;
            margin-top: 7px;
            color: #dc2626;
            font-size: 12px;
        }

        .signin-message {
            display: block;
            margin-bottom: 18px;
            padding: 12px 14px;
            border-radius: 10px;
            overflow-wrap: anywhere;
            font-size: 14px;
            line-height: 1.5;
        }

        .signin-message:empty { display: none; }

        .signin-button {
            display: block;
            width: 100%;
            min-height: 54px;
            padding: 13px 18px;
            border: 0;
            border-radius: 11px;
            background: var(--blue);
            color: #fff;
            font: inherit;
            font-size: 15px;
            font-weight: 700;
            cursor: pointer;
            box-shadow: 0 8px 18px rgba(37,99,235,.2);
            transition: background .2s, transform .2s;
        }

        .signin-button:hover {
            background: var(--blue-dark);
            transform: translateY(-1px);
        }

        .signin-button:focus-visible,
        .home-link:focus-visible,
        .footer-links a:focus-visible {
            outline: 3px solid #93c5fd;
            outline-offset: 3px;
        }

        .footer-links {
            margin-top: 24px;
            padding-top: 20px;
            border-top: 1px solid #edf2f7;
            text-align: center;
            color: var(--muted);
            font-size: 14px;
            line-height: 2;
        }

        .footer-links a {
            color: var(--blue);
            font-weight: 700;
            text-decoration: none;
        }

        .footer-links a:hover { text-decoration: underline; }

        .card-footer {
            padding: 0 20px 23px;
            color: #94a3b8;
            font-size: 12px;
            text-align: center;
        }

        @media (max-width: 900px) {
            .main-layout {
                max-width: 650px;
                grid-template-columns: minmax(0, 1fr);
                gap: 30px;
                padding: 36px 22px;
            }

            .intro { text-align: center; }

            .intro h1,
            .intro-text {
                margin-left: auto;
                margin-right: auto;
            }

            .intro h1 { font-size: 40px; }

            .benefits {
                max-width: 380px;
                margin: auto;
                text-align: left;
            }

            .signin-card {
                max-width: 530px;
                justify-self: center;
            }
        }

        @media (max-width: 575px) {
            .topbar {
                min-height: 66px;
                padding: 10px 14px;
            }

            .brand { gap: 9px; }

            .brand-icon {
                width: 40px;
                height: 40px;
                font-size: 22px;
            }

            .brand-name { font-size: 15px; }
            .brand-tagline { font-size: 11px; }

            .home-link {
                padding: 9px 8px;
                font-size: 12px;
            }

            .main-layout {
                padding: 28px 15px;
                gap: 26px;
            }

            .eyebrow {
                margin-bottom: 16px;
                font-size: 10px;
            }

            .intro h1 {
                font-size: 32px;
                letter-spacing: -.7px;
            }

            .intro-text {
                font-size: 14px;
                line-height: 1.7;
                margin-bottom: 20px;
            }

            .benefits { gap: 11px; }
            .benefits li { font-size: 13px; }

            .signin-card { border-radius: 17px; }

            .card-content { padding: 27px 21px 24px; }

            .form-heading h2 { font-size: 27px; }
            .form-heading { margin-bottom: 24px; }
            .form-input, .signin-button { min-height: 51px; }
            .field { margin-bottom: 19px; }
        }

        @media (prefers-reduced-motion: reduce) {
            *, *::before, *::after {
                transition: none !important;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server" class="signin-page">

        <header class="topbar">
            <a href="Default.aspx" class="brand">
                <span class="brand-icon" aria-hidden="true">📚</span>
                <span>
                    <span class="brand-name">Haseen Books Store</span>
                    <span class="brand-tagline">Stories Worth Reading</span>
                </span>
            </a>

            <a href="Default.aspx" class="home-link">
                &larr; Back to Store
            </a>
        </header>

        <main class="main-layout">
            <section class="intro">
                <div class="eyebrow">YOUR NEXT CHAPTER STARTS HERE</div>

                <h1>
                    Welcome back to your
                    <span>reading world.</span>
                </h1>

                <p class="intro-text">
                    Sign in to explore your bookstore account, discover
                    your next favourite book, and keep track of your orders
                    in one convenient place.
                </p>

                <ul class="benefits">
                    <li><span class="check">✓</span> Discover books and new stories</li>
                    <li><span class="check">✓</span> Manage your account and orders</li>
                    <li><span class="check">✓</span> Enjoy a convenient shopping experience</li>
                </ul>
            </section>

            <section class="signin-card" aria-labelledby="signinHeading">
                <div class="card-accent"></div>

                <div class="card-content">
                    <div class="form-heading">
                        <h2 id="signinHeading">Sign in</h2>
                        <p>Enter your account details to continue.</p>
                    </div>

                    <asp:Label ID="MessageLabel" runat="server"
                        EnableViewState="false"
                        CssClass="signin-message"
                        role="alert" />

                    <div class="field">
                        <asp:Label runat="server"
                            AssociatedControlID="EmailTextBox"
                            CssClass="field-label"
                            Text="Email address" />

                        <div class="input-wrap">
                            <span class="input-icon" aria-hidden="true">✉</span>
                            <asp:TextBox ID="EmailTextBox" runat="server"
                                CssClass="form-input"
                                TextMode="Email"
                                MaxLength="256"
                                placeholder="you@example.com"
                                autocomplete="username" />
                        </div>

                        <asp:RequiredFieldValidator runat="server"
                            ControlToValidate="EmailTextBox"
                            ErrorMessage="Please enter your email address."
                            CssClass="validation-message"
                            Display="Dynamic" />
                    </div>

                    <div class="field">
                        <asp:Label runat="server"
                            AssociatedControlID="PasswordTextBox"
                            CssClass="field-label"
                            Text="Password" />

                        <div class="input-wrap">
                            <span class="input-icon" aria-hidden="true">🔒</span>
                            <asp:TextBox ID="PasswordTextBox" runat="server"
                                CssClass="form-input"
                                TextMode="Password"
                                placeholder="Enter your password"
                                autocomplete="current-password" />
                        </div>

                        <asp:RequiredFieldValidator runat="server"
                            ControlToValidate="PasswordTextBox"
                            ErrorMessage="Please enter your password."
                            CssClass="validation-message"
                            Display="Dynamic" />
                    </div>

                    <asp:Button ID="SignInButton" runat="server"
                        Text="Sign In to Your Account  →"
                        CssClass="signin-button"
                        OnClick="SignInButton_Click" />

                    <div class="footer-links">
                        Don't have an account?
                        <a href="SignUp.aspx">Create an account</a>
                        <div>
                            <a href="Default.aspx">Continue browsing the store</a>
                        </div>
                    </div>
                </div>

                <div class="card-footer">
                    Haseen Books Store · Stories Worth Reading
                </div>
            </section>
        </main>

    </form>
</body>
</html>
