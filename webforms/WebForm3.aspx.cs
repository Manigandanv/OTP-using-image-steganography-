using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication4
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnsubmit(object sender, EventArgs e)
        {
            //String un = Request.Form["login"];
            //String pw = Request.Form["password"];
            String strConnString = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=paytmdata;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
            SqlConnection con = new SqlConnection(strConnString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from paytm where name=@username and pwd=@password", con);
            cmd.Parameters.AddWithValue("@username", txtUserName.Text);
            cmd.Parameters.AddWithValue("@password", txtPWD.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                //Response.Write("correct");
                Response.Redirect("CCValidation.html");
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Username and Password')</script>");
            }
        }
    }
}