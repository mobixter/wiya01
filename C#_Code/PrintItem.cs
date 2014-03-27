using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

namespace ContentAdmin
{
    public class PrintItem : UserControl
    {
        public void Inicio(string name)
        {
            HtmlGenericControl div1 = new HtmlGenericControl("div");
            //div1.ID = id.ToString();
            div1.InnerText = name;

            this.Controls.Add(div1);
        }
    }
}