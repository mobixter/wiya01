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
    public partial class PageTelefono : System.Web.UI.Page
    {
        private Telefono objTelefono = Telefono.Instance;
        private Funciones objFunciones = new Funciones();
        private OperacionRol objOperacionRol = OperacionRol.Instance;
        private DataTable tableOperacionRol = new DataTable();

        protected override void OnInit(EventArgs e)
        {
            if (Session["validado"] == null)
                Response.Redirect("~/Login.aspx");
            objFunciones.CleanFilter(RadGrid1);
            base.OnInit(e);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            tableOperacionRol = objOperacionRol.TableOperacionRol;
            foreach (DataRow objRow in tableOperacionRol.Rows)
            {
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1061)
                {
                    LinkButton btnInsert = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
                    btnInsert.Visible = true;
                    break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            tableOperacionRol = objOperacionRol.TableOperacionRol;
            foreach (DataRow objRow in tableOperacionRol.Rows)
            {
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1064)
                {
                    RadGrid1.AutoGenerateEditColumn = true;
                    break;
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objTelefono.TableTelefono;
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                this.objTelefono.Dispose();
            if (e.CommandName == "ExportToExcel")
            {

            }
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            if (this.objTelefono.TableTelefono != null)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ReportHandset.xls");
                Response.Write("<HTML>");
                Response.Write("<BODY>");
                Response.Write("<table width='600' border='1' cellspacing='0' cellpadding='0'>");
                Response.Write("	<TR>");
                Response.Write("	   <TD align='center'><b>ID</b></TD>");
                Response.Write("	   <TD align='center'><b>STR ID</b></TD>");
                Response.Write("	   <TD align='center'><b>HANDSET NAME</b></TD>");
                Response.Write("	   <TD align='center'><b>MANUFACTURER</b></TD>");
                Response.Write("	</TR>");

                if (this.objTelefono.TableTelefono.Rows.Count > 0)
                {
                    foreach (DataRow objRow in this.objTelefono.TableTelefono.Rows)
                    {
                        if (objRow["handsetId"].ToString() != "0")
                        {
                            Response.Write("	<TR>");
                            Response.Write("	   <TD align='left'>" + objRow["handsetId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["handsetStrId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["handsetName"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["manufacturerName"].ToString() + "</TD>");
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