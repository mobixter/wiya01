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
    public sealed class Handset : Base
    {
        private static volatile Handset objInstance = null;
        private static object syncRoot = new object();
        
        private DataTable TbHandset = new DataTable();

        private string _handsetStrId;
        private long _fabricanteId;

        public string HandsetStrId
        {
            get { return _handsetStrId; }
            set { _handsetStrId = value; }
        }

        public long FabricanteId
        {
            get { return _fabricanteId; }
            set { _fabricanteId = value; }
        }

        private Handset()
        {
        }

        public static Handset Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Handset();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableHandset
        {
            get
            {
                if (TbHandset != null)
                {
                    if (this.TbHandset.Rows.Count != 0)
                        return this.TbHandset;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_HandsetList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbHandset = objResultado.Tables["data"];
                    else
                        this.TbHandset = null;
                }
                else
                    this.TbHandset = null;
                return this.TbHandset;
            }
        }

        public Handset GetHandset(string list)
        {
            Handset objHandset = new Handset();
            int intId_Handset = 0;
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0"))
                    intId_Handset = Convert.ToInt32(id);
            }

            DataRow objRow = TableHandset.Select("handsetId = '" + intId_Handset + "'")[0];
            objHandset.Id = Convert.ToInt32(objRow["handsetId"].ToString());
            objHandset.HandsetStrId = objRow["handsetStrId"].ToString();
            return objHandset;
        }

        public long InsertHandset()
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_HandsetInsert";
            DbParameterCollection objParameter = new DbParameterCollection();
            objParameter.Add(new DbParameter("manufacturerId", _fabricanteId));
            objParameter.Add(new DbParameter("handsetStrId", _handsetStrId));
            objParameter.Add(new DbParameter("handsetName", base.NombreSp));
            objInsert.ParameterCollection = objParameter;
            return objInsert.Insert();
        }

        public void Dispose()
        {
            objInstance = null;
            TbHandset = null;
        }
    }
}