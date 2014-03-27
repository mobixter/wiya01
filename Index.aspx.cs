using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ContentAdmin
{
    public partial class Index : System.Web.UI.Page
    {
        private Funciones objFunciones = new Funciones();
        private Usuario objUsuario = Usuario.Instance;
        private MenuAdm objMenu = MenuAdm.Instance;
        
        protected override void OnInit(EventArgs e)
        {
            if (Session["validado"] == null)
                Response.Redirect("~/Login.aspx");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DataTable objTable = new DataTable();
            
            objMenu.Usuario = objUsuario.Id;
            objTable = objMenu.TableMenu;
            objTable = null;
        }
    }
}