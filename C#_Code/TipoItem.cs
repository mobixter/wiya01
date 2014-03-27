using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class TipoItem : Base
    {
        private static volatile TipoItem objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbTipoItem = new DataTable();

        private TipoItem()
        {
        }

        public static TipoItem Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new TipoItem();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableTipoItem
        {
            get
            {
                if (TbTipoItem != null)
                {
                    if (this.TbTipoItem.Rows.Count != 0)
                        return this.TbTipoItem;
                }
                
                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ContentTypeList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbTipoItem = objResultado.Tables["data"];
                    else
                        this.TbTipoItem = null;
                }
                else
                    this.TbTipoItem = null;
                return this.TbTipoItem;
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbTipoItem = null;
        }
    }
}