using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;

namespace ContentAdmin
{
    public partial class Menu : System.Web.UI.Page
    {
        private MenuAdm objMenu = MenuAdm.Instance;
        private Funciones objFunciones = new Funciones();
        private Usuario objUsuario = new Usuario();
        private Pais objPais = new Pais();
        private LinkButton lnkButton;
        private Label lblButton;

        protected override void OnInit(EventArgs e)
        {
            LoadMenu();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void LoadMenu()
        {
            try
            {
                if (objMenu != null)
                {
                    if (objMenu.TableMenu.Rows.Count > 0)
                    {
                        foreach (DataRow objRow in objMenu.TableMenu.Rows)
                        {
                            lnkButton = new LinkButton();
                            lblButton = new Label();
                            lnkButton.ID = objRow["intId_Menu"].ToString();
                            lnkButton.Text = objRow["strTitulo_Menu"].ToString();
                            lnkButton.CssClass = "menuOut";
                            lnkButton.Attributes.Add("Onclick", "javascript:window.parent.atrabajo.location.href='" + objRow["strUrl_Menu"].ToString() + "';");
                            lnkButton.Attributes.Add("onmouseover", "menu_onmouseover('" + objRow["intId_Menu"].ToString() + "')");
                            lnkButton.Attributes.Add("onmouseout", "menu_onmouseout('" + objRow["intId_Menu"].ToString() + "')");
                            PanelMenu.Controls.Add(lnkButton);
                            lblButton.Text = " | ";
                            PanelMenu.Controls.Add(lblButton);
                        }
                    }
                }
            }
            catch { }
        }
    }
}