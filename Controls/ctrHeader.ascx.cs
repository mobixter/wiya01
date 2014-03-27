using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContentAdmin
{
    public partial class ctrHeader : System.Web.UI.UserControl
    {
        private string _mensaje;

        public string Mensaje
        {
            get { return _mensaje; }
            set { _mensaje = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Inicio()
        {
            this.lblSeccion.Text = _mensaje;
        }
    }
}