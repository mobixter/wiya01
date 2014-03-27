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
    public sealed class Proveedor : Base
    {
        private static volatile Proveedor objInstance = null;
        private Usuario objUsuario = Usuario.Instance;
        private static object syncRoot = new object();
        private bool _ctr = false;
        private string _providerContactName;
        private string _providerPhone;
        private string _providerEmail;
        private string _providerLocator;

        public bool Ctr
        {
            get { return _ctr; }
            set { _ctr = value; }
        }

        public string ProviderContactName
        {
            get { return _providerContactName; }
            set { _providerContactName = value; }
        }

        public string ProviderPhone
        {
            get { return _providerPhone; }
            set { _providerPhone = value; }
        }

        public string ProviderEmail
        {
            get { return _providerEmail; }
            set { _providerEmail = value; }
        }

        public string ProviderLocator
        {
            get { return _providerLocator; }
            set { _providerLocator = value; }
        }

        private DataTable TbProveedor = new DataTable();

        private Proveedor()
        {
        }

        public static Proveedor Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Proveedor();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableProveedor
        {
            get
            {
                if (TbProveedor != null)
                {
                    if (this.TbProveedor.Rows.Count != 0)
                        return this.TbProveedor;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ProviderList";
                if (objUsuario.ProveedorId != 0)
                    objSelect.Parameter = new DbParameter("providerId", objUsuario.ProveedorId);

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbProveedor = objResultado.Tables["data"];
                    else
                        this.TbProveedor = null;
                }
                else
                    this.TbProveedor = null;
                return this.TbProveedor;
            }
        }

        public long UpdateProvider(Hashtable Proveedor)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_ProviderUpdate";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("providerId", Proveedor["providerId"]));
            objParameterCollection.Add(new DbParameter("providerName", Proveedor["providerName"]));
            objParameterCollection.Add(new DbParameter("providerContactName", Proveedor["providerContactName"]));
            objParameterCollection.Add(new DbParameter("providerPhone", Proveedor["providerPhone"]));
            objParameterCollection.Add(new DbParameter("providerEmail", Proveedor["providerEmail"]));
            objParameterCollection.Add(new DbParameter("providerLocator", Proveedor["providerLocator"]));
            objUpdate.ParameterCollection = objParameterCollection;
            return objUpdate.Update();
        }

        public long InsertProvider(Hashtable Proveedor)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_ProviderInsert";
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("providerName", Proveedor["providerName"]));
            objParameterCollection.Add(new DbParameter("providerContactName", Proveedor["providerContactName"]));
            objParameterCollection.Add(new DbParameter("providerPhone", Proveedor["providerPhone"]));
            objParameterCollection.Add(new DbParameter("providerEmail", Proveedor["providerEmail"]));
            objParameterCollection.Add(new DbParameter("providerLocator", Proveedor["providerLocator"]));
            objInsert.ParameterCollection = objParameterCollection;
            return objInsert.Insert();
        }

        public Proveedor GetProvider(string list)
        {
            Proveedor objProveedor = new Proveedor();
            int artistId = 0;
            foreach (string id in list.Split(','))
            {
                if ((id != "") && (id != "0"))
                    artistId = Convert.ToInt32(id);
            }

            DataRow objRow = TableProveedor.Select("providerId = '" + artistId + "'")[0];
            objProveedor.Id = Convert.ToInt32(objRow["providerId"].ToString());
            objProveedor.NombreSp = objRow["providerName"].ToString();
            objProveedor.ProviderContactName = objRow["providerContactName"].ToString();
            objProveedor.ProviderPhone = objRow["providerPhone"].ToString();
            objProveedor.ProviderEmail = objRow["providerEmail"].ToString();
            objProveedor.ProviderLocator = objRow["providerLocator"].ToString();
            
            return objProveedor;
        }

        public void Dispose()
        {
            objInstance = null;
            TbProveedor = null;
        }
    }
}