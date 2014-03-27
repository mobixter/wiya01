using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class Telefono : Base
    {
        private static volatile Telefono objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbTelefono = new DataTable();

        private Telefono()
        {
        }

        private string _strId;
        private long _fabricanteId;

        public string StrId
        {
            get { return _strId; }
            set { _strId = value; }
        }
        
        public long FabricanteId
        {
            get { return _fabricanteId; }
            set { _fabricanteId = value; }
        }

        public static Telefono Instance
        {
            get
            {
                if (objInstance == null)    
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Telefono();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableTelefono
        {
            get
            {
                if (TbTelefono != null)
                {
                    if (this.TbTelefono.Rows.Count != 0)
                        return this.TbTelefono;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_HandsetList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbTelefono = objResultado.Tables["data"];
                    else
                        this.TbTelefono = null;
                }
                else
                    this.TbTelefono = null;
                return this.TbTelefono;
            }
        }

        public long InsertTelefono()
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_HandsetInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("manufacturerId", _fabricanteId));
            objParameterCollection.Add(new DbParameter("handsetStrId", _strId));
            objParameterCollection.Add(new DbParameter("handsetName", NombreSp));
            objInsert.ParameterCollection = objParameterCollection;

            return objInsert.Insert();
        }

        public void Dispose()
        {
            objInstance = null;
            TbTelefono = null;
        }
    }
}