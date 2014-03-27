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
    public sealed class Item : Base
    {
        private static volatile Item objInstance = null;
        private static object syncRoot = new object();
        private Usuario objUsuario = Usuario.Instance;

        private DataTable TbItem = new DataTable();

        private string _descripcion;
        private int _proveedorId;
        private int _tipoItemId;
        private int _artistaId;
        private string _previewName;
        private int _status;

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
        
        public int ProveedorId
        {
            get { return _proveedorId; }
            set { _proveedorId = value; }
        }
        
        public int TipoItemId
        {
            get { return _tipoItemId; }
            set { _tipoItemId = value; }
        }
        
        public int ArtistaId
        {
            get { return _artistaId; }
            set { _artistaId = value; }
        }

        public string PreviewName
        {
            get { return _previewName; }
            set { _previewName = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private Item()
        {
        }

        public static Item Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Item();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableItem
        {
            get
            {
                if (TbItem != null)
                {
                    if (this.TbItem.Rows.Count != 0)
                        return this.TbItem;
                }
                
                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ItemList";
                if(objUsuario.ProveedorId != 0)
                    objSelect.Parameter = new DbParameter("providerId", objUsuario.ProveedorId);

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbItem = objResultado.Tables["data"];
                    else
                    {
                        DataRow row = objResultado.Tables["data"].NewRow();
                        row["itemId"] = 0;
                        row["itemStrId"] = "";
                        row["itemNameSp"] = "";
                        row["itemNameEn"] = "";
                        row["itemNamePo"] = "";
                        row["itemDescriptionSp"] = "";
                        row["itemDescriptionEn"] = "";
                        row["itemDescriptionPo"] = "";
                        row["providerId"] = 0;
                        row["contentTypeId"] = 0;
                        row["artistId"] = 0;
                        row["artistName"] = "";
                        row["contentTypeName"] = "";
                        row["providerName"] = "";
                        row["previewName"] = "";
                        row["chargeTypeId"] = 0;
                        objResultado.Tables["data"].Rows.InsertAt(row, 0);
                        this.TbItem = objResultado.Tables["data"];
                    }
                }
                else
                    this.TbItem = null;
                return this.TbItem;
            }
        }

        public DataTable UpdateItem(Hashtable Item)
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_ItemUpdate";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            if (long.Parse(Item["itemId"].ToString()) != -4)
                objParameterCollection.Add(new DbParameter("itemId", Item["itemId"]));
            objParameterCollection.Add(new DbParameter("itemStrId", Item["itemStrId"]));
            objParameterCollection.Add(new DbParameter("itemNameSp", Item["itemNameSp"]));
            objParameterCollection.Add(new DbParameter("itemNameEn", Item["itemNameEn"]));
            objParameterCollection.Add(new DbParameter("itemNamePo", Item["itemNamePo"]));
            objParameterCollection.Add(new DbParameter("itemDescriptionSp", Item["itemDescriptionSp"]));
            objParameterCollection.Add(new DbParameter("itemDescriptionEn", Item["itemDescriptionEn"]));
            objParameterCollection.Add(new DbParameter("itemDescriptionPo", Item["itemDescriptionPo"]));
            objParameterCollection.Add(new DbParameter("providerId", Item["providerId"]));
            objParameterCollection.Add(new DbParameter("contentTypeId", Item["contentTypeId"]));
            objParameterCollection.Add(new DbParameter("artistId", Item["artistId"]));
            objParameterCollection.Add(new DbParameter("itemAdvisory", Item["itemAdvisory"]));
            objParameterCollection.Add(new DbParameter("itemIsrcGrid", Item["itemIsrcGrid"]));
            objParameterCollection.Add(new DbParameter("itemUpc", Item["itemUpc"]));
            objParameterCollection.Add(new DbParameter("chargeTypeId", Item["chargeTypeId"]));
            objParameterCollection.Add(new DbParameter("keywords", Item["keywords"]));
            if (Item["setelement_id"] == null)
                objParameterCollection.Add(new DbParameter("setElemntId", "a"));
            else
                objParameterCollection.Add(new DbParameter("setElemntId", Item["setelement_id"]));

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

        public long InsertItem(Hashtable Item)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemStrId", Item["itemStrId"]));
            objParameterCollection.Add(new DbParameter("itemNameSp", Item["itemNameSp"]));
            objParameterCollection.Add(new DbParameter("itemNameEn", Item["itemNameEn"]));
            objParameterCollection.Add(new DbParameter("itemNamePo", Item["itemNamePo"]));
            objParameterCollection.Add(new DbParameter("itemDescriptionSp", Item["itemDescriptionSp"]));
            objParameterCollection.Add(new DbParameter("itemDescriptionEn", Item["itemDescriptionEn"]));
            objParameterCollection.Add(new DbParameter("itemDescriptionPo", Item["itemDescriptionPo"]));
            objParameterCollection.Add(new DbParameter("providerId", Item["providerId"]));
            objParameterCollection.Add(new DbParameter("contentTypeId", Item["contentTypeId"]));
            objParameterCollection.Add(new DbParameter("artistId", Item["artistId"]));
            objParameterCollection.Add(new DbParameter("itemAdvisory", Item["itemAdvisory"]));
            objParameterCollection.Add(new DbParameter("itemIsrcGrid", Item["itemIsrcGrid"]));
            objParameterCollection.Add(new DbParameter("itemUpc", Item["itemUpc"]));
            objParameterCollection.Add(new DbParameter("chargeTypeId", Item["chargeTypeId"]));
            objParameterCollection.Add(new DbParameter("keywords", Item["keywords"]));
            objParameterCollection.Add(new DbParameter("key", Item["key"]));
            objParameterCollection.Add(new DbParameter("setElemntId", Item["settlement_id"]));
            objInsert.ParameterCollection = objParameterCollection;
            var retreiveResult = objInsert.Insert();
            return retreiveResult;
        }

        public long DeleteItem(long itemId)
        {
           
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_ItemDelete";
            objDelete.Parameter = new DbParameter("itemId", itemId);
            return objDelete.Delete();
        }
        public void ReplaceItem(string  itemstrId)
        {
            System.Data.SqlClient.SqlConnection con2 = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DB_ServiceAdmin"].ToString());
            System.Data.SqlClient.SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand();
            con2.Open();
            cmd2.Connection = con2;
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.CommandText = "dbo.usp_ItemReplace";
            cmd2.Parameters.AddWithValue("@itemStrId", itemstrId);
            cmd2.ExecuteNonQuery();
            con2.Close();
        }
        public void ItemKeyDelete(string ItemKey)
        {
            System.Data.SqlClient.SqlConnection con2 = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DB_ServiceAdmin"].ToString());
            System.Data.SqlClient.SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand();
            con2.Open();
            cmd2.Connection = con2;
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.CommandText = "dbo.usp_ItemKeyDelete";
            cmd2.Parameters.AddWithValue("@ItemKey", ItemKey );
            cmd2.ExecuteNonQuery();
            con2.Close();
        }
        
        public double GetItemId(string itemStrId, string key)
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_ItemBulkList";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemStrId", itemStrId));
            objParameterCollection.Add(new DbParameter("key", key));
            objSelect.ParameterCollection = objParameterCollection;

            objResultado = objSelect.Select();

            if (objResultado != null)
            {
                if (objResultado.Tables["data"].Rows.Count > 0)
                {
                    var retreiveItemId = Int32.Parse(objResultado.Tables["data"].Rows[0]["itemId"].ToString());
                    return retreiveItemId;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        public Item GetItem(string list)
        {
            Item objItem = new Item();
            int itemId = 0;
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0"))
                    itemId = Convert.ToInt32(id);
            }

            DataRow objRow = TableItem.Select("itemId = '" + itemId + "'")[0];
            objItem.Id = Convert.ToInt32(objRow["itemId"].ToString());
            objItem.NombreSp = objRow["itemNameSp"].ToString();
            objItem.NombreEn = objRow["itemNameEn"].ToString();
            objItem.NombrePo = objRow["itemNamepo"].ToString();
            return objItem;
        }

        public void Dispose()
        {
            objInstance = null;
            TbItem = null;
        }
    }
}