using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class HandsetGroup : Base
    {
        private static volatile HandsetGroup objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbHandsetGroup = new DataTable();

        private HandsetGroup()
        {
        }

        public static HandsetGroup Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new HandsetGroup();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableHandsetGroup
        {
            get
            {
                if (TbHandsetGroup != null)
                {
                    if (this.TbHandsetGroup.Rows.Count != 0)
                        return this.TbHandsetGroup;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_HandsetGroupList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbHandsetGroup = objResultado.Tables["data"];
                    else
                        this.TbHandsetGroup = null;
                }
                else
                    this.TbHandsetGroup = null;
                return this.TbHandsetGroup;
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbHandsetGroup = null;
        }
    }
}