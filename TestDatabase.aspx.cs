
using System;
using BookStoreWebForms.Data;

namespace BookStoreWebForms
{
    public partial class TestDatabase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var connection = Db.GetConnection())
                {
                    connection.Open();
                    Response.Write("Database connection successful!");
                }
            }
            catch (Exception ex)
            {
                Response.Write("Database connection failed: " +
                    Server.HtmlEncode(ex.Message));
            }
        }
    }
}
