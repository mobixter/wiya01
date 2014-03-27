using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContentAdmin
{
    public partial class Header : System.Web.UI.Page
    {
        private Usuario objUsuario = Usuario.Instance;

        protected override void OnInit(EventArgs e)
        {
            if (Session["validado"] == null)
                Response.Redirect("~/Login.aspx");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrHeaderId.Mensaje = "Welcome " + objUsuario.NombreSp + ", Last entry date: " + objUsuario.FechaUltLogin.ToString();
            ctrHeaderId.Inicio();
        }
    }
}