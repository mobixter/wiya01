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
    public partial class PageTipoItem : System.Web.UI.Page
    {
        private TipoItem objTipoItem = TipoItem.Instance;
        private Funciones objFunciones = new Funciones();

        protected override void OnInit(EventArgs e)
        {
            if (Session["validado"] == null)
                Response.Redirect("~/Login.aspx");
            objFunciones.CleanFilter(RadGrid1);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objTipoItem.TableTipoItem;
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                this.objTipoItem.Dispose();
            if (e.CommandName == "ExportToExcel")
            {

            }
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            if (this.objTipoItem.TableTipoItem != null)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ReportContentType.xls");
                Response.Write("<HTML>");
                Response.Write("<BODY>");
                Response.Write("<table width='600' border='1' cellspacing='0' cellpadding='0'>");
                Response.Write("	<TR>");
                Response.Write("	   <TD align='center'><b>ID</b></TD>");
                Response.Write("	   <TD align='center'><b>CONTENT TYPE NAME</b></TD>");
                Response.Write("	</TR>");

                if (this.objTipoItem.TableTipoItem.Rows.Count > 0)
                {
                    foreach (DataRow objRow in this.objTipoItem.TableTipoItem.Rows)
                    {
                        if (objRow["contentTypeId"].ToString() != "0")
                        {
                            Response.Write("	<TR>");
                            Response.Write("	   <TD align='left'>" + objRow["contentTypeId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["contentTypeName"].ToString() + "</TD>");
                            Response.Write("	</TR>");
                        }
                    }
                }
                
                Response.Write("</table>");
                Response.Write("</BODY>");
                Response.Write("</HTML>");
                Response.End();
            }
        }
    }
}