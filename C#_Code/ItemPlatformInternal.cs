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
    public class ItemPlatformInternal : Base
    {
        private static volatile ItemPlatformInternal objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbItemPlatformInternal = new DataTable();
        private Plataforma objPlatform = Plataforma.Instance;

        private long _itemId;
        private int _platformId = 0;
        private decimal _precio = 0;

        public long ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public int PlatformId
        {
          get { return _platformId; }
          set { _platformId = value; }
        }

        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        private ItemPlatformInternal()
        {
        }

        public static ItemPlatformInternal Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new ItemPlatformInternal();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableItemPlatformInternal
        {
            get
            {
                if (TbItemPlatformInternal != null)
                {
                    if (this.TbItemPlatformInternal.Rows.Count != 0)
                        return this.TbItemPlatformInternal;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ItemPlatformInternalList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbItemPlatformInternal = objResultado.Tables["data"];
                    else
                    {
                        DataRow row = objResultado.Tables["data"].NewRow();
                        row["platformId"] = 0;
                        row["itemId"] = 0;
                        objResultado.Tables["data"].Rows.InsertAt(row, 0);

                        this.TbItemPlatformInternal = objResultado.Tables["data"];
                    }
                }
                else
                    this.TbItemPlatformInternal = null;
                return this.TbItemPlatformInternal;
            }
        }

        public long InsertItemPlatformInternal()
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemPlatformInternalInsert";

            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemId", _itemId));
            objParameterCollection.Add(new DbParameter("platformId", _platformId));
            objParameterCollection.Add(new DbParameter("price", _precio));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }

        public void DeleteItemPlatformInternal()
        {
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_ItemPlatformInternalDelete";
            objDelete.Parameter = new DbParameter("itemId", _itemId);
            objDelete.Delete();

            DataRow[] objRows = TableItemPlatformInternal.Select("itemId = '" + _itemId + "'");
            if (objRows.Length > 0)
            {
                foreach (DataRow objRow in objRows)
                    TableItemPlatformInternal.Rows.Remove(objRow);
            }
        }

        public ListItem[] GetPlatformAll(long itemId)
        {
            ListItem[] listItem = null;
            string filter = "-1";

            int i = 0;
            if (TableItemPlatformInternal != null)
            {
                DataRow[] objRows = TableItemPlatformInternal.Select("itemId = '" + itemId + "'");
                foreach (DataRow objRow in objRows)
                    filter += "," + objRow["platformId"].ToString();
                objRows = objPlatform.TablePlataforma.Select("platformId NOT IN (" + filter + ")");

                if (objRows != null)
                {
                    listItem = new ListItem[objRows.Length];
                    foreach (DataRow objRow in objRows)
                    {
                        listItem[i] = new ListItem(objRow["platformName"].ToString(), objRow["platformId"].ToString());
                        i++;
                    }
                }
            }
            else
            {
                listItem = new ListItem[objPlatform.TablePlataforma.Rows.Count];
                foreach (DataRow objRow in objPlatform.TablePlataforma.Rows)
                {
                    listItem[i] = new ListItem(objRow["platformName"].ToString(), objRow["platformId"].ToString());
                    i++;
                }
            }

            return listItem;
        }

        public ListItem[] GetHandsetAssociated(long itemId)
        {
            ListItem[] listItem = null;
            string filter = "-2";
            int i = 0;

            if (TableItemPlatformInternal != null)
            {
                DataRow[] objRows = TableItemPlatformInternal.Select("itemId = '" + itemId + "'");
                foreach (DataRow objRow in objRows)
                    filter += "," + objRow["platformId"].ToString();

                objRows = objPlatform.TablePlataforma.Select("platformId IN (" + filter + ")");
                if (objRows != null)
                {
                    listItem = new ListItem[objRows.Length];
                    foreach (DataRow objRow in objRows)
                    {
                        listItem[i] = new ListItem(objRow["platformName"].ToString(), objRow["platformId"].ToString());
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

        public void InsertItemPlatformInternal(string list)
        {
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "-1"))
                {
                    _platformId = int.Parse(id);
                    InsertItemPlatformInternal();
                    AddItemPlatform();
                }
            }
            this.Dispose();
        }

        private void AddItemPlatform()
        {
            if (TableItemPlatformInternal.Select("itemId = '" + _itemId + "' AND platformId = '" + _platformId + "'").Length == 0)
            {
                DataRow newRow = this.TableItemPlatformInternal.NewRow();
                newRow["platformId"] = _platformId;
                newRow["itemId"] = _itemId;
                this.TableItemPlatformInternal.Rows.Add(newRow);
                this.TableItemPlatformInternal.AcceptChanges();
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbItemPlatformInternal = null;
        }
    }
}