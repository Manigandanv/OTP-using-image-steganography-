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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          String n=Request.Form["uname"];
          String p = Request.Form["pwd"];
          String em = Request.Form["email"];
          String phn = Request.Form["phone"];
          String a = Request.Form["address"];
         // String strConnString = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=paytmdata;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
          //SqlConnection conn = new SqlConnection(strConnString);
          
         //String insertCmd = "insert into pt values ('@name')";
         //String strQuery = "insert into paytm(name) values (@name)";
         String strQuery = "insert into paytm(name,pwd,email,phno,address) values (@name,@pwd,@email,@phno,@address)";
           SqlCommand cmd = new SqlCommand(strQuery);
           cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = n;
          cmd.Parameters.Add("@pwd", SqlDbType.VarChar).Value = p;
           cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = em;
           cmd.Parameters.Add("@phno", SqlDbType.VarChar).Value = phn;
           cmd.Parameters.Add("@address", SqlDbType.VarChar).Value =a;
            InsertUpdateData(cmd);
            Response.Redirect("WebForm1.aspx");
        }
      
      private Boolean InsertUpdateData(SqlCommand cmd)
        {
            String strConnString = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=paytmdata;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        
    }
}