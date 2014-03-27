using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContentAdmin
{
    public partial class LogOff : System.Web.UI.Page
    {
        private Item objItem = Item.Instance;
        private Categoria objCategoria = Categoria.Instance;
        private SubCategoria objSubCategoria = SubCategoria.Instance;
        private TipoItem objTipoItem = TipoItem.Instance;
        private TipoNivel objTipoNivel = TipoNivel.Instance;
        private Archivo objArchivo = Archivo.Instance;
        private MenuAdm objMenu = MenuAdm.Instance;
        private OperacionRol objOperacionRol = OperacionRol.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            if (Request.Cookies["UserSettings"] != null)
            {
                HttpCookie myCookie = new HttpCookie("UserSettings");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            objItem.Dispose();
            objCategoria.Dispose();
            objSubCategoria.Dispose();
            objTipoItem.Dispose();
            objTipoNivel.Dispose();
            objArchivo.Dispose();
            objMenu.Dispose();
            objOperacionRol.Dispose();

            Response.Redirect("~/Login.aspx", true);
        }
    }
}