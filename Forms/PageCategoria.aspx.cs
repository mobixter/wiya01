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
    public partial class PageCategoria : System.Web.UI.Page
    {
        private Categoria objCategoria = Categoria.Instance;
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1011)
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1014)
                {
                    RadGrid1.AutoGenerateEditColumn = true;
                    break;
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objCategoria.TableCategoria.Select("categoryId NOT IN (0,-1)");
        }

        protected void RadGrid1_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            DataRow[] changedRows = this.objCategoria.TableCategoria.Select("categoryId = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["categoryId"]);
            if (changedRows.Length != 1)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to locate the category for updating."));
                e.Canceled = true;
                return;
            }
            Hashtable Categoria = new Hashtable();
            Categoria["categoryId"] = Convert.ToInt32(changedRows[0]["categoryId"].ToString());
            Categoria["categoryNameSp"] = (userControl.FindControl("txtNombreSp") as TextBox).Text;
            Categoria["categoryNameEn"] = (userControl.FindControl("txtNombreEn") as TextBox).Text;
            Categoria["categoryNamePo"] = (userControl.FindControl("txtNombrePo") as TextBox).Text;
            long id = this.objCategoria.UpdateCategory(Categoria);

            if (id == 0)
            {
                changedRows[0].BeginEdit();
                try
                {
                    foreach (DictionaryEntry entry in Categoria)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changedRows[0].EndEdit();
                    this.objCategoria.TableCategoria.AcceptChanges();
                }
                catch (Exception ex)
                {
                    changedRows[0].CancelEdit();

                    Label lblError = new Label();
                    lblError.Text = "Unable to update category. Reason: " + ex.Message;
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
                    lblError.Text = "Unable to update category";
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
            DataRow newRow = this.objCategoria.TableCategoria.NewRow();

            //Insert new values
            Hashtable Categoria = new Hashtable();
            Categoria["categoryNameSp"] = (userControl.FindControl("txtNombreSp") as TextBox).Text;
            Categoria["categoryNameEn"] = (userControl.FindControl("txtNombreEn") as TextBox).Text;
            Categoria["categoryNamePo"] = (userControl.FindControl("txtNombrePo") as TextBox).Text;
            Categoria["categoryId"] = Convert.ToInt32(this.objCategoria.InsertCategory(Categoria));

            if (Convert.ToInt32(Categoria["categoryId"]) > 0)
            {
                try
                {
                    foreach (DictionaryEntry entry in Categoria)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    this.objCategoria.TableCategoria.Rows.Add(newRow);
                    this.objCategoria.TableCategoria.AcceptChanges();
                }
                catch (Exception ex)
                {
                    Label lblError = new Label();
                    lblError.Text = "Unable to insert category. Reason: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                    RadGrid1.Controls.Add(lblError);

                    e.Canceled = true;
                }
            }
            else
            {
                Label lblError = new Label();
                if (Categoria["categoryId"].ToString() == "-2")
                    lblError.Text = "There's a record with this name";
                else
                    lblError.Text = "Unable to insert category";
                lblError.ForeColor = System.Drawing.Color.Red;
                RadGrid1.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                this.objCategoria.Dispose();
            if (e.CommandName == "ExportToExcel")
            {

            }
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            if (objCategoria.TableCategoria != null)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ReportCategory.xls");
                Response.Write("<HTML>");
                Response.Write("<BODY>");
                Response.Write("<table width='600' border='1' cellspacing='0' cellpadding='0'>");
                Response.Write("	<TR>");
                Response.Write("	   <TD align='center'><b>ID</b></TD>");
                Response.Write("	   <TD align='center'><b>CATEGORY NAME (SPANISH)</b></TD>");
                Response.Write("	   <TD align='center'><b>CATEGORY NAME (ENGLISH)</b></TD>");
                Response.Write("	   <TD align='center'><b>CATEGORY NAME (PORTUGUESE)</b></TD>");
                Response.Write("	</TR>");

                if (this.objCategoria.TableCategoria.Rows.Count > 1)
                {
                    foreach (DataRow objRow in this.objCategoria.TableCategoria.Rows)
                    {
                        if (objRow["categoryId"].ToString() != "0")
                        {
                            Response.Write("	<TR>");
                            Response.Write("	   <TD align='left'>" + objRow["categoryId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["categoryNameSp"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["categoryNameEn"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["categoryNamepPo"].ToString() + "</TD>");
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