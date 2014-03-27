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
    public class ItemHandset : Base
    {
        private static volatile ItemHandset objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbItemHandset = new DataTable();
        private Handset objHandset = Handset.Instance;

        private long _itemId;
        private int _handsetId = 0;
        private string _handsetStrId = null;
        private string _mimeTypeSuffixes = null;
        private long _fileId;

        public long ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public int HandsetId
        {
            get { return _handsetId; }
            set { _handsetId = value; }
        }

        public string HandsetStrId
        {
            get { return _handsetStrId; }
            set { _handsetStrId = value; }
        }


        public string MimeTypeSuffixes
        {
            get { return _mimeTypeSuffixes; }
            set { _mimeTypeSuffixes = value; }
        }

        public long FileId
        {
            get { return _fileId; }
            set { _fileId = value; }
        }

        private ItemHandset()
        {
        }

        public static ItemHandset Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new ItemHandset();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableItemHandset
        {
            get
            {
                if (TbItemHandset != null)
                {
                    if (this.TbItemHandset.Rows.Count != 0)
                        return this.TbItemHandset;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ItemHandsetList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbItemHandset = objResultado.Tables["data"];
                    else
                    {
                        DataRow row = objResultado.Tables["data"].NewRow();
                        row["HandsetId"] = 0;
                        row["itemId"] = 0;
                        row["fileId"] = 0;
                        objResultado.Tables["data"].Rows.InsertAt(row, 0);

                        this.TbItemHandset = objResultado.Tables["data"];
                    }
                }
                else
                    this.TbItemHandset = null;
                return this.TbItemHandset;
            }
        }

        public long InsertItemHandset()
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemHandsetInsert";

            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemId", _itemId));
            objParameterCollection.Add(new DbParameter("fileId", _fileId));
            if (_handsetId != 0)
                objParameterCollection.Add(new DbParameter("handsetId", _handsetId));
            if (_handsetStrId != null)
                objParameterCollection.Add(new DbParameter("handsetStrId", _handsetStrId));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }
        
        public string DupsHandsets()
        {
                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                DbParameterCollection objParameterCollection = new DbParameterCollection();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_DupItemHandsetList";
                objParameterCollection.Add(new DbParameter("fileId", _fileId));
                objParameterCollection.Add(new DbParameter("handsetStrId", _handsetStrId));
                objSelect.ParameterCollection = objParameterCollection;

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                    {
                        foreach (DataRow objRow in objResultado.Tables["data"].Rows) {
                            return objRow["duphandsets"].ToString();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
        }
        
        public void DeleteItemHandset()
        {
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_ItemHandsetDelete";
            objDelete.Parameter = new DbParameter("itemId", _itemId);
            objDelete.Delete();

            DataRow[] objRows = TableItemHandset.Select("itemId = '" + _itemId + "'");
            if (objRows.Length > 0)
            {
                foreach (DataRow objRow in objRows)
                    TableItemHandset.Rows.Remove(objRow);
            }
        }

        public void DeleteItemHandset(long itemId, long handsetId, long fileId)
        {
            DeleteData objDelete = new DeleteData();
            objDelete.Connection = base.ConnectionStrings;
            objDelete.StoreProcedure = "dbo.usp_ItemHandsetDelete";
            objDelete.Parameter = new DbParameter("itemId", itemId);
            objDelete.Parameter = new DbParameter("handsetId", handsetId);
            objDelete.Parameter = new DbParameter("fileId", fileId);
            objDelete.Delete();

            DataRow[] objRows = TableItemHandset.Select("itemId = '" + itemId + "' and handsetId = '" + handsetId + "' and fileId = " + fileId  + "'");
            if (objRows.Length > 0)
            {
                foreach (DataRow objRow in objRows)
                    TableItemHandset.Rows.Remove(objRow);
            }
        }

        public ListItem[] GetHandsetAll(long itemId)
        {
            ListItem[] listItem = null;
            string filter = "-1";

            int i = 0;
            if (TableItemHandset != null)
            {
                DataRow[] objRows = TableItemHandset.Select("itemId = '" + itemId + "'");
                foreach (DataRow objRow in objRows)
                    filter += "," + objRow["handsetId"].ToString();
                objRows = objHandset.TableHandset.Select("handsetId NOT IN (" + filter + ")");

                if (objRows != null)
                {
                    listItem = new ListItem[objRows.Length];
                    foreach (DataRow objRow in objRows)
                    {
                        listItem[i] = new ListItem(objRow["handsetStrId"].ToString(), objRow["handsetId"].ToString());
                        i++;
                    }
                }
            }
            else
            {
                listItem = new ListItem[objHandset.TableHandset.Rows.Count];
                foreach (DataRow objRow in objHandset.TableHandset.Rows)
                {
                    listItem[i] = new ListItem(objRow["handsetStrId"].ToString(), objRow["handsetId"].ToString());
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

            if (TableItemHandset != null)
            {
                DataRow[] objRows = TableItemHandset.Select("itemId = '" + itemId + "'");
                foreach (DataRow objRow in objRows)
                    filter += "," + objRow["handsetId"].ToString();

                objRows = objHandset.TableHandset.Select("handsetId IN (" + filter + ")");
                if (objRows != null)
                {
                    listItem = new ListItem[objRows.Length];
                    foreach (DataRow objRow in objRows)
                    {
                        listItem[i] = new ListItem(objRow["handsetStrId"].ToString(), objRow["handsetId"].ToString());
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

        public void InsertItemHandset(string list)
        {
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0") && (id != "-1"))
                {
                    _handsetId = int.Parse(id);
                    InsertItemHandset();
                    AddItemhandset();
                }
            }
            this.Dispose();
        }

        private void AddItemhandset()
        {
            if (TableItemHandset.Select("itemId = '" + _itemId + "' AND handsetId = '" + _handsetId + "'").Length == 0)
            {
                DataRow newRow = this.TableItemHandset.NewRow();
                newRow["handsetId"] = _handsetId;
                newRow["itemId"] = _itemId;
                this.TableItemHandset.Rows.Add(newRow);
                this.TableItemHandset.AcceptChanges();
            }
        }

        public DataTable ItemHandsetList(long itemId, long fileId)
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_ItemHandsetList";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("itemId", itemId));
            objParameterCollection.Add(new DbParameter("fileId", fileId));
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
            TbItemHandset = null;
        }
    }
}