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
    public sealed class Envio : Base
    {
        private static volatile Envio objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbEnvio = new DataTable();

        private Envio()
        {
        }

        public static Envio Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Envio();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableEnvio
        {
            get
            {
                if (TbEnvio != null)
                {
                    if (this.TbEnvio.Rows.Count != 0)
                        return this.TbEnvio;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_EnvioListar";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                    {
                        this.TbEnvio = objResultado.Tables["data"];
                    }
                    else
                        this.TbEnvio = null;
                }
                else
                    this.TbEnvio = null;
                return this.TbEnvio;
            }
        }

        public long UpdateEnvio(long inputMessageId)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_EnvioEditar";
            objUpdate.Parameter = new DbParameter("inputMessageId", inputMessageId);
            return objUpdate.Update();
        }

        public void Dispose()
        {
            objInstance = null;
            TbEnvio = null;
        }
    }
}