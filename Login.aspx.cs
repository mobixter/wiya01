using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

namespace ContentAdmin
{
    public partial class Login : System.Web.UI.Page
    {
        CryptorEngine objCryptor = new CryptorEngine();
        private Usuario objUsuario = Usuario.Instance;
        private OperacionRol objOperacionRol = OperacionRol.Instance;
        private DataTable tableOperacionRol = new DataTable();
        private Rol objRol = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //string clave = objCryptor.Encriptar("adminwilaen");
            //clave = "";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DataTable dtUser = new DataTable();
            if (Page.IsPostBack)
            {

                //System.Data.SqlClient.SqlConnection Conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DB_ServiceAdmin"].ConnectionString);
                //    Conn.Open();
                

                
                //System.Data.SqlClient.SqlDataAdapter Dap = new System.Data.SqlClient.SqlDataAdapter("Select * from usuario  where strLogin_Usario='" + txtLogin.Text + "' ", Conn );
                //Dap.Fill(dtUser);




                objUsuario.Dispose();
                objUsuario.Login = txtLogin.Text;
                objUsuario.Clave = objCryptor.Encriptar(txtPassword.Text);
                objUsuario = objUsuario.VerifyUser();

                if (objUsuario != null)
                {
                    objRol = new Rol(objUsuario.Rol.Id);
                    objOperacionRol.ObjRol = objRol;
                    tableOperacionRol = objOperacionRol.TableOperacionRol;
                    if (tableOperacionRol != null)
                    {
                        if (tableOperacionRol.Rows.Count > 0)
                        {
                            objUsuario.WriteEntry(objUsuario.Id);
                            Session["validado"] = true;
                            Response.Redirect("Index.aspx", false);
                        }
                        else
                            lblError.Visible = true;
                    }
                    else
                        lblError.Visible = true;
                }
                else
                    lblError.Visible = true;
            }
        }
    }
}