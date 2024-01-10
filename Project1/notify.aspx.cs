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
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;

namespace Project1
{
    public partial class notify : System.Web.UI.Page
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
        }
        protected void SaveBtn_Click1(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Wakhar_Aalam"].ConnectionString;

            string trainingID = textbox1.Text;
            string mailid = Session["user"].ToString();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string query = "SELECT EMP_ID FROM Employee_Details WHERE EMP_EMAIL = @mailid";

                SqlCommand command = new SqlCommand(query, myConnection);

                command.Parameters.AddWithValue("@mailid", mailid);
                myConnection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                string stringValue = dataTable.Rows[0]["EMP_ID"].ToString();

                try
                {

                    string checkQuery = "SELECT COUNT(*) FROM [dbo].[course_completion] WHERE Training_id = @TrainingID AND EMP_ID = @stringValue";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, myConnection))
                    {
                        checkCommand.Parameters.AddWithValue("@TrainingID", trainingID);
                        checkCommand.Parameters.AddWithValue("@stringValue", stringValue);
                        int existingCount = (int)checkCommand.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            YourLabel.Visible = true;
                            YourLabel.Text = "Training already Completed";
                            
                        }
                        else
                        {
                            string insertQuery = "INSERT INTO [dbo].[course_completion] (Training_id, EMP_ID) VALUES (@TrainingID, @stringValue)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, myConnection))
                            {
;
                                insertCommand.Parameters.AddWithValue("@TrainingID", trainingID);
                                insertCommand.Parameters.AddWithValue("@stringValue", stringValue);
                                insertCommand.ExecuteNonQuery();
                                YourLabel.Visible = true;
                                YourLabel.Text = "Course Completion Notified Successfully!";
                                Response.Redirect("completed.aspx");

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    YourLabel.Visible = true;
                    YourLabel.Text = "An error occurred: " + ex.Message;
                }
                myConnection.Close();
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
