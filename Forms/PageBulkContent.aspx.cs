using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.IO;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.Archiver;
using System.Threading;
using System.Xml;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;

namespace ContentAdmin
{
    public partial class PageBulkContent : System.Web.UI.Page
    {
        protected string postBackStr;
        private Configuracion objConfig = new Configuracion();
        private TipoItem objTipoItem = TipoItem.Instance;
        private Artista objArtista = Artista.Instance;
        private Categoria objCategoria = Categoria.Instance;
        private HandsetGroup objHandsetGroup = HandsetGroup.Instance;
        private Item objItem = Item.Instance;
        private Archivo objArchivo = Archivo.Instance;
        private Preview objPreview = Preview.Instance;
        private ItemPreview objItemPreview;
        private Proveedor objProveedor = Proveedor.Instance;
        private ItemCategoria objItemCategoria = ItemCategoria.Instance;
        private ItemHandset objItemHandset = ItemHandset.Instance;
        private ItemPlatformInternal objItemPI = ItemPlatformInternal.Instance;
        private int numGroups = 0;
        private int numReg = 0;

        protected override void OnInit(EventArgs e)
        {
            if (Session["validado"] == null)
                Response.Redirect("~/Login.aspx");
            objArtista.Dispose();
            objCategoria.Dispose();
            objProveedor.Dispose();
            objItemCategoria.Dispose();
            objItemHandset.Dispose();
            objItemPI.Dispose();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            postBackStr = Page.ClientScript.GetPostBackEventReference(this, "MyCustomArgument");
            statusLabel.Text = "";
            
        }

        private string GetKey()
        {
            string strDate = String.Format("{0:u}", DateTime.Now);
            strDate = strDate.Replace("-", "");
            strDate = strDate.Replace(":", "");
            strDate = strDate.Replace(" ", "");
            strDate = strDate.Replace("Z", "");

            return strDate;
        }

