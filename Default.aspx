
<%@ Page Title="Home - Haseen Books Store" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="BookStoreWebForms._Default" %>

<asp:Content ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

<style>
    .haseen-home {
        color: #233044;
        overflow: hidden;
    }

    .haseen-home .hero {
        background: linear-gradient(135deg, #edf6ff, #ffffff 65%, #e3f0ff);
        border-radius: 24px;
        padding: 65px 36px;
        margin-top: 25px;
        margin-bottom: 45px;
    }

    .haseen-home .eyebrow {
        color: #1769aa;
        font-weight: 700;
        letter-spacing: 2px;
        text-transform: uppercase;
        font-size: 12px;
    }

    .haseen-home .hero h1 {
        font-weight: 800;
        line-height: 1.15;
        font-size: clamp(2.2rem, 5vw, 4rem);
        color: #16365c;
    }

    .haseen-home .hero p {
        color: #5c6b7d;
        font-size: 1.1rem;
        line-height: 1.8;
        max-width: 580px;
    }

    .haseen-home .section-space {
        margin-bottom: 55px;
    }

    .haseen-home .section-title {
        color: #16365c;
        font-weight: 800;
    }

    .haseen-home .info-card {
        height: 100%;
        border: 1px solid #e6edf5;
        border-radius: 18px;
        padding: 26px;
        background: #fff;
        transition: transform .2s ease, box-shadow .2s ease;
    }

    .haseen-home .info-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 12px 30px rgba(27, 68, 110, .09);
    }

    .haseen-home .icon-box {
        width: 54px;
        height: 54px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #eaf4ff;
        border-radius: 15px;
        color: #1769aa;
        font-size: 25px;
        margin-bottom: 18px;
    }

    .haseen-home .category-link {
        display: block;
        background: #fff;
        border: 1px solid #e6edf5;
        border-radius: 14px;
        padding: 20px 12px;
        text-align: center;
        color: #244363;
        text-decoration: none;
        height: 100%;
        font-weight: 600;
    }

    .haseen-home .category-link:hover {
        background: #edf6ff;
        border-color: #a9d1fa;
        color: #075ca8;
    }

    .haseen-home .cta {
        border-radius: 22px;
        padding: 38px;
        background: #173f6b;
        color: #fff;
    }

    .haseen-home .cta p {
        color: #dce9f8;
        line-height: 1.7;
    }

    .haseen-home .muted-copy {
        color: #64748b;
        line-height: 1.8;
    }

    @media (max-width: 576px) {
        .haseen-home .hero {
            padding: 38px 22px;
            border-radius: 18px;
        }

        .haseen-home .cta {
            padding: 27px 22px;
        }
    }
</style>

<div class="haseen-home container py-3">

    <section class="hero">
        <div class="row align-items-center g-4">
            <div class="col-lg-7">
                <span class="eyebrow">Welcome to Haseen Books Store</span>

                <h1 class="mt-3 mb-4">
                    Every Book Opens a New World.
                </h1>

                <p class="mb-4">
                    Discover inspiring stories, expand your knowledge,
                    explore new ideas, and find books that match your
                    interests. Your next favourite read is waiting for you.
                </p>

                <div class="d-flex flex-wrap gap-2">
                    <a href="Books.aspx"
                       class="btn btn-primary btn-lg px-4">
                        Explore Books
                    </a>
                    <a href="BecomeSeller.aspx"
   class="btn btn-outline-primary btn-lg px-4">
    Register as a Seller
