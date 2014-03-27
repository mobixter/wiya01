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
    public sealed class Categoria : Base 
    {
        private static volatile Categoria objInstance = null;
        private static object syncRoot = new object();
        private string _categoriaSelected;
        private bool _ctr = false;

        public string CategoriaSelected
        {
            get { return _categoriaSelected; }
            set { _categoriaSelected = value; }
        }

        public bool Ctr
        {
            get { return _ctr; }
            set { _ctr = value; }
        }

        private DataTable TbCategoria = new DataTable();

        private Categoria()
        {
        }

        public static Categoria Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Categoria();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableCategoria
        {
            get
            {
                if (TbCategoria != null)
                {
                    if (this.TbCategoria.Rows.Count != 0)
                        return this.TbCategoria;
                }
                
                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_CategoryList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbCategoria = objResultado.Tables["data"];                    
                    else
                        this.TbCategoria = null;
                }
                else
                    this.TbCategoria = null;
                return this.TbCategoria;
            }
        }

        public long UpdateCategory(Hashtable Categoria)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_CategoryUpdate";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("categoryId", Categoria["categoryId"]));
            objParameterCollection.Add(new DbParameter("categoryNameSp", Categoria["categoryNameSp"]));
            objParameterCollection.Add(new DbParameter("categoryNameEn", Categoria["categoryNameEn"]));
            objParameterCollection.Add(new DbParameter("categoryNamePo", Categoria["categoryNamePo"]));
            objUpdate.ParameterCollection = objParameterCollection;
            return objUpdate.Update();
        }

        public long InsertCategory(Hashtable Categoria)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_CategoryInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("categoryNameSp", Categoria["categoryNameSp"]));
            objParameterCollection.Add(new DbParameter("categoryNameEn", Categoria["categoryNameEn"]));
            objParameterCollection.Add(new DbParameter("categoryNamePo", Categoria["categoryNamePo"]));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }

        public Categoria GetCategoria(string list)
        {
            Categoria objCategoria = new Categoria();
            int intId_Categoria = 0;
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0"))
                    intId_Categoria = Convert.ToInt32(id);
            }

            DataRow objRow = TableCategoria.Select("categoryId = '" + intId_Categoria + "'")[0];
            objCategoria.Id = Convert.ToInt32(objRow["categoryId"].ToString());
            objCategoria.NombreSp = objRow["categoryNameSp"].ToString();
            objCategoria.NombreEn = objRow["categoryNameEn"].ToString();
            objCategoria.NombrePo = objRow["categoryNamePo"].ToString();
            return objCategoria;
        }
        
        public void Dispose()
        {
            objInstance = null;
            TbCategoria = null;
        }
    }
}