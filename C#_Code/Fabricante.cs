using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class Fabricante : Base
    {
        private static volatile Fabricante objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbFabricante = new DataTable();

        private Fabricante()
        {
        }

        public static Fabricante Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Fabricante();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableFabricante
        {
            get
            {
                if (TbFabricante != null)
                {
                    if (this.TbFabricante.Rows.Count != 0)
                        return this.TbFabricante;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ManufacturerList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbFabricante = objResultado.Tables["data"];
                    else
                        this.TbFabricante = null;
                }
                else
                    this.TbFabricante = null;
                return this.TbFabricante;
            }
        }

        public long InsertFabricante()
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ManufacturerInsert";
            objInsert.Parameter = new DbParameter("manufacturerName", NombreSp);
            return objInsert.Insert();
        }

        public void Dispose()
        {
            objInstance = null;
            TbFabricante = null;
        }
    }
}