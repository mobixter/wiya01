using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    public class Charge: Base
    {
        private int _platformId;
        private int _contentTypeId;
        private int _chargeTypeId;
        private long _itemId = 0;

        public int PlatformId
        {
            get { return _platformId; }
            set { _platformId = value; }
        }
        
        public int ContentTypeId
        {
            get { return _contentTypeId; }
            set { _contentTypeId = value; }
        }

        public int ChargeTypeId
        {
            get { return _chargeTypeId; }
            set { _chargeTypeId = value; }
        }

        public long ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public DataSet GetCharge()
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_ChargeList";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("platformId", _platformId));
            objParameterCollection.Add(new DbParameter("contentTypeId", _contentTypeId));
            objParameterCollection.Add(new DbParameter("chargeTypeId", _chargeTypeId));
            if (_itemId != 0)
                objParameterCollection.Add(new DbParameter("itemId", _itemId));
            objSelect.ParameterCollection = objParameterCollection;
            objResultado = objSelect.Select();

            if (objResultado != null)
            {
                if (objResultado.Tables["data"].Rows.Count > 0)
                {
                    return objResultado;
                }
                else { return null; }
            }
            else { return null; }
        }
    }
}