using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Telerik.Web.UI;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.Archiver;

namespace ContentAdmin
{
    public partial class PageItem : System.Web.UI.Page
    {
        private Item objItem = Item.Instance;
        private Funciones objFunciones = new Funciones();
        private Categoria objCategoria = Categoria.Instance;
        private ItemCategoria objItemCategoria = ItemCategoria.Instance;
        private Preview objPreview = Preview.Instance;
        private ItemPreview objItemPreview;
        private Archivo objArchivo = Archivo.Instance;
        private HandsetGroup objHandsetGroup = HandsetGroup.Instance;
        private Configuracion objConfig = new Configuracion();
        private OperacionRol objOperacionRol = OperacionRol.Instance;
        private DataTable tableOperacionRol = new DataTable();
        private ItemHandset objItemHandset = ItemHandset.Instance;
        private ItemPlatformInternal objInternal = ItemPlatformInternal.Instance;
        
        private Handset objHandset = Handset.Instance;

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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1031)
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
                if (int.Parse(objRow["intId_Operacion"].ToString()) == 1034)
                {
                    RadGrid1.AutoGenerateEditColumn = true;
                    break;
                }
            }
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
            Hashtable target = null;

            target = CustomersChecked;

            if (box.Checked)
            {
                target[item["itemId"].Text] = true;
            }
            else
            {
                target[item["itemId"].Text] = null;
            }
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            this.RadGrid1.DataSource = objItem.TableItem.Select("itemId NOT IN (0,-1)");
        }

        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
            bool checkHeader = true;
            foreach (GridDataItem dataItem in RadGrid1.MasterTableView.Items)
            {
                if (!(dataItem.FindControl("CheckBox1") as CheckBox).Checked)
                {
                    checkHeader = false;
                    break;
                }
            }
            GridHeaderItem headerItem = RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            (headerItem.FindControl("headerChkbox") as CheckBox).Checked = checkHeader;
        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in RadGrid1.MasterTableView.Items)
            {
                (dataItem.FindControl("CheckBox1") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void RadGrid1_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            DataRow[] changedRows = this.objItem.TableItem.Select("itemId = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["itemId"]);
            if (changedRows.Length != 1)
            {
                RadGrid1.Controls.Add(new LiteralControl("Unable to locate the Item for updating."));
                e.Canceled = true;
                return;
            }
            //Update values
            RadAsyncUpload objUploadPreview = (userControl.FindControl("rdUploadPreview") as RadAsyncUpload);
            RadAsyncUpload objUploadThumbnail = (userControl.FindControl("rdUploadThumbnail") as RadAsyncUpload);
            RadAsyncUpload objUploadFile = (userControl.FindControl("rdUploadFile") as RadAsyncUpload);

            string idCategory = string.Empty;
            string idHandset = string.Empty;
            string idPlatform = string.Empty;
            foreach (string strRequest in Request.Form)
            {
                if (strRequest.Contains("itemIdAsociados"))
                    idCategory = Request.Form.Get(strRequest);
                if (strRequest.Contains("handsetIdAsociados"))
                    idHandset = Request.Form.Get(strRequest);
                if (strRequest.Contains("platformIdAsociados"))
                    idPlatform = Request.Form.Get(strRequest);
            }
            
            Hashtable Item = new Hashtable();
            Item["itemId"] = Convert.ToInt32(changedRows[0]["itemId"].ToString());
            Item["itemStrId"] = (userControl.FindControl("txtStrId") as TextBox).Text;
            Item["itemNameSp"] = (userControl.FindControl("txtNombreSp") as TextBox).Text;
            Item["itemNameEn"] = (userControl.FindControl("txtNombreEn") as TextBox).Text;
            Item["itemNamePo"] = (userControl.FindControl("txtNombrePo") as TextBox).Text;
            Item["itemDescriptionSp"] = (userControl.FindControl("txtDescSp") as TextBox).Text;
            Item["itemDescriptionEn"] = (userControl.FindControl("txtDescEn") as TextBox).Text;
            Item["itemDescriptionPo"] = (userControl.FindControl("txtDescPo") as TextBox).Text;
            Item["providerId"] = Convert.ToInt32((userControl.FindControl("ctrProveedor") as ListControl).Value);
            Item["providerName"] = (userControl.FindControl("ctrProveedor") as ListControl).ValueText;
            Item["contentTypeId"] = Convert.ToInt32((userControl.FindControl("ctrTipoItem") as ListControl).Value);
            Item["contentTypeName"] = (userControl.FindControl("ctrTipoItem") as ListControl).ValueText;
            Item["artistId"] = Convert.ToInt32((userControl.FindControl("ctrArtista") as ListControl).Value);
            Item["artistName"] = (userControl.FindControl("ctrArtista") as ListControl).ValueText;
            Item["itemAdvisory"] = (userControl.FindControl("cboAdvisory") as DropDownList).SelectedValue;
            Item["itemIsrcGrid"] = (userControl.FindControl("txtIsrcGrid") as TextBox).Text;
            Item["itemUpc"] = (userControl.FindControl("txtUpc") as TextBox).Text;
            Item["keywords"] = (userControl.FindControl("txtKeyword") as TextBox).Text;
            Item["chargeTypeId"] = Convert.ToInt32((userControl.FindControl("ctrChaegeType") as ListControl).Value);
            
            if (objUploadPreview.UploadedFiles.Count > 0)
                Item["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Preview_" + objUploadPreview.UploadedFiles[0].FileName;
            Item["mimeTypeId"] = 0;
            bool minimalSpecifications = Convert.ToBoolean((userControl.FindControl("cbMinimal") as CheckBox).Checked);

            long id = long.Parse(this.objItem.UpdateItem(Item).Rows[0]["Status"].ToString());
            if (id == 0)
            {
                changedRows[0].BeginEdit();
                try
                {
                    foreach (DictionaryEntry entry in Item)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changedRows[0].EndEdit();
                    this.objItem.TableItem.AcceptChanges();

                    objItemCategoria.ItemId = Convert.ToInt32(Item["itemId"].ToString());
                    objItemCategoria.DeleteItemCategory();
                    objItemCategoria.InsertItemCategory(idCategory);

                    objItemHandset.ItemId = objItemCategoria.ItemId;
                    objItemHandset.DeleteItemHandset();
                    if (idHandset != "")
                        objItemHandset.InsertItemHandset(idHandset);

                    objInternal.ItemId = objItemCategoria.ItemId;
                    objInternal.DeleteItemPlatformInternal();
                    if (idPlatform != "")
                        objInternal.InsertItemPlatformInternal(idPlatform);
                    
                    Hashtable Preview = new Hashtable();
                    int i = 0;
                    if (objUploadPreview.UploadedFiles.Count > 0)
                    {
                        this.objPreview.DeletePreview(Convert.ToInt32(Item["itemId"].ToString()), 0);

                        Preview["previewType"] = 0;
                        Preview["previewDefault"] = 1;
                        foreach (UploadedFile objFile in objUploadPreview.UploadedFiles)
                        {
                            if (i != 0)
                                Preview["previewDefault"] = 0;
                            Preview["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Preview_" + objFile.FileName;

                            FileInfo objFilePreview = new FileInfo(objConfig.RutaPreview + "\\" + Preview["previewName"].ToString());
                            Preview["mimeTypeSuffixes"] = objFilePreview.Extension.Replace(".", "");

                            Preview["previewId"] = Convert.ToInt32(this.objPreview.InsertPreview(Preview));
                            File.Copy(objConfig.RutaPreview + "\\" + objFile.FileName,
                                objConfig.RutaPreview + "\\" + Preview["previewName"].ToString(), true);
                            File.Delete(objConfig.RutaPreview + "\\" + objFile.FileName);

                            if (Convert.ToInt32(Preview["previewId"]) > 0)
                            {
                                objItemPreview = new ItemPreview();
                                objItemPreview.ItemId = Convert.ToInt32(Item["itemId"]);
                                objItemPreview.PreviewId = Convert.ToInt32(Preview["previewId"]);
                                if (objItemPreview.InsertItemPreview() < 0)
                                {
                                    Label lblError = new Label();
                                    lblError.Text = "Error inserting itemPreview.";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    RadGrid1.Controls.Add(lblError);
                                    e.Canceled = true;
                                }
                            }
                            else
                            {
                                Label lblError = new Label();
                                lblError.Text = "There's a record (preview) with this name.";
                                lblError.ForeColor = System.Drawing.Color.Red;
                                RadGrid1.Controls.Add(lblError);
                                e.Canceled = true;
                            }
                            i++;
                        }
                    }


                    if (objUploadThumbnail.UploadedFiles.Count > 0)
                    {
                        this.objPreview.DeletePreview(Convert.ToInt32(Item["itemId"].ToString()), 1);
                        Preview["previewType"] = 1;
                        Preview["previewDefault"] = 1;
                        foreach (UploadedFile objFile in objUploadThumbnail.UploadedFiles)
                        {
                            if (i != 0)
                                Preview["previewDefault"] = 0;
                            Preview["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Thumbnail_" + objFile.FileName;

                            FileInfo objFilePreview = new FileInfo(objConfig.RutaPreview + "\\" + Preview["previewName"].ToString());
                            Preview["mimeTypeSuffixes"] = objFilePreview.Extension.Replace(".", "");

                            Preview["previewId"] = Convert.ToInt32(this.objPreview.InsertPreview(Preview));
                            File.Copy(objConfig.RutaThumbnail + "\\" + objFile.FileName,
                                objConfig.RutaThumbnail + "\\" + Preview["previewName"].ToString(), true);
                            File.Delete(objConfig.RutaThumbnail + "\\" + objFile.FileName);

                            if (Convert.ToInt32(Preview["previewId"]) > 0)
                            {
                                objItemPreview = new ItemPreview();
                                objItemPreview.ItemId = Convert.ToInt32(Item["itemId"]);
                                objItemPreview.PreviewId = Convert.ToInt32(Preview["previewId"]);
                                if (objItemPreview.InsertItemPreview() < 0)
                                {
                                    Label lblError = new Label();
                                    lblError.Text = "Error inserting itemThumbnail.";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    RadGrid1.Controls.Add(lblError);
                                    e.Canceled = true;
                                }
                            }
                            else
                            {
                                Label lblError = new Label();
                                lblError.Text = "There's a record (thumbnail) with this name.";
                                lblError.ForeColor = System.Drawing.Color.Red;
                                RadGrid1.Controls.Add(lblError);
                                e.Canceled = true;
                            }
                            i++;
                        }
                    }

                    if (objUploadFile.UploadedFiles.Count > 0)
                    {
                        objArchivo.DeleteFile(Convert.ToInt32(Item["itemId"].ToString()));
                        Hashtable FileMaster = new Hashtable();
                        string strZip = objUploadFile.UploadedFiles[0].FileName;

                        string fileList = UnZipFile(strZip);
                        if (ValidateFiles(minimalSpecifications, fileList, Convert.ToInt16(Item["contentTypeId"])))
                        {
                            foreach (string file in Directory.GetFiles(fileList))
                            {
                                FileInfo objFile = new FileInfo(file);
                                if (!objFile.Name.ToString().Contains(".zip"))
                                {
                                    string group = string.Empty;

                                    if (Convert.ToInt16(Item["contentTypeId"]) == 1)
                                    {
                                        System.Drawing.Image objImage = System.Drawing.Image.FromFile(file);
                                        FileMaster["fileProviderType"] = "Wallpaper_" + objImage.Width.ToString() + "x" + objImage.Height.ToString();
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 2)
                                    {
                                        System.Drawing.Image objImage = System.Drawing.Image.FromFile(file);
                                        FileMaster["fileProviderType"] = "Animation_" + objImage.Width.ToString() + "x" + objImage.Height.ToString();
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 3)
                                        FileMaster["fileProviderType"] = "RealTone_" + objFile.Extension.Replace(".", "");
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 4)
                                        FileMaster["fileProviderType"] = "PolyTone_" + objFile.Extension.Replace(".", "");

                                    FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + objFile.Name;
                                    FileMaster["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                    FileMaster["itemId"] = Item["itemId"];
                                    FileMaster["fileType"] = 0;
                                    FileMaster["fileSize"] = objFile.Length;
                                    if (objArchivo.InsertFile(FileMaster) < 0)
                                    {
                                        Label lblError = new Label();
                                        lblError.Text = "Error inserting fileMaster.";
                                        lblError.ForeColor = System.Drawing.Color.Red;
                                        RadGrid1.Controls.Add(lblError);
                                        e.Canceled = true;
                                    }

                                    if (Convert.ToInt16(Item["contentTypeId"]) == 1)
                                    {
                                        File.Copy(file, objConfig.RutaFileWallpaper + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }

                                    if (Convert.ToInt16(Item["contentTypeId"]) == 2)
                                    {
                                        File.Copy(file, objConfig.RutaFileAnimations + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 3)
                                    {
                                        File.Copy(file, objConfig.RutaFileRealTones + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 4)
                                    {
                                        File.Copy(file, objConfig.RutaFilePolyTones + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 5)
                                    {
                                        File.Copy(file, objConfig.RutaFileFullTracks + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 6)
                                    {
                                        File.Copy(file, objConfig.RutaFileVideo + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 7)
                                    {
                                        File.Copy(file, objConfig.RutaFileApplications + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }
                                    if (Convert.ToInt16(Item["contentTypeId"]) == 8)
                                    {
                                        File.Copy(file, objConfig.RutaFileThemes + "\\" + FileMaster["fileName"].ToString(), true);
                                        File.Delete(file);
                                    }
                                }
                            }
                            Directory.Delete(objConfig.RutaZip + "\\" + strZip.Replace(".zip", ""), true);
                        }
                        else
                        {
                            Label lblError = new Label();
                            lblError.Text = "File master specifications are incorrect.";
                            lblError.ForeColor = System.Drawing.Color.Red;
                            RadGrid1.Controls.Add(lblError);

                            e.Canceled = true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Label lblError = new Label();
                    lblError.Text = "Unable to insert Item. Reason: " + ex.Message;
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
                    lblError.Text = "Unable to update Item";
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
            DataRow newRow = this.objItem.TableItem.NewRow();

            //Insert new values
            RadAsyncUpload objUploadPreview = (userControl.FindControl("rdUploadPreview") as RadAsyncUpload);
            RadAsyncUpload objUploadThumbnail = (userControl.FindControl("rdUploadThumbnail") as RadAsyncUpload);
            RadAsyncUpload objUploadFile = (userControl.FindControl("rdUploadFile") as RadAsyncUpload);
            if (objUploadPreview.UploadedFiles.Count == 0)
            {
                Label lblError = new Label();
                lblError.Text = "No hay imagenes de preview para cargar.";
                lblError.ForeColor = System.Drawing.Color.Red;
                RadGrid1.Controls.Add(lblError);
                e.Canceled = true;
            }
            else
            {
                if (objUploadFile.UploadedFiles.Count == 0)
                {
                    Label lblError = new Label();
                    lblError.Text = "No hay .zip para cargar.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    RadGrid1.Controls.Add(lblError);
                    e.Canceled = true;
                }
                else {
                    string idCategory = string.Empty;
                    string idHandset = string.Empty;
                    string idPlatform = string.Empty;
                    foreach (string strRequest in Request.Form)
                    {
                        if (strRequest.Contains("itemIdAsociados"))
                            idCategory = Request.Form.Get(strRequest);
                        if (strRequest.Contains("handsetIdAsociados"))
                            idHandset = Request.Form.Get(strRequest);
                        if (strRequest.Contains("platformIdAsociados"))
                            idPlatform = Request.Form.Get(strRequest);
                    }
                    
                    Hashtable Item = new Hashtable();
                    Item["itemStrId"] = (userControl.FindControl("txtStrId") as TextBox).Text;
                    Item["itemNameSp"] = (userControl.FindControl("txtNombreSp") as TextBox).Text;
                    Item["itemNameEn"] = (userControl.FindControl("txtNombreEn") as TextBox).Text;
                    Item["itemNamePo"] = (userControl.FindControl("txtNombrePo") as TextBox).Text;
                    Item["itemDescriptionSp"] = (userControl.FindControl("txtDescSp") as TextBox).Text;
                    Item["itemDescriptionEn"] = (userControl.FindControl("txtDescEn") as TextBox).Text;
                    Item["itemDescriptionPo"] = (userControl.FindControl("txtDescPo") as TextBox).Text;
                    Item["providerId"] = Convert.ToInt32((userControl.FindControl("ctrProveedor") as ListControl).Value);
                    Item["providerName"] = (userControl.FindControl("ctrProveedor") as ListControl).ValueText;
                    Item["contentTypeId"] = Convert.ToInt32((userControl.FindControl("ctrTipoItem") as ListControl).Value);
                    Item["contentTypeName"] = (userControl.FindControl("ctrTipoItem") as ListControl).ValueText;
                    Item["artistId"] = Convert.ToInt32((userControl.FindControl("ctrArtista") as ListControl).Value);
                    Item["artistName"] = (userControl.FindControl("ctrArtista") as ListControl).ValueText;
                    Item["itemAdvisory"] = (userControl.FindControl("cboAdvisory") as DropDownList).SelectedValue;
                    Item["itemIsrcGrid"] = (userControl.FindControl("txtIsrcGrid") as TextBox).Text;
                    Item["itemUpc"] = (userControl.FindControl("txtUpc") as TextBox).Text;
                    Item["keywords"] = (userControl.FindControl("txtKeyword") as TextBox).Text;
                    Item["chargeTypeId"] = Convert.ToInt32((userControl.FindControl("ctrChaegeType") as ListControl).Value);
                    Item["itemId"] = Convert.ToInt32(this.objItem.InsertItem(Item));
                    Item["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Preview_" + objUploadPreview.UploadedFiles[0].FileName;
                    Item["mimeTypeId"] = 0;
                    bool minimalSpecifications = Convert.ToBoolean((userControl.FindControl("cbMinimal") as CheckBox).Checked);
                    

                    if (Convert.ToInt32(Item["itemId"]) > 0)
                    {
                        try
                        {
                            foreach (DictionaryEntry entry in Item)
                            {
                                newRow[(string)entry.Key] = entry.Value;
                            }
                            this.objItem.TableItem.Rows.Add(newRow);
                            this.objItem.TableItem.AcceptChanges();

                            objItemCategoria.ItemId = Convert.ToInt32(Item["itemId"].ToString());
                            objItemCategoria.InsertItemCategory(idCategory);

                            if (idHandset != "")
                            {
                                objItemHandset.ItemId = objItemCategoria.ItemId;
                                objItemHandset.InsertItemHandset(idHandset);
                            }

                            if (idPlatform != "")
                            {
                                objInternal.ItemId = objItemCategoria.ItemId;
                                objInternal.InsertItemPlatformInternal(idPlatform);
                            }

                            int i = 0;
                            Hashtable Preview = new Hashtable();
                            Preview["previewType"] = 0;
                            Preview["previewDefault"] = 1;
                            foreach (UploadedFile objFile in objUploadPreview.UploadedFiles)
                            {
                                if (i != 0)
                                    Preview["previewDefault"] = 0;
                                Preview["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Preview_" + objFile.FileName;

                                FileInfo objFilePreview = new FileInfo(objConfig.RutaPreview + "\\" + Preview["previewName"].ToString());
                                Preview["mimeTypeSuffixes"] = objFilePreview.Extension.Replace(".", "");

                                Preview["previewId"] = Convert.ToInt32(this.objPreview.InsertPreview(Preview));
                                File.Move(objConfig.RutaPreview + "\\" + objFile.FileName,
                                    objConfig.RutaPreview + "\\" + Preview["previewName"].ToString());
                                                                
                                if (Convert.ToInt32(Preview["previewId"]) > 0)
                                {
                                    objItemPreview = new ItemPreview();
                                    objItemPreview.ItemId = Convert.ToInt32(Item["itemId"]);
                                    objItemPreview.PreviewId = Convert.ToInt32(Preview["previewId"]);
                                    if (objItemPreview.InsertItemPreview() < 0)
                                    {
                                        Label lblError = new Label();
                                        lblError.Text = "Error inserting itemPreview.";
                                        lblError.ForeColor = System.Drawing.Color.Red;
                                        RadGrid1.Controls.Add(lblError);
                                        e.Canceled = true;
                                    }
                                }
                                else {
                                    Label lblError = new Label();
                                    lblError.Text = "There's a record (preview) with this name.";
                                    lblError.ForeColor = System.Drawing.Color.Red;
                                    RadGrid1.Controls.Add(lblError);
                                    e.Canceled = true;
                                }
                                i++;
                            }

                            if (objUploadThumbnail.UploadedFiles.Count > 0)
                            {
                                Preview["previewType"] = 1;
                                Preview["previewDefault"] = 1;
                                foreach (UploadedFile objFile in objUploadThumbnail.UploadedFiles)
                                {
                                    if (i != 0)
                                        Preview["previewDefault"] = 0;
                                    Preview["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Thumbnail_" + objFile.FileName;

                                    FileInfo objFilePreview = new FileInfo(objConfig.RutaPreview + "\\" + Preview["previewName"].ToString());
                                    Preview["mimeTypeSuffixes"] = objFilePreview.Extension.Replace(".", "");

                                    Preview["previewId"] = Convert.ToInt32(this.objPreview.InsertPreview(Preview));
                                    File.Move(objConfig.RutaThumbnail + "\\" + objFile.FileName,
                                        objConfig.RutaThumbnail + "\\" + Preview["previewName"].ToString());

                                    if (Convert.ToInt32(Preview["previewId"]) > 0)
                                    {
                                        objItemPreview = new ItemPreview();
                                        objItemPreview.ItemId = Convert.ToInt32(Item["itemId"]);
                                        objItemPreview.PreviewId = Convert.ToInt32(Preview["previewId"]);
                                        if (objItemPreview.InsertItemPreview() < 0)
                                        {
                                            Label lblError = new Label();
                                            lblError.Text = "Error inserting itemThumbnail.";
                                            lblError.ForeColor = System.Drawing.Color.Red;
                                            RadGrid1.Controls.Add(lblError);
                                            e.Canceled = true;
                                        }
                                    }
                                    else
                                    {
                                        Label lblError = new Label();
                                        lblError.Text = "There's a record (thumbnail) with this name.";
                                        lblError.ForeColor = System.Drawing.Color.Red;
                                        RadGrid1.Controls.Add(lblError);
                                        e.Canceled = true;
                                    }
                                    i++;
                                }
                            }

                            Hashtable FileMaster = new Hashtable();
                            string strZip = objUploadFile.UploadedFiles[0].FileName;

                            string fileList = UnZipFile(strZip);
                            if (ValidateFiles(minimalSpecifications, fileList, Convert.ToInt16(Item["contentTypeId"])))
                            {
                                foreach (string file in Directory.GetFiles(fileList))
                                {
                                    FileInfo objFile = new FileInfo(file);
                                    if (!objFile.Name.ToString().Contains(".zip"))
                                    {
                                        string group = string.Empty;

                                        if (Convert.ToInt16(Item["contentTypeId"]) == 1)
                                        {
                                            System.Drawing.Image objImage = System.Drawing.Image.FromFile(file);
                                            FileMaster["fileProviderType"] = "Wallpaper_" + objImage.Width.ToString() + "x" + objImage.Height.ToString();
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 2)
                                        {
                                            System.Drawing.Image objImage = System.Drawing.Image.FromFile(file);
                                            FileMaster["fileProviderType"] = "Animation_" + objImage.Width.ToString() + "x" + objImage.Height.ToString();
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 3)
                                            FileMaster["fileProviderType"] = "RealTone_" + objFile.Extension.Replace(".", "");
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 4)
                                            FileMaster["fileProviderType"] = "PolyTone_" + objFile.Extension.Replace(".", "");
                                            
                                        FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + objFile.Name;
                                        FileMaster["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                        FileMaster["itemId"] = Item["itemId"];
                                        FileMaster["fileType"] = 0;
                                        FileMaster["fileSize"] = objFile.Length;
                                        if (objArchivo.InsertFile(FileMaster) < 0)
                                        {
                                            Label lblError = new Label();
                                            lblError.Text = "Error inserting fileMaster.";
                                            lblError.ForeColor = System.Drawing.Color.Red;
                                            RadGrid1.Controls.Add(lblError);
                                            e.Canceled = true;
                                        }

                                        if (Convert.ToInt16(Item["contentTypeId"]) == 1)
                                        {
                                            File.Copy(file, objConfig.RutaFileWallpaper + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                            
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 2)
                                        {
                                            File.Copy(file, objConfig.RutaFileAnimations + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 3)
                                        {
                                            File.Copy(file, objConfig.RutaFileRealTones + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 4)
                                        {
                                            File.Copy(file, objConfig.RutaFilePolyTones + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 5)
                                        {
                                            File.Copy(file, objConfig.RutaFileFullTracks + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 6)
                                        {
                                            File.Copy(file, objConfig.RutaFileVideo + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 7)
                                        {
                                            File.Copy(file, objConfig.RutaFileApplications + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                        if (Convert.ToInt16(Item["contentTypeId"]) == 8)
                                        {
                                            File.Copy(file, objConfig.RutaFileThemes + "\\" + FileMaster["fileName"].ToString(), true);
                                            File.Delete(file);
                                        }
                                    }
                                }
                                Directory.Delete(objConfig.RutaZip + "\\" + strZip.Replace(".zip", ""), true);
                            }
                            else
                            {
                                Label lblError = new Label();
                                lblError.Text = "File master specifications are incorrect.";
                                lblError.ForeColor = System.Drawing.Color.Red;
                                RadGrid1.Controls.Add(lblError);

                                e.Canceled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Label lblError = new Label();
                            lblError.Text = "Unable to insert Item. Reason: " + ex.Message;
                            lblError.ForeColor = System.Drawing.Color.Red;
                            RadGrid1.Controls.Add(lblError);

                            e.Canceled = true;
                        }
                    }
                    else
                    {
                        Label lblError = new Label();
                        if (Item["itemId"].ToString() == "-2")
                            lblError.Text = "There's a record with this name";
                        else
                            lblError.Text = "Unable to insert Item";
                        lblError.ForeColor = System.Drawing.Color.Red;
                        RadGrid1.Controls.Add(lblError);

                        e.Canceled = true;
                    }
                }
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RebindGrid")
            {
                this.objItem.Dispose();
                this.objItemCategoria.Dispose();
            }
                
            if (e.CommandName == "ExportToExcel")
            {

            }
        }

        private string UnZipFile(string fileName)
        {
            ZipForge archiver = new ZipForge();
            try
            {
                archiver.FileName = objConfig.RutaZip + "\\" + fileName;
                archiver.OpenArchive(System.IO.FileMode.Open);
                archiver.BaseDir = objConfig.RutaZip + "\\" + fileName.Replace(".zip", "");
                archiver.ExtractFiles("*.*");
                archiver.CloseArchive();
                bool hay = false;
                foreach (string dir in Directory.GetDirectories(archiver.BaseDir))
                {
                    foreach (string dir_1 in Directory.GetDirectories(dir))
                    {
                        hay = true;
                        File.Delete(objConfig.RutaZip + "\\" + fileName);
                        if (hay)
                            return dir_1;
                    }
                    hay = true;
                    File.Delete(objConfig.RutaZip + "\\" + fileName);
                    if (hay)
                        return dir;
                }
                File.Delete(objConfig.RutaZip + "\\" + fileName);
                if (!hay)
                    return archiver.BaseDir;
            }
            catch (ArchiverException ae)
            {
                Label lblError = new Label();
                lblError.Text = "Message: " + ae.Message + "\t Error code: " + ae.ErrorCode + "";
                lblError.ForeColor = System.Drawing.Color.Red;
                RadGrid1.Controls.Add(lblError);
            }
            return "";
        }

        private Boolean ValidateFiles(bool minimalSpecifications, string fileList, int contentType)
        {
            bool validate = true;
            string expression = "";
            #region wallpaper
            if ((contentType == 1) || (contentType == 2))
            {
                if (minimalSpecifications)
                {
                    expression = "handsetGroupMinimal = 1 and contentType = " + contentType.ToString() + "";
                    foreach (DataRow objRow in objHandsetGroup.TableHandsetGroup.Select(expression))
                    {
                        if (validate)
                        {
                            validate = false;
                            foreach (string file in Directory.GetFiles(fileList))
                            {
                                FileInfo objFile = new FileInfo(file);
                                if (!objFile.Name.ToString().Contains(".zip"))
                                {
                                    System.Drawing.Image objImage = System.Drawing.Image.FromFile(file);
                                    string group = objImage.Width.ToString() + "x" + objImage.Height.ToString();
                                    if (group == objRow["handsetGroupName"].ToString())
                                        validate = true;
                                    objImage.Dispose();
                                }
                            }
                        }
                        return validate;
                    }
                }
                else
                {
                    expression = "contentType = " + contentType.ToString() + "";
                    foreach (DataRow objRow in objHandsetGroup.TableHandsetGroup.Select(expression))
                    {
                        if (validate)
                        {
                            validate = false;
                            foreach (string file in Directory.GetFiles(fileList))
                            {
                                FileInfo objFile = new FileInfo(file);
                                if (!objFile.Name.ToString().Contains(".zip"))
                                {
                                    System.Drawing.Image objImage = System.Drawing.Image.FromFile(file);
                                    string group = objImage.Width.ToString() + "x" + objImage.Height.ToString();
                                    if (group == objRow["handsetGroupName"].ToString())
                                        validate = true;
                                    objImage.Dispose();
                                }
                            }
                        }
                        else
                            return validate;
                    }
                }
            }
            #endregion

            #region Sonido
            if ((contentType == 3) || (contentType == 4))
            {
                if (minimalSpecifications)
                {
                    expression = "handsetGroupMinimal = 1 and contentType = " + contentType.ToString() + "";
                    foreach (DataRow objRow in objHandsetGroup.TableHandsetGroup.Select(expression))
                    {
                        if (validate)
                        {
                            validate = false;
                            foreach (string file in Directory.GetFiles(fileList))
                            {
                                FileInfo objFile = new FileInfo(file);
                                if (!objFile.Name.ToString().Contains(".zip"))
                                {
                                    string group = objFile.Extension.Replace(".", "");
                                    if (group == objRow["handsetGroupName"].ToString())
                                        validate = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    expression = "contentType = " + contentType.ToString() + "";
                    foreach (DataRow objRow in objHandsetGroup.TableHandsetGroup.Select(expression))
                    {
                        if (validate)
                        {
                            validate = false;
                            foreach (string file in Directory.GetFiles(fileList))
                            {
                                FileInfo objFile = new FileInfo(file);
                                if (!objFile.Name.ToString().Contains(".zip"))
                                {
                                    string group = objFile.Extension.Replace(".", "");
                                    if (group == objRow["handsetGroupName"].ToString())
                                        validate = true;
                                }
                            }
                        }
                    }
                }
                
                return validate;
            }
            #endregion

            #region Video
            if (contentType == 6)
            {
                if (minimalSpecifications)
                {
                    expression = "handsetGroupMinimal = 1 and contentType = " + contentType.ToString() + "";
                    foreach (DataRow objRow in objHandsetGroup.TableHandsetGroup.Select(expression))
                    {
                        if (validate)
                        {
                            validate = false;
                            foreach (string file in Directory.GetFiles(fileList))
                            {
                                FileInfo objFile = new FileInfo(file);

                                if (!objFile.Name.ToString().Contains(".zip"))
                                {
                                    Encoder enc = new Encoder();
                                    enc.FFmpegPath = objConfig.RutaFfmpeg;
                                    VideoFile inputv = new VideoFile(file);
                                    enc.GetVideoInfo(inputv);

                                    string group = inputv.Width.ToString() + "x" + inputv.Height.ToString();
                                    if(group ==  objRow["handsetGroupDimension"].ToString())
                                        validate = true;
                                    
                                    if((inputv.BitRate > double.Parse(objRow["handsetGroupBitrate"].ToString())) && (validate))
                                        validate = false;

                                    if((inputv.Duration.Seconds > double.Parse(objRow["handsetGroupDuration"].ToString())) && (validate))
                                        validate = false;

                                    if ((objFile.Length > double.Parse(objRow["handsetGroupSize"].ToString())) && (validate))
                                        validate = false;

                                    if ((double.Parse(inputv.RawVideoFormat.ToString().Split(',')[3].Split(' ')[1].Split('.')[0]) > double.Parse(objRow["handsetGroupFps"].ToString())) && (validate))
                                        validate = false;

                                    if((inputv.VideoFormat != objRow["handsetGroupFormat"].ToString()) && (validate))
                                        validate = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    expression = "contentType = " + contentType.ToString() + "";
                    foreach (DataRow objRow in objHandsetGroup.TableHandsetGroup.Select(expression))
                    {
                        if (validate)
                        {
                            validate = false;
                            foreach (string file in Directory.GetFiles(fileList))
                            {
                                FileInfo objFile = new FileInfo(file);

                                if (!objFile.Name.ToString().Contains(".zip"))
                                {
                                    Encoder enc = new Encoder();
                                    enc.FFmpegPath = objConfig.RutaFfmpeg;
                                    VideoFile inputv = new VideoFile(file);
                                    enc.GetVideoInfo(inputv);

                                    string group = inputv.Width.ToString() + "x" + inputv.Height.ToString();
                                    if (group == objRow["handsetGroupDimension"].ToString())
                                        validate = true;

                                    if ((inputv.BitRate > double.Parse(objRow["handsetGroupBitrate"].ToString())) && (validate))
                                        validate = false;

                                    if ((inputv.Duration.Seconds > double.Parse(objRow["handsetGroupDuration"].ToString())) && (validate))
                                        validate = false;

                                    if ((objFile.Length > double.Parse(objRow["handsetGroupSize"].ToString())) && (validate))
                                        validate = false;

                                    if ((double.Parse(inputv.RawVideoFormat.ToString().Split(',')[3].Split(' ')[1].Split('.')[0]) > double.Parse(objRow["handsetGroupFps"].ToString())) && (validate))
                                        validate = false;

                                    if ((inputv.VideoFormat != objRow["handsetGroupFormat"].ToString()) && (validate))
                                        validate = false;
                                }
                            }
                        }
                    }
                }

                return validate;
            }
            #endregion

            #region Video
            if ((contentType == 7) || (contentType == 9))
            {
                

                return validate;
            }
            #endregion

            return validate;
        }

        //private Boolean InfoDirectory(string dir)
        //{
        //    foreach (string dir_1 in Directory.GetDirectories(dir))
        //    {
        //        InfoSubDirectory(dir_1);
        //    }
        //}

        //private Boolean InfoSubDirectory(string dir)
        //{
        //    foreach (string dir_1 in Directory.GetDirectories(dir))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            if (this.objItem.TableItem != null)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=ReportItem.xls");
                Response.Write("<HTML>");
                Response.Write("<BODY>");
                Response.Write("<table width='600' border='1' cellspacing='0' cellpadding='0'>");
                Response.Write("	<TR>");
                Response.Write("	   <TD align='center'><b>ID</b></TD>");
                Response.Write("	   <TD align='center'><b>STR ID</b></TD>");
                Response.Write("	   <TD align='center'><b>ITEM NAME (Spanish)</b></TD>");
                Response.Write("	   <TD align='center'><b>ITEM NAME (English)</b></TD>");
                Response.Write("	   <TD align='center'><b>ITEM NAME (Portuguese)</b></TD>");
                Response.Write("	   <TD align='center'><b>DESCRIPTION (Spanish)</b></TD>");
                Response.Write("	   <TD align='center'><b>DESCRIPTION (English)</b></TD>");
                Response.Write("	   <TD align='center'><b>DESCRIPTION (Portuguese)</b></TD>");
                Response.Write("	   <TD align='center'><b>PROVIDER NAME</b></TD>");
                Response.Write("	   <TD align='center'><b>CONTENT TYPE</b></TD>");
                Response.Write("	   <TD align='center'><b>ARTIST</b></TD>");
                Response.Write("	</TR>");

                if (this.objItem.TableItem.Rows.Count > 1)
                {
                    foreach (DataRow objRow in this.objItem.TableItem.Rows)
                    {
                        if (objRow["itemId"].ToString() != "0")
                        {
                            Response.Write("	<TR>");
                            Response.Write("	   <TD align='left'>" + objRow["itemId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["itemStrId"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["itemNameSp"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["itemNameEn"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["itemNamePo"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["itemDescriptionSp"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["itemDescriptionEn"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["itemDescriptionPo"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["providerName"].ToString() + "</TD>");
                            Response.Write("	   <TD align='left'>" + objRow["contentTypeName"].ToString() + "</TD>");
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

        protected void btDelete_Click(object sender, EventArgs e)
        {
            
            foreach (DictionaryEntry entry in this.CustomersChecked)
            {
                if ( entry.Value != null)
                objItem.DeleteItem(long.Parse(entry.Key.ToString()));
                
            }
          
            this.objItem.Dispose();
            this.objItemCategoria.Dispose();
            this.RadGrid1.DataSource = null;
            objItem.TableItem.Clear();
            this.RadGrid1.Rebind();
            
        }

       

      
    }
}