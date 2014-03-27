using Telerik.Web.UI;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using DbAcces.Data;

namespace ContentAdmin
{
    public class FilteringColumn : GridTemplateColumn
    {
        private Categoria objCategoria = Categoria.Instance;
        private SubCategoria objSubCategoria = SubCategoria.Instance;
        private TipoItem objTipoItem = TipoItem.Instance;
        private TipoNivel objTipoNivel = TipoNivel.Instance;
        private Tematico objTematico = Tematico.Instance;
        private Proveedor objProveedor = Proveedor.Instance;
        private Artista objArtista = Artista.Instance;
        private Fabricante objFabricante = Fabricante.Instance;
        private Item objItem = Item.Instance;
        private Plataforma objPlataforma = Plataforma.Instance;

        private int _width = 180;
        private int _height = 250;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        protected override void SetupFilterControls(TableCell cell)
        {
            RadComboBox rcBox = new RadComboBox();
            rcBox.ID = "dd_" + this.DataField;
            rcBox.AutoPostBack = true;
            rcBox.DataTextField = this.DataField;
            rcBox.DataValueField = this.DataField;
            rcBox.SelectedIndexChanged += rcBox_SelectedIndexChanged;
            rcBox.Width = _width;
            if (_height > 0)
                rcBox.Height = _height;
            DataTable table = null;

            if (this.DataField == "providerName")
                table = objProveedor.TableProveedor;
            if (this.DataField == "contentTypeName")
                table = objTipoItem.TableTipoItem;
            if (this.DataField == "artistName")
                table = objArtista.TableArtista;
            if (this.DataField == "manufacturerName")
                table = objFabricante.TableFabricante;
            if (this.DataField == "categoryNameSp")
                table = objCategoria.TableCategoria;
            if (this.DataField == "platformId")
                table = objPlataforma.TablePlataforma;
            if (this.DataField == "NoExport")
            {
                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Columns.Add(this.DataField);


                DataRow dr = dt.NewRow();
                dr[this.DataField] = "";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[this.DataField] = "true";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[this.DataField] = "false";
                dt.Rows.Add(dr);
                table = dt;
            }

            if (table.Rows[0][this.DataField].ToString() != "" && this.DataField != "NoExport")
            {
                DataRow row = table.NewRow();
                row[this.DataField] = "";
                table.Rows.InsertAt(row, 0);
            }
            rcBox.DataSource = table;
            cell.Controls.Add(rcBox);
        }

        protected override void SetCurrentFilterValueToControl(TableCell cell)
        {
            if (!(this.CurrentFilterValue == ""))
            {
                if (((RadComboBox)cell.Controls[0]).Items.FindItemByText(this.CurrentFilterValue) != null)
                    ((RadComboBox)cell.Controls[0]).Items.FindItemByText(this.CurrentFilterValue).Selected = true;
                else
                    ((RadComboBox)cell.Controls[0]).Items.FindItemByText("").Selected = true;
            }
        }

        protected override string GetCurrentFilterValueFromControl(TableCell cell)
        {
            string currentValue = ((RadComboBox)cell.Controls[0]).SelectedItem.Value;
            this.CurrentFilterFunction = (currentValue != "") ? GridKnownFunction.EqualTo : GridKnownFunction.NoFilter;
            if (this.DataField == "providerName")
                objCategoria.CategoriaSelected = currentValue;
            if (this.DataField == "NoExport")
            {
                PageComercializacion ob = new PageComercializacion();
                ob.Session["Pending"] = null;
            }
            return currentValue;  
        }

        private void rcBox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ((GridFilteringItem)(((RadComboBox)sender).Parent.Parent)).FireCommandEvent("Filter", new Pair());
        }
    }
}