using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;

namespace ContentAdmin
{
    public partial class PageDevices : System.Web.UI.Page
    {
        private DataTable tableOperacionRol = new DataTable();
        private CDevices objDevices = new CDevices();
        protected void Page_Load(object sender, EventArgs e)
        {
            Grid();
        }

        private void Grid() 
        {
            
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objDevices.TableDevices;
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            string HandsetComp = Convert.ToString((userControl.FindControl("txtNombreSp") as TextBox).Text);
            objDevices.InsertCompatibilidad(HandsetComp);
        }
    }
}