using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using DbAcces.Data;

namespace ContentAdmin
{
    public class UsuarioPais: Base 
    {
        private Usuario _usuario;
        private Pais _pais;
        private int _selected;

        public Usuario Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        public Pais Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }

        public int Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public IList<Pais> GetUsuarioPais(UsuarioPais objUP)
        {
            IList<Pais> results = new List<Pais>();
            Pais objPais;
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_AdmUsuarioPaisListar";

            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("intId_Usuario", objUP.Usuario.Id));
            objParameterCollection.Add(new DbParameter("selected", objUP.Selected));
            objSelect.ParameterCollection = objParameterCollection;
            objResultado = objSelect.Select();

            if (objResultado != null)
            {
                if (objResultado.Tables["data"].Rows.Count > 0)
                {
                    foreach (DataRow objRow in objResultado.Tables["data"].Rows)
                    {
                        objPais = new Pais();
                        if (objRow["strId_Pais"].ToString() == "A00")
                            objPais.NombreSp = "Todos";
                        else
                            objPais.NombreSp = objRow["strNombre_Pais"].ToString();
                        objPais.StrId = objRow["strId_Pais"].ToString();
                        results.Add(objPais);
                    }
                    return results;
                }
                else
                    return null;
            }
            return null;
        }

        //public long InsertUsuarioPais(UsuarioPais objUP)
        //{
        //    AdministraObjetos objProceso = new AdministraObjetos();
        //    long objResultado;
        //    objProceso.StringConexion = base.ConnectionStrings;

        //    string[] arrAtributos = new string[] { null, 
        //        "intId_Usuario", 
        //        "strId_Pais" };
        //    object[] arrDatos = new object[] { null, 
        //        objUP.Usuario.Id, 
        //        objUP.Pais.StrId };
        //    string[] arrParametros = new string[] { null, 
        //        "entero", "string" };

        //    objResultado = objProceso.RegistrarObjeto("dbo.usp_AdmUsuarioPaisInsertar", arrAtributos, arrDatos, arrParametros);
        //    return objResultado;
        //}

        //public long DeleteUsuarioPais(UsuarioPais objUP)
        //{
        //    AdministraObjetos objProceso = new AdministraObjetos();
        //    long objResultado;
        //    objProceso.StringConexion = base.ConnectionStrings;

        //    string[] arrAtributos = new string[] { null, 
        //        "intId_Usuario", 
        //        "strId_Pais" };
        //    object[] arrDatos = new object[] { null, 
        //        objUP.Usuario.Id, 
        //        objUP.Pais.StrId };
        //    string[] arrParametros = new string[] { null, 
        //        "entero", "string" };

        //    objResultado = objProceso.RegistrarObjeto("dbo.usp_AdmUsuarioPaisEliminar", arrAtributos, arrDatos, arrParametros);
        //    return objResultado;
        //}
    }
}
