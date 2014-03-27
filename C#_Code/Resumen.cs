using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DbAcces.Data;

namespace ContentAdmin
{
    [Serializable()]
    public sealed class Resumen : Base
    {
        private static volatile Resumen objInstance = null;
        private static object syncRoot = new object();

        private DataTable TbResumen = new DataTable();

        private Resumen()
        {
        }

        public static Resumen Instance
        {
            get
            {
                if (objInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (objInstance == null)
                            objInstance = new Resumen();
                    }
                }
                return objInstance;
            }
        }

        public DataTable TableResumen
        {
            get
            {
                if (TbResumen != null)
                {
                    if (this.TbResumen.Rows.Count != 0)
                        return this.TbResumen;
                }

                DataSet objResultado = new DataSet();
                SelectData objSelect = new SelectData();
                objSelect.Connection = base.ConnectionStrings;
                objSelect.StoreProcedure = "dbo.usp_ResumenListar";

                objResultado = objSelect.Select();
                if (objResultado != null)
                {
                    if (objResultado.Tables["data"].Rows.Count > 0)
                        this.TbResumen = objResultado.Tables["data"];
                    else
                        this.TbResumen = null;
                }
                else
                    this.TbResumen = null;
                return this.TbResumen;
            }
        }

        public string XmlResumen()
        {
            Dispose();
            string strXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                "<Tree>" +
                "<Node Value=\"1\" Text=\"Resumen\" Expanded=\"True\">";
            foreach (DataRow objRows in this.TableResumen.Select("intPadre_Resumen = 1"))
            {
                strXml += "<Node Value=\"" + objRows["intId_Resumen"].ToString() + "\" Expanded=\"False\" Text=\"" + objRows["strNombre_Resumen"].ToString() + "\" ImageUrl=\"~/Images/Ico/" + objRows["strImage_Resumen"].ToString() + "\" >";
                foreach (DataRow objRow in this.TableResumen.Select("intPadre_Resumen = " + objRows["intId_Resumen"].ToString()))
                {
                    strXml += "<Node Value=\"" + objRow["intId_Resumen"].ToString() + "\" Text=\"" + objRow["strNombre_Resumen"].ToString() + "\" />";
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
            TbResumen = null;
        }
    }
}