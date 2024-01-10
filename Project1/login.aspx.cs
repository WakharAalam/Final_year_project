using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project1
{
    public partial class login : System.Web.UI.Page
    {
        SqlDataAdapter sda = new SqlDataAdapter();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Response.Redirect("https://localhost:44308/empview.aspx");
            }
            string connectionString = ConfigurationManager.ConnectionStrings["Wakhar_Aalam"].ConnectionString;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string user = TextBox1.Text.Trim();
            string userEmail = TextBox1.Text;
            string password = TextBox2.Text;
            string connectionString = ConfigurationManager.ConnectionStrings["Wakhar_Aalam"].ConnectionString;
            using (SqlConnection myConnection = new SqlConnection(connectionString))

            {
                string query = "SELECT COUNT(*) FROM emp_credentials WHERE EMP_EMAIL = @Email AND Password = @Password";

                SqlCommand command = new SqlCommand(query, myConnection);
                command.Parameters.AddWithValue("@Email", userEmail);
                command.Parameters.AddWithValue("@Password", password);

                myConnection.Open();

                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 1)
                {
                    Session["user"] = user;
                    Response.Redirect("https://localhost:44308/empview.aspx");
                }
                else
                {
                    // Display an error message for wrong credentials
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid credentials!')", true);
                }
            }
        }
    }
}