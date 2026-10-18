<%@ Page Language="C#" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Database Connection Test</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Database Connection Test</h2>

        <asp:Label ID="ResultLabel"
                   runat="server" />
    </form>
</body>
</html>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var setting =
                ConfigurationManager.ConnectionStrings["db74321"];

            if (setting == null ||
                string.IsNullOrWhiteSpace(setting.ConnectionString))
            {
                ResultLabel.Text =
                    "FAIL: Connection string 'db74321' is missing.";
                return;
            }

            using (var connection =
                   new SqlConnection(setting.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "SELECT DB_NAME()", connection))
                {
                    var databaseName = command.ExecuteScalar();

                    ResultLabel.Text =
                        "SUCCESS: Connected to database: " +
                        Server.HtmlEncode(
                            Convert.ToString(databaseName));
                }
            }
        }
       catch (Exception ex)
{
    ResultLabel.Text =
        "Error type: " +
        Server.HtmlEncode(ex.GetType().FullName) +
        "<br/>Error message: " +
        Server.HtmlEncode(ex.Message);
}
    }
</script>