        protected void sendContent_Click(object sender, EventArgs e)
       {
            
            Hashtable Item = new Hashtable();
            Hashtable Dups = new Hashtable();
            XmlDocument objXml = new XmlDocument();

            TextWriter log = new StreamWriter(objConfig.RutaFileBulk + "log.txt");
            
            var logMessages = new List<String>();
            string expression = "";
            if ((chReplace.Checked == true) && (checkBox.Checked == true))
            {
                Mensaje("Error: Please select either update or replace.|error");
                return;
            }
            logMessages.Add("Begin file verification.|info");
            log.WriteLine("Begin file verification.|info");

            try
            {
                if (rdUploadFile.UploadedFiles.Count > 0)
                {
                    int onetime = 1;
                    foreach (string dir in Directory.GetDirectories(objConfig.RutaFileBulk))
                    {
                        if (!dir.Contains("Respaldo"))
                            Directory.Delete(dir, true);
                    }

                    string strZip = rdUploadFile.UploadedFiles[0].FileName;
                    string fileList = UnZipFile(strZip);
                    string xmlFile = Directory.GetFiles(fileList, "*.xml")[0];



                    if (!File.Exists(xmlFile))
                    {
                        logMessages.Add("Could not find file XML: " + xmlFile);
                        log.WriteLine("Could not find file XML: " + xmlFile);
                    }
                    else
                    {
                        string key = GetKey();

                        objXml.Load(xmlFile);

                        Dups["itemStrId"] = "";
                        foreach (XmlNode objNode in objXml.SelectNodes("/dle-batch/download-items")[0].ChildNodes)
                        {
                            if (objNode.ChildNodes.Count > 0)
                            {
                                if (numReg < numGroups)
                                {
                                    logMessages.Add("Error: You must add the correct number of files for this Content: " + Item["itemStrId"] + " Name: " + Item["itemNameSp"] + " The Item was deleted|error");
                                    log.WriteLine("Error: You must add the correct number of files for this Content: " + Item["itemStrId"] + " Name: " + Item["itemNameSp"] + " The Item was deleted|error");
                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                    numGroups = 0;
                                    numReg = 0;
                                }
                                else
                                {
                                    numGroups = 0;
                                    numReg = 0;
                                }

                                string strDate = String.Format("{0:u}", DateTime.Now);
                                Item["categoryId"] = "";
                                Item["itemUpc"] = "";
                                Item["setelement_id"] = "";
                                Item["itemIsrcGrid"] = "";
                                Item["platformId"] = "";
                                Item["itemId"] = "0";
                                Item["price"] = "0";
                                Item["key"] = key;


                                onetime = 1;
                                //Obtener la informacion del contenido
                                foreach (XmlNode objChildNode in objNode.ChildNodes)
                                {
                                    if ((objChildNode.Name == "provider-unique-id") && (chReplace.Checked) && onetime == 1)
                                    {
                                        objItem.ReplaceItem(objChildNode.InnerText.ToString());
                                        onetime = 0;

                                    }
                                    if ((objChildNode.Name == "provider-unique-id") && (!checkBox.Checked))//strIdItem
                                        Item["itemId"] = objItem.GetItemId(objChildNode.InnerText, key);

                                    if (objChildNode.Name == "provider-unique-id")//strIdItem
                                    {

                                        foreach (string itemStrIdDup in Dups["itemStrId"].ToString().Split(','))
                                        {
                                            if (objChildNode.ChildNodes.Item(0).InnerText == itemStrIdDup)
                                            {
                                                Item["itemId"] = -99;
                                                break;
                                            }
                                        }
                                    }
                                    if (long.Parse(Item["itemId"].ToString()) == 0)
                                    {
                                        if (objChildNode.ChildNodes.Count > 0)
                                        {
                                            if (objChildNode.Name == "provider-unique-id")//strIdItem
                                                Item["itemStrId"] = objChildNode.ChildNodes.Item(0).InnerText;
                                            if (objChildNode.Name == "dle-type")//ContentType
                                            {
                                                expression = "contentTypeName = '" + objChildNode.ChildNodes.Item(0).InnerText + "'";
                                                if (objTipoItem.TableTipoItem.Select(expression).Length > 0)
                                                    Item["contentTypeId"] = objTipoItem.TableTipoItem.Select(expression)[0]["contentTypeId"];
                                                else{
                                                    logMessages.Add("Error: Invalid contentTypeName on Item: " + Item["itemStrId"] + " Name: " + objChildNode.ChildNodes.Item(0).InnerText + "|error");
                                                    log.WriteLine("Error: Invalid contentTypeName on Item: " + Item["itemStrId"] + " Name: " + objChildNode.ChildNodes.Item(0).InnerText + "|error");
                                                }
                                            }

                                            if (objChildNode.Name.ToLower() == "names")
                                            {
                                                foreach (XmlNode objNameNode in objChildNode.ChildNodes)
                                                {
                                                    if (objNameNode.Name.ToLower() == "name-sp")
                                                        Item["itemNameSp"] = (objNameNode.ChildNodes.Item(0).InnerText);
                                                    if (objNameNode.Name.ToLower() == "name-en")
                                                        Item["itemNameEn"] = (objNameNode.ChildNodes.Item(0).InnerText);
                                                    if (objNameNode.Name.ToLower() == "name-po")
                                                        Item["itemNamePo"] = (objNameNode.ChildNodes.Item(0).InnerText);
                                                }
                                            }

                                            if (objChildNode.Name.ToLower() == "captions")
                                            {
                                                foreach (XmlNode objCaptionNode in objChildNode.ChildNodes)
                                                {
                                                    if (objCaptionNode.Name.ToLower() == "caption-sp")
                                                        Item["itemDescriptionSp"] = (objCaptionNode.ChildNodes.Item(0).InnerText);
                                                    if (objCaptionNode.Name.ToLower() == "caption-en")
                                                        Item["itemDescriptionEn"] = (objCaptionNode.ChildNodes.Item(0).InnerText);
                                                    if (objCaptionNode.Name.ToLower() == "caption-po")
                                                        Item["itemDescriptionPo"] = (objCaptionNode.ChildNodes.Item(0).InnerText);
                                                }
                                            }

                                            if (objChildNode.Name.ToLower() == "categories")
                                            {
                                                Hashtable Categoria = new Hashtable();
                                                foreach (XmlNode objCategoryNode in objChildNode.ChildNodes)
                                                {
                                                    switch (objCategoryNode.Name.ToLower())
                                                    {
                                                        case "category-sp":
                                                            Categoria["categoryNameSp"] = (objCategoryNode.ChildNodes.Item(0).InnerText);
                                                            Item["keywords"] = (objCategoryNode.ChildNodes.Item(0).InnerText);
                                                            break;
                                                        case "category-en":
                                                            Categoria["categoryNameEn"] = (objCategoryNode.ChildNodes.Item(0).InnerText);
                                                            Item["keywords"] += ", " + (objCategoryNode.ChildNodes.Item(0).InnerText);
                                                            break;
                                                        case "category-po":
                                                            Categoria["categoryNamePo"] = (objCategoryNode.ChildNodes.Item(0).InnerText);
                                                            Item["keywords"] += ", " + (objCategoryNode.ChildNodes.Item(0).InnerText);
                                                            break;
                                                    }
                                                }
                                                if (objCategoria.TableCategoria != null)
                                                {
                                                    expression = "categoryNameSp = '" + Categoria["categoryNameSp"].ToString() + "'";
                                                    if (objCategoria.TableCategoria.Select(expression).Length > 0)
                                                    {
                                                        Item["categoryId"] += "," + objCategoria.TableCategoria.Select(expression)[0]["categoryId"].ToString();
                                                        Categoria["categoryId"] = objCategoria.TableCategoria.Select(expression)[0]["categoryId"].ToString();
                                                        objCategoria.UpdateCategory(Categoria);
                                                    }
                                                    else
                                                    {
                                                        long cat = 0;
                                                        cat = objCategoria.InsertCategory(Categoria);

                                                        if (cat > 0)
                                                        {
                                                            logMessages.Add("Category " + Categoria["categoryNameSp"].ToString() + " sucessfully inserted|info");
                                                            log.WriteLine("Category " + Categoria["categoryNameSp"].ToString() + " sucessfully inserted|info");
                                                            
                                                            Item["categoryId"] += "," + cat.ToString();
                                                            DataRow rowCategoria = objCategoria.TableCategoria.NewRow();
                                                            rowCategoria["categoryId"] = cat;
                                                            rowCategoria["categoryNameSp"] = Categoria["categoryNameSp"].ToString();
                                                            rowCategoria["categoryNameEn"] = Categoria["categoryNameEn"].ToString();
                                                            rowCategoria["categoryNamePo"] = Categoria["categoryNamePo"].ToString();
                                                            objCategoria.TableCategoria.Rows.Add(rowCategoria);
                                                        }
                                                        else{
                                                            logMessages.Add("There was an error inserting the next category. Name " + Categoria["categoryNameSp"].ToString() + "|error");
                                                            log.WriteLine("There was an error inserting the next category. Name " + Categoria["categoryNameSp"].ToString() + "|error");
                                                        }
                                                        
                                                    }
                                                }
                                                else
                                                {
                                                    long cat = 0;
                                                    cat = objCategoria.InsertCategory(Categoria);

                                                    if (cat > 0)
                                                    {
                                                        logMessages.Add("Category " + Categoria["categoryNameSp"].ToString() + " sucessfully inserted|info");
                                                        log.WriteLine("Category " + Categoria["categoryNameSp"].ToString() + " sucessfully inserted|info");
                                                        Item["categoryId"] += "," + cat.ToString();
                                                        DataRow rowCategoria = objCategoria.TableCategoria.NewRow();
                                                        rowCategoria["categoryId"] = cat;
                                                        rowCategoria["categoryNameSp"] = Categoria["categoryNameSp"].ToString();
                                                        rowCategoria["categoryNameEn"] = Categoria["categoryNameEn"].ToString();
                                                        rowCategoria["categoryNamePo"] = Categoria["categoryNamePo"].ToString();
                                                        objCategoria.TableCategoria.Rows.Add(rowCategoria);
                                                    }
                                                    else{
                                                        logMessages.Add("There was an error inserting the next category. Name " + Categoria["categoryNameSp"].ToString() + "|error");
                                                        log.WriteLine("There was an error inserting the next category. Name " + Categoria["categoryNameSp"].ToString() + "|error");
                                                    }

                                                }
                                            }

                                            if (objChildNode.Name.ToLower() == "resources")
                                            {
                                                logMessages.Add("checking files: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + "|info");
                                                log.WriteLine("checking files: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + "|info");
                                                Item["itemId"] = -4;
                                                if (Item["itemAdvisory"] == null){
                                                    logMessages.Add("Information data 'advisory' was not provided.|error");
                                                    log.WriteLine("Information data 'advisory' was not provided.|error");
                                                }
                                                else if (Item["chargeTypeId"] == null){
                                                    logMessages.Add("Information data 'chargeTypeId' was not provided.|error");
                                                    log.WriteLine("Information data 'chargeTypeId' was not provided.|error");
                                                }
                                                else if (Item["providerId"] == null){
                                                    logMessages.Add("The content provider don't exists, verify the next item: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + " NO existe.|error");
                                                    log.WriteLine("The content provider don't exists, verify the next item: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + " NO existe.|error");
                                                }
                                                else
                                                {
                                                    DataTable objResultado = new DataTable();
                                                    if (checkBox.Checked)
                                                    {
                                                        objResultado = this.objItem.UpdateItem(Item);
                                                        if (objResultado != null)
                                                        {
                                                            Item["itemId"] = long.Parse(objResultado.Rows[0]["Status"].ToString());
                                                            objItemCategoria.ItemId = long.Parse(objResultado.Rows[0]["itemId"].ToString());
                                                            objItemHandset.ItemId = long.Parse(objResultado.Rows[0]["itemId"].ToString());
                                                            objItemPI.ItemId = long.Parse(objResultado.Rows[0]["itemId"].ToString());
                                                        }
                                                        else
                                                        {
                                                            logMessages.Add("The content : " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + ". Don't exists.|error");
                                                            log.WriteLine("The content : " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + ". Don't exists.|error");
                                                            Item["itemId"] = "-99";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Item["itemId"] = Convert.ToInt32(this.objItem.InsertItem(Item));
                                                        objItemCategoria.ItemId = long.Parse(Item["itemId"].ToString());
                                                        objItemHandset.ItemId = long.Parse(Item["itemId"].ToString());
                                                        objItemPI.ItemId = long.Parse(Item["itemId"].ToString());
                                                    }
                                                }
                                                objItemPI.DeleteItemPlatformInternal();
                                                foreach (XmlNode objNodo in objXml.SelectNodes("/dle-batch/download-items/download-item/platforms")[0].ChildNodes)
                                                {
                                                    objItemPI.PlatformId = int.Parse(objNodo.ChildNodes.Item(0).InnerText);
                                                    if (objNodo.Attributes.Count > 0)
                                                    {
                                                        if (objNodo.Attributes["price"].Name == "price")
                                                            objItemPI.Precio = decimal.Parse(objNodo.Attributes["price"].Value);
                                                    }
                                                    objItemPI.InsertItemPlatformInternal();
                                                    objItemPI.Precio = 0;
                                                    objItemPI.PlatformId = 0;
                                                }

                                                if (long.Parse(Item["itemId"].ToString()) == 0)
                                                {
                                                    objItemCategoria.DeleteItemCategory();
                                                    objItemCategoria.InsertItemCategory(Item["categoryId"].ToString());
                                                    logMessages.Add("The Item: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + " sucessfully updated|info");
                                                    log.WriteLine("The Item: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + " sucessfully updated|info");

                                                }

                                                else if (long.Parse(Item["itemId"].ToString()) >= 0)
                                                {
                                                    objItemCategoria.ItemId = Convert.ToInt32(Item["itemId"].ToString());
                                                    objItemCategoria.InsertItemCategory(Item["categoryId"].ToString());
                                                    objItemPI.InsertItemPlatformInternal(Item["platformId"].ToString());

                                                    Hashtable FileMaster = new Hashtable();
                                                    logMessages.Add("Item: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + " sucessfully inserted|info");
                                                    log.WriteLine("Item: " + Item["itemStrId"].ToString() + " - " + Item["itemNameSp"].ToString() + " sucessfully inserted|info");
                                                    int i = 0, j = 0;
                                                    foreach (XmlNode objResourceNode in objChildNode.ChildNodes)
                                                    {
                                                        string file = string.Empty;
                                                        /***TODO: Verificar la cantidad de archivos que estan enviando, para asegurar que estan enviando lo minimo***/
                                                        if (objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "preview")
                                                        {
                                                            numReg += 1;
                                                            if (objResourceNode.ChildNodes.Item(2).Name.ToLower() == "resource-id")
                                                                file = fileList + "\\" + objResourceNode.ChildNodes.Item(2).InnerText;
                                                            else if (objResourceNode.ChildNodes.Item(3).Name.ToLower() == "resource-id")
                                                                file = fileList + "\\" + objResourceNode.ChildNodes.Item(3).InnerText;

                                                            if ((File.Exists(file)) && (long.Parse(Item["itemId"].ToString()) != -99))
                                                            {
                                                                FileInfo objFile = new FileInfo(file);
                                                                Hashtable Preview = new Hashtable();
                                                                Preview["previewType"] = 0;
                                                                Preview["previewDefault"] = 1;
                                                                if (i != 0)
                                                                    Preview["previewDefault"] = 0;
                                                                Preview["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Preview_" + objFile.Name;
                                                                Preview["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                                                Preview["previewId"] = Convert.ToInt32(this.objPreview.InsertPreview(Preview));
                                                                File.Copy(file, objConfig.RutaPreview + "\\" + Preview["previewName"].ToString(), true);
                                                                if (Convert.ToInt32(Preview["previewId"]) > 0)
                                                                {
                                                                    objItemPreview = new ItemPreview();
                                                                    objItemPreview.ItemId = Convert.ToInt32(Item["itemId"]);
                                                                    objItemPreview.PreviewId = Convert.ToInt32(Preview["previewId"]);
                                                                    if (objItemPreview.InsertItemPreview() < 0)
                                                                        Mensaje("Ha ocurrido un error inserting itemPreview.|error");
                                                                    this.objPreview.Dispose();
                                                                }
                                                                else
                                                                {
                                                                    logMessages.Add("There was an error inserting the next file: " + objFile.Name + "|error");
                                                                    log.WriteLine("There was an error inserting the next file: " + objFile.Name + "|error");
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    logMessages.Add("Item sucessfully deleted.|error");
                                                                    log.WriteLine("Item sucessfully deleted.|error");
                                                                }

                                                                i++;
                                                            }
                                                            else
                                                            {
                                                                if (long.Parse(Item["itemId"].ToString()) != -99)
                                                                {
                                                                    logMessages.Add("Could not find file preview: " + file + ". Item sucessfully deleted.|error");
                                                                    log.WriteLine("Could not find file preview: " + file + ". Item sucessfully deleted.|error");
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                        }
                                                        else if (objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "thumbnail")
                                                        {
                                                            if (objResourceNode.ChildNodes.Item(2).Name.ToLower() == "resource-id")
                                                                file = fileList + "\\" + objResourceNode.ChildNodes.Item(2).InnerText;
                                                            else if (objResourceNode.ChildNodes.Item(3).Name.ToLower() == "resource-id")
                                                                file = fileList + "\\" + objResourceNode.ChildNodes.Item(3).InnerText;

                                                            if ((File.Exists(file)) && (long.Parse(Item["itemId"].ToString()) != -99))
                                                            {
                                                                FileInfo objFile = new FileInfo(file);
                                                                Hashtable Preview = new Hashtable();
                                                                Preview["previewType"] = 1;
                                                                Preview["previewDefault"] = 1;
                                                                if (j != 0)
                                                                    Preview["previewDefault"] = 0;
                                                                Preview["previewName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_Thumbnail_" + objFile.Name;
                                                                Preview["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                                                Preview["previewId"] = Convert.ToInt32(this.objPreview.InsertPreview(Preview));
                                                                File.Copy(file, objConfig.RutaThumbnail + "\\" + Preview["previewName"].ToString(), true);
                                                                if (Convert.ToInt32(Preview["previewId"]) > 0)
                                                                {
                                                                    objItemPreview = new ItemPreview();
                                                                    objItemPreview.ItemId = Convert.ToInt32(Item["itemId"]);
                                                                    objItemPreview.PreviewId = Convert.ToInt32(Preview["previewId"]);
                                                                    if (objItemPreview.InsertItemPreview() < 0){
                                                                        logMessages.Add("There was an error inserting the thumbnail file " + file + "|error");
                                                                        log.WriteLine("There was an error inserting the thumbnail file " + file + "|error");
                                                                    }
                                                                    this.objPreview.Dispose();
                                                                }
                                                                else
                                                                {
                                                                    logMessages.Add("There was an error inserting the thumbnail file: " + file + ". Item sucessfully deleted.|error");
                                                                    log.WriteLine("There was an error inserting the thumbnail file: " + file + ". Item sucessfully deleted.|error");
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                }

                                                                j++;
                                                            }
                                                            else
                                                            {
                                                                if (long.Parse(Item["itemId"].ToString()) != -99)
                                                                {
                                                                    logMessages.Add("Could not find file thumbnail: " + file + ". Item sucessfully deleted.|error");
                                                                    log.WriteLine("Could not find file thumbnail: " + file + ". Item sucessfully deleted.|error");
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                        }
                                                        else if ((objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "game") ||
                                                            (objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "application"))
                                                        {
                                                            numReg += 1;
                                                            if (long.Parse(Item["itemId"].ToString()) != -99)
                                                            {
                                                                string newName = string.Empty;
                                                                string oldName = string.Empty;
                                                                Item["fileId"] = "";
                                                                foreach (XmlNode objGameNode in objResourceNode.ChildNodes)
                                                                {
                                                                    long fileId = 0;
                                                                    if (objGameNode.Name.ToLower() == "files")
                                                                    {
                                                                        foreach (XmlNode objGameNodeChild in objGameNode.ChildNodes)
                                                                        {
                                                                            file = fileList + "\\" + objGameNodeChild.ChildNodes.Item(1).InnerText;

                                                                            if (File.Exists(file))
                                                                            {
                                                                                FileInfo objFile = new FileInfo(file);
                                                                                if (ValidateFiles(objFile, int.Parse(Item["contentTypeId"].ToString()), null))
                                                                                {
                                                                                    if (Convert.ToInt16(Item["contentTypeId"]) == 7)
                                                                                        FileMaster["fileProviderType"] = "Application_" + objFile.Extension.Replace(".", "");
                                                                                    else
                                                                                        FileMaster["fileProviderType"] = "Game_" + objFile.Extension.Replace(".", "");
                                                                                    FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + objFile.Name;
                                                                                    FileMaster["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                                                                    FileMaster["itemId"] = Item["itemId"];
                                                                                    FileMaster["fileType"] = 0;
                                                                                    FileMaster["fileSize"] = objFile.Length;

                                                                                    if (objFile.Extension == ".jar")
                                                                                    {
                                                                                        oldName = objFile.Name;
                                                                                        newName = FileMaster["fileName"].ToString();
                                                                                    }

                                                                                    int resul = 0;
                                                                                    if (objFile.Extension == ".jad")
                                                                                        resul = UpdateJad(file, oldName, newName);

                                                                                    if (resul == 0)
                                                                                    {
                                                                                        fileId = objArchivo.InsertFile(FileMaster);

                                                                                        if (fileId == -2){
                                                                                            logMessages.Add("Master file already exists: " + objFile.Name + "|error");
                                                                                            log.WriteLine("Master file already exists: " + objFile.Name + "|error");
                                                                                        }
                                                                                        else if (fileId == -1){
                                                                                            logMessages.Add("There was an error inserting the Master file: " + objFile.Name + "|error");
                                                                                            log.WriteLine("There was an error inserting the Master file: " + objFile.Name + "|error");
                                                                                        }
                                                                                        else
                                                                                            Item["fileId"] += "," + fileId.ToString();

                                                                                        if (Convert.ToInt16(Item["contentTypeId"]) == 7)
                                                                                            File.Copy(file, objConfig.RutaFileApplications + "\\" + FileMaster["fileName"].ToString(), true);
                                                                                        else
                                                                                            File.Copy(file, objConfig.RutaFileGames + "\\" + FileMaster["fileName"].ToString(), true);
                                                                                        objArchivo.Dispose();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        logMessages.Add("Could not find the jar file. " + file + ". Item sucessfully deleted.|error");
                                                                                        log.WriteLine("Could not find the jar file. " + file + ". Item sucessfully deleted.|error");
                                                                                        objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                                        Item["itemId"] = -99;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                                    Item["itemId"] = -99;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    if (objGameNode.Name.ToLower() == "supported-handsets")
                                                                    {
                                                                        objItemHandset.ItemId = long.Parse(Item["itemId"].ToString());
                                                                        foreach (XmlNode objGameNodeChild in objGameNode.ChildNodes)
                                                                        {
                                                                            foreach (string id in Item["fileId"].ToString().Split(','))
                                                                            {
                                                                                if ((id != "") && (id != "-1"))
                                                                                {
                                                                                    objItemHandset.FileId = long.Parse(id);
                                                                                    objItemHandset.HandsetStrId = (objGameNodeChild).Attributes["value"].Value;
                                                                                    string duplicateHandsets = objItemHandset.DupsHandsets();

                                                                                    if (duplicateHandsets != null)
                                                                                    {
                                                                                        Dups["itemStrId"] += "," + duplicateHandsets;
                                                                                        logMessages.Add("File in 2 or More Handsets. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                        log.WriteLine("File in 2 or More Handsets. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                        objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                                        Item["itemId"] = -99;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        long ih = objItemHandset.InsertItemHandset();
                                                                                        if (ih < 0)
                                                                                        {
                                                                                            logMessages.Add("There was an error inserting the Handset. Handset don't exists. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                            log.WriteLine("There was an error inserting the Handset. Handset don't exists. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                            objItem.ItemKeyDelete(key);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "video")
                                                        {
                                                            numReg += 1;
                                                            file = fileList + "\\" + objResourceNode.ChildNodes.Item(4).InnerText;
                                                            string type = objResourceNode.ChildNodes.Item(1).InnerText;

                                                            if ((File.Exists(file)) && (long.Parse(Item["itemId"].ToString()) != -99))
                                                            {
                                                                FileInfo objFile = new FileInfo(file);
                                                                if (ValidateFiles(objFile, int.Parse(Item["contentTypeId"].ToString()), type))
                                                                {
                                                                    VideoInfo objInfo = new VideoInfo();
                                                                    objInfo = GetVideoInfo(objFile);
                                                                    FileMaster["fileProviderType"] = "Video_" + type;
                                                                    FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + objFile.Name;
                                                                    FileMaster["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                                                    FileMaster["itemId"] = Item["itemId"];
                                                                    FileMaster["fileType"] = 0;
                                                                    FileMaster["fileSize"] = objFile.Length;
                                                                    //Atributos video
                                                                    FileMaster["video_codec"] = objInfo.Vcodec;
                                                                    FileMaster["audio_bitrate"] = objInfo.Input_Audio_Bitrate;
                                                                    FileMaster["video_bitrate"] = objInfo.Input_Video_Bitrate;
                                                                    FileMaster["fps"] = int.Parse(objInfo.FrameRate.Split(' ')[0].Split('.')[0]);
                                                                    FileMaster["bitrate"] = objInfo.Video_Bitrate;
                                                                    if ((type == "class1") || (type == "class2") || (type == "class3"))
                                                                        FileMaster["audio_codec"] = "amrnb";
                                                                    else
                                                                        FileMaster["audio_codec"] = objInfo.Acodec;
                                                                    if ((type == "class1") || (type == "class2") || (type == "class3") || (type == "class4"))
                                                                        FileMaster["format"] = "qcif";
                                                                    else
                                                                        FileMaster["format"] = "qvga";

                                                                    if (objArchivo.InsertFile(FileMaster) < 0){
                                                                        logMessages.Add("There was an error inserting the Master file video: " + objFile.Name + "|error");
                                                                        log.WriteLine("There was an error inserting the Master file video: " + objFile.Name + "|error");
                                                                    }
                                                                    File.Copy(file, objConfig.RutaFileVideo + "\\" + FileMaster["fileName"].ToString(), true);

                                                                    objArchivo.Dispose();
                                                                }
                                                                else
                                                                {
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (long.Parse(Item["itemId"].ToString()) != -99)
                                                                {
                                                                    logMessages.Add("Could not find master file video: " + file + ". Item sucessfully deleted.|error");
                                                                    log.WriteLine("Could not find master file video: " + file + ". Item sucessfully deleted.|error");
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                        }
                                                        else if (objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "full_track")
                                                        {
                                                            numReg += 1;
                                                            file = fileList + "\\" + objResourceNode.ChildNodes.Item(4).InnerText;
                                                            if ((File.Exists(file)) && (long.Parse(Item["itemId"].ToString()) != -99))
                                                            {
                                                                FileInfo objFile = new FileInfo(file);
                                                                string group = objResourceNode.ChildNodes.Item(1).InnerText.ToLower();
                                                                if (ValidateFiles(objFile, int.Parse(Item["contentTypeId"].ToString()), group))
                                                                {
                                                                    FileMaster["fileProviderType"] = "Full_Track_" + group;
                                                                    FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + objFile.Name;
                                                                    if (objFile.Extension.Replace(".", "").ToLower() == "aac")
                                                                        FileMaster["mimeTypeSuffixes"] = "aac_1";
                                                                    else
                                                                        FileMaster["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                                                    FileMaster["itemId"] = Item["itemId"];
                                                                    FileMaster["fileType"] = 0;
                                                                    FileMaster["fileSize"] = objFile.Length;

                                                                    if (objArchivo.InsertFile(FileMaster) < 0){
                                                                        logMessages.Add("There was an error inserting the Master file: " + objFile.Name + "|error");
                                                                        log.WriteLine("There was an error inserting the Master file: " + objFile.Name + "|error");
                                                                    }

                                                                    File.Copy(file, objConfig.RutaFileFullTracks + "\\" + FileMaster["fileName"].ToString(), true);

                                                                    objArchivo.Dispose();
                                                                }
                                                                else
                                                                {
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (long.Parse(Item["itemId"].ToString()) != -99)
                                                                {
                                                                    logMessages.Add("Could not find master file: " + file + ". Item sucessfully deleted.|error");
                                                                    log.WriteLine("Could not find master file: " + file + ". Item sucessfully deleted.|error");
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            numReg += 1;
                                                            file = fileList + "\\" + objResourceNode.ChildNodes.Item(4).InnerText;
                                                            if ((File.Exists(file)) && (long.Parse(Item["itemId"].ToString()) != -99))
                                                            {
                                                                FileInfo objFile = new FileInfo(file);
                                                                if (ValidateFiles(objFile, int.Parse(Item["contentTypeId"].ToString()), null))
                                                                {
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


                                                                    string classType = objResourceNode.ChildNodes.Item(1).InnerText;
                                                                    classType = classType.Replace("/", "");
                                                                    if (classType != null && (Convert.ToInt16(Item["contentTypeId"]) == 3 || Convert.ToInt16(Item["contentTypeId"]) == 4))
                                                                        FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + classType + "_" + objFile.Name;
                                                                    else
                                                                        FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + objFile.Name;
                                                                    FileMaster["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                                                    FileMaster["itemId"] = Item["itemId"];
                                                                    FileMaster["fileType"] = 0;
                                                                    FileMaster["fileSize"] = objFile.Length;
                                                                    FileMaster["classType"] = classType;
                                                                    if (objArchivo.InsertFile(FileMaster) < 0){
                                                                        logMessages.Add("There was an error inserting the Master file: " + objFile.Name + "|error");
                                                                        log.WriteLine("There was an error inserting the Master file: " + objFile.Name + "|error");
                                                                    }

                                                                    if (Convert.ToInt16(Item["contentTypeId"]) == 1)
                                                                        File.Copy(file, objConfig.RutaFileWallpaper + "\\" + FileMaster["fileName"].ToString(), true);
                                                                    if (Convert.ToInt16(Item["contentTypeId"]) == 2)
                                                                        File.Copy(file, objConfig.RutaFileAnimations + "\\" + FileMaster["fileName"].ToString(), true);
                                                                    if (Convert.ToInt16(Item["contentTypeId"]) == 3)
                                                                        File.Copy(file, objConfig.RutaFileRealTones + "\\" + FileMaster["fileName"].ToString(), true);
                                                                    if (Convert.ToInt16(Item["contentTypeId"]) == 4)
                                                                        File.Copy(file, objConfig.RutaFilePolyTones + "\\" + FileMaster["fileName"].ToString(), true);
                                                                    if (Convert.ToInt16(Item["contentTypeId"]) == 5)
                                                                        File.Copy(file, objConfig.RutaFileFullTracks + "\\" + FileMaster["fileName"].ToString(), true);
                                                                    if (Convert.ToInt16(Item["contentTypeId"]) == 6)
                                                                        File.Copy(file, objConfig.RutaFileVideo + "\\" + FileMaster["fileName"].ToString(), true);
                                                                    objArchivo.Dispose();
                                                                }
                                                                else
                                                                {
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (long.Parse(Item["itemId"].ToString()) != -99)
                                                                {
                                                                    logMessages.Add("Could not find master file: " + file + ". Item sucessfully deleted.|error");
                                                                    log.WriteLine("Could not find master file: " + file + ". Item sucessfully deleted.|error");
                                                                    objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                    Item["itemId"] = -99;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (long.Parse(Item["itemId"].ToString()) == -2){
                                                        logMessages.Add("The Item name " + Item["itemNameSp"].ToString() + " already exists.|error");
                                                        log.WriteLine("The Item name " + Item["itemNameSp"].ToString() + " already exists.|error");
                                                    }
                                                    else if (long.Parse(Item["itemId"].ToString()) == -3){
                                                        logMessages.Add("There was an error inserting the Item. The content Id " + Item["itemStrId"].ToString() + " exists.|error");
                                                        log.WriteLine("There was an error inserting the Item. The content Id " + Item["itemStrId"].ToString() + " exists.|error");
                                                    }
                                                    else{
                                                        logMessages.Add("There was an error inserting the Item. Error code: " + Item["itemId"].ToString() + "|error");
                                                        log.WriteLine("There was an error inserting the Item. The content Id " + Item["itemStrId"].ToString() + " exists.|error");
                                                    }
                                                }
                                            }
                                        }
                                        if (objChildNode.Name.ToLower() == "meta")
                                        {
                                            if ((objChildNode).Attributes["name"].Value == "advisory")
                                                Item["itemAdvisory"] = (objChildNode).Attributes["value"].Value;
                                            if ((objChildNode).Attributes["name"].Value == "type")
                                                Item["chargeTypeId"] = (objChildNode).Attributes["value"].Value;
                                            if ((objChildNode).Attributes["name"].Value == "isrc_grid")
                                                Item["itemIsrcGrid"] = (objChildNode).Attributes["value"].Value;
                                            if ((objChildNode).Attributes["name"].Value == "upc")
                                                Item["itemUpc"] = (objChildNode).Attributes["value"].Value;
                                            if ((objChildNode).Attributes["name"].Value == "settlement_id")
                                                Item["settlement_id"] = (objChildNode).Attributes["value"].Value;
                                            if ((objChildNode).Attributes["name"].Value == "primary_provider")
                                            {
                                                expression = "providerName = '" + (objChildNode).Attributes["value"].Value + "'".Replace("  ", " ");
                                                if (objProveedor.TableProveedor.Select(expression).Length > 0)
                                                    Item["providerId"] = objProveedor.TableProveedor.Select(expression)[0]["providerId"];
                                            }
                                            if ((objChildNode).Attributes["name"].Value == "artist")
                                            {
                                                if (objArtista.TableArtista != null)
                                                {
                                                    expression = "artistName = '" + (objChildNode).Attributes["value"].Value + "'".Replace("  ", " ");
                                                    if (objArtista.TableArtista.Select(expression).Length > 0)
                                                        Item["artistId"] = objArtista.TableArtista.Select(expression)[0]["artistId"];
                                                    else
                                                    {
                                                        Hashtable Artista = new Hashtable();
                                                        Artista["artistName"] = (objChildNode).Attributes["value"].Value;
                                                        Item["artistId"] = objArtista.InsertArtist(Artista);
                                                        if (long.Parse(Item["artistId"].ToString()) > 0)
                                                        {
                                                            logMessages.Add("Artist inserted: " + (objChildNode).Attributes["value"].Value + "|info");
                                                            log.WriteLine("Artist inserted: " + (objChildNode).Attributes["value"].Value + "|info");
                                                            DataRow rowArtista = objArtista.TableArtista.NewRow();
                                                            rowArtista["artistId"] = long.Parse(Item["artistId"].ToString());
                                                            rowArtista["artistName"] = Artista["artistName"].ToString();
                                                            objArtista.TableArtista.Rows.Add(rowArtista);
                                                        }
                                                        else{
                                                            logMessages.Add("There was an error inserting the Artists. " + (objChildNode).Attributes["value"].Value + "|error");
                                                            log.WriteLine("There was an error inserting the Artists. " + (objChildNode).Attributes["value"].Value + "|error");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Hashtable Artista = new Hashtable();
                                                    Artista["artistName"] = (objChildNode).Attributes["value"].Value;
                                                    Item["artistId"] = objArtista.InsertArtist(Artista);
                                                    if (long.Parse(Item["artistId"].ToString()) > 0)
                                                    {
                                                        logMessages.Add("Artist inserted: " + (objChildNode).Attributes["value"].Value + "|info");
                                                        log.WriteLine("Artist inserted: " + (objChildNode).Attributes["value"].Value + "|info");
                                                        DataRow rowArtista = objArtista.TableArtista.NewRow();
                                                        rowArtista["artistId"] = long.Parse(Item["artistId"].ToString());
                                                        rowArtista["artistName"] = Artista["artistName"].ToString();
                                                        objArtista.TableArtista.Rows.Add(rowArtista);
                                                    }
                                                    else{
                                                        logMessages.Add("There was an error inserting the Artists. " + (objChildNode).Attributes["value"].Value + "|error");
                                                        log.WriteLine("There was an error inserting the Artists. " + (objChildNode).Attributes["value"].Value + "|error");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (long.Parse(Item["itemId"].ToString()) > 0)
                                    {
                                        Hashtable FileMaster = new Hashtable();
                                        if (objNode.ChildNodes.Item(7).Attributes.Item(0).InnerText == "primary_provider")
                                            expression = "providerName = '" + objNode.ChildNodes.Item(7).Attributes.Item(1).InnerText + "'".Replace("  ", " ");
                                        else if (objNode.ChildNodes.Item(8).Attributes.Item(0).InnerText == "primary_provider")
                                            expression = "providerName = '" + objNode.ChildNodes.Item(8).Attributes.Item(1).InnerText + "'".Replace("  ", " ");
                                        else if (objNode.ChildNodes.Item(9).Attributes.Item(0).InnerText == "primary_provider")
                                            expression = "providerName = '" + objNode.ChildNodes.Item(9).Attributes.Item(1).InnerText + "'".Replace("  ", " ");

                                        if (objProveedor.TableProveedor.Select(expression).Length > 0)
                                            Item["providerId"] = objProveedor.TableProveedor.Select(expression)[0]["providerId"];

                                        foreach (XmlNode objResourceNode in objChildNode.ChildNodes)
                                        {
                                            string file = string.Empty;
                                            if (objChildNode.Name.ToLower() == "resources")
                                            {
                                                if ((objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "game") ||
                                                            (objResourceNode.ChildNodes.Item(0).InnerText.ToLower() == "application"))
                                                {
                                                    numReg += 1;
                                                    string contentType = objResourceNode.ChildNodes.Item(0).InnerText;
                                                    expression = "contentTypeName = '" + objResourceNode.ChildNodes.Item(0).InnerText.ToLower() + "'";
                                                    Item["contentTypeId"] = objTipoItem.TableTipoItem.Select(expression)[0]["contentTypeId"];

                                                    if (long.Parse(Item["itemId"].ToString()) != -99)
                                                    {
                                                        Item["fileId"] = "";
                                                        foreach (XmlNode objGameNode in objResourceNode.ChildNodes)
                                                        {
                                                            long fileId = 0;
                                                            string mimeTypeSuffixes = string.Empty;

                                                            if (objGameNode.Name.ToLower() == "files")
                                                            {
                                                                string newName = string.Empty;
                                                                string oldName = string.Empty;
                                                                foreach (XmlNode objGameNodeChild in objGameNode.ChildNodes)
                                                                {
                                                                    file = fileList + "\\" + objGameNodeChild.ChildNodes.Item(1).InnerText;

                                                                    if (File.Exists(file))
                                                                    {
                                                                        FileInfo objFile = new FileInfo(file);
                                                                        if (ValidateFiles(objFile, int.Parse(Item["contentTypeId"].ToString()), null))
                                                                        {
                                                                            FileMaster["fileProviderType"] = contentType + objFile.Extension.Replace(".", "");
                                                                            FileMaster["fileName"] = Item["itemId"].ToString() + "_" + Item["providerId"] + "_" + objFile.Name;
                                                                            FileMaster["mimeTypeSuffixes"] = objFile.Extension.Replace(".", "");
                                                                            FileMaster["itemId"] = Item["itemId"];
                                                                            FileMaster["fileType"] = 0;
                                                                            FileMaster["fileSize"] = objFile.Length;

                                                                            mimeTypeSuffixes = objFile.Extension.Replace(".", "");


                                                                            if (objFile.Extension == ".jar")
                                                                            {
                                                                                oldName = objFile.Name;
                                                                                newName = FileMaster["fileName"].ToString();
                                                                            }

                                                                            int resul = 0;
                                                                            if (objFile.Extension == ".jad")
                                                                                resul = UpdateJad(file, oldName, newName);

                                                                            if (resul == 0)
                                                                            {
                                                                                fileId = objArchivo.InsertFile(FileMaster);
                                                                                if (fileId == -2){
                                                                                    logMessages.Add("Master file already exists: " + objFile.Name + "|error");
                                                                                    log.WriteLine("Master file already exists: " + objFile.Name + "|error");
                                                                                }
                                                                                if (fileId == -1){
                                                                                    logMessages.Add("There was an error inserting the Master file: " + objFile.Name + "|error");
                                                                                    log.WriteLine("Master file already exists: " + objFile.Name + "|error");
                                                                                }
                                                                                else
                                                                                    Item["fileId"] += "," + fileId.ToString();

                                                                                if (Convert.ToInt16(Item["contentTypeId"]) == 7)
                                                                                    File.Copy(file, objConfig.RutaFileApplications + "\\" + FileMaster["fileName"].ToString(), true);
                                                                                else
                                                                                    File.Copy(file, objConfig.RutaFileGames + "\\" + FileMaster["fileName"].ToString(), true);

                                                                                //File.Copy(file, objConfig.RutaFileGames + "\\" + FileMaster["fileName"].ToString(), true);
                                                                                objArchivo.Dispose();
                                                                            }
                                                                            else
                                                                            {
                                                                                logMessages.Add("Could not find the jar file. " + file + ". Item sucessfully deleted.|error");
                                                                                log.WriteLine("Could not find the jar file. " + file + ". Item sucessfully deleted.|error");
                                                                                objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                                Item["itemId"] = -99;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                            Item["itemId"] = -99;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        logMessages.Add("There was an error inserting the format: " + file + "|error");
                                                                        log.WriteLine("There was an error inserting the format: " + file + "|error");
                                                                        objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                        Item["itemId"] = -99;
                                                                    }
                                                                }
                                                            }
                                                            if (objGameNode.Name.ToLower() == "supported-handsets")
                                                            {
                                                                objItemHandset.ItemId = long.Parse(Item["itemId"].ToString());
                                                                foreach (XmlNode objGameNodeChild in objGameNode.ChildNodes)
                                                                {
                                                                    foreach (string id in Item["fileId"].ToString().Split(','))
                                                                    {
                                                                        if ((id != "") && (id != "-1"))
                                                                        {
                                                                            objItemHandset.FileId = long.Parse(id);
                                                                            objItemHandset.HandsetStrId = (objGameNodeChild).Attributes["value"].Value;
                                                                            string duplicateHandsets = objItemHandset.DupsHandsets();

                                                                            if (duplicateHandsets != null)
                                                                            {
                                                                                Dups["itemStrId"] += "," + duplicateHandsets;
                                                                                logMessages.Add("File in 2 or More Handsets. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                log.WriteLine("File in 2 or More Handsets. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                                                                                Item["itemId"] = -99;
                                                                            }
                                                                            else
                                                                            {
                                                                                long ih = objItemHandset.InsertItemHandset();
                                                                                if (ih < 0)
                                                                                {
                                                                                    logMessages.Add("There was an error inserting the Handset. Handset don't exists. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                    log.WriteLine("There was an error inserting the Handset. Handset don't exists. Name: " + (objGameNodeChild).Attributes["value"].Value + "|error");
                                                                                    objItem.ItemKeyDelete(key);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (long.Parse(Item["itemId"].ToString()) == -99){
                                            logMessages.Add("There was an error inserting the Item. Error code: " + Item["itemId"].ToString() + "|error");
                                            log.WriteLine("There was an error inserting the Item. Error code: " + Item["itemId"].ToString() + "|error");
                                        }
                                    }
                                    else if (long.Parse(Item["itemId"].ToString()) == -9)
                                    {
                                        if (objChildNode.Name == "provider-unique-id"){//strIdItem
                                            logMessages.Add("ERROR: The provider-unique-id already exists. " + objChildNode.ChildNodes.Item(0).InnerText + "|error");
                                            log.WriteLine("There was an error inserting the Item. Error code: " + Item["itemId"].ToString() + "|error");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                logMessages.Add("Invalid XML file.|error");
                                log.WriteLine("Invalid XML file.|error");
                            }
                        }

                        //Create directory if not found giving an error so Write Code  by TI

                        if (!Directory.Exists(objConfig.RutaFileBulkRespaldo))
                        {
                            Directory.CreateDirectory(objConfig.RutaFileBulkRespaldo);
                        }
                        if ((chReplace.Checked) && (File.Exists(objConfig.RutaFileBulkRespaldo + "\\" + rdUploadFile.UploadedFiles[0].FileName)))
                        {
                            File.Delete(objConfig.RutaFileBulkRespaldo + "\\" + rdUploadFile.UploadedFiles[0].FileName);
                        }
                        if (!File.Exists(objConfig.RutaFileBulkRespaldo + "\\" + rdUploadFile.UploadedFiles[0].FileName))
                            File.Move(objConfig.RutaFileBulk + "\\" + rdUploadFile.UploadedFiles[0].FileName,
                                objConfig.RutaFileBulkRespaldo + "\\" + rdUploadFile.UploadedFiles[0].FileName);
                        else
                            File.Move(objConfig.RutaFileBulk + "\\" + rdUploadFile.UploadedFiles[0].FileName,
                                objConfig.RutaFileBulkRespaldo + "\\" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Millisecond + "_" +
                                rdUploadFile.UploadedFiles[0].FileName);
                        if (numReg < numGroups)
                        {
                            logMessages.Add("Error: You must add the correct number of files for this Content: " + Item["itemStrId"] + " Name: " + Item["itemNameSp"] + ". The Item was deleted|error");
                            log.WriteLine("Error: You must add the correct number of files for this Content: " + Item["itemStrId"] + " Name: " + Item["itemNameSp"] + ". The Item was deleted|error");
                            objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                            numGroups = 0;
                            numReg = 0;
                        }
                        else
                        {
                            numGroups = 0;
                            numReg = 0;
                        }

                        objItem.Dispose();
                    }
                }
                else
                {
                    logMessages.Add("There was an error reading the zip file.|error");
                    log.WriteLine("There was an error reading the zip file.|error");
                }
            }
            catch (Exception ex)
            {
                logMessages.Add("Generic error. Description: " + ex.Message + "|error");
                log.WriteLine("Generic error. Description: " + ex.Message + "|error");
                if (Item["itemId"] != null)
                {
                    if (Item["itemId"].ToString() != "")
                        objItem.DeleteItem(long.Parse(Item["itemId"].ToString()));
                }
            }

            logMessages.Add("End file verification.|info");
            log.WriteLine("End file verification.|info");

            foreach(var logScreen in logMessages){
                Mensaje(logScreen);
            }
            statusLabel.Text = "Verify Log";
            log.Close();
        }

        protected void downloadLog_Click(object sender, EventArgs e)
        {
                string URL = objConfig.RutaFileBulk + "log.txt";
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
        
        protected void clearLog_Click(object sender, EventArgs e)
        {
            EventLogConsole1.LoggedEvents.Clear();
        }

        private string  UnZipFile(string fileName)
        {
            Mensaje("Decompressing files.|info");
            ZipForge archiver = new ZipForge();
            try
            {
                archiver.FileName = objConfig.RutaFileBulk + "\\" + fileName;
                archiver.OpenArchive(System.IO.FileMode.Open);
                archiver.BaseDir = objConfig.RutaFileBulk + "\\" + fileName.Replace(".zip", "");
                archiver.ExtractFiles("*.*");
                archiver.CloseArchive();

                if (Directory.GetFiles(archiver.BaseDir).Length == 0)
                {
                    Mensaje("Files must be on root of file zip.|error");
                    archiver.BaseDir = "";
                }

                return archiver.BaseDir;
            }
            catch (ArchiverException ae)
            {
                Mensaje("There was an error Decompressing the files. Description:" + ae.Message + "\t Error code: " + ae.ErrorCode + "|error");
            }
            return "";
        }

        private Boolean ValidateFiles(FileInfo objFile, int contentType, string type)
        {
            bool validate = false;
            string expression = "";

            #region wallpaper
            if ((contentType == 1) || (contentType == 2))
            {
                if (numGroups == 0)
                    numGroups = 4;

                System.Drawing.Image objImage = System.Drawing.Image.FromFile(objFile.FullName);
                string group = objImage.Width.ToString() + "x" + objImage.Height.ToString();
                expression = "contentType = " + contentType.ToString() + " and handsetGroupName = '" + group + "'";
                if (objHandsetGroup.TableHandsetGroup.Select(expression).Length > 0)
                    validate = true;
                else
                    Mensaje("There was an error verifying the file. Don't exists the group " + group + " on file: " + objFile.Name + "|error");
                return validate;
            }
            #endregion

            #region Sonido
            if ((contentType == 3) || (contentType == 4))
            {
                if ((contentType == 3) && (numGroups == 0))
                    numGroups = 1;
                if ((contentType == 4) && (numGroups == 0))
                    numGroups = 1;

                string group = objFile.Extension.Replace(".", "");
                expression = "contentType = " + contentType.ToString() + " and handsetGroupName = '" + group + "'";
                if (objHandsetGroup.TableHandsetGroup.Select(expression).Length > 0)
                    validate = true;
                else
                    Mensaje("There was an error verifying the file. Don't exists the group " + group + " on file: " + objFile.Name + "|error");
            }
            #endregion

            #region Full_track
            if (contentType == 5)
            {
                if (numGroups == 0)
                    numGroups = 2;

                string group = type;
                expression = "contentType = " + contentType.ToString() + " and handsetGroupName = '" + group + "'";
                DataRow[] objRows = objHandsetGroup.TableHandsetGroup.Select(expression);

                if (objRows.Length > 0)
                {
                    MediaHandler oMediaManagerPro = new MediaHandler();
                    oMediaManagerPro.FFMPEGPath = objConfig.RutaFfmpeg;

                    oMediaManagerPro.FileName = objFile.Name;
                    oMediaManagerPro.InputPath = objFile.DirectoryName;

                    VideoInfo videoInfo = new VideoInfo();
                    videoInfo = oMediaManagerPro.Get_Info();

                    string channel = "";
                    if(videoInfo.FFMPEGOutput.Contains("stereo"))
                        channel = "stereo";
                    else
                        channel = videoInfo.Channel;

                    DataRow objRow = objRows[0];

                    if (channel != objRow["handsetGroupChannel"].ToString().ToLower())
                        Mensaje("There was an error verifying the file. The Channel file is not valid, the channel file must be equal to " + objRow["handsetGroupChannel"].ToString() + " on file: " + objFile.Name + "|error");
                    else
                        validate = true;
                }
                else
                    Mensaje("There was an error verifying the file (Full_track). Don't exists the group " + group + " on file: " + objFile.Name + "|error");
                return validate;
            }
            #endregion

            #region Video
            if (contentType == 6)
            {
                if (numGroups == 0)
                    numGroups = 4;

                string group = type;
                expression = "contentType = " + contentType.ToString() + " and handsetGroupName = '" + group + "'";
                DataRow[] objRows = objHandsetGroup.TableHandsetGroup.Select(expression);
                
                if (objRows.Length > 0)
                {
                    Content.Converter objConverter = new Content.Converter(objConfig.RutaFfmpeg);
                    Content.VideoFile objVideoFile = objConverter.GetVideoInfo(objFile.FullName);
                    string videoFormat = objVideoFile.Width.ToString() + "x" + objVideoFile.Height.ToString();

                    int lenght = int.Parse((objFile.Length/1024).ToString());

                    DataRow objRow = objRows[0];

                    if (objVideoFile.Duration.Seconds > int.Parse(objRow["handsetGroupDuration"].ToString()))
                        Mensaje("There was an error verifying the file. The file duration can't be greater than " + objRow["handsetGroupDuration"].ToString() + " seconds on file: " + objFile.Name + "|error");
                    else if (videoFormat != objRow["handsetGroupDimension"].ToString())
                        Mensaje("There was an error verifying the file. Invalid format size on file: " + objFile.Name + "|error");
                    else
                        validate = true;
                }
                else
                    Mensaje("There was an error verifying the file (video). Don't exists the group " + group + " on file: " + objFile.Name + "|error");
                return validate;
            }
            #endregion

            #region Juegos
            if (contentType == 9)
            {
                if (numGroups == 0)
                    numGroups = 1;

                string group = objFile.Extension.Replace(".", "");
                expression = "contentType = " + contentType.ToString() + " and handsetGroupName = '" + group + "'";
                if (objHandsetGroup.TableHandsetGroup.Select(expression).Length > 0)
                    validate = true;
                else
                    Mensaje("There was an error verifying the file. Don't exists the group " + group + " on file: " + objFile.Name + "|error");
            }
            #endregion

            #region Juegos
            if (contentType == 7)
            {
                if (numGroups == 0)
                    numGroups = 1;

                string group = objFile.Extension.Replace(".", "");
                expression = "contentType = " + contentType.ToString() + " and handsetGroupName = '" + group + "'";
                if (objHandsetGroup.TableHandsetGroup.Select(expression).Length > 0)
                    validate = true;
                else
                    Mensaje("There was an error verifying the file. Don't exists the group " + group + " on file: " + objFile.Name + "|error");
            }
            #endregion

            return validate;
        }

        private VideoInfo GetVideoInfo(FileInfo objFile)
        {
            MediaHandler oMediaManagerPro = new MediaHandler();
            oMediaManagerPro.FFMPEGPath = objConfig.RutaFfmpeg;

            oMediaManagerPro.FileName = objFile.Name;
            oMediaManagerPro.InputPath = objFile.DirectoryName;

            VideoInfo videoInfo = new VideoInfo();
            videoInfo = oMediaManagerPro.Get_Info();

            return videoInfo;
        }

        public void Mensaje(string mensaje)
        {
            EventLogConsole1.LoggedEvents.Add(mensaje);
        }

        protected void rdUploadFile_Init(object sender, EventArgs e)
        {
            rdUploadFile.TargetFolder = objConfig.RutaFileBulk;
        }

        private int UpdateJad(string fileName, string originalName, string newName)
        {
            StringBuilder newFile = new StringBuilder();
            try 
            {
                string temp = "";
                string[] f = File.ReadAllLines(fileName);
                foreach (string line in f)
                {
                    if (line.Contains(originalName))
                    {
                        temp = line.Replace(originalName, newName);
                        newFile.Append(temp + "\r\n");
                        continue;
                    }
                    newFile.Append(line + "\r\n");
                }
                File.WriteAllText(fileName, newFile.ToString());
                return 0;
            }
            catch (Exception ex)
            {
                Mensaje("There was an error editing the file jad.|error");
                return -1;
            }
        }


    }
}