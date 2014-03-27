using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ContentAdmin
{
    public class Configuracion
    {
        public string RutaFile
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile"); }
        }

        public string RutaFileMaster
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master"; }
        }

        public string RutaPreview
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Preview"; }
        }

        public string RutaThumbnail
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Thumbnail"; }
        }

        public string RutaBaseZip
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Zip"; }
        }

        public string RutaZip
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Zip\\Temp"; }
        }

        public string RutaRespaldo
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Respaldo"; }
        }

        public string RutaFileWallpaper
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\Wallpapers"; }
        }

        public string RutaFileAnimations
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\Animations"; }
        }

        public string RutaFileApplications
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\Applications"; }
        }

        public string RutaFileFullTracks
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\FullTracks"; }
        }

        public string RutaFilePolyTones
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\PolyTones"; }
        }

        public string RutaFileRealTones
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\RealTones"; }
        }

        public string RutaFileThemes
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\Themes"; }
        }

        public string RutaFileVideo
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\Videos"; }
        }

        public string RutaFileGames
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Master\\Games"; }
        }

        public string RutaFileBulk
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Bulk"; }
        }

        public string RutaFileBulkRespaldo
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFile") + "Bulk\\Respaldo"; }
        }

        public string RutaFfmpeg
        {
            get
            { return ConfigurationManager.AppSettings.Get("RutaFfmpeg"); }
        }

        public string[] RutaVirtualZip
        {
            get
            {
                string[] path = new string[] { ConfigurationManager.AppSettings.Get("RutaVirtualZip") };
                return path; 
            }
        }

        public string GetFileName()
        {
            return "_Name_" + (DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString()).ToString();
        }

        public string ProviderId
        {
            get
            { return ConfigurationManager.AppSettings.Get("CodProviderMomac"); }
        }
    }
}