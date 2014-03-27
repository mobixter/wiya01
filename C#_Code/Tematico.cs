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
    public sealed class Tematico : Base
    {
        private static volatile Tematico objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbTematico = new DataTable();

        private Tematico()
        {
        }

        public static Tematico Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Tematico();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableTematico
        {
            get
            {
                if (TbTematico != null)
                {
                    if (this.TbTematico.Rows.Count != 0)
                        return this.TbTematico;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_TematicoListar";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                    {
                        DataRow row = objResultado.Tables["data"].NewRow();
                        row["intId_Tematico"] = 0;
                        row["strNombre_Tematico"] = "";
                        objResultado.Tables["data"].Rows.InsertAt(row, 0);

                        this.TbTematico = objResultado.Tables["data"];
                    }
                    else
                        this.TbTematico = null;
                }
                else
                    this.TbTematico = null;
                return this.TbTematico;
            }
        }

        public long UpdateTematico(Hashtable Tematico)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_TematicoActualizar";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("intId_Tematico", Tematico["intId_Tematico"]));
            objParameterCollection.Add(new DbParameter("strNombre_Tematico", Tematico["strNombre_Tematico"]));
            objUpdate.ParameterCollection = objParameterCollection;
            return objUpdate.Update();
        }

        public long InsertTematico(Hashtable Tematico)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_TematicoInsertar";
            objInsert.Parameter = new DbParameter("strNombre_Tematico", Tematico["strNombre_Tematico"]);
            return objInsert.Insert();
        }

        public void Dispose()
        {
            objInstance = null;
            TbTematico = null;
        }
    }
}