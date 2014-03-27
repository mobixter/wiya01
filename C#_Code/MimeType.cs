using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class MimeType : Base
    {
        private static volatile MimeType objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbMimeType = new DataTable();

        private MimeType()
        {
        }

        public static MimeType Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new MimeType();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableMimeType
        {
            get
            {
                if (TbMimeType != null)
                {
                    if (this.TbMimeType.Rows.Count != 0)
                        return this.TbMimeType;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_MimeTypeList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbMimeType = objResultado.Tables["data"];
                    else
                        this.TbMimeType = null;
                }
                else
                    this.TbMimeType = null;
                return this.TbMimeType;
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbMimeType = null;
        }
    }
}