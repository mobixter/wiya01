using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContentAdmin.Controls.Forms
{
    public partial class ctrCompatibles : System.Web.UI.UserControl
    {
        CDevices objdevices = new CDevices();
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownList1.DataSource = objdevices.TableDevices22H();
            DropDownList1.DataTextField = "NameHandset";
            DropDownList1.DataValueField = "Id";
            DropDownList1.DataBind();
        }
    }
}