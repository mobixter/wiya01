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
using System.Collections;

namespace ContentAdmin
{
    public class Base
    {
        private ConnectionStringSettingsCollection connectionStrings = ConfigurationManager.ConnectionStrings;
        private int _id;
        private string _nombreSp;
        private string _nombreEn;
        private string _nombrePo;
        
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public string NombreSp
        {
            get { return _nombreSp; }
            set { _nombreSp = value; }
        }

        public string NombreEn
        {
            get { return _nombreEn; }
            set { _nombreEn = value; }
        }

        public string NombrePo
        {
            get { return _nombrePo; }
            set { _nombrePo = value; }
        }

        public string ConnectionStrings
        {
            get
            {
                return connectionStrings["DB_ServiceAdmin"].ConnectionString;
            }
        }

        public string ConnectionStringsSms
        {
            get
            {
                return connectionStrings["DB_SMSPlatform_Connections"].ConnectionString;
            }
        }
    }
}
