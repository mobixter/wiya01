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
    public sealed class Plataforma : Base
    {
        private static volatile Plataforma objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbPlataforma = new DataTable();
        private int _idioma;

        public int Idioma
        {
            get { return _idioma; }
            set { _idioma = value; }
        }

        private Plataforma()
        {
        }

        public static Plataforma Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Plataforma();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TablePlataforma
        {
            get
            {
                if (TbPlataforma != null)
                {
                    if (this.TbPlataforma.Rows.Count != 0)
                        return this.TbPlataforma;
                }
                
                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_PlatformList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbPlataforma = objResultado.Tables["data"];
                    else
                        this.TbPlataforma = null;
                }
                else
                    this.TbPlataforma = null;
                return this.TbPlataforma;
            }
        }

        public long UpdatePlatform(Hashtable Plataforma)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_PlatformUpdate";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("platformId", Plataforma["platformId"]));
            objParameterCollection.Add(new DbParameter("platformName", Plataforma["platformName"]));
            objParameterCollection.Add(new DbParameter("idiomId", Plataforma["idiomId"]));
            objUpdate.ParameterCollection = objParameterCollection;
            return objUpdate.Update();
        }

        public long InsertPlatform(Hashtable Plataforma)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_PlatformInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("platformName", Plataforma["platformName"]));
            objParameterCollection.Add(new DbParameter("idiomId", Plataforma["idiomId"]));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }

        public Plataforma GetPlataforma(string list)
        {
            Plataforma objPlataforma = new Plataforma();
            int platformName = 0;
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0"))
                    platformName = Convert.ToInt32(id);
            }

            DataRow objRow = TablePlataforma.Select("platformName = '" + platformName + "'")[0];
            objPlataforma.Id = Convert.ToInt32(objRow["platformId"].ToString());
            objPlataforma.NombreSp = objRow["platformName"].ToString();
            return objPlataforma;
        }

        public void Dispose()
        {
            objInstance = null;
            TbPlataforma = null;
        }
    }

    public enum Platform
    {
        Spanish = 1,
        English = 2,
        Portuguese = 3
    }
}