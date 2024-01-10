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
using System.Net.Mail;
using System.Net;

namespace Project1
{
    public partial class update : System.Web.UI.Page
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataSet ds = new DataSet();
        protected void page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("Admin_login.aspx");

            }
            else
                con.ConnectionString = "Data Source = Wakhar_Aalam; Initial Catalog = sample; Integrated Security = True";
            con.Open();
            showdata();
        }
        protected void SaveBtn_Click1(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Wakhar_Aalam"].ConnectionString;
            string trainingName = textbox2.Text;
            string trainingID = textbox1.Text;
            string comment = textbox3.Text;

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                try
                {
                    myConnection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM [dbo].[Training_Details] WHERE Training_id = @TrainingID";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, myConnection))
                    {
                        checkCommand.Parameters.AddWithValue("@TrainingID", trainingID);
                        int existingCount = (int)checkCommand.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            // Training ID already exists, show a message to the user
                            YourLabel.Text = "Training ID already exists!";
                            YourLabel.Visible = true;
                        }
                        else
                        {
                            string insertQuery = "INSERT INTO [dbo].[Training_Details] (Training_name, Training_id, comments) VALUES (@TrainingName, @TrainingID, @comments)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, myConnection))
                            {
                                insertCommand.Parameters.AddWithValue("@TrainingName", trainingName);
                                insertCommand.Parameters.AddWithValue("@TrainingID", trainingID);
                                insertCommand.Parameters.AddWithValue("@comments", comment);
                                insertCommand.ExecuteNonQuery();
                                YourLabel.Visible = true;
                                YourLabel.Text = "Details Added Successfully!";

                            }
                            string myString = "active";
                            checkQuery = "SELECT DISTINCT EMP_EMAIL FROM [dbo].[Employee_details] WHERE EMP_STATUS = @myString";
                            using (SqlCommand checkCommand1 = new SqlCommand(checkQuery, myConnection))
                            {
                                checkCommand1.Parameters.AddWithValue("@myString", myString);

                                SqlDataReader reader = checkCommand1.ExecuteReader();

                                // Check if the reader has rows
                                if (reader.HasRows)
                                {
                                    List<string> columnValues = new List<string>();
                                    while (reader.Read())
                                    {
                                        // Retrieve the value from the specified column
                                        string value = reader["EMP_EMAIL"].ToString();

                                        // Add the value to the list
                                        columnValues.Add(value);

                                        string subject = "new Course Notification";
                                        string body = "New Course has been Added with Course ID:" + trainingID + "and Course Name:" + trainingName ;

                                        try
                                        {
                                            // Sender's email credentials
                                            string senderEmail = "wakharaalam66@gmail.com"; // Replace with your Gmail address
                                            string password = "erjtmdgqvqvssbnp";     // Replace with your Gmail password

                                            // Configure the SMTP client for Gmail
                                            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                                            {
                                                Port = 587,
                                                EnableSsl = true,
                                                UseDefaultCredentials = false,
                                                Credentials = new NetworkCredential(senderEmail, password)
                                            };

                                            foreach (string recipientEmail in columnValues)
                                            {
                                                // Create the email message for each recipient
                                                MailMessage mailMessage = new MailMessage
                                                {
                                                    From = new MailAddress(senderEmail),
                                                    Subject = subject,
                                                    Body = body,
                                                    IsBodyHtml = true
                                                };
                                                mailMessage.To.Add(recipientEmail.Trim());

                                                // Send the email to each recipient
                                                smtpClient.Send(mailMessage);
                                            }

                                            lblStatus.Text = "Email(s) sent successfully!";
                                        }
                                        catch (Exception ex)
                                        {
                                            lblStatus.Text = $"Failed to send email(s): {ex.Message}";
                                        }
                                    }
                                }
                            }


                        }

                    }
                }
                catch (Exception ex)
                {
                    YourLabel.Text = "An error occurred: " + ex.Message;
                }
            }
        }
        protected void linkServerClick_ServerClick(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Admin_login.aspx");
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Admin_login.aspx");
        }
        public void showdata()
        {
            cmd.CommandText = "select * from credentials where Email='" + Session["user"] + " ' ";
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            sda.Fill(ds);
            // label1.Text= ds.Tables[0].Rows[0]["AdminName"].ToString();
            label2.Text = ds.Tables[0].Rows[0]["Email"].ToString();
        }
    }
}
