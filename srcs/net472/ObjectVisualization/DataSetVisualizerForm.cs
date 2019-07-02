using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ObjectVisualization
{
    public partial class DataSetVisualizerForm : Form
    {
        public DataSet Target { get; set; }

        public DataSetVisualizerForm()
        {
            InitializeComponent();
        }

        private void DataSetVisualizerForm_Shown(object sender, EventArgs e)
        {
            if (Target is null)
                return;
            
            var items = Target.Tables.Cast<DataTable>()
                .Select(x => new { TableName = x.TableName, Value = x })
                .ToList();

            cboTable.DataSource = items;
            cboTable.DisplayMember = "TableName";
            cboTable.ValueMember = "Value";
            dgvTable.DataSource = items[0].Value;
        }

        private void cboTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvTable.DataSource = cboTable.SelectedValue;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
