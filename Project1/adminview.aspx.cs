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
    public partial class adminview : System.Web.UI.Page
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
            string valueToSend = textbox1.Text.Trim();
            Session["ValueToSend"] = valueToSend;
            Response.Redirect("view.aspx");
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
