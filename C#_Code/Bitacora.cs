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
using DbAcces.Data;

namespace ContentAdmin
{
    public class Bitacora: Base
    {
        private string _modulo;
        private string _accion;
        private string _que;
        private string _queNuevo;
        
        public string Modulo
        {
            get { return _modulo; }
            set { _modulo = value; }
        }

        public string Accion
        {
            get { return _accion; }
            set { _accion = value; }
        }

        public string Que
        {
            get { return _que; }
            set { _que = value; }
        }

        public string QueNuevo
        {
            get { return _queNuevo; }
            set { _queNuevo = value; }
        }

        private Usuario objUsuario
        {
            get
            {
                return (Usuario)HttpContext.Current.Session["usuario"];
            }
        }

        private Pais objPais
        {
            get
            {
                return (Pais)HttpContext.Current.Session["pais"];
            }
        }

        public void InsertBitacora(Bitacora objBitacora)
        {
            InsertData objInsert = new InsertData();
            objInsert.Connection = base.ConnectionStrings;
            objInsert.StoreProcedure = "dbo.usp_AdmBitacoraInsertar";

            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("intId_Usuario", objUsuario.Id));
            objParameterCollection.Add(new DbParameter("strNombre_Usuario", objUsuario.NombreSp));
            objParameterCollection.Add(new DbParameter("strModulo_Bitacora", objBitacora.Modulo));
            objParameterCollection.Add(new DbParameter("strAccion_Bitacora", objBitacora.Accion));
            objParameterCollection.Add(new DbParameter("strQue_Bitacora", objBitacora.Que));
            objParameterCollection.Add(new DbParameter("strQueNuevo_Bitacora", objBitacora.QueNuevo));
            objParameterCollection.Add(new DbParameter("strId_Pais", objPais.StrId));
            objInsert.ParameterCollection = objParameterCollection;
            objInsert.Insert();
        }
    }
}