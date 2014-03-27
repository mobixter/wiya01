using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    public class ItemPlataformaSend : Base
    {
        private int _plataforma;

        public int Plataforma
        {
            get { return _plataforma; }
            set { _plataforma = value; }
        }

        public long InsertItemPlataformaSend(ItemPlataforma objItemPlataforma)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ItemPlataformaSendInsertar";

            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("intId_Item", objItemPlataforma.Itemid));
            objParameterCollection.Add(new DbParameter("intId_Plataforma", _plataforma));
            objParameterCollection.Add(new DbParameter("intId_Categoria", objItemPlataforma.CategoryId));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }
    }
}