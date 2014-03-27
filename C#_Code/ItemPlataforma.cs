using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    public class ItemPlataforma: Base
    {
        private long _itemid;
        private string _itemNameSp;
        private int _categoryId;
        private string _categoryNameSp;
        private int _contentTypeId;
        private string _contentTypeName;
        private string _previewName;
        private int _platformId;
        private int _providerId;
        private string _providerName;
        private string _itemIdStr;
        private bool _NoExport;

        public bool NoExport
        {
            get { return _NoExport; }
            set { _NoExport = value; }
        }

        public long Itemid
        {
            get { return _itemid; }
            set { _itemid = value; }
        }
        
        public string ItemNameSp
        {
            get { return _itemNameSp; }
            set { _itemNameSp = value; }
        }
        
        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }
        
        public string CategoryNameSp
        {
            get { return _categoryNameSp; }
            set { _categoryNameSp = value; }
        }
        
        public int ContentTypeId
        {
            get { return _contentTypeId; }
            set { _contentTypeId = value; }
        }
        
        public string ContentTypeName
        {
            get { return _contentTypeName; }
            set { _contentTypeName = value; }
        }
        
        public string PreviewName
        {
            get { return _previewName; }
            set { _previewName = value; }
        }
        
        public int PlatformId
        {
            get { return _platformId; }
            set { _platformId = value; }
        }

        public int ProviderId
        {
            get { return _providerId; }
            set { _providerId = value; }
        }

        public string ProviderName
        {
            get { return _providerName; }
            set { _providerName = value; }
        }

        public string ItemIdStr
        {
            get { return _itemIdStr; }
            set { _itemIdStr = value; }
        }
    
        public IList<ItemPlataforma> GetItemPlatform()
        {
            IList<ItemPlataforma> objIP = new List<ItemPlataforma>();
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_ItemPlatformList";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("platformId", _platformId));
            objSelect.ParameterCollection = objParameterCollection;
            objResultado = objSelect.Select();
            if (objResultado != null)
            {
                if (objResultado.Tables["data"].Rows.Count > 0)
                {
                    foreach (DataRow objRow in objResultado.Tables["data"].Rows)
                    {
                        ItemPlataforma objItem = new ItemPlataforma();
                        objItem.Itemid = Convert.ToInt32(objRow["itemid"]);
                        objItem.ItemIdStr = objRow["itemStrId"].ToString();
                        objItem.ItemNameSp = objRow["itemNameSp"].ToString();
                        objItem.CategoryId = Convert.ToInt32(objRow["categoryId"]);
                        objItem.CategoryNameSp = objRow["categoryNameSp"].ToString();
                        objItem.ContentTypeId = Convert.ToInt32(objRow["contentTypeId"]);
                        objItem.ContentTypeName = objRow["contentTypeName"].ToString();
                        objItem.PreviewName = objRow["previewName"].ToString();
                        objItem.ProviderId = Convert.ToInt32(objRow["providerId"]);
                        objItem.ProviderName = objRow["providerName"].ToString();
                        objItem.NoExport = Convert.ToBoolean(objRow["NoExport"]);
                        objIP.Add(objItem);
                    }
                }
                else
                    objIP = null;
            }
            else
                objIP = null;
            return objIP;
        }


        public long InsertItemPlatform(ItemPlataforma objItemPlataforma)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemPlatformInsert";

            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemId", objItemPlataforma.Itemid));
            objParameterCollection.Add(new DbParameter("platformId", _platformId));
            objParameterCollection.Add(new DbParameter("categoryId", objItemPlataforma.CategoryId));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }

        public long DeleteItemPlataforma(ItemPlataforma objItemPlataforma)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemPlataformaEliminar";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            return objInsert.Insert();
        }
    }
}