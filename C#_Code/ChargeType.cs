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
    public sealed class ChargeType : Base
    {
        private static volatile ChargeType objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbChargeType = new DataTable();
        
        private ChargeType()
        {
        }

        public static ChargeType Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new ChargeType();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableChargeType
        {
            get
            {
                if (TbChargeType != null)
                {
                    if (this.TbChargeType.Rows.Count != 0)
                        return this.TbChargeType;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ChargeTypeList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbChargeType = objResultado.Tables["data"];
                    else
                        this.TbChargeType = null;
                }
                else
                    this.TbChargeType = null;
                return this.TbChargeType;
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbChargeType = null;
        }
    }
}