</a>

                    <a href="About.aspx"
                       class="btn btn-outline-primary btn-lg px-4">
                        About Our Store
                    </a>
                </div>

                <div class="mt-4 small text-secondary">
                    A welcoming place for readers, learners, and book lovers.
                </div>
            </div>

            <div class="col-lg-5">
                <div class="info-card">
                    <div class="icon-box">📚</div>
                    <h3 class="h4 fw-bold">Your Reading Journey Starts Here</h3>
                    <p class="muted-copy mb-3">
                        Browse our collection, compare titles and prices,
                        and add available books to your shopping cart.
                    </p>
                    <a href="Books.aspx" class="fw-semibold text-primary">
                        Start browsing →
                    </a>
                </div>
            </div>
        </div>
    </section>

    <section class="section-space">
        <div class="text-center mb-4">
            <span class="eyebrow">Find Your Next Read</span>
            <h2 class="section-title mt-2">Explore the Possibilities</h2>
            <p class="muted-copy">
                Whatever your interests, make time for a good book.
            </p>
        </div>

        <div class="row g-3">
            <div class="col-6 col-md-3">
                <a href="Books.aspx" class="category-link">
                    <div class="fs-2 mb-2">📖</div>
                    Fiction & Stories
                    <div class="small text-secondary mt-1">
                        Discover new worlds
                    </div>
                </a>
            </div>

            <div class="col-6 col-md-3">
                <a href="Books.aspx" class="category-link">
                    <div class="fs-2 mb-2">🎓</div>
                    Education
                    <div class="small text-secondary mt-1">
                        Learn something new
                    </div>
                </a>
            </div>

            <div class="col-6 col-md-3">
                <a href="Books.aspx" class="category-link">
                    <div class="fs-2 mb-2">💡</div>
                    Personal Growth
                    <div class="small text-secondary mt-1">
                        Build your knowledge
                    </div>
                </a>
            </div>

            <div class="col-6 col-md-3">
                <a href="Categories.aspx" class="category-link">
                    <div class="fs-2 mb-2">🗂️</div>
                    All Categories
                    <div class="small text-secondary mt-1">
                        Browse available topics
                    </div>
                </a>
            </div>
        </div>
    </section>

    <section class="section-space">
        <div class="text-center mb-4">
            <span class="eyebrow">A Better Shopping Experience</span>
            <h2 class="section-title mt-2">Books, Made Simple</h2>
        </div>

        <div class="row g-4">
            <div class="col-md-4">
                <div class="info-card">
                    <div class="icon-box">🔎</div>
                    <h3 class="h5 fw-bold">Easy Discovery</h3>
                    <p class="muted-copy">
                        Search by book title or author and use category
                        filters to find books that interest you.
                    </p>
                    <a href="Books.aspx">Browse the collection</a>
                </div>
            </div>

            <div class="col-md-4">
                <div class="info-card">
                    <div class="icon-box">🛒</div>
                    <h3 class="h5 fw-bold">Convenient Shopping</h3>
                    <p class="muted-copy">
                        Add available books to your cart and review
                        your selections before proceeding to checkout.
                    </p>
                    <a href="Cart.aspx">View your cart</a>
                </div>
            </div>

            <div class="col-md-4">
                <div class="info-card">
                    <div class="icon-box">📦</div>
                    <h3 class="h5 fw-bold">Order Information</h3>
                    <p class="muted-copy">
                        Visit your orders page to review your purchases
                        and see the order information available to you.
                    </p>
                    <a href="MyOrders.aspx">View my orders</a>
                </div>
            </div>
        </div>
    </section>

    <section class="section-space">
        <div class="cta">
            <div class="row align-items-center g-4">
                <div class="col-lg-8">
                    <h2 class="fw-bold">A Little Reading Can Go a Long Way.</h2>
                    <p class="mb-0">
                        Make your next book part of a new adventure.
                        Explore our collection and find something worth reading.
                    </p>
                </div>
                <div class="col-lg-4 text-lg-end">
                    <a href="Books.aspx"
                       class="btn btn-light btn-lg px-4">
                        Find a Book
                    </a>
                </div>
            </div>
        </div>
    </section>

    <section class="text-center pb-5">
        <h2 class="section-title">We're Here to Help</h2>
        <p class="muted-copy">
            Have a question about our store or need assistance?
            We would be happy to hear from you.
        </p>
        <a href="Contact.aspx" class="btn btn-outline-primary px-4">
            Contact Us
        </a>
    </section>

</div>

</asp:Content>
