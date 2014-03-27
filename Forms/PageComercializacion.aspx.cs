using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using DbAcces.Data;
using System.IO;
using System.Configuration;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.Archiver;

namespace ContentAdmin
{
    public partial class PageComercializacion : System.Web.UI.Page
    {
        private ItemPlataforma objIP = new ItemPlataforma();
        private Plataforma objPlataforma = Plataforma.Instance;
        private Configuracion objConfig = new Configuracion();
        private string fileName;
        private string DFileName;
        private string DFilePath;             
        protected override void OnInit(EventArgs e)
        {
            if (Session["validado"] == null)
                Response.Redirect("~/Login.aspx");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadVariable();
            ctrPlataforma.Enabled = true;
            ctrPlataforma.Post = true;
            ctrPlataforma.Selected = (int)Session["plataformaId"];
            ctrPlataforma.Inicio(ctrPlataforma.ID, objPlataforma.TablePlataforma);
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = PendingItem;
        }

        protected void RadGrid2_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid2.DataSource = ShippedItem;
        }

        protected IList<ItemPlataforma> PendingItem
        {
            get
            {
                try
                {
                    object obj = Session["Pending"];
                    if (obj == null)
                    {
                        objIP.PlatformId = (int)Session["plataformaId"];
                        obj = objIP.GetItemPlatform();
                        if (obj == null)
                            obj = new List<ItemPlataforma>();
                        else
                            Session["Pending"] = obj;
                    }
                    return (IList<ItemPlataforma>)obj;
                }
                catch
                {
                    Session["Pending"] = null;
                }
                return new List<ItemPlataforma>();
             }
            set { Session["Pending"] = value; }
        }

        protected IList<ItemPlataforma> ShippedItem
        {
            get
            {
                try
                {
                    object obj = Session["Shipped"];
                    if (obj == null)
                    {
                        Session["Shipped"] = obj = new List<ItemPlataforma>();
                    }
                    return (IList<ItemPlataforma>)obj;
                }
                catch
                {
                    Session["Shipped"] = null;
                }
                return new List<ItemPlataforma>();
            }
            set { Session["Shipped"] = value; }
        }

        private static ItemPlataforma GetItem(IEnumerable<ItemPlataforma> itemToSearchIn, int itemId)
        {
            foreach (ItemPlataforma objIP in itemToSearchIn)
            {
                if (objIP.Itemid == itemId)
                {
                    return objIP;
                }
            }
            return null;
        }

