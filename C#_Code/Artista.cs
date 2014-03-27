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
    public sealed class Artista : Base
    {
        private static volatile Artista objInstance = null;
        private static object syncRoot = new object();
        private string _artistaSelected;
        private bool _ctr = false;

        public string ArtistaSelected
        {
            get { return _artistaSelected; }
            set { _artistaSelected = value; }
        }

        public bool Ctr
        {
            get { return _ctr; }
            set { _ctr = value; }
        }

        private DataTable TbArtista = new DataTable();

        private Artista()
        {
        }

        public static Artista Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Artista();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableArtista
        {
            get
            {
                if (TbArtista != null)
                {
                    if (this.TbArtista.Rows.Count != 0)
                        return this.TbArtista;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ArtistList";
                

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbArtista = objResultado.Tables["data"];
                    else
                        this.TbArtista = null;
                }
                else
                    this.TbArtista = null;
                return this.TbArtista;
            }
        }

        public long UpdateArtist(Hashtable Artista)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_ArtistUpdate";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("artistId", Artista["artistId"]));
            objParameterCollection.Add(new DbParameter("artistName", Artista["artistName"]));
            objUpdate.ParameterCollection = objParameterCollection;
            return objUpdate.Update();
        }

        public long InsertArtist(Hashtable Artista)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ArtistInsert";
            objInsert.Parameter = new DbParameter("artistName", Artista["artistName"]);
            return objInsert.Insert();
        }

        public Artista GetArtista(string list)
        {
            Artista objArtista = new Artista();
            int artistId = 0;
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0"))
                    artistId = Convert.ToInt32(id);
            }

            DataRow objRow = TableArtista.Select("artistId = '" + artistId + "'")[0];
            objArtista.Id = Convert.ToInt32(objRow["artistId"].ToString());
            objArtista.NombreSp = objRow["artistName"].ToString();
            return objArtista;
        }

        public void Dispose()
        {
            objInstance = null;
            TbArtista = null;
        }
    }
}