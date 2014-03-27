using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class TipoNivel : Base
    {
        private static volatile TipoNivel objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbTipoNivel = new DataTable();

        private TipoNivel()
        {
        }

        public static TipoNivel Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new TipoNivel();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableTipoNivel
        {
            get
            {
                if (TbTipoNivel != null)
                {
                    if (this.TbTipoNivel.Rows.Count != 0)
                        return this.TbTipoNivel;
                }
                
                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_TipoNivelListar";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                    {
                        DataRow row = objResultado.Tables["data"].NewRow();
                        row["intId_TipoNivel"] = 0;
                        row["strNombre_TipoNivel"] = "";
                        objResultado.Tables["data"].Rows.InsertAt(row, 0);

                        this.TbTipoNivel = objResultado.Tables["data"];
                    }
                    else
                        this.TbTipoNivel = null;
                }
                else
                    this.TbTipoNivel = null;
                return this.TbTipoNivel;
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbTipoNivel = null;
        }
    }
}