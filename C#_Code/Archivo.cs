using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class Archivo : Base
    {
        private static volatile Archivo objInstance = null;
        private static object syncRoot = new object();

        private long _item;
        public long Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private long _platform;
        public long Platform
        {
            get { return _platform; }
            set { _platform = value; }
        }

        private DataTable TbArchivo = new DataTable();

        private Archivo()
        {
        }

        public static Archivo Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Archivo();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableArchivo
        {
            get
            {
                if (TbArchivo != null)
                {
                    if (this.TbArchivo.Rows.Count != 0)
                        return this.TbArchivo;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_FileList";
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objParameterCollection.Add(new DbParameter("itemId", _item));
                objParameterCollection.Add(new DbParameter("platformId", _platform));
                objSelect.ParameterCollection = objParameterCollection;

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbArchivo = objResultado.Tables["data"];
                    else
                        this.TbArchivo = null;
                }
                else
                    this.TbArchivo = null;
                return this.TbArchivo;
            }
        }

        public long InsertFile(Hashtable Files)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_FileInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("fileName", Files["fileName"]));
            objParameterCollection.Add(new DbParameter("mimeTypeSuffixes", Files["mimeTypeSuffixes"]));
            objParameterCollection.Add(new DbParameter("itemId", Files["itemId"]));
            objParameterCollection.Add(new DbParameter("fileType", Files["fileType"]));
            objParameterCollection.Add(new DbParameter("fileProviderType", Files["fileProviderType"]));
            objParameterCollection.Add(new DbParameter("fileSize", Files["fileSize"]));
            

            if (Files["video_codec"] != null)
                objParameterCollection.Add(new DbParameter("fileVideoCodec", Files["video_codec"]));
            if (Files["audio_codec"] != null)
                objParameterCollection.Add(new DbParameter("fileAudioCodec", Files["audio_codec"]));
            if (Files["audio_bitrate"] != null)
                objParameterCollection.Add(new DbParameter("fileAudioBitrate", Files["audio_bitrate"]));
            if (Files["video_bitrate"] != null)
                objParameterCollection.Add(new DbParameter("fileVideoBitrate", Files["video_bitrate"]));
            if (Files["fps"] != null)
                objParameterCollection.Add(new DbParameter("fileFps", Files["fps"]));
            if (Files["bitrate"] != null)
                objParameterCollection.Add(new DbParameter("fileBitrate", Files["bitrate"]));
            if (Files["format"] != null)
                objParameterCollection.Add(new DbParameter("fileFormat", Files["format"]));
            if (Files["classType"] != null)
                objParameterCollection.Add(new DbParameter("fileClassType", Files["classType"]));

            objInsert.ParameterCollection = objParameterCollection;
            long Result =  objInsert.Insert();
            return Result;
        }

        public long DeleteFile(long itemId)
        {
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_FileDelete";
            objDelete.Parameter = new DbParameter("itemId", itemId);
            return objDelete.Delete();
        }

        public DataTable ListFileHandset(long itemId, long handsetId)
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_FileHandsetList";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemId", itemId));
            objParameterCollection.Add(new DbParameter("handsetId", handsetId));

            objSelect.ParameterCollection = objParameterCollection;
            objResultado = objSelect.Select();
            if (objResultado.Tables["data"].Rows.Count > 0)
                return objResultado.Tables["data"];
            else
                return null;
        }

        public DataTable ListFile(long itemId)
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_FileList";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemId", itemId));
            objSelect.ParameterCollection = objParameterCollection;
            objResultado = objSelect.Select();
            if (objResultado.Tables["data"].Rows.Count > 0)
                return objResultado.Tables["data"];
            else
                return null;
        }

        public void Dispose()
        {
            objInstance = null;
            TbArchivo = null;
        }
    }
}