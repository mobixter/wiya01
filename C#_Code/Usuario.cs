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
    [Serializable()]
    public sealed class Usuario : Base
    {
        private static volatile Usuario objInstance = null;
        private static object syncRoot = new object();

        private Bitacora objBitacora = new Bitacora();
        private string _login;
        private string _clave;
        private string _email;
        private Rol _rol;
        private DateTime _fechaUltLogin;
        private Pais _pais;
        private int _proveedorId;

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public Rol Rol
        {
            get { return _rol; }
            set { _rol = value; }
        }

        public DateTime FechaUltLogin
        {
            get { return _fechaUltLogin; }
            set { _fechaUltLogin = value; }
        }

        public Pais Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }

        public int ProveedorId
        {
            get { return _proveedorId; }
            set { _proveedorId = value; }
        }

        public static Usuario Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Usuario();
                    }
                }
                return objInstance;
            }
        }

        private DataTable TbUsuario = new DataTable();

        public Usuario VerifyUser()
        {
            DataSet objResultado = new DataSet();
            SelectData objSelect = new SelectData();
            objSelect.Connection = base.ConnectionStrings;
            objSelect.StoreProcedure = "dbo.usp_UsuarioVerificar";
            
            DbParameterCollection objParameterCollection = new DbParameterCollection();
            objParameterCollection.Add(new DbParameter("strLogin_Usuario", _login));
            objParameterCollection.Add(new DbParameter("strPwd_Usuario", _clave));
            objSelect.ParameterCollection = objParameterCollection;
            objResultado = objSelect.Select();

            Usuario objUser = Usuario.Instance;
            Rol objRol = new Rol();

            if (objResultado != null)
            {
                if (objResultado.Tables["data"].Rows.Count > 0)
                {
                    foreach (DataRow objRow in objResultado.Tables["data"].Rows)
                    {
                        objUser.Id = int.Parse(objRow["intId_Usuario"].ToString());
                        objUser.NombreSp = objRow["strNombre_Usuario"].ToString();
                        objUser.FechaUltLogin = DateTime.Parse(objRow["datFechaLogin_Usuario"].ToString());
                        objUser.ProveedorId = int.Parse(objRow["providerId"].ToString());
                        objRol.Id = int.Parse(objRow["intId_Rol"].ToString());
                        objRol.NombreSp = objRow["strNombre_Rol"].ToString();
                        objUser.Rol = objRol;
                    }
                    return objUser;
                }
                else { return null; }
            }
            else { return null; }
        }

        public void WriteEntry(int intId_Usuario)
        {
            UpdateData objUpdate = new UpdateData();
            objUpdate.Connection = base.ConnectionStrings;
            objUpdate.StoreProcedure = "dbo.usp_UsuarioEntradaRegistrar";
            objUpdate.Parameter = new DbParameter("intId_Usuario", intId_Usuario);

            objUpdate.Update();



            //AdministraObjetos objProceso = new AdministraObjetos();
            //objProceso.StringConexion = base.StringConexion;
            
            //string[] arrAtributos = new string[] { null, "intId_Usuario" };
            //object[] arrDatos = new object[] { null, objUsuario.Id };
            //string[] arrParametros = new string[] { null, "entero" };

            //long objResultado = objProceso.RegistrarObjeto("dbo.usp_AdmUsuarioEntradaRegistrar", arrAtributos, arrDatos, arrParametros);

            //objBitacora.Accion = "WriteEntry";
            //objBitacora.Modulo = "Login";
            //objBitacora.Que = "0";
            //objBitacora.QueNuevo = objResultado.ToString();
            //objBitacora.InsertBitacora(objBitacora);
        }

        public long InsertUsuario(Usuario objUsuario)
        {
            //AdministraObjetos objProceso = new AdministraObjetos();
            //long objResultado;
            //objProceso.StringConexion = base.StringConexion;

            //string[] arrAtributos = new string[] { null, "strNombre_Usuario", "strLogin_Usuario", "strClave_Usuario", "intId_Rol", "strEmail_Usuario" };
            //object[] arrDatos = new object[] { null, objUsuario.Nombre, objUsuario.Login, objUsuario.Clave, objUsuario.Rol.Id, objUsuario.Email };
            //string[] arrParametros = new string[] { null, "string", "string", "string", "entero", "string" };
            //objResultado = objProceso.RegistrarObjeto("dbo.usp_AdmUsuarioInsertar", arrAtributos, arrDatos, arrParametros);

            //objBitacora.Accion = "Insert";
            //objBitacora.Modulo = "Usuario";
            //objBitacora.Que = "0";
            //objBitacora.QueNuevo = objUsuario.Nombre + " - " + objUsuario.Login + " - " + objUsuario.Clave + " - " + objUsuario.Rol.Id + " - " + objUsuario.Email;
            //objBitacora.InsertBitacora(objBitacora);

            //return objResultado;
            return 0;
        }

        public long UpdateUsuario(Usuario objUsuario, DataRow objRow)
        {
            //AdministraObjetos objProceso = new AdministraObjetos();
            //long objResultado;
            //objProceso.StringConexion = base.StringConexion;

            //string[] arrAtributos = new string[] { null, "intId_Usuario", "strNombre_Usuario", "strLogin_Usuario", "intId_Rol", "strEmail_Usuario" };
            //object[] arrDatos = new object[] { null, objUsuario.Id, objUsuario.Nombre, objUsuario.Login, objUsuario.Rol.Id, objUsuario.Email };
            //string[] arrParametros = new string[] { null, "entero", "string", "string", "entero", "string" };

            //objResultado = objProceso.RegistrarObjeto("dbo.usp_AdmUsuarioModificar", arrAtributos, arrDatos, arrParametros);

            //objBitacora.Accion = "Update";
            //objBitacora.Modulo = "Usuario";
            //objBitacora.Que = objRow["intId_Usuario"] + " - " + objRow["strNombre_Usuario"] + " - " + objRow["strLogin_Usuario"] + " - " + objRow["intId_Rol"] + " - " + objRow["strEmail_Usuario"];
            //objBitacora.QueNuevo = objUsuario.Id.ToString() + " - " + objUsuario.Nombre + " - " + objUsuario.Login + " - " + objUsuario.Rol.Id + " - " + objUsuario.Email;
            //objBitacora.InsertBitacora(objBitacora);

            //return objResultado;
            return 0;
        }

        public long ChangePwd(Usuario objUsuario)
        {
            //AdministraObjetos objProceso = new AdministraObjetos();
            //objProceso.StringConexion = base.StringConexion;

            //string[] arrAtributos = new string[] { null, "intId_Usuario", "strClave_Usuario" };
            //object[] arrDatos = new object[] { null, objUsuario.Id, objUsuario.Clave };
            //string[] arrParametros = new string[] { null, "entero", "string" };

            //long objResultado = objProceso.RegistrarObjeto("dbo.usp_AdmUsuarioNuevaClaveRegistrar", arrAtributos, arrDatos, arrParametros);

            //objBitacora.Accion = "ChangePwd";
            //objBitacora.Modulo = "Usuario";
            //objBitacora.Que = "0";
            //objBitacora.QueNuevo = objUsuario.Id + " - " + objUsuario.Clave;
            //objBitacora.InsertBitacora(objBitacora);

            //return objResultado;
            return 0;
        }

        public void Dispose()
        {
            objInstance = null;
        }
    }
}
