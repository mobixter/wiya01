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
    public sealed class SubCategoria : Base
    {
        private static volatile SubCategoria objInstance = null;
        private static object syncRoot = new object();

        private Categoria objCategoria = Categoria.Instance;

        private DataTable TbSubCategoria = new DataTable();

        private SubCategoria()
        {
        }

        public static SubCategoria Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new SubCategoria();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableSubCategoria
        {
            get
            {
                if (TbSubCategoria != null)
                {
                    if (this.TbSubCategoria.Rows.Count != 0)
                    {
                        if ((objCategoria.CategoriaSelected != "") && (objCategoria.CategoriaSelected != null))
                        {
                            DataTable NewTbSubCategoria = new DataTable();
                            NewTbSubCategoria.Columns.Add("intId_SubCategoria", typeof(int));
                            NewTbSubCategoria.Columns.Add("strNombre_SubCategoria", typeof(string));
                            NewTbSubCategoria.Columns.Add("intId_Categoria", typeof(int));
                            NewTbSubCategoria.Columns.Add("strNombre_Categoria", typeof(string));

                            DataRow[] objRows = this.TbSubCategoria.Select("strNombre_Categoria = '" + objCategoria.CategoriaSelected + "'");
                            if (objRows.Length == 0)
                            {
                                NewTbSubCategoria.Rows.Add(0, "", 0, "");
                                return NewTbSubCategoria;
                            }

                            foreach (DataRow objRow in objRows)
                            {
                                NewTbSubCategoria.ImportRow(objRow);
                            }
                            return NewTbSubCategoria;
                        }
                        else
                            return this.TbSubCategoria;
                    }
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_SubCategoriaListar";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                    {
                        DataRow row = objResultado.Tables["data"].NewRow();
                        row["intId_SubCategoria"] = 0;
                        row["strNombre_SubCategoria"] = "";
                        objResultado.Tables["data"].Rows.InsertAt(row, 0);

                        this.TbSubCategoria = objResultado.Tables["data"];
                    }
                    else
                        this.TbSubCategoria = null;
                }
                else
                    this.TbSubCategoria = null;
                return this.TbSubCategoria;
            }
        }

        public long UpdateSubCategoria(Hashtable SubCategoria)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_SubCategoriaActualizar";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("intId_SubCategoria", SubCategoria["intId_SubCategoria"]));
            objParameterCollection.Add(new DbParameter("strNombre_SubCategoria", SubCategoria["strNombre_SubCategoria"]));
            objParameterCollection.Add(new DbParameter("intId_Categoria", SubCategoria["intId_Categoria"]));
            objUpdate.ParameterCollection = objParameterCollection;
            return objUpdate.Update();
        }

        public long InsertSubCategoria(Hashtable SubCategoria)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_SubCategoriaInsertar";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("strNombre_SubCategoria", SubCategoria["strNombre_SubCategoria"]));
            objParameterCollection.Add(new DbParameter("intId_Categoria", SubCategoria["intId_Categoria"]));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }

        public void Dispose()
        {
            objInstance = null;
            TbSubCategoria = null;
        }
    }
}