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
    public sealed class OperacionRol : Base
    {
        private static volatile OperacionRol objInstance = null;
        private static object syncRoot = new object();
        private string _operacionRolSelected;
        private bool _ctr = false;
        private Rol _objRol;

        public string OperacionRolSelected
        {
            get { return _operacionRolSelected; }
            set { _operacionRolSelected = value; }
        }

        public bool Ctr
        {
            get { return _ctr; }
            set { _ctr = value; }
        }

        public Rol ObjRol
        {
            get { return _objRol; }
            set { _objRol = value; }
        }

        private DataTable TbOperacionRol = new DataTable();

        private OperacionRol()
        {
        }

        public static OperacionRol Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new OperacionRol();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableOperacionRol
        {
            get
            {
                if (TbOperacionRol != null)
                {
                    if (this.TbOperacionRol.Rows.Count != 0)
                        return this.TbOperacionRol;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_OperacionRolList";
                objSelect.Parameter = new DbParameter("intId_Rol", ObjRol.Id);

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbOperacionRol = objResultado.Tables["data"];
                    else
                        this.TbOperacionRol = null;
                }
                else
                    this.TbOperacionRol = null;
                return this.TbOperacionRol;
            }
        }

        public long UpdateArtist(Hashtable OperacionRol)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_ArtistUpdate";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("artistId", OperacionRol["artistId"]));
            objParameterCollection.Add(new DbParameter("artistName", OperacionRol["artistName"]));
            objUpdate.ParameterCollection = objParameterCollection;
            return objUpdate.Update();
        }

        public long InsertArtist(Hashtable OperacionRol)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ArtistInsert";
            objInsert.Parameter = new DbParameter("artistName", OperacionRol["artistName"]);
            return objInsert.Insert();
        }

        public void Dispose()
        {
            objInstance = null;
            TbOperacionRol = null;
        }
    }
}