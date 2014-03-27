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
    public partial class ctrItem : System.Web.UI.UserControl
    {
        private TipoItem objTipoItem = TipoItem.Instance;
        private Proveedor objProveedor = Proveedor.Instance;
        private Artista objArtista = Artista.Instance;
        private MimeType objMimeType = MimeType.Instance;
        private ItemCategoria objItemCategoria = ItemCategoria.Instance;
        private ItemHandset objItemHandset = ItemHandset.Instance;
        private ItemPlatformInternal objInternal = ItemPlatformInternal.Instance;
        private ChargeType objChargeType = ChargeType.Instance;
        private Configuracion objConfig = new Configuracion();

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
            LoadVariable();
            LoadList();

            objTipoItem.Dispose();
            ctrTipoItem.Enabled = true;
            ctrTipoItem.Inicio(ctrTipoItem.ID, objTipoItem.TableTipoItem);

            objProveedor.Dispose();
            ctrProveedor.Enabled = true;
            ctrProveedor.Inicio(ctrProveedor.ID, objProveedor.TableProveedor);

            objArtista.Dispose();
            ctrArtista.Enabled = true;
            ctrArtista.Inicio(ctrArtista.ID, objArtista.TableArtista);

            objChargeType.Dispose();
            ctrChaegeType.Enabled = true;
            ctrChaegeType.Inicio(ctrChaegeType.ID, objChargeType.TableChargeType);

            //objMimeType.Dispose();
            //ctrMimeTypePreview.Enabled = true;
            //ctrMimeTypePreview.Inicio(ctrMimeTypePreview.ID, objMimeType.TableMimeType);

            //ctrMimeTypeFile.Enabled = true;
            //ctrMimeTypeFile.Validacion = false;
            //ctrMimeTypeFile.Inicio(ctrMimeTypeFile.ID, objMimeType.TableMimeType);
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
            this.DataBinding += new System.EventHandler(this.ItemDetails_DataBinding);
        }
        #endregion

        protected void ItemDetails_DataBinding(object sender, System.EventArgs e)
        {
            if (DataItem is Telerik.Web.UI.GridInsertionObject)
                lblItem.Text = "Inserting new record...";
            else
            {
                lblItem.Text = "Id: " + DataBinder.Eval(DataItem, "itemid").ToString();
                txtNombreSp.Text = DataBinder.Eval(DataItem, "itemNameSp").ToString();
                txtNombreEn.Text = DataBinder.Eval(DataItem, "itemNameEn").ToString();
                txtNombrePo.Text = DataBinder.Eval(DataItem, "itemNamePo").ToString();
                txtStrId.Text = DataBinder.Eval(DataItem, "itemStrId").ToString();
                txtDescSp.Text = DataBinder.Eval(DataItem, "itemDescriptionSp").ToString();
                txtDescEn.Text = DataBinder.Eval(DataItem, "itemDescriptionEn").ToString();
                txtDescPo.Text = DataBinder.Eval(DataItem, "itemDescriptionPo").ToString();
                txtIsrcGrid.Text = DataBinder.Eval(DataItem, "itemIsrcGrid").ToString();
                txtUpc.Text = DataBinder.Eval(DataItem, "itemUpc").ToString();
                txtKeyword.Text = DataBinder.Eval(DataItem, "keywords").ToString();
                string setElemntId;
                if(DataBinder.Eval(DataItem, "setElemntId").ToString() != "")
                    setElemntId = DataBinder.Eval(DataItem, "setElemntId").ToString();
                else
                    setElemntId = DataBinder.Eval(DataItem, "settlementId").ToString();
                txtSetElemntId.Text = setElemntId;
                setElemntId = DataBinder.Eval(DataItem, "settlementId").ToString();
                cboAdvisory.SelectedValue = DataBinder.Eval(DataItem, "itemAdvisory").ToString();
            }
        }

       

        private void LoadVariable()
        {
            if (!Page.IsPostBack)
            {
                ctrTipoItem.Selected = 0;
                ctrProveedor.Selected = 0;
                ctrArtista.Selected = 0;
                ctrChaegeType.Selected = 0;
                //ctrMimeTypePreview.Selected = 0;
                //ctrMimeTypeFile.Selected = 0;
            }
            else
            {
                if (DataItem is Telerik.Web.UI.GridInsertionObject)
                {
                    ctrTipoItem.Selected = Convert.ToInt32(Request.Form["ctrTipoItem$ctrTipoItem"]);                    
                    ctrProveedor.Selected = 0;
                    ctrArtista.Selected = 0;
                    ctrChaegeType.Selected = 0;
                    //ctrMimeTypePreview.Selected = 0;
                    //ctrMimeTypeFile.Selected = 0;

                    if (ctrTipoItem.Selected == 1)
                    {
                        txtNombreSp.Enabled = false;
                        txtNombreEn.Enabled = false;
                        txtNombrePo.Enabled = false;
                        ctrProveedor.Enabled = false;
                    }
                }
                else
                {
                    ctrTipoItem.Selected = Convert.ToInt64(DataBinder.Eval(_dataItem, "contentTypeId"));
                    ctrProveedor.Selected = Convert.ToInt64(DataBinder.Eval(_dataItem, "providerId"));
                    ctrArtista.Selected = Convert.ToInt64(DataBinder.Eval(_dataItem, "artistId"));
                    ctrChaegeType.Selected = Convert.ToInt64(DataBinder.Eval(_dataItem, "chargeTypeId"));
                    //ctrMimeTypePreview.Selected = Convert.ToInt64(DataBinder.Eval(_dataItem, "mimeTypeId"));
                    //if ((ctrTipoItem.Selected == 7) || (ctrTipoItem.Selected == 9))
                    //{
                    //    handsetIdTodos.Enabled = true;
                    //    handsetIdAsociados.Enabled = true;
                    //}
                    //else
                    //{
                    //    handsetIdTodos.Enabled = false;
                    //    handsetIdAsociados.Enabled = false;
                    //}
                }
            }
        }

        private void LoadList()
        {
            if (_dataItem != null)
            {
                if (DataItem is Telerik.Web.UI.GridInsertionObject)
                {
                    itemIdTodos.Items.Clear();
                    itemIdTodos.Items.AddRange(objItemCategoria.GetCategoryAll(0));
                    itemIdAsociados.Items.Clear();

                    //handsetIdTodos.Items.Clear();
                    //handsetIdTodos.Items.AddRange(objItemHandset.GetHandsetAll(0));
                    //handsetIdAsociados.Items.Clear();

                    platformIdTodos.Items.Clear();
                    platformIdTodos.Items.AddRange(objInternal.GetPlatformAll(0));
                    platformIdAsociados.Items.Clear();
                }
                else
                {
                    /*Category*/
                    itemIdTodos.Items.Clear();
                    itemIdTodos.Items.AddRange(objItemCategoria.GetCategoryAll(Convert.ToInt64(DataBinder.Eval(_dataItem, "itemId"))));
                    itemIdAsociados.Items.Clear();
                    itemIdAsociados.Items.AddRange(objItemCategoria.GetCategoryAssociated(Convert.ToInt64(DataBinder.Eval(_dataItem, "itemId"))));
                    /*Handset*/
                    ////handsetIdTodos.Items.Clear();
                    ////handsetIdTodos.Items.AddRange(objItemHandset.GetHandsetAll(Convert.ToInt64(DataBinder.Eval(_dataItem, "itemId"))));
                    ////handsetIdAsociados.Items.Clear();
                    ////handsetIdAsociados.Items.AddRange(objItemHandset.GetHandsetAssociated(Convert.ToInt64(DataBinder.Eval(_dataItem, "itemId"))));
                    /*Platform*/
                    platformIdTodos.Items.Clear();
                    platformIdTodos.Items.AddRange(objInternal.GetPlatformAll(Convert.ToInt64(DataBinder.Eval(_dataItem, "itemId"))));
                    platformIdAsociados.Items.Clear();
                    platformIdAsociados.Items.AddRange(objInternal.GetHandsetAssociated(Convert.ToInt64(DataBinder.Eval(_dataItem, "itemId"))));
                }
            }
        }

        protected void rdUploadPreview_Init(object sender, EventArgs e)
        {
            rdUploadPreview.TargetFolder = objConfig.RutaPreview;
        }

        protected void rdUploadThumbnail_Init(object sender, EventArgs e)
        {
            rdUploadThumbnail.TargetFolder = objConfig.RutaThumbnail;
        }

        protected void rdUploadFile_Init(object sender, EventArgs e)
        {
            rdUploadFile.TargetFolder = objConfig.RutaZip;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }

      
    }
}