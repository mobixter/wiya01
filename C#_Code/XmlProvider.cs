using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using Telerik.Web.UI;

namespace ContentAdmin
{
    public class XmlProvider
    {
        private Item objItem = Item.Instance;
        private Archivo objArchivo = Archivo.Instance;
        private Entidad objEntidad = Entidad.Instance;
        private ItemCategoria objItemCategoria = ItemCategoria.Instance;
        private Categoria objCategoria = Categoria.Instance;
        private Artista objArtista = Artista.Instance;
        private Configuracion objConfig = new Configuracion();
        private ItemPlataformaSend objIPS = new ItemPlataformaSend();
        private ItemPreview objItemPreview = new ItemPreview();
        private Plataforma objPlataforma = Plataforma.Instance;
        private ItemHandset objItemHandset = ItemHandset.Instance;
        private Handset objHandset = Handset.Instance;
        private Charge objCharge = null;
        private string archivo;
        
        private IList<ItemPlataforma> _objItemPlataforma;
        private RadTreeView _treeView;
        private string _fileName;
        private string contentTypeId;
        private string _dirName;
        private int _plataformaId;

        public IList<ItemPlataforma> ObjItemPlataforma
        {
            get { return _objItemPlataforma; }
            set { _objItemPlataforma = value; }
        }

