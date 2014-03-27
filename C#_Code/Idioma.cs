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
    public sealed class Idioma : Base
    {
        private static volatile Idioma objInstance = null;
        private static object syncRoot = new object();
        private string _idiomaSelected;
        private bool _ctr = false;

        public string ArtistaSelected
        {
            get { return _idiomaSelected; }
            set { _idiomaSelected = value; }
        }

        public bool Ctr
        {
            get { return _ctr; }
            set { _ctr = value; }
        }

        private DataTable TbIdioma = new DataTable();
        
        private Idioma()
        {
        }

        public static Idioma Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Idioma();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableIdioma
        {
            get
            {
                if (TbIdioma != null)
                {
                    if (this.TbIdioma.Rows.Count != 0)
                        return this.TbIdioma;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_IdiomList";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbIdioma = objResultado.Tables["data"];
                    else
                        this.TbIdioma = null;
                }
                else
                    this.TbIdioma = null;
                return this.TbIdioma;
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbIdioma = null;
        }
    }
}