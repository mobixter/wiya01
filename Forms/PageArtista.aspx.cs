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
    public partial class PageArtista : System.Web.UI.Page
    {
        private Artista objArtista = Artista.Instance;
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1001)
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1004)
                {
                    RadGrid1.AutoGenerateEditColumn = true;
                    break;
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objArtista.TableArtista.Select("artistId NOT IN (0,-1)"); 
        }

        protected void RadGrid1_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            DataRow[] changedRows = this.objArtista.TableArtista.Select("artistId = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["artistId"]);
            if (changedRows.Length != 1)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to locate the Artist for updating."));
                e.Canceled = true;
                return;
            }
            Hashtable Artista = new Hashtable();
            Artista["artistId"] = Convert.ToInt32(changedRows[0]["artistId"].ToString());
            Artista["artistName"] = (userControl.FindControl("txtNombre") as TextBox).Text;
            long id = this.objArtista.UpdateArtist(Artista);

            if (id == 0)
            {
                changedRows[0].BeginEdit();
                try
                {
                    foreach (DictionaryEntry entry in Artista)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changedRows[0].EndEdit();
                    this.objArtista.TableArtista.AcceptChanges();
                }
                catch (Exception ex)
                {
                    changedRows[0].CancelEdit();

                    Label lblError = new Label();
                    lblError.Text = "Unable to update artist. Reason: " + ex.Message;
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
                   lblError.Text = "Unable to update Artist";
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
            DataRow newRow = this.objArtista.TableArtista.NewRow();

            //Insert new values
            Hashtable Artista = new Hashtable();
            Artista["artistName"] = (userControl.FindControl("txtNombre") as TextBox).Text;
            Artista["artistId"] = Convert.ToInt32(this.objArtista.InsertArtist(Artista));

            if (Convert.ToInt32(Artista["artistId"]) > 0)
            {
                try
                {
                    foreach (DictionaryEntry entry in Artista)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    this.objArtista.TableArtista.Rows.Add(newRow);
                    this.objArtista.TableArtista.AcceptChanges();
                }
                catch (Exception ex)
                {
                    Label lblError = new Label();
                    lblError.Text = "Unable to insert Artist. Reason: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                    RadGrid1.Controls.Add(lblError);

                    e.Canceled = true;
                }
            }
            else
            {
                Label lblError = new Label();
                if (Artista["artistId"].ToString() == "-2")
                    lblError.Text = "There's a record with this name";
                else
                    lblError.Text = "Unable to insert Artist";
                lblError.ForeColor = System.Drawing.Color.Red;
                RadGrid1.Controls.Add(lblError);

                e.Canceled = true;
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                this.objArtista.Dispose();
            if (e.CommandName == "ExportToExcel")
            {
                
            }
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            if (this.objArtista.TableArtista != null)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ReportArtist.xls");
                Response.Write("<HTML>");
                Response.Write("<BODY>");
                Response.Write("<table width='600' border='1' cellspacing='0' cellpadding='0'>");
                Response.Write("	<TR>");
                Response.Write("	   <TD align='center'><b>ID</b></TD>");
                Response.Write("	   <TD align='center'><b>ARTIST NAME</b></TD>");
                Response.Write("	</TR>");

                if (this.objArtista.TableArtista.Rows.Count > 1)
                {
                    foreach (DataRow objRow in this.objArtista.TableArtista.Rows)
                    {
                        if (objRow["artistId"].ToString() != "0")
                        {
                            Response.Write("	<TR>");
                            Response.Write("	   <TD align='left'>" + objRow["artistId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["artistName"].ToString() + "</TD>");
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