        public RadTreeView TreeView
        {
            get { return _treeView; }
            set { _treeView = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string ContentTypeId
        {
            get { return contentTypeId; }
            set { contentTypeId = value; }
        }

        public string DirName
        {
            get { return _dirName; }
            set { _dirName = value; }
        }

        public int PlataformaId
        {
            get { return _plataformaId; }
            set { _plataformaId = value; }
        }

        public void Writer()
        {
            try 
            {
                string strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                    "<!DOCTYPE dle-batch PUBLIC \" -//MOMAC/DTD DLE EXT DELIVERY1.1/\" \"http://wapdlm.momac.net//dtd/dle-external-delivery-1_0.dtd\">" +
                "<dle-batch>" + 
                    "<contact>op@wilaen.com</contact>" +
                    "<download-items>";
                objItem.Dispose();

                foreach (ItemPlataforma objIP in _objItemPlataforma)
                {
                    int idiomaId = int.Parse(objPlataforma.TablePlataforma.Select("platformId = " + _plataformaId)[0]["idiomId"].ToString());
                    DataRow[] objRowsItem = objItem.TableItem.Select("itemId = " + objIP.Itemid.ToString() + " AND contentTypeId = " + ContentTypeId);

                    if (objRowsItem.Length > 0)
                    {
                        objCharge = new Charge();
                        objCharge.PlatformId = _plataformaId;
                        objCharge.ContentTypeId = int.Parse(ContentTypeId);
                        objCharge.ChargeTypeId = int.Parse(objRowsItem[0]["chargeTypeId"].ToString());
                        if ((objCharge.PlatformId == 2)||(objCharge.PlatformId == 7))
                            objCharge.ItemId = objIP.Itemid;
                        DataSet objResultado = new DataSet();
                        objResultado = objCharge.GetCharge();

                        if (objResultado != null)
                        {
                            this.objArchivo.Dispose();
                            this.objArchivo.Item = objIP.Itemid;
                            if ((ContentTypeId == "3") || (ContentTypeId == "4") || (ContentTypeId == "5"))
                            {
                                this.objArchivo.Platform = _plataformaId;
                            }
                            System.Data.DataTable objTable = new System.Data.DataTable();
                            objTable = this.objArchivo.TableArchivo;
                            string xmlCategoria = string.Empty;
                            string xmlName = string.Empty;
                            
                            //throw new Exception("No hay un registro asociado al Item correspondiente.");
                            if (objTable != null)
                            {
                                if (objTable.Rows.Count > 0)
                                {
                                    DataRow objRowsArtist = objArtista.TableArtista.Select("artistId = " + objRowsItem[0]["artistid"].ToString())[0];
                                    DataRow[] objRowsIC = objItemCategoria.TableItemCategoria.Select("itemId = " + objIP.Itemid.ToString());
                                    DataRow[] objRowsIH = objItemHandset.TableItemHandset.Select("itemId = " + objIP.Itemid.ToString());

                                    string providerUniqueId = objConfig.ProviderId + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "_" + objRowsItem[0]["itemId"].ToString();
                                    strXml += "<download-item>" +
                                                    "<provider-unique-id>" + objIP.ItemIdStr + "</provider-unique-id>";
                                    if ((ContentTypeId == "7") || (ContentTypeId == "9"))
                                        strXml += "<dle-type>application</dle-type>";
                                    else
                                        strXml += "<dle-type>" + objRowsItem[0]["contentTypeName"].ToString().ToLower() + "</dle-type>";
                                    switch (idiomaId)
                                    {
                                        case (int)Platform.Spanish:
                                            xmlName = "<name><![CDATA[" + objRowsItem[0]["itemNameSp"].ToString() + "]]></name>" +
                                                "<caption><![CDATA[" + objRowsItem[0]["itemDescriptionSp"].ToString() + "]]></caption>";
                                            if ((ContentTypeId == "3") || (ContentTypeId == "4") || (ContentTypeId == "5"))
                                            {
                                                xmlName = "<name><![CDATA[" + objRowsArtist["artistName"].ToString() + " - " + objRowsItem[0]["itemNameSp"].ToString() + "]]></name>" +
                                                    "<caption><![CDATA[" + objRowsArtist["artistName"].ToString() + " - " + objRowsItem[0]["itemNameSp"].ToString() + "]]></caption>";
                                            }
                                            else
                                            {
                                                xmlName = "<name><![CDATA[" + objRowsItem[0]["itemNameSp"].ToString() + "]]></name>" +
                                                    "<caption><![CDATA[" + objRowsItem[0]["itemDescriptionSp"].ToString() + "]]></caption>";
                                            }
                                            /*INICIO CATEGORIA*/
                                            foreach (DataRow objRow in objRowsIC)
                                            {
                                                DataRow[] objRowsCategoria = objCategoria.TableCategoria.Select("categoryId = " + objRow["categoryId"].ToString());
                                                xmlCategoria += "<meta name=\"genre\" value=\"" + objRowsCategoria[0]["categoryNameSp"].ToString().Replace("&", "&amp;") + "\"/>";
                                            }
                                            /*FIN CATEGORIA*/
                                            break;
                                        case (int)Platform.English:
                                            xmlName = "<name><![CDATA[" + objRowsItem[0]["itemNameEn"].ToString() + "]]></name>" +
                                                "<caption><![CDATA[" + objRowsItem[0]["itemDescriptionen"].ToString() + "]]></caption>";
                                            if ((ContentTypeId == "3") || (ContentTypeId == "4") || (ContentTypeId == "5"))
                                            {
                                                xmlName = "<name><![CDATA[" + objRowsArtist["artistName"].ToString() + " - " + objRowsItem[0]["itemNameEn"].ToString() + "]]></name>" +
                                                    "<caption><![CDATA[" + objRowsArtist["artistName"].ToString() + " - " + objRowsItem[0]["itemNameEn"].ToString() + "]]></caption>";
                                            }
                                            else
                                            {
                                                xmlName = "<name><![CDATA[" + objRowsItem[0]["itemNameEn"].ToString() + "]]></name>" +
                                                    "<caption><![CDATA[" + objRowsItem[0]["itemDescriptionen"].ToString() + "]]></caption>";
                                            }
                                            /*INICIO CATEGORIA*/
                                            foreach (DataRow objRow in objRowsIC)
                                            {
                                                DataRow[] objRowsCategoria = objCategoria.TableCategoria.Select("categoryId = " + objRow["categoryId"].ToString());
                                                xmlCategoria += "<meta name=\"genre\" value=\"" + objRowsCategoria[0]["categoryNameEn"].ToString().Replace("&", "&amp;") + "\"/>";
                                            }
                                            /*FIN CATEGORIA*/
                                            break;
                                        case (int)Platform.Portuguese:
                                            xmlName = "<name><![CDATA[" + objRowsItem[0]["itemNamePo"].ToString() + "]]></name>" +
                                                "<caption><![CDATA[" + objRowsItem[0]["itemDescriptionPo"].ToString() + "]]></caption>";
                                            if ((ContentTypeId == "3") || (ContentTypeId == "4") || (ContentTypeId == "5"))
                                            {
                                                xmlName = "<name><![CDATA[" + objRowsArtist["artistName"].ToString() + " - " + objRowsItem[0]["itemNamePo"].ToString() + "]]></name>" +
                                                    "<caption><![CDATA[" + objRowsArtist["artistName"].ToString() + " - " + objRowsItem[0]["itemNamePo"].ToString() + "]]></caption>";
                                            }
                                            else
                                            {
                                                xmlName = "<name><![CDATA[" + objRowsItem[0]["itemNamePo"].ToString() + "]]></name>" +
                                                    "<caption><![CDATA[" + objRowsItem[0]["itemDescriptionPo"].ToString() + "]]></caption>";
                                            }
                                            /*INICIO CATEGORIA*/
                                            foreach (DataRow objRow in objRowsIC)
                                            {
                                                DataRow[] objRowsCategoria = objCategoria.TableCategoria.Select("categoryId = " + objRow["categoryId"].ToString());
                                                xmlCategoria += "<meta name=\"genre\" value=\"" + objRowsCategoria[0]["categoryNamePo"].ToString().Replace("&", "&amp;") + "\"/>";
                                            }
                                            /*FIN CATEGORIA*/
                                            break;
                                    }
                                    strXml += xmlName;
                                    strXml += "<meta name=\"primary_provider\" value=\"" + objConfig.ProviderId + "\"/>";
                                    strXml += "<meta name=\"secondary_provider\" value=\"" + objRowsItem[0]["providerName"].ToString() + "\"/>";
                                    /*INICIO CATEGORIA*/
                                    strXml += xmlCategoria;
                                    /*FIN CATEGORIA*/

                                    /*KEYWORDS*/
                                    strXml += "<meta name=\"keywords\" value=\"" + objRowsItem[0]["keywords"].ToString().Replace("&", "&amp;") + "\"/>";
                                    if (ContentTypeId == "9")
                                        strXml += "<meta name=\"provider_path\" value=\"\" />";

                                    strXml += "<meta name=\"advisory\" value=\"" + objRowsItem[0]["itemAdvisory"].ToString() + "\"/>";
                                    /*PRECIO*/
                                    switch (_plataformaId)
                                    {
                                        case 1:
                                            strXml += "<meta name=\"price_excl_tax\" value=\"" + objResultado.Tables["data"].Rows[0]["chargeAmount"].ToString() + "\"/>";
                                            if(objRowsItem[0]["setElemntId"].ToString() != "")
                                                strXml += "<meta name=\"settlement_id\" value=\"" + objRowsItem[0]["setElemntId"].ToString() + "\"/>";
                                            else
                                                strXml += "<meta name=\"settlement_id\" value=\"" + objRowsItem[0]["settlementId"].ToString() + "\"/>";
                                            break;
                                        case 2:
                                            strXml += "<meta name=\"price\" value=\"" + objResultado.Tables["data"].Rows[0]["chargeAmount"].ToString() + "\"/>";
                                            if(objRowsItem[0]["setElemntId"].ToString() != "")
                                                strXml += "<meta name=\"settlement_id\" value=\"" + objRowsItem[0]["setElemntId"].ToString() + "\"/>";
                                            else
                                                strXml += "<meta name=\"settlement_id\" value=\"" + objRowsItem[0]["settlementId"].ToString() + "\"/>";
                                            break;
                                        default:
                                            strXml += "<meta name=\"price_excl_tax\" value=\"" + objResultado.Tables["data"].Rows[0]["chargeAmount"].ToString() + "\"/>";
                                            break;
                                    }

                                    if ((ContentTypeId == "3") || (ContentTypeId == "4") || (ContentTypeId == "5"))
                                    {
                                        strXml += "<meta name=\"artist\" value=\"" + objRowsArtist["artistName"].ToString() + "\"/>" +
                                            "<meta name=\"type\" value=\"" + objRowsItem[0]["contentTypeName"].ToString().ToLower() + "\"/>" +
                                            "<meta name=\"isrc_grid\" value=\"" + objRowsItem[0]["itemIsrcGrid"].ToString() + "\"/>" +
                                            "<meta name=\"upc\" value=\"" + objRowsItem[0]["itemUpc"].ToString() + "\"/>";
                                    }

                                    strXml += "<inherit-meta-data value=\"1\"/>";

                                    strXml += "<resources>";
                                    if ((ContentTypeId == "7") || (ContentTypeId == "9"))
                                    {
                                        DataTable objFileList = new DataTable();
                                        objFileList = objArchivo.ListFile(objIP.Itemid);
                                        if (objFileList != null)
                                        {
                                            foreach (DataRow objRow in objFileList.Rows)
                                            {
                                                string fileName = objRow["fileName"].ToString();

                                                strXml += "<resource>" +
                                                    "<dle-type>application</dle-type>" +
                                                    "<mime-type>" + objRow["mimeTypeName"].ToString() + "</mime-type>" +
                                                    "<provider-type>" + fileName + "</provider-type>";

                                                if ((objRow["mimeTypeName"].ToString() == "application/java-archive")
                                                        || (objRow["mimeTypeName"].ToString() == "application/vnd.android.package-archive"))
                                                {
                                                    DataTable objHandsetList = new DataTable();
                                                    objHandsetList = objItemHandset.ItemHandsetList(objIP.Itemid, long.Parse(objRow["fileId"].ToString()));
                                                    if (objHandsetList != null)
                                                    {
                                                        strXml += "<supported-user-agents>";
                                                        foreach (DataRow objRowHandset in objHandsetList.Rows)
                                                        {
                                                            if (!objRowHandset["handsetStrId"].ToString().Equals("samsung-sgh-t155") || !objRowHandset["handsetStrId"].ToString().Equals("lge-lg231") ||
                                                                !objRowHandset["handsetStrId"].ToString().Equals("mot-w418") || !objRowHandset["handsetStrId"].ToString().Equals("samsung-sgh-t201") ||
                                                                !objRowHandset["handsetStrId"].ToString().Equals("lg-lg225"))
                                                            {
                                                                strXml += "<supported-user-agent value=\"" + objRowHandset["handsetStrId"].ToString() + "\"/>";
                                                            }
                                                        }
                                                        strXml += "</supported-user-agents>";
                                                    }
                                                }
                                                strXml += "<meta name=\"provider_path\" value=\"./" + fileName + "\" />";
                                                strXml += "<file-ref type=\"local\">./" + fileName + "</file-ref>" +
                                                    "</resource>";

                                                if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 7)
                                                    File.Copy(objConfig.RutaFileApplications + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + fileName, true);
                                                else
                                                    File.Copy(objConfig.RutaFileGames + "\\" + objRow["fileName"].ToString(),
                                                        _fileName + "\\" + fileName, true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataRow objRow in objTable.Rows)
                                        {
                                            strXml += "<resource>" +
                                                        "<dle-type>" + objRowsItem[0]["contentTypeName"].ToString().ToLower() + "</dle-type>" +
                                                        "<mime-type>" + objRow["mimeTypeName"].ToString() + "</mime-type>" +
                                                        "<provider-type>" + objRow["fileName"].ToString() + "</provider-type>";

                                            switch (ContentTypeId)
                                            {
                                                case "3":
                                                    strXml += "<meta name=\"filesize\" value=\"" + objRow["fileSize"].ToString() + "\" />";
                                                    break;
                                                case "4":
                                                    strXml += "<meta name=\"filesize\" value=\"" + objRow["fileSize"].ToString() + "\" />";
                                                    break;
                                                case "5":
                                                    strXml += "<meta name=\"filesize\" value=\"" + objRow["fileSize"].ToString() + "\" />";
                                                    break;
                                                case "6":
                                                    strXml += "<attribute value=\"" + objRow["fileVideoCodec"].ToString() + "\" name=\"video_codec\" />";
                                                    strXml += "<attribute value=\"" + objRow["fileAudioCodec"].ToString() + "\" name=\"audio_codec\" />";
                                                    strXml += "<attribute value=\"" + objRow["fileFps"].ToString() + "\" name=\"fps\" />";
                                                    strXml += "<attribute value=\"" + objRow["fileBitrate"].ToString() + "\" name=\"bitrate\" />";
                                                    strXml += "<attribute value=\"" + objRow["fileFormat"].ToString() + "\" name=\"format\" />";
                                                    strXml += "<attribute value=\"" + objRow["fileSize"].ToString() + "\" name=\"filesize\" />";
                                                    break;
                                            }
                                            
                                            strXml += "<file-ref type=\"local\">./" + objRow["fileName"].ToString() + "</file-ref>" +
                                                    "</resource>";

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 1)
                                                File.Copy(objConfig.RutaFileWallpaper + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 2)
                                                File.Copy(objConfig.RutaFileAnimations + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 3)
                                                if (File.Exists(objConfig.RutaFileRealTones + "\\" + objRow["fileName"].ToString()))                                                     
                                                File.Copy(objConfig.RutaFileRealTones + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 4)
                                                File.Copy(objConfig.RutaFilePolyTones + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 5)
                                                File.Copy(objConfig.RutaFileFullTracks + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 6)
                                                File.Copy(objConfig.RutaFileVideo + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 7)
                                                File.Copy(objConfig.RutaFileApplications + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);

                                            if (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 8)
                                                File.Copy(objConfig.RutaFileThemes + "\\" + objRow["fileName"].ToString(),
                                                    _fileName + "\\" + objRow["fileName"].ToString(), true);
                                        }
                                    }
                                    
                                    if ((Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 3) || (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 4) ||
                                        (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 5))
                                    {
                                        System.Data.DataTable objTablePreview = new System.Data.DataTable();
                                        objItemPreview.ItemId = Convert.ToInt32(objRowsItem[0]["itemId"]);
                                        objItemPreview.Type = 1;
                                        objTablePreview = objItemPreview.GetItemPreview();
                                        if (objTablePreview != null)
                                        {
                                            if (objTablePreview.Rows.Count > 0)
                                            {
                                                foreach (DataRow objRow in objTablePreview.Rows)
                                                {
                                                    strXml += "<resource>" +
                                                                "<dle-type>thumbnail</dle-type>" +
                                                                "<mime-type>" + objRow["mimeTypeName"].ToString() + "</mime-type>" +
                                                                "<provider-type>preview</provider-type>";
                                                    if ((ContentTypeId == "3") || (ContentTypeId == "4") || (ContentTypeId == "5"))
                                                        strXml += "<resource-id>" + objRow["previewName"].ToString() + "</resource-id>";
                                                    
                                                    strXml += "<meta name=\"provider_path\" value=\"./" + objRow["previewName"].ToString() + "\" />" +
                                                                "<file-ref type=\"local\">./" + objRow["previewName"].ToString() + "</file-ref>" +
                                                            "</resource>";

                                                    File.Copy(objConfig.RutaThumbnail + "\\" + objRow["previewName"].ToString(),
                                                        _fileName + "\\" + objRow["previewName"].ToString(), true);
                                                }
                                            }
                                        }
                                    }

                                    if ((Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 7) || (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 8)
                                        || (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 9) || (Convert.ToInt16(objRowsItem[0]["contentTypeId"]) == 6))
                                    {
                                        System.Data.DataTable objTablePreview = new System.Data.DataTable();
                                        objItemPreview.ItemId = Convert.ToInt32(objRowsItem[0]["itemId"]);
                                        objItemPreview.Type = 0;
                                        objTablePreview = objItemPreview.GetItemPreview();
                                        if (objTablePreview != null)
                                        {
                                            if (objTablePreview.Rows.Count > 0)
                                            {
                                                int i = 0;
                                                foreach (DataRow objRow in objTablePreview.Rows)
                                                {
                                                    strXml += "<resource>";
                                                    if (i == 0)
                                                        strXml += "<dle-type>preview</dle-type>";
                                                    else
                                                        strXml += "<dle-type>preview_" + i.ToString() + "</dle-type>";

                                                     strXml += "<mime-type>" + objRow["mimeTypeName"].ToString() + "</mime-type>" +
                                                            "<provider-type>" + objRow["previewName"].ToString() + "</provider-type>";

                                                    //if ((ContentTypeId == "3") || (ContentTypeId == "4") || (ContentTypeId == "9") || (ContentTypeId == "6"))
                                                    //    strXml += "<resource-id>" + objRow["previewName"].ToString() + "</resource-id>";
                                                    //else
                                                    //    strXml += "<resource-id/>";

                                                    if (ContentTypeId != "6")
                                                        strXml += "<meta name=\"provider_path\" value=\"./" + objRow["previewName"].ToString() + "\" />";
                                                        
                                                    strXml += "<file-ref type=\"local\">./" + objRow["previewName"].ToString() + "</file-ref>" +
                                                        "</resource>";

                                                    File.Copy(objConfig.RutaPreview + "\\" + objRow["previewName"].ToString(),
                                                        _fileName + "\\" + objRow["previewName"].ToString(), true);
                                                    i++;
                                                }
                                            }
                                        }
                                    }

                                    strXml += "</resources>" +
                                    "</download-item>";

                                    objIP.PlatformId = _plataformaId;
                                    objIP.InsertItemPlatform(objIP);
                                }
                            }
                        }
                        else
                        { }
                    }
                }
                strXml += "</download-items>" +
                    "</dle-batch>";

                XmlDocument objXml = new XmlDocument();
                objXml.LoadXml(strXml);
                //if(ValidateDoc(objXml))
                objXml.Save(_fileName + "\\ContentUpload.xml");

                //XmlSchemaSetValidador objValidator = new XmlSchemaSetValidador();
                //if (objValidator.CompruebaXMLvsXSD("ImportContent", "F:\\SchemaZed.xsd", objConfig.RutaFile.Replace("fileName", _fileName) + "ContentUpload.xml"))
                //    return true;
                //else
                //    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetResource(int contentTypeId)
        {
            return "";
        }

        private static bool ValidateDoc(XmlDocument doc)
        {
            bool isXmlValid = true;
            StringBuilder xmlValMsg = new StringBuilder();

            StringWriter sw = new StringWriter();
            doc.Save(sw);
            //doc.Save(TestFilename);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            settings.ValidationType = ValidationType.DTD;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(delegate(object sender, ValidationEventArgs args)
            {
                isXmlValid = false;
                xmlValMsg.AppendLine(args.Message);
            });

            XmlReader validator = XmlReader.Create(new StringReader(sw.ToString()), settings);

            while (validator.Read())
            {
            }
            validator.Close();

            string message = xmlValMsg.ToString();
            return isXmlValid;
        }

        private void StyleReporteWriter(ref XmlTextWriter writer)
        {
            writer.WriteStartElement("Styles");

            writer.WriteStartElement("Style");

            writer.WriteAttributeString("ss:ID", "Default");
            writer.WriteAttributeString("ss:Name", "Normal");
            writer.WriteStartElement("Alignment");
            writer.WriteAttributeString("ss:Vertical", "Bottom");
            writer.WriteEndElement();

            writer.WriteEndElement();     

            writer.WriteEndElement();
        }

        private void Celda(ref XmlTextWriter writer, string columMerge, string style, string tipo, string valor)
        {
            writer.WriteStartElement("Cell");
            if (columMerge != "0")
                writer.WriteAttributeString("ss:MergeAcross", columMerge);
            writer.WriteAttributeString("ss:StyleID", style);

            writer.WriteStartElement("Data");
            writer.WriteAttributeString("ss:Type", tipo);
            writer.WriteString(valor);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}