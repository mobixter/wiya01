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
    public partial class PageProveedor : System.Web.UI.Page
    {
        private Proveedor objProveedor = Proveedor.Instance;
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1051)
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1054)
                {
                    RadGrid1.AutoGenerateEditColumn = true;
                    break;
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = this.objProveedor.TableProveedor.Select("providerId NOT IN (0,-1)");
        }

        protected void RadGrid1_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            DataRow[] changedRows = this.objProveedor.TableProveedor.Select("providerId = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["providerId"]);
            if (changedRows.Length != 1)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to locate the provider for updating."));
                e.Canceled = true;
                return;
            }
            Hashtable Proveedor = new Hashtable();
            Proveedor["providerId"] = Convert.ToInt32(changedRows[0]["providerId"].ToString());
            Proveedor["providerName"] = (userControl.FindControl("txtNombre") as TextBox).Text;
            Proveedor["providerContactName"] = (userControl.FindControl("txtContacto") as TextBox).Text;
            Proveedor["providerPhone"] = (userControl.FindControl("txtTelefono") as TextBox).Text;
            Proveedor["providerEmail"] = (userControl.FindControl("txtEmail") as TextBox).Text;
            Proveedor["providerLocator"] = (userControl.FindControl("txtLocalizador") as TextBox).Text;
            long id = this.objProveedor.UpdateProvider(Proveedor);

            if (id == 0)
            {
                changedRows[0].BeginEdit();
                try
                {
                    foreach (DictionaryEntry entry in Proveedor)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changedRows[0].EndEdit();
                    this.objProveedor.TableProveedor.AcceptChanges();
                }
                catch (Exception ex)
                {
                    changedRows[0].CancelEdit();

                    Label lblError = new Label();
                    lblError.Text = "Unable to update Provider. Reason: " + ex.Message;
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
                    lblError.Text = "Unable to update Provider";
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
            DataRow newRow = this.objProveedor.TableProveedor.NewRow();

            //Insert new values
            Hashtable Proveedor = new Hashtable();
            Proveedor["providerName"] = (userControl.FindControl("txtNombre") as TextBox).Text;
            Proveedor["providerContactName"] = (userControl.FindControl("txtContacto") as TextBox).Text;
            Proveedor["providerPhone"] = (userControl.FindControl("txtTelefono") as TextBox).Text;
            Proveedor["providerEmail"] = (userControl.FindControl("txtEmail") as TextBox).Text;
            Proveedor["providerLocator"] = (userControl.FindControl("txtLocalizador") as TextBox).Text;
            Proveedor["providerId"] = Convert.ToInt32(this.objProveedor.InsertProvider(Proveedor));

            if (Convert.ToInt32(Proveedor["providerId"]) > 0)
            {
                try
                {
                    foreach (DictionaryEntry entry in Proveedor)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    this.objProveedor.TableProveedor.Rows.Add(newRow);
                    this.objProveedor.TableProveedor.AcceptChanges();
                }
                catch (Exception ex)
                {
                    Label lblError = new Label();
                    lblError.Text = "Unable to insert Provider. Reason: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                    RadGrid1.Controls.Add(lblError);

                    e.Canceled = true;
                }
            }
            else
            {
                Label lblError = new Label();
                if (Proveedor["providerId"].ToString() == "-2")
                    lblError.Text = "There's a record with this name";
                else
                    lblError.Text = "Unable to insert provider";
                lblError.ForeColor = System.Drawing.Color.Red;
                RadGrid1.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                this.objProveedor.Dispose();
            if (e.CommandName == "ExportToExcel")
            {

            }
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            if (this.objProveedor.TableProveedor != null)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ReportProvider.xls");
                Response.Write("<HTML>");
                Response.Write("<BODY>");
                Response.Write("<table width='600' border='1' cellspacing='0' cellpadding='0'>");
                Response.Write("	<TR>");
                Response.Write("	   <TD align='center'><b>ID</b></TD>");
                Response.Write("	   <TD align='center'><b>PROVIDER NAME</b></TD>");
                Response.Write("	   <TD align='center'><b>CONTACT NAME</b></TD>");
                Response.Write("	   <TD align='center'><b>PHONE</b></TD>");
                Response.Write("	   <TD align='center'><b>EMAIL NAME</b></TD>");
                Response.Write("	   <TD align='center'><b>LOCATOR</b></TD>");
                Response.Write("	</TR>");

                if (this.objProveedor.TableProveedor.Rows.Count > 1)
                {
                    foreach (DataRow objRow in this.objProveedor.TableProveedor.Rows)
                    {
                        if (objRow["providerId"].ToString() != "0")
                        {
                            Response.Write("	<TR>");
                            Response.Write("	   <TD align='left'>" + objRow["providerId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["providerName"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["providerContactName"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["providerPhone"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["providerEmail"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["providerLocator"].ToString() + "</TD>");
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