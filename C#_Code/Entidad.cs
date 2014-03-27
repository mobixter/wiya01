using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class Entidad : Base
    {
        private static volatile Entidad objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbEntidad = new DataTable();

        private Entidad()
        {
        }

        public static Entidad Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Entidad();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableEntidad
        {
            get
            {
                if (TbEntidad != null)
                {
                    if (this.TbEntidad.Rows.Count != 0)
                        return this.TbEntidad;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_EntidadListar";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbEntidad = objResultado.Tables["data"];
                    else
                        this.TbEntidad = null;
                }
                else
                    this.TbEntidad = null;
                return this.TbEntidad;
            }
        }

        public string XmlEntidad()
        {
            string strXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                "<Tree>" +
                "<Node Value=\"999\" Text=\"Entidades\" Expanded=\"True\">"; 
            foreach (DataRow objRows in this.TableEntidad.Select("intPadre_Entidad = 999"))
            {
                strXml += "<Node Value=\"" + objRows["intId_Entidad"].ToString() + "\" Text=\"" + objRows["strNombre_Entidad"].ToString() + "\" >";
                foreach (DataRow objRow in this.TableEntidad.Select("intPadre_Entidad = " + objRows["intId_Entidad"].ToString()))
                {
                    strXml += "<Node Value=\"" + objRow["intId_Entidad"].ToString() + "\" Text=\"" + objRow["strNombre_Entidad"].ToString() + "\" LongDesc=\"" + objRow["intId_TipoNivel"].ToString() + "\" ImageUrl=\"~/Images/Ico/" + objRow["strImage_Entidad"].ToString() + "\" />";
                }
                strXml += "</Node>";
            }

            strXml += "</Node>";
            strXml += "</Tree>";
            return strXml;
        }

        public void Dispose()
        {
            objInstance = null;
            TbEntidad = null;
        }
    }
}