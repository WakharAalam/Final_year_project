using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Office.Word;
using System.Drawing;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace Project1
{
    public partial class view1 : System.Web.UI.Page
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataSet ds = new DataSet();
        string receivedValue = string.Empty;
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
            if (Session["ValueToSend"] != null)
            {
                receivedValue = Session["ValueToSend"].ToString();


                string connectionString = ConfigurationManager.ConnectionStrings["Wakhar_Aalam"].ConnectionString; // Replace with your database connection string

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Employee_details.EMP_ID, Employee_details.EMP_EMAIL, CompletionDetails.Training_ID FROM Employee_details LEFT JOIN(SELECT * FROM course_completion WHERE Training_ID = @receivedValue) AS CompletionDetails ON Employee_details.EMP_ID = CompletionDetails.EMP_ID"; // Replace YourTableName with your actual table name

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ReceivedValue", receivedValue);
                    connection.Open();

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataTable.Columns.Add("Status", typeof(string));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        String valueA = Convert.ToString(row["Training_ID"]);

                        // Perform comparison and update "ResultColumn" accordingly
                        if (valueA == receivedValue)
                        {
                            row["Status"] = "Completed";
                        }
                        else
                        {
                            row["Training_ID"] = receivedValue;
                            row["Status"] = "Not Completed";
                        }
                    }

                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                }

                //Session.Remove("ValueToSend"); // Optionally remove from session
            }

        }
        protected void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewData.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    // Replace GridView1 with the ID of your GridView
                    GridView1.RenderControl(hw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }

        // Override Render method to avoid "Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server" error
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {

            List<string> mailValues = new List<string>();

            foreach (GridViewRow row in GridView1.Rows)
            {
                // Get the cell value of the current column
                string mailVal = row.Cells[1].Text;
                string statusVal = row.Cells[3].Text;

                if (statusVal == "Not Completed")
                {
                    mailValues.Add(mailVal);
                }

            }

            string subject = "Incomplete Course Notification";
            string body = "You have not completed the course :" + receivedValue+". Kindly Complete the course at the earliest.";

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

                foreach (string recipientEmail in mailValues)
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
        protected void linkServerClick_ServerClick(object sender, EventArgs e)
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