        protected void RadGrid1_RowDrop(object sender, GridDragDropEventArgs e)
        {
            if (string.IsNullOrEmpty(e.HtmlElement))
            {
                if (e.DraggedItems[0].OwnerGridID == RadGrid1.ClientID)
                {
                    // items are drag from pending to shipped grid
                    if ((e.DestDataItem == null && ShippedItem.Count == 0) ||
                        e.DestDataItem != null && e.DestDataItem.OwnerGridID == RadGrid2.ClientID)
                    {
                        IList<ItemPlataforma> shippedItems = ShippedItem;
                        IList<ItemPlataforma> pendingItems = PendingItem;
                        int destinationIndex = -1;
                        if (e.DestDataItem != null)
                        {
                            ItemPlataforma itemPlataforma = GetItem(shippedItems, Convert.ToInt32(e.DestDataItem.GetDataKeyValue("itemId")));
                            destinationIndex = (itemPlataforma != null) ? shippedItems.IndexOf(itemPlataforma) : -1;
                        }

                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            ItemPlataforma tmpItem = GetItem(pendingItems, Convert.ToInt32(draggedItem.GetDataKeyValue("itemId")));
                            //if (tmpItem.NoExport == false)
                            //    tmpItem = null;
                            if (tmpItem != null)
                            {
                                if (destinationIndex > -1)
                                {
                                    if (e.DropPosition == GridItemDropPosition.Below)
                                        destinationIndex += 1;
                                    shippedItems.Insert(destinationIndex, tmpItem);
                                }
                                else
                                    shippedItems.Add(tmpItem);

                                objIP.PlatformId = (int)Session["plataformaId"];
                                pendingItems.Remove(tmpItem);
                            }
                        }

                        ShippedItem = shippedItems;
                        PendingItem = pendingItems;
                        RadGrid1.Rebind();
                        RadGrid2.Rebind();
                    }
                    else if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == RadGrid1.ClientID)
                    {
                        //reorder items in pending grid
                        IList<ItemPlataforma> pendingItems = PendingItem;
                        ItemPlataforma itemPlataforma = GetItem(pendingItems, Convert.ToInt32(e.DestDataItem.GetDataKeyValue("itemId")));
                        int destinationIndex = pendingItems.IndexOf(itemPlataforma);

                        if (e.DropPosition == GridItemDropPosition.Above && e.DestDataItem.ItemIndex > e.DraggedItems[0].ItemIndex)
                            destinationIndex -= 1;
                        if (e.DropPosition == GridItemDropPosition.Below && e.DestDataItem.ItemIndex < e.DraggedItems[0].ItemIndex)
                            destinationIndex += 1;

                        List<ItemPlataforma> itemsToMove = new List<ItemPlataforma>();
                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            ItemPlataforma tmpItem = GetItem(pendingItems, Convert.ToInt32(draggedItem.GetDataKeyValue("itemId")));
                            if (tmpItem != null)
                                itemsToMove.Add(tmpItem);
                        }

                        foreach (ItemPlataforma itemToMove in itemsToMove)
                        {
                            pendingItems.Remove(itemToMove);                           
                            pendingItems.Insert(destinationIndex, itemToMove);
                        }
                        PendingItem = pendingItems;
                        RadGrid1.Rebind();

                        int destinationItemIndex = destinationIndex - (RadGrid1.PageSize * RadGrid1.CurrentPageIndex);
                        e.DestinationTableView.Items[destinationItemIndex].Selected = true;
                    }
                }
            }
        }

        protected void RadGrid2_RowDrop(object sender, GridDragDropEventArgs e)
        {
            if (string.IsNullOrEmpty(e.HtmlElement))
            {
                if (e.DraggedItems[0].OwnerGridID == RadGrid2.ClientID)
                {
                    // items are drag from pending to shipped grid
                    if ((e.DestDataItem == null && PendingItem.Count == 0) ||
                        e.DestDataItem != null && e.DestDataItem.OwnerGridID == RadGrid1.ClientID)
                    {
                        IList<ItemPlataforma> shippedItems = ShippedItem;
                        IList<ItemPlataforma> pendingItems = PendingItem;
                        int destinationIndex = -1;
                        if (e.DestDataItem != null)
                        {
                            ItemPlataforma itemPlataforma = GetItem(pendingItems, Convert.ToInt32(e.DestDataItem.GetDataKeyValue("itemId")));
                            destinationIndex = (itemPlataforma != null) ? pendingItems.IndexOf(itemPlataforma) : -1;
                        }

                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            ItemPlataforma tmpItem = GetItem(shippedItems, Convert.ToInt32(draggedItem.GetDataKeyValue("itemId")));
                            if (tmpItem != null)
                            {
                                if (destinationIndex > -1)
                                {
                                    if (e.DropPosition == GridItemDropPosition.Below)
                                        destinationIndex += 1;
                                    pendingItems.Add(tmpItem);
                                }
                                else
                                    pendingItems.Add(tmpItem);

                                objIP.PlatformId = (int)Session["plataformaId"];
                                shippedItems.Remove(tmpItem);
                            }
                        }

                        ShippedItem = shippedItems;
                        PendingItem = pendingItems;
                        RadGrid2.Rebind();
                        RadGrid1.Rebind();
                    }
                    else if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == RadGrid2.ClientID)
                    {
                        //reorder items in pending grid
                        IList<ItemPlataforma> pendingItems = PendingItem;
                        ItemPlataforma itemPlataforma = GetItem(pendingItems, Convert.ToInt32(e.DestDataItem.GetDataKeyValue("itemId")));
                        int destinationIndex = pendingItems.IndexOf(itemPlataforma);

                        if (e.DropPosition == GridItemDropPosition.Above && e.DestDataItem.ItemIndex > e.DraggedItems[0].ItemIndex)
                            destinationIndex -= 1;
                        if (e.DropPosition == GridItemDropPosition.Below && e.DestDataItem.ItemIndex < e.DraggedItems[0].ItemIndex)
                            destinationIndex += 1;

                        List<ItemPlataforma> itemsToMove = new List<ItemPlataforma>();
                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            ItemPlataforma tmpItem = GetItem(pendingItems, Convert.ToInt32(draggedItem.GetDataKeyValue("itemId")));
                            if (tmpItem != null)
                                itemsToMove.Add(tmpItem);
                        }

                        foreach (ItemPlataforma itemToMove in itemsToMove)
                        {
                            pendingItems.Remove(itemToMove);
                            pendingItems.Insert(destinationIndex, itemToMove);
                        }
                        PendingItem = pendingItems;
                        RadGrid2.Rebind();

                        int destinationItemIndex = destinationIndex - (RadGrid2.PageSize * RadGrid2.CurrentPageIndex);
                        e.DestinationTableView.Items[destinationItemIndex].Selected = true;
                    }
                }
            }
        }

        private void LoadVariable()
        {
            if (!Page.IsPostBack)
            {
                Session["plataformaId"] = 1;
                Session["Pending"] = null;
                Session["Shipped"] = null;
            }
            else
            {
                if ((int)Session["plataformaId"] != int.Parse(Request.Form["ctrPlataforma$ctrPlataforma"]))
                {
                    Session["plataformaId"] = int.Parse(Request.Form["ctrPlataforma$ctrPlataforma"]);
                    Session["Pending"] = null;
                    Session["Shipped"] = null;
                    RadGrid1.Rebind();
                    RadGrid2.Rebind();
                }
            }

            lnkButton.CssClass = "menuOut";
            lnkButton.Attributes.Add("Onclick", "href='" + "PageDownloadZip.aspx" + "'");
        }

        private Hashtable CustomersChecked
        {
            get
            {
                object res = ViewState["_cc"];
                if (res == null)
                {
                    res = new Hashtable();
                    ViewState["_cc"] = res;
                }

                return (Hashtable)res;
            }
        }

        protected void CheckChanged(object sender, System.EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            GridDataItem item = (GridDataItem)box.NamingContainer;

            System.Data.SqlClient.SqlConnection con2 = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DB_ServiceAdmin"].ToString());
            System.Data.SqlClient.SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand();
            con2.Open();
            cmd2.Connection = con2;
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.CommandText = "dbo.usp_ItemExport";
            cmd2.Parameters.AddWithValue("@platformid", (int)Session["plataformaId"]);
            cmd2.Parameters.AddWithValue("@itemid", Convert.ToInt64(item["itemId"].Text));
            cmd2.Parameters.AddWithValue("@NoExport", Convert.ToBoolean(box.Checked));
            cmd2.ExecuteNonQuery();
            con2.Close();
            Session["Pending"] = null;
            RadGrid1.Rebind();
        }
     
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem & e.Item.IsInEditMode)
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                DropDownList dropdown = (DropDownList)item.FindControl("NoExport");
                dropdown.SelectedValue = item.GetDataKeyValue("NoExport").ToString();
            }

        } 

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                Session["Shipped"] = null;
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
                Session["Pending"] = null;
        }

        protected void DLFile()
        {
            if (DFileName != null && DFileName != "")
            {
                string URL = DFilePath + DFileName  ;
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(URL);
                if (fileInfo.Exists)
                {
                    HttpContext.Current.Response.ContentType = "application/x-download";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(fileInfo.Name));
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                    HttpContext.Current.Response.End();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string d = "";
            DFileName = null;
            Session["fileName"] = "ContentXml" + objConfig.GetFileName();
            fileName = Session["fileName"].ToString();
            msg.Visible = false;
            try
            {
                foreach (string dir in Directory.GetDirectories(objConfig.RutaBaseZip))
                {
                    DirectoryInfo objDir = new DirectoryInfo(dir);
                    if (!dir.Contains("Temp"))
                    {
                        Directory.CreateDirectory(dir + "\\" + fileName.Replace("Name", objDir.Name));

                        XmlProvider objXmlCp = new XmlProvider();
                        objXmlCp.ObjItemPlataforma = ShippedItem;
                        objXmlCp.PlataformaId = int.Parse(ctrPlataforma.Value);
                        objXmlCp.FileName = dir + "\\" + fileName.Replace("Name", objDir.Name);
                        objXmlCp.DirName = fileName.Replace("Name", objDir.Name);
                        switch (objDir.Name)
                        {
                            case "Animations":
                                objXmlCp.ContentTypeId = "2";
                                objXmlCp.Writer();
                                break;
                            case "Applications":
                                objXmlCp.ContentTypeId = "7";
                                objXmlCp.Writer();
                                break;
                            case "FullTracks":
                                objXmlCp.ContentTypeId = "5";
                                objXmlCp.Writer();
                                break;
                            case "PolyTones":
                                objXmlCp.ContentTypeId = "4";
                                objXmlCp.Writer();
                                break;
                            case "RealTones":
                                objXmlCp.ContentTypeId = "3";
                                objXmlCp.Writer();
                                break;
                            case "Video":
                                objXmlCp.ContentTypeId = "6";
                                objXmlCp.Writer();
                                break;
                            case "Wallpapers":
                                objXmlCp.ContentTypeId = "1";
                                objXmlCp.Writer();
                                break;
                            case "Games":
                                objXmlCp.ContentTypeId = "9";
                                objXmlCp.Writer();
                                break;
                        }
                    }
                }

                foreach (string dir in Directory.GetDirectories(objConfig.RutaBaseZip))
                {
                    if (!dir.Contains("Temp"))
                    {
                        DirectoryInfo objDir = new DirectoryInfo(dir);
                        ZipForge objZip = new ZipForge();
                        objZip.FileName = dir + "\\" + fileName.Replace("Name", objDir.Name) + ".zip";
                        objZip.OpenArchive(FileMode.Create);
                        objZip.BaseDir = dir + "\\" + fileName.Replace("Name", objDir.Name) + "\\";
                        
                        objZip.AddFiles();
                        objZip.CloseArchive();
                    }
                }
                string AFileName = fileName + ".zip";
                foreach (string dir in Directory.GetDirectories(objConfig.RutaBaseZip))
                {

                    d = dir;
                    DirectoryInfo objDir = new DirectoryInfo(dir);
                    if (!dir.Contains("Temp"))
                    {
                        Directory.Delete(dir + "\\" + fileName.Replace("Name", objDir.Name), true);
                        foreach (string file in Directory.GetFiles(dir))
                        {
                            FileInfo objFile = new FileInfo(file);

                            if ((objFile.Length / 1024) <= 1)
                                objFile.Delete();
                            else
                            {

                                if (AFileName.Replace ("Name",objDir.Name ) ==objFile.Name )
                                {
                                    DFileName = objFile.Name;
                                    DFilePath = objFile.DirectoryName + "\\";
                                }
                                if (objFile.CreationTime.Day != DateTime.Now.Day)
                                {
                                    objFile.MoveTo(objConfig.RutaRespaldo + "\\" + objFile.Name);
                                    DFilePath = objConfig.RutaRespaldo.Replace("\\", "/") + "/" + objFile.Name;
                                }
                            }
                        }
                    }
                }

                msg.InnerText = "El proceso fué ejecutado exitosamente!";
                Session["Shipped"] = null;
                RadGrid2.Rebind();
            }
            catch (Exception ex)
            {
                foreach (string dir in Directory.GetDirectories(objConfig.RutaBaseZip))
                {
                    DirectoryInfo objDir = new DirectoryInfo(dir);
                    if (!dir.Contains("Temp") && Directory.Exists (dir + "\\" + fileName.Replace("Name", objDir.Name) ))
                    {
                        Directory.Delete(dir + "\\" + fileName.Replace("Name", objDir.Name), true);
                    }
                }
                msg.InnerText = "Error al ejecutar el proceso. " + ex.Message + " " + d;
            }

            msg.Visible = true;
            DLFile();
            
        }
         
    }
}