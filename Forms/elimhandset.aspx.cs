using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContentAdmin.Forms
{
    public partial class elimhandset : System.Web.UI.Page
    {
        CDevices cd = new CDevices();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int handset=Convert.ToInt32( Request.QueryString["id"]);
                cd.deleteCompatibilidad(handset);
                
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Response.Redirect("PageGroups.aspx?IdH=" + Request.QueryString["group"]);
            }
        }
    }
}