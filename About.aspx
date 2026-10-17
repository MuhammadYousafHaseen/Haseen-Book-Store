
<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>About Us | Haseen Books Store</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description"
          content="Learn about Haseen Books Store, our mission, values, and commitment to readers." />

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

        .about-hero {
            background: linear-gradient(135deg, #e6f2ff, #fff);
            border-radius: 24px;
            padding: 65px 30px;
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

        .content-card {
            height: 100%;
            background: #fff;
            border: 1px solid #e4ecf5;
            border-radius: 18px;
            padding: 28px;
        }

        .content-card p,
        .body-copy {
            color: #617086;
            line-height: 1.9;
        }

        .value-icon {
            font-size: 30px;
            margin-bottom: 12px;
        }

        .about-cta {
            background: #173f6b;
            color: #fff;
            border-radius: 22px;
            padding: 38px;
        }

        .about-cta p {
            color: #dce9f8;
            line-height: 1.8;
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
                <a class="nav-link active" aria-current="page" href="About.aspx">About</a>
                <a class="nav-link" href="Contact.aspx">Contact</a>
            </div>
        </div>
    </div>
</nav>

<main class="container py-4 py-lg-5">

    <section class="about-hero mb-5">
        <span class="eyebrow">Our Story</span>
        <h1 class="display-4 fw-bold mt-3 mb-4"
            style="color:#16365c;">
            Welcome to Haseen Books Store
        </h1>

        <p class="lead text-secondary mb-4" style="max-width:850px;line-height:1.9;">
            We believe books can inspire imagination, encourage learning,
            introduce fresh perspectives, and bring people closer to the
            ideas that matter to them.
        </p>

        <p class="body-copy mb-4" style="max-width:850px;">
            Haseen Books Store is an online bookstore experience designed
            to make discovering and shopping for books straightforward.
            Our goal is to provide a welcoming place where readers can
            explore available titles, compare details, and manage their
            book purchases conveniently.
        </p>

        <a href="Books.aspx" class="btn btn-primary btn-lg px-4">
            Explore Our Books
        </a>
    </section>

    <section class="mb-5">
        <div class="row g-4 align-items-start">
            <div class="col-lg-6">
                <span class="eyebrow">Why We Exist</span>
                <h2 class="section-title mt-2 mb-3">Our Mission</h2>

                <p class="body-copy">
                    Our mission is to make book discovery simple and
                    enjoyable. We want readers to be able to find books
                    that match their interests, explore different subjects,
                    and move through the shopping process with confidence.
                </p>

                <p class="body-copy">
                    Whether someone is reading for education, professional
                    development, entertainment, or personal interest,
                    we want their experience with our store to feel clear,
                    welcoming, and convenient.
                </p>
            </div>

            <div class="col-lg-6">
                <span class="eyebrow">Our Direction</span>
                <h2 class="section-title mt-2 mb-3">Our Vision</h2>

                <p class="body-copy">
                    We aspire to build a dependable online destination
                    for book lovers, students, and lifelong learners.
                    We want to keep improving how customers discover
                    books, review their selections, and access their
                    order information.
                </p>

                <p class="body-copy">
                    As the store grows, our focus is on useful features,
                    clear information, a user-friendly design, and
                    thoughtful customer support.
                </p>
            </div>
        </div>
    </section>

    <section class="mb-5">
        <div class="text-center mb-4">
            <span class="eyebrow">What Matters to Us</span>
            <h2 class="section-title mt-2">The Values Behind Our Store</h2>
            <p class="body-copy">
                The principles that guide the experience we aim to provide.
            </p>
        </div>

        <div class="row g-4">
            <div class="col-md-4">
                <div class="content-card">
                    <div class="value-icon">📚</div>
                    <h3 class="h5 fw-bold">Love of Learning</h3>
                    <p class="mb-0">
                        Books help us explore ideas, develop skills,
                        and see the world from different perspectives.
                        We welcome every reader's curiosity.
                    </p>
                </div>
            </div>

            <div class="col-md-4">
                <div class="content-card">
                    <div class="value-icon">🤝</div>
                    <h3 class="h5 fw-bold">Customer First</h3>
                    <p class="mb-0">
                        Clear book information, accessible navigation,
                        and straightforward shopping are important parts
                        of a helpful customer experience.
                    </p>
                </div>
            </div>

            <div class="col-md-4">
                <div class="content-card">
                    <div class="value-icon">✨</div>
                    <h3 class="h5 fw-bold">Continuous Improvement</h3>
                    <p class="mb-0">
                        We believe a good online store should keep
                        improving its usability, information, and
                        features as customer needs evolve.
                    </p>
                </div>
            </div>
        </div>
    </section>

    <section class="mb-5">
        <h2 class="section-title mb-3">A Simple Way to Shop for Books</h2>

        <div class="content-card">
            <div class="row g-4">
                <div class="col-md-4">
                    <h3 class="h6 fw-bold">1. Discover</h3>
                    <p class="body-copy mb-0">
                        Browse the catalog and search by title or author.
                        Use category filters to narrow down your choices.
                    </p>
                </div>

                <div class="col-md-4">
                    <h3 class="h6 fw-bold">2. Choose</h3>
                    <p class="body-copy mb-0">
                        Review available book details, prices, and stock
                        information before adding a title to your cart.
                    </p>
                </div>

                <div class="col-md-4">
                    <h3 class="h6 fw-bold">3. Keep Track</h3>
                    <p class="body-copy mb-0">
                        Use your cart and orders pages to review your
                        selections and access your purchase information.
                    </p>
                </div>
            </div>
        </div>
    </section>

    <section class="about-cta text-center">
        <h2 class="fw-bold mb-3">Every Reader Has a Story.</h2>
        <p class="mb-4">
            Start your next chapter with a book that interests you.
            We look forward to being part of your reading journey.
        </p>

        <div class="d-flex justify-content-center flex-wrap gap-2">
            <a href="Books.aspx" class="btn btn-light btn-lg px-4">
                Browse Books
            </a>

            <a href="Contact.aspx"
               class="btn btn-outline-light btn-lg px-4">
                Contact Us
            </a>
        </div>
    </section>

</main>

<footer class="mt-5 py-4 bg-white">
    <div class="container text-center small">
        <div class="fw-semibold">Haseen Books Store</div>
        <div class="mt-1">
            A welcoming place for readers and lifelong learners.
        </div>
        <div class="mt-2">
            <a href="Default.aspx" class="text-decoration-none">Home</a>
            <span class="mx-2">·</span>
            <a href="Books.aspx" class="text-decoration-none">Books</a>
            <span class="mx-2">·</span>
            <a href="Contact.aspx" class="text-decoration-none">Contact</a>
        </div>
    </div>
</footer>

</form>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
