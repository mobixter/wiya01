using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ContentAdmin
{
    public class ListControl : UserControl
    {
        DropDownList cboList = new DropDownList();
        CompareValidator cvalList = new CompareValidator();

        private long _selected;
        private bool _post = false;
        private bool _validacion = true;
        private bool _enabled = true;
        public long Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public bool Post
        {
            get { return _post; }
            set { _post = value; }
        }

        public bool Validacion
        {
            get { return _validacion; }
            set { _validacion = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public void Inicio(string id, DataTable objTable)
        {
            cboList.ID = id;
            cboList.AutoPostBack = _post;
            cboList.Enabled = _enabled;
            cboList.CssClass = "texto";
            cboList.Width = 220;

            cvalList.ID = "cval_" + id;
            cvalList.ControlToValidate = id;
            cvalList.Display = ValidatorDisplay.Dynamic;
            cvalList.ErrorMessage = "Obligatorio";
            cvalList.CssClass = "LabelRequerido";
            cvalList.Operator = ValidationCompareOperator.GreaterThan;
            cvalList.ValueToCompare = "0";
            cvalList.Enabled = _validacion;

            if (objTable != null)
            {
                if (objTable.Rows.Count == 1)
                {
                    cboList.Items.Clear();
                    cboList.Items.Add(new ListItem(objTable.Rows[0][1].ToString(), objTable.Rows[0][0].ToString()));
                    cboList.Enabled = false;
                }
                else
                {
                    cboList.Items.Clear();
                    int i = 0;
                    foreach (DataRow objRow in objTable.Rows)
                    {
                        cboList.Items.Add(new ListItem(objRow[1].ToString(), objRow[0].ToString()));
                        if (_selected == long.Parse(objRow[0].ToString()))
                            cboList.SelectedIndex = i;
                        i += 1;
                    }
                }
            }
            this.Controls.Add(cboList);
            this.Controls.Add(cvalList);
        }

        public void cboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public string Value
        {
            get
            {
                try { return cboList.SelectedItem.Value.ToString(); }
                catch { return "0"; }

            }
        }

        public string ValueText
        {
            get
            {
                try { return cboList.SelectedItem.Text.ToString(); }
                catch { return "0"; }
            }
        }
    }
}