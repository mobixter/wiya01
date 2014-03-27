using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    public class ItemPreview: Base
    {
        private long _itemId;
        private long _previewId;
        private int _type;

        public long ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }
        
        public long PreviewId
        {
            get { return _previewId; }
            set { _previewId = value; }
        }

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public DataTable GetItemPreview()
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_ItemPreviewList";
            DbParameterCollection objParameterCollection = new DbParameterCollection();

            objParameterCollection.Add(new DbParameter("itemId", _itemId));
            objParameterCollection.Add(new DbParameter("previewType", _type));
            objSelect.ParameterCollection = objParameterCollection;

            objResultado = objSelect.Select();
            if (objResultado != null)
            {
                if (objResultado.Tables["data"].Rows.Count > 0)
                    return objResultado.Tables["data"];
                else
                    return null;
            }
            else
                return null;
        }

        public long InsertItemPreview()
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemPreviewInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();

            objParameterCollection.Add(new DbParameter("itemId", _itemId));
            objParameterCollection.Add(new DbParameter("previewId", _previewId));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }
    }
}