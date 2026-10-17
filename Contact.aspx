
<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Contact Us | Haseen Books Store</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description"
          content="Contact Haseen Books Store for questions about books, orders, and shopping assistance." />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
          rel="stylesheet" />

    <style>
        body {
            background: #f7faff;
            color: #27364a;
        }

        .navbar-brand {
            color: #1769aa !important;
            font-weight: 800;
        }

        .contact-hero {
            background: linear-gradient(135deg, #e6f2ff, #fff);
            padding: 55px 30px;
            border-radius: 24px;
        }

        .eyebrow {
            color: #1769aa;
            font-size: 12px;
            font-weight: 800;
            letter-spacing: 2px;
            text-transform: uppercase;
        }

        .section-title {
            color: #16365c;
            font-weight: 800;
        }

        .contact-card {
            background: #fff;
            border: 1px solid #e4ecf5;
            border-radius: 18px;
            padding: 28px;
            height: 100%;
            overflow-wrap: anywhere;
        }

        .contact-icon {
            display: flex;
            align-items: center;
            justify-content: center;
            width: 52px;
            height: 52px;
            background: #eaf4ff;
            border-radius: 14px;
            font-size: 25px;
            margin-bottom: 18px;
        }

        .muted-copy {
            color: #64748b;
            line-height: 1.85;
        }

        .contact-link {
            color: #1769aa;
            font-weight: 600;
            text-decoration: none;
        }

        .contact-link:hover {
            text-decoration: underline;
        }

        .faq-item {
            background: #fff;
            border: 1px solid #e4ecf5;
            border-radius: 14px;
            padding: 22px;
            height: 100%;
        }

        footer {
            color: #68778b;
            border-top: 1px solid #e4ecf5;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

<nav class="navbar navbar-expand-lg bg-white border-bottom shadow-sm">
    <div class="container">
        <a class="navbar-brand" href="Default.aspx">Haseen Books Store</a>

        <button class="navbar-toggler" type="button"
                data-bs-toggle="collapse"
                data-bs-target="#mainNav"
                aria-controls="mainNav"
                aria-expanded="false"
                aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="mainNav">
            <div class="navbar-nav ms-auto align-items-lg-center gap-lg-2">
                <a class="nav-link" href="Default.aspx">Home</a>
                <a class="nav-link" href="Books.aspx">Browse Books</a>
                <a class="nav-link" href="Categories.aspx">Categories</a>
                <a class="nav-link" href="Cart.aspx">Shopping Cart</a>
                <a class="nav-link" href="MyOrders.aspx">My Orders</a>
                <a class="nav-link" href="About.aspx">About</a>
                <a class="nav-link active" aria-current="page"
                   href="Contact.aspx">Contact</a>
            </div>
        </div>
    </div>
</nav>

<main class="container py-4 py-lg-5">

    <section class="contact-hero mb-5">
        <span class="eyebrow">We'd Love to Hear from You</span>

        <h1 class="display-4 fw-bold mt-3 mb-3"
            style="color:#16365c;">
            How Can We Help?
        </h1>

        <p class="lead text-secondary mb-4"
           style="max-width:800px;line-height:1.8;">
            Have a question about a book, need help with your order,
            or want to share feedback? Contact Haseen Books Store.
            We're happy to hear from our readers.
        </p>

        <a href="#contact-details" class="btn btn-primary btn-lg px-4">
            Get in Touch
        </a>
    </section>

    <section id="contact-details" class="mb-5">
        <div class="text-center mb-4">
            <span class="eyebrow">Contact Information</span>
            <h2 class="section-title mt-2">Choose How to Reach Us</h2>
            <p class="muted-copy">
                Use the contact method that works best for you.
            </p>
        </div>

        <div class="row g-4">
            <div class="col-md-6">
                <div class="contact-card">
                    <div class="contact-icon">📞</div>

                    <h3 class="h5 fw-bold">Call Us</h3>

                    <p class="muted-copy">
                        Have a quick question about our bookstore?
                        You can reach out using the phone number below.
                    </p>

                    <p class="mb-3">
                        <a class="contact-link"
                           href="tel:+923218971071">
                            +92 321 8971071
                        </a>
                    </p>

                    <a href="tel:+923218971071"
                       class="btn btn-outline-primary">
                        Call Now
                    </a>

                    <p class="small text-secondary mt-3 mb-0">
                        Availability may vary. Please call to confirm
                        a suitable time to speak.
                    </p>
                </div>
            </div>

            <div class="col-md-6">
                <div class="contact-card">
                    <div class="contact-icon">✉️</div>

                    <h3 class="h5 fw-bold">Email Us</h3>

                    <p class="muted-copy">
                        For questions, suggestions, or order-related
                        enquiries, send us an email with the details
                        of how we can help.
                    </p>

                    <p class="mb-3">
                        <a class="contact-link"
                           href="mailto:muhammadyousafhaseen@gmail.com">
                            muhammadyousafhaseen@gmail.com
                        </a>
                    </p>

                    <a href="mailto:muhammadyousafhaseen@gmail.com?subject=Haseen%20Books%20Store%20Enquiry"
                       class="btn btn-primary">
                        Send an Email
                    </a>

                    <p class="small text-secondary mt-3 mb-0">
                        Include relevant order details in your message
                        when asking about an existing purchase.
                    </p>
                </div>
            </div>
        </div>
    </section>

    <section class="mb-5">
        <div class="row g-4">
            <div class="col-lg-5">
                <span class="eyebrow">Customer Support</span>
                <h2 class="section-title mt-2 mb-3">
                    We're Here to Help with Your Reading Journey
                </h2>

                <p class="muted-copy">
                    Whether you're looking for information about a title,
                    have a question about the shopping process, or want
                    to share an idea for improving our store, your
                    message is welcome.
                </p>

                <p class="muted-copy">
                    To help us understand your enquiry, please explain
                    your question clearly. If it relates to an order,
                    include the relevant order number, but never send
                    your password or other sensitive login information.
                </p>

                <a href="Books.aspx" class="btn btn-outline-primary">
                    Continue Browsing Books
                </a>
            </div>

            <div class="col-lg-7">
                <div class="contact-card">
                    <h3 class="h5 fw-bold mb-3">Before You Contact Us</h3>

                    <div class="mb-4">
                        <h4 class="h6 fw-bold">Book availability</h4>
                        <p class="muted-copy mb-0">
                            Visit the book catalog to check the available
                            titles, displayed prices, and stock information.
                        </p>
                    </div>

                    <div class="mb-4">
                        <h4 class="h6 fw-bold">Shopping cart</h4>
                        <p class="muted-copy mb-0">
                            Review your selected books and quantities
                            on the Shopping Cart page before checkout.
                        </p>
                    </div>

                    <div class="mb-4">
                        <h4 class="h6 fw-bold">Existing orders</h4>
                        <p class="muted-copy mb-0">
                            Visit My Orders to review your purchases
                            and open an order for more details.
                        </p>
                    </div>

                    <div>
                        <h4 class="h6 fw-bold">Suggestions and feedback</h4>
                        <p class="muted-copy mb-0">
                            We welcome constructive suggestions that
                            can help us make the bookstore easier to use.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="mb-5">
        <div class="text-center mb-4">
            <span class="eyebrow">Quick Answers</span>
            <h2 class="section-title mt-2">Frequently Asked Questions</h2>
        </div>

        <div class="row g-3">
            <div class="col-md-6">
                <div class="faq-item">
                    <h3 class="h6 fw-bold">
                        How do I find a particular book?
                    </h3>
                    <p class="muted-copy mb-0">
                        Open Browse Books and use the search field
                        to search by title or author. You can also
                        use the category filter.
                    </p>
                </div>
            </div>

            <div class="col-md-6">
                <div class="faq-item">
                    <h3 class="h6 fw-bold">
                        How can I check my order?
                    </h3>
                    <p class="muted-copy mb-0">
                        Open My Orders to review your purchases.
                        Sign in if your account requires authentication.
                    </p>
                </div>
            </div>

            <div class="col-md-6">
                <div class="faq-item">
                    <h3 class="h6 fw-bold">
                        What if a book is out of stock?
                    </h3>
                    <p class="muted-copy mb-0">
                        Check the catalog for availability. If you need
                        more information, contact us using the phone
                        number or email listed above.
                    </p>
                </div>
            </div>

            <div class="col-md-6">
                <div class="faq-item">
                    <h3 class="h6 fw-bold">
                        How can I share feedback?
                    </h3>
                    <p class="muted-copy mb-0">
                        Send an email describing your suggestion,
                        question, or experience with Haseen Books Store.
                    </p>
                </div>
            </div>
        </div>
    </section>

    <section class="text-center p-4 p-lg-5 bg-white border rounded-4">
        <h2 class="section-title mb-3">Thank You for Visiting</h2>

        <p class="muted-copy mx-auto mb-4" style="max-width:700px;">
            Thank you for choosing Haseen Books Store. Every question
            and suggestion gives us an opportunity to improve the
            experience we provide to our readers.
        </p>

        <div class="d-flex justify-content-center flex-wrap gap-2">
            <a href="Books.aspx" class="btn btn-primary px-4">
                Explore Books
            </a>

            <a href="About.aspx" class="btn btn-outline-primary px-4">
                Learn About Us
            </a>
        </div>
    </section>

</main>

<footer class="mt-5 py-4 bg-white">
    <div class="container text-center small">
        <div class="fw-semibold">Haseen Books Store</div>

        <div class="mt-2">
            <a href="tel:+923218971071" class="text-decoration-none">
                +92 321 8971071
            </a>
            <span class="mx-2">·</span>
            <a href="mailto:muhammadyousafhaseen@gmail.com"
               class="text-decoration-none">
                Email Us
            </a>
        </div>

        <div class="mt-2">
            © <%= DateTime.Now.Year %> Haseen Books Store.
            All rights reserved.
        </div>
    </div>
</footer>

</form>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
