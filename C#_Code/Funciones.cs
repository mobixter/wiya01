using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.Archiver;
using Telerik.Web.UI;

namespace ContentAdmin
{
    public class Funciones
    {
        private NameValueCollection appSettings;
        private ZipForge ZipFile = new ZipForge();
        private string rutaLog;
        private string rutaZip;
        private string rutaDll;
        private string rutaBase;

        private void CargarVariables()
        {
            appSettings = ConfigurationManager.AppSettings;
            rutaLog = appSettings["RutaLog"].ToString();
            rutaZip = appSettings["DirZip"].ToString();
            rutaDll = appSettings["DirDll"].ToString();
            rutaBase = appSettings["DirBase"].ToString();
        }

        public void Zip(string file)
        {
            CargarVariables();
            string folderZip = Path.GetDirectoryName(rutaBase) + "\\..\\..\\";
            Directory.SetCurrentDirectory(folderZip);

            if (!Directory.Exists(rutaBase + rutaZip))
                Directory.CreateDirectory(rutaBase + rutaZip);

            ZipFile.Active = false;
            ZipFile.CompressionLevel = CompressionLevel.Fastest;
            ZipFile.CompressionMode = 1;
            ZipFile.ExtractCorruptedFiles = false;
            ZipFile.InMemory = false;
            ZipFile.OpenCorruptedArchives = true;
            ZipFile.Password = "";
            ZipFile.SFXStub = null;
            ZipFile.SpanningMode = SpanningMode.None;
            ZipFile.TempDir = null;

            ZipFile.FileName = rutaBase + rutaZip + file + ".zip";
            ZipFile.OpenArchive(FileMode.Create);
            ZipFile.BaseDir = rutaBase + rutaDll;
            ZipFile.AddFiles(file + ".dll");
            ZipFile.CloseArchive();
        }

        public void CleanFilter(RadGrid objGrid)
        {
            ArrayList objList = new ArrayList();
            foreach (RadMenuItem objMenu in objGrid.FilterMenu.Items)
            {
                if ((objMenu.Text != "NoFilter") && (objMenu.Text != "EqualTo") && (objMenu.Text != "NotEqualTo") && (objMenu.Text != "GreaterThan")
                     && (objMenu.Text != "LessThan") && (objMenu.Text != "Between") && (objMenu.Text != "NotBetween") && (objMenu.Text != "Contains")
                     && (objMenu.Text != "DoesNotContain") && (objMenu.Text != "StartsWith") && (objMenu.Text != "EndsWith"))
                    objList.Add(objMenu);
            }
            foreach (RadMenuItem objMenu in objList)
            {
                objGrid.FilterMenu.Items.Remove(objMenu); 
            }
        }
    }
}
