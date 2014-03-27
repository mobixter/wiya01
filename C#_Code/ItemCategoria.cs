using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;
using System.Web.UI.WebControls;

namespace ContentAdmin
{
    [Serializable()]
    public class ItemCategoria:Base
    {
        private static volatile ItemCategoria objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbItemCategoria = new DataTable();
        private Categoria objCategoria = Categoria.Instance;

        private long _itemId;

        public long ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        private ItemCategoria()
        {
        }

        public static ItemCategoria Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new ItemCategoria();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableItemCategoria
        {
            get
            {
                if (TbItemCategoria != null)
                {
                    if (this.TbItemCategoria.Rows.Count != 0)
                        return this.TbItemCategoria;
                }
                
                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ItemCategoryList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbItemCategoria = objResultado.Tables["data"];
                    else
                    {
                        DataRow row = objResultado.Tables["data"].NewRow();
                        row["categoryId"] = 0;
                        row["itemId"] = 0;
                        objResultado.Tables["data"].Rows.InsertAt(row, 0);

                        this.TbItemCategoria = objResultado.Tables["data"];
                    }
                }
                else
                    this.TbItemCategoria = null;
                return this.TbItemCategoria;
            }
        }

        public void InsertItemCategory(string list, long itemId)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemCategoryInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();

            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0") && (id != "-1"))
                {
                    objParameterCollection.Add(new DbParameter("categoryId", Convert.ToInt32(id)));
                    objParameterCollection.Add(new DbParameter("itemId", itemId));
                    objInsert.ParameterCollection = objParameterCollection;
                    objInsert.Insert();
                    AddItemCategory(Convert.ToInt32(id), itemId);
                }
            }
        }

        public void DeleteItemCategory()
        {
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_ItemCategoryDelete";
            objDelete.Parameter = new DbParameter("itemId", _itemId);
            objDelete.Delete();

            DataRow[] objRows = TableItemCategoria.Select("itemId = '" + _itemId + "'");
            if (objRows.Length > 0)
            {
                foreach (DataRow objRow in objRows)
                    TableItemCategoria.Rows.Remove(objRow);
            }
        }

        public void DeleteItemCategoria(long itemId)
        {
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_ItemCategoryDelete";
            objDelete.Parameter = new DbParameter("itemId", itemId);
            objDelete.Delete();

            DataRow[] objRows = TableItemCategoria.Select("itemId = '" + itemId + "'");
            if (objRows.Length > 0)
            {
                foreach (DataRow objRow in objRows)
                    TableItemCategoria.Rows.Remove(objRow);
            }
        }

        public ListItem[] GetCategoryAll(long itemId)
        {
            ListItem[] listItem = null;
            string filter = "-1";
            
            int i = 0;
            if (TableItemCategoria != null)
            {
                DataRow[] objRows = TableItemCategoria.Select("itemId = '" + itemId + "'");
                foreach (DataRow objRow in objRows)
                    filter += "," + objRow["categoryId"].ToString();
                objRows = objCategoria.TableCategoria.Select("categoryId NOT IN (" + filter + ")");
                
                if (objRows != null)
                {
                    listItem = new ListItem[objRows.Length];
                    foreach (DataRow objRow in objRows)
                    {
                        listItem[i] = new ListItem(objRow["categoryNameSp"].ToString() + " *|* " + objRow["categoryNameEn"].ToString() + " *|* " + objRow["categoryNamePo"].ToString(), objRow["categoryId"].ToString());
                        i++;
                    }   
                }
            }
            else
            {
                listItem = new ListItem[objCategoria.TableCategoria.Rows.Count];
                foreach (DataRow objRow in objCategoria.TableCategoria.Rows)
                {
                    listItem[i] = new ListItem(objRow["categoryNameSp"].ToString(), objRow["categoryId"].ToString());
                    i++;
                } 
            }

            return listItem;
        }

        public ListItem[] GetCategoryAssociated(long itemId)
        {
            ListItem[] listItem = null;
            string filter = "-2";
            int i = 0;

            if (TableItemCategoria != null)
            {
                DataRow[] objRows = TableItemCategoria.Select("itemId = '" + itemId + "'");
                foreach (DataRow objRow in objRows)
                    filter += "," + objRow["categoryId"].ToString();
                objRows = objCategoria.TableCategoria.Select("categoryId IN (" + filter + ")");
                if (objRows != null)
                {
                    listItem = new ListItem[objRows.Length];
                    foreach (DataRow objRow in objRows)
                    {
                        listItem[i] = new ListItem(objRow["categoryNameSp"].ToString() + " *|* " + objRow["categoryNameEn"].ToString() + " *|* " + objRow["categoryNamePo"].ToString(), objRow["categoryId"].ToString());
                        i++;
                    } 
                }
            }
            else
            {
                listItem = new ListItem[1];
                listItem[i] = new ListItem();
            }

            return listItem;
        }

        public void InsertItemCategory(string list)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemCategoryInsert";
            DbParameterCollection objParameterCollection;

            foreach (string id in list.Split(','))
            {
                objParameterCollection = new DbParameterCollection();
                if ((id != "") && (id != "0") && (id != "-1"))
                {
                    objParameterCollection.Add(new DbParameter("categoryId", Convert.ToInt32(id)));
                    objParameterCollection.Add(new DbParameter("itemId", _itemId));
                    objInsert.ParameterCollection = objParameterCollection;
                    objInsert.Insert();
                    AddItemCategory(Convert.ToInt32(id), _itemId);
                }
            }
        }

        private void AddItemCategory(int categoryId, long itemId)
        {
            if (TableItemCategoria.Select("itemId = '" + itemId + "' AND categoryId = '" + categoryId + "'").Length == 0)
            {
                DataRow newRow = this.TableItemCategoria.NewRow();
                newRow["categoryId"] = categoryId;
                newRow["itemId"] = itemId;
                this.TableItemCategoria.Rows.Add(newRow);
                this.TableItemCategoria.AcceptChanges();
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbItemCategoria = null;
        }
    }
}