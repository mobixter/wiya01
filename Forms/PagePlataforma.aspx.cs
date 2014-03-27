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
    public partial class PagePlataforma : System.Web.UI.Page
    {
        private Plataforma objPlataforma = Plataforma.Instance;
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1041)
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1044)
                {
                    RadGrid1.AutoGenerateEditColumn = true;
                    break;
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objPlataforma.TablePlataforma;
        }

        protected void RadGrid1_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            DataRow[] changedRows = this.objPlataforma.TablePlataforma.Select("platformId = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["platformId"]);
            if (changedRows.Length != 1)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to locate the Platform for updating."));
                e.Canceled = true;
                return;
            }
            Hashtable Platforma = new Hashtable();
            Platforma["platformId"] = Convert.ToInt32(changedRows[0]["platformId"].ToString());
            Platforma["platformName"] = (userControl.FindControl("txtNombre") as TextBox).Text;
            Platforma["idiomId"] = Convert.ToInt32((userControl.FindControl("ctrIdiom") as ListControl).Value);
            Platforma["idiomName"] = (userControl.FindControl("ctrIdiom") as ListControl).ValueText;
            long id = this.objPlataforma.UpdatePlatform(Platforma);

            if (id == 0)
            {
                changedRows[0].BeginEdit();
                try
                {
                    foreach (DictionaryEntry entry in Platforma)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changedRows[0].EndEdit();
                    this.objPlataforma.TablePlataforma.AcceptChanges();
                }
                catch (Exception ex)
                {
                    changedRows[0].CancelEdit();

                    Label lblError = new Label();
                    lblError.Text = "Unable to update Platform. Reason: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                    RadGrid1.Controls.Add(lblError);

                    e.Canceled = true;
                }
            }
            else
            {
                Label lblError = new Label();
                if (id == -2)
                    lblError.Text = "There's a record with this name";
                else
                    lblError.Text = "Unable to update Platform";
                lblError.ForeColor = System.Drawing.Color.Red;
                RadGrid1.Controls.Add(lblError);

                e.Canceled = true;
            }

            RadGrid1.MasterTableView.ClearEditItems();
        }

        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            //Create new row in the DataSource
            DataRow newRow = this.objPlataforma.TablePlataforma.NewRow();

            //Insert new values
            Hashtable Platforma = new Hashtable();
            Platforma["platformName"] = (userControl.FindControl("txtNombre") as TextBox).Text;
            Platforma["idiomId"] = Convert.ToInt32((userControl.FindControl("ctrIdiom") as ListControl).Value);
            Platforma["idiomName"] = (userControl.FindControl("ctrIdiom") as ListControl).ValueText;
            Platforma["platformId"] = Convert.ToInt32(this.objPlataforma.InsertPlatform(Platforma));

            if (Convert.ToInt32(Platforma["platformId"]) > 0)
            {
                try
                {
                    foreach (DictionaryEntry entry in Platforma)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    this.objPlataforma.TablePlataforma.Rows.Add(newRow);
                    this.objPlataforma.TablePlataforma.AcceptChanges();
                }
                catch (Exception ex)
                {
                    Label lblError = new Label();
                    lblError.Text = "Unable to insert Platform. Reason: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                    RadGrid1.Controls.Add(lblError);

                    e.Canceled = true;
                }
            }
            else
            {
                Label lblError = new Label();
                if (Platforma["platformId"].ToString() == "-2")
                    lblError.Text = "There's a record with this name";
                else
                    lblError.Text = "Unable to insert Platform";
                lblError.ForeColor = System.Drawing.Color.Red;
                RadGrid1.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                this.objPlataforma.Dispose();
            if (e.CommandName == "ExportToExcel")
            {

            }
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            if (this.objPlataforma.TablePlataforma != null)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ReportPlatform.xls");
                Response.Write("<HTML>");
                Response.Write("<BODY>");
                Response.Write("<table width='600' border='1' cellspacing='0' cellpadding='0'>");
                Response.Write("	<TR>");
                Response.Write("	   <TD align='center'><b>ID</b></TD>");
                Response.Write("	   <TD align='center'><b>PLATFORM NAME</b></TD>");
                Response.Write("	</TR>");

                if (this.objPlataforma.TablePlataforma.Rows.Count > 0)
                {
                    foreach (DataRow objRow in this.objPlataforma.TablePlataforma.Rows)
                    {
                        if (objRow["platformId"].ToString() != "0")
                        {
                            Response.Write("	<TR>");
                            Response.Write("	   <TD align='left'>" + objRow["platformId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["platformName"].ToString() + "</TD>");
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