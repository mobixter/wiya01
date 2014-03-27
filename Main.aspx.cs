using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContentAdmin
{
    public partial class Main : System.Web.UI.Page
    {
        private Funciones objFunciones = new Funciones();

        protected override void OnInit(EventArgs e)
        {
            if (Session["validado"] == null)
                Response.Redirect("~/Login.aspx");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}