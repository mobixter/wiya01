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

namespace ContentAdmin
{
    public class Pais: Base
    {
        private Bitacora objBitacora = new Bitacora();
        private string _strId;
        private int _user;
        private string _dbaps;
        private string _dbzed;
        private string _dbsms;
        private string _ico;

        public string StrId
        {
            get { return _strId; }
            set { _strId = value; }
        }

        public string Dbaps
        {
            get { return _dbaps; }
            set { _dbaps = value; }
        }

        public string Dbzed
        {
            get { return _dbzed; }
            set { _dbzed = value; }
        }

        public string Dbsms
        {
            get { return _dbsms; }
            set { _dbsms = value; }
        }

        public string Ico
        {
            get { return _ico; }
            set { _ico = value; }
        }

        public DataSet GetPais(Pais objPais)
        {
            //AdministraObjetos objProceso = new AdministraObjetos();
            //DataSet objResultado = new DataSet();
            //objProceso.StringConexion = base.StringConexion;

            //string[] arrAtributos = new string[] {null };
            //object[] arrDatos = new object[] {null };
            //string[] arrParametros = new string[] {null };

            //objResultado = objProceso.ConsultarObjetoDS("dbo.usp_AdmPaisListar", arrAtributos, arrDatos, arrParametros);
            //return objResultado;
            return null;
        }

        public Pais GetPais(string strNombre_Pais)
        {
            //AdministraObjetos objProceso = new AdministraObjetos();
            //DataSet objResultado = new DataSet();
            //Pais objPais = new Pais();
            //objProceso.StringConexion = base.StringConexion;

            //string[] arrAtributos = new string[] { "", "strNombre_Pais" };
            //object[] arrDatos = new object[] { null, strNombre_Pais };
            //string[] arrParametros = new string[] { "", "string" };

            //objResultado = objProceso.ConsultarObjetoDS("dbo.usp_AdmPaisListar", arrAtributos, arrDatos, arrParametros);
            //if (objResultado != null)
            //{
            //    if (objResultado.Tables["data"].Rows.Count > 0)
            //    {
            //        objPais.StrId = objResultado.Tables["data"].Rows[0]["strId_Pais"].ToString();
            //    }
            //}
            //return objPais;
            return null;
        }

        public Pais GetDetallePais(Pais objPais)
        {
            //AdministraObjetos objProceso = new AdministraObjetos();
            //DataSet objResultado = new DataSet();
            //Pais pais = new Pais();
            //objProceso.StringConexion = base.StringConexion;

            //string[] arrAtributos = new string[] { null, "strId_Pais" };
            //object[] arrDatos = new object[] { null, objPais.StrId };
            //string[] arrParametros = new string[] { null, "string" };

            //objResultado = objProceso.ConsultarObjetoDS("dbo.usp_AdmPaisListar", arrAtributos, arrDatos, arrParametros);
            //if (objResultado != null)
            //{
            //    if (objResultado.Tables["data"].Rows.Count > 0)
            //    {
            //        foreach (DataRow objRow in objResultado.Tables["data"].Rows)
            //        {
            //            pais.StrId = objRow["strId_Pais"].ToString();
            //            pais.Nombre = objRow["strNombre_Pais"].ToString();
            //            pais.Dbaps = objRow["strDBAPS_Instancia"].ToString();
            //            pais.Dbsms = objRow["strDBSMS_Instancia"].ToString();
            //            pais.Dbzed = objRow["strDBZED_Instancia"].ToString();
            //            pais.Ico = objRow["strIco_Pais"].ToString();
            //        }
            //    }
            //}
            //return pais;
            return null;
        }

        public long InsertPais(Pais objPais)
        {
            //AdministraObjetos objProceso = new AdministraObjetos();
            //long objResultado;
            //objProceso.StringConexion = base.StringConexion;

            //string[] arrAtributos = new string[] { null, "strId_Pais", "strNombre_Pais" };
            //object[] arrDatos = new object[] { null, objPais.StrId, objPais.Nombre };
            //string[] arrParametros = new string[] { null, "string", "string" };
            //objResultado = objProceso.RegistrarObjeto("dbo.usp_AdmPaisInsertar", arrAtributos, arrDatos, arrParametros);

            //objBitacora.Accion = "Insert";
            //objBitacora.Modulo = "País";
            //objBitacora.Que = "0";
            //objBitacora.QueNuevo = objPais.StrId + " - " + objPais.Nombre;
            //objBitacora.InsertBitacora(objBitacora);

            //return objResultado;
            return 0;
        }
    }
}
