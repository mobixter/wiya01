using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;

namespace ContentAdmin.Forms
{
    public partial class PageGroups : System.Web.UI.Page
    {
        private DataTable tableOperacionRol = new DataTable();
        private CDevices objDevices = new CDevices();
        private int IdHandset;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.IdHandset = Convert.ToInt32(Request.QueryString["IdH"]);
            Grid();
        }
        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            DataTable dt = new DataTable();
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            int HandsetComp = Convert.ToInt32((userControl.FindControl("textid") as TextBox).Text);
            dt=objDevices.TableDevices234(HandsetComp);
            string Nmae="";
            if(dt.Rows.Count>1){
                if (dt.Rows != null)
                {
                    try
                    {

                        foreach (DataRow dtRow in dt.Rows)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                Nmae = dtRow[dc].ToString();
                                
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }

                error.InnerHtml = "Sorry, the model you selected is already assigned to the group : " + Nmae;
            }
            else{
            objDevices.InsertCompatibilidad22(this.IdHandset, HandsetComp);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }

        private void Grid()
        {
           
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objDevices.TableDevices2(this.IdHandset);
        }
    }
}