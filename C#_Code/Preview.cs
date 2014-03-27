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
    public sealed class Preview : Base
    {
        private static volatile Preview objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbPreview = new DataTable();

        private Preview()
        {
        }

        public static Preview Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Preview();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TablePreview
        {
            get
            {
                if (TbPreview != null)
                {
                    if (this.TbPreview.Rows.Count != 0)
                        return this.TbPreview;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_PreviewList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbPreview = objResultado.Tables["data"];
                    else
                        this.TbPreview = null;
                }
                else
                    this.TbPreview = null;
                return this.TbPreview;
            }
        }

        public long InsertPreview(Hashtable Preview)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_PreviewInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("previewName", Preview["previewName"]));
            objParameterCollection.Add(new DbParameter("mimeTypeSuffixes", Preview["mimeTypeSuffixes"]));
            objParameterCollection.Add(new DbParameter("previewType", Preview["previewType"]));
            objParameterCollection.Add(new DbParameter("previewDefault", Preview["previewDefault"]));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }

        public long DeletePreview(long itemId, int previewType)
        {
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_PreviewDelete";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemId", itemId));
            objParameterCollection.Add(new DbParameter("previewType", previewType));
            objDelete.ParameterCollection = objParameterCollection;
            return objDelete.Delete();
        }

        public void Dispose()
        {
            objInstance = null;
            TbPreview = null;
        }
    }
}