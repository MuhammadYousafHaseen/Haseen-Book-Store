
using System;
using BookStoreWebForms.Data;

namespace BookStoreWebForms
{
    public partial class Categories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryRepeater.DataSource =
                    new CategoryService().GetActiveCategories();

                CategoryRepeater.DataBind();
            }
        }
    }
}
