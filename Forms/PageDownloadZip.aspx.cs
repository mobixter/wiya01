using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContentAdmin
{
    public partial class PageDownloadZip : System.Web.UI.Page
    {
        private Configuracion objConfig = new Configuracion();

        protected void Page_Load(object sender, EventArgs e)
        {
            RadFileExplorerZip.Configuration.ViewPaths = objConfig.RutaVirtualZip;
            RadFileExplorerZip.DataBind();
        }

        protected void RadFileExplorerZip_ItemCommand(object sender, Telerik.Web.UI.RadFileExplorerEventArgs e)
        {
            string prueba = "";
        }
    }
}