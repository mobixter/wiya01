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
    public sealed class MenuAdm : Base
    {
        private static volatile MenuAdm objInstance = null;
        private static object syncRoot = new object();
        private DataTable TbMenu = new DataTable();

        private int _usuario;

        public int Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        private MenuAdm()
        {
        }

        public static MenuAdm Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new MenuAdm();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableMenu
        {
            get
            {
                //if (TbMenu != null)
                //{
                //    if (this.TbMenu.Rows.Count != 0)
                //        return this.TbMenu;
                //}

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_UsuarioMenuListar";
                objSelect.Parameter = new DbParameter("intId_Usuario", _usuario);
                
                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                    {
                        this.TbMenu = objResultado.Tables["data"];
                    }
                    else
                        this.TbMenu = null;
                }
                else
                    this.TbMenu = null;
                return this.TbMenu;
            }
        }

        public void Dispose()
        {
            objInstance = null;
            TbMenu = null;
        }
    }
}
