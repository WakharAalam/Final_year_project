using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using ClosedXML.Excel;
using System.Reflection.Emit;

namespace Project1
{
    public partial class completed : System.Web.UI.Page
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataSet ds = new DataSet();
        protected void page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("login.aspx");

            }
            else
                con.ConnectionString = "Data Source = Wakhar_Aalam; Initial Catalog = sample; Integrated Security = True";
            con.Open();
            showdata();

            string mailid = Session["user"].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["Wakhar_Aalam"].ConnectionString; // Replace with your database connection string

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT cc.EMP_ID, cc.Training_id " +
               "FROM course_completion cc " +
               "LEFT JOIN Employee_Details ed ON cc.EMP_ID = ed.EMP_ID " +
               "WHERE ed.EMP_EMAIL = @mailid";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@mailid", mailid);
                connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                GridView1.DataSource = dataTable;
                GridView1.DataBind();
            }

        }

        protected void linkServerClick_ServerClick(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx");
        }

        public void showdata()
        {
            cmd.CommandText = "select * from emp_credentials where EMP_EMAIL='" + Session["user"] + " ' ";
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            sda.Fill(ds);
            // label1.Text= ds.Tables[0].Rows[0]["AdminName"].ToString();
            label2.Text = ds.Tables[0].Rows[0]["EMP_EMAIL"].ToString();
        }
    }
}
