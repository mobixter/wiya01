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
    public partial class ctrCategoria : System.Web.UI.UserControl
    {
        private object _dataItem = null;

        public object DataItem
        {
            get
            {
                return this._dataItem;
            }
            set
            {
                this._dataItem = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///        Required method for Designer support - do not modify 
        ///        the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DataBinding += new System.EventHandler(this.CategoryDetails_DataBinding);

        }
        #endregion

        protected void CategoryDetails_DataBinding(object sender, System.EventArgs e)
        {
            if (DataItem is Telerik.Web.UI.GridInsertionObject)
                lblItem.Text = "Inserting new record...";
            else
            {
                lblItem.Text = "Id: " + DataBinder.Eval(DataItem, "categoryId").ToString();
                txtNombreSp.Text = DataBinder.Eval(DataItem, "categoryNameSp").ToString();
                txtNombreEn.Text = DataBinder.Eval(DataItem, "categoryNameEn").ToString();
                txtNombrePo.Text = DataBinder.Eval(DataItem, "categoryNamePo").ToString();
            }
        }
